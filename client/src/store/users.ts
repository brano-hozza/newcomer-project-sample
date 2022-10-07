import { defineStore } from 'pinia';

import type { IUser } from '../types/user';

type TUserState = {
	users: IUser[];
	userDetails: IUser | null;
	loading: boolean;
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
		history: []
	}),
	actions: {
		/**
		 * Action to fetch users from the API
		 */
		async fetchUsers() {
			this.loading = true;
			try {
				const response = await fetch('http://localhost:5000/api/users');
				this.users = await response.json();
			} catch (e) {
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
				const response = await fetch(
					'http://localhost:5000/api/users/old'
				);
				this.users = await response.json();
			} catch (e) {
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
				const response = await fetch(
					`http://localhost:5000/api/users/${id}`
				);
				this.userDetails = await response.json();
			} catch (e) {
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
				const response = await fetch(
					`http://localhost:5000/api/users/${id}/history`
				);
				this.history = await response.json();
			} catch (e) {
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
				await fetch(
					`http://localhost:5000/api/users/${id}${
						soft ? '/soft' : ''
					}`,
					{
						method: 'DELETE'
					}
				);
				this.users = this.users.filter(user => user.id !== id);
			} catch (e) {
				//eslint-disable-next-line no-console
				console.log(e);
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
				const resp = await fetch('http://localhost:5000/api/users', {
					method: 'POST',
					headers: {
						'Content-Type': 'application/json'
					},
					body: JSON.stringify(user)
				});
				const newUser = (await resp.json()) as IUser;
				this.users = [...this.users, newUser];
			} catch (e) {
				//eslint-disable-next-line no-console
				console.log(e);
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
				await fetch(`http://localhost:5000/api/users/${user.id}`, {
					method: 'PUT',
					headers: {
						'Content-Type': 'application/json'
					},
					body: JSON.stringify(user)
				});
				this.users = this.users.map(u => (u.id === user.id ? user : u));
			} catch (e) {
				//eslint-disable-next-line no-console
				console.log(e);
			}
		}
	}
});
