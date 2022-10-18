import { defineStore } from 'pinia';

import { apiCall } from '@/helpers/api';
import type { IUser } from '@/types/user';

type TUserState = {
	users: IUser[];
	userDetails: IUser | null;
	loading: boolean;
	userExists: string | null;
	networkError: boolean;
	history: {
		id: number;
		positionId: number;
		userId: number;
		startDate: string;
		endDate: string;
	}[];
};

export const useUsersStore = defineStore('users', {
	state: (): TUserState => ({
		users: [],
		userDetails: null,
		loading: false,
		history: [],
		userExists: null,
		networkError: false
	}),
	actions: {
		/**
		 * Function to display network error
		 */
		setNetworkError() {
			this.networkError = true;
			setTimeout(() => {
				this.networkError = false;
			}, 4000);
		},
		/**
		 * Action to fetch users from the API
		 */
		async fetchUsers() {
			this.loading = true;
			try {
				const response = await apiCall('/api/users');
				this.users = await response.json();
			} catch (e) {
				this.setNetworkError();
				this.users = [];
			}
			this.loading = false;
		},
		/**
		 * Action to fetch users from the API
		 */
		async fetchOldUsers() {
			this.loading = true;
			try {
				const response = await apiCall('/api/users/old');
				this.users = await response.json();
			} catch (e) {
				this.setNetworkError();
				this.users = [];
			}
			this.loading = false;
		},
		/**
		 * Action to fetch user details from the API
		 * @param id - user id
		 */
		async fetchUserDetails(id: number) {
			this.loading = true;
			try {
				const response = await apiCall(`/api/users/${id}`);
				this.userDetails = await response.json();
			} catch (e) {
				this.setNetworkError();
				this.userDetails = null;
			}
			this.loading = false;
		},
		/**
		 * Action to fetch user history from the API
		 * @param id - user id
		 */
		async fetchUserHistory(id: number) {
			this.loading = true;
			try {
				const response = await apiCall(`/api/users/${id}/history`);
				this.history = await response.json();
			} catch (e) {
				this.setNetworkError();
				this.history = [];
			}
			this.loading = false;
		},
		/**
		 * Action to delete user
		 * @param id - user id
		 */
		async deleteUser(id: number, soft: boolean) {
			this.loading = true;
			try {
				await apiCall(`/api/users/${id}${soft ? '/soft' : ''}`, {
					method: 'DELETE'
				});
				this.users = this.users.filter(user => user.id !== id);
			} catch (e) {
				this.setNetworkError();
			}
			this.loading = false;
		},
		/**
		 * Action to create user
		 * @param user - user object
		 */
		async createUser(user: IUser) {
			this.loading = true;
			try {
				const resp = await apiCall('/api/users', {
					method: 'POST',
					headers: {
						'Content-Type': 'application/json'
					},
					body: JSON.stringify(user)
				});
				
				if (resp.status === 403) {
					this.userExists = 'Používateľ s týmto menom už existuje';
					setTimeout(() => {
						this.userExists = null;
					}, 4000);
					throw new Error('User already exists');
				}
				const newUser = (await resp.json()) as IUser;
				this.users = [...this.users, newUser];
			} catch (e) {
				this.setNetworkError();
			}
			this.loading = false;
		},
		/**
		 * Action to update user
		 * @param user - user object
		 */
		async updateUser(user: IUser) {
			this.loading = true;
			try {
				const resp = await apiCall(`/api/users/${user.id}`, {
					method: 'PUT',
					headers: {
						'Content-Type': 'application/json'
					},
					body: JSON.stringify(user)
				});
				
				if (resp.status === 403) {
					this.userExists = 'Používateľ s týmto menom už existuje';
					setTimeout(() => {
						this.userExists = null;
					}, 4000);
					throw new Error('User already exists');
				}
				this.users = this.users.map(u => (u.id === user.id ? user : u));
			} catch (e) {
				this.setNetworkError();
			}
			this.loading = false;
		}
	}
});
