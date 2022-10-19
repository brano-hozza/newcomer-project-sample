import { defineStore } from 'pinia';

import { apiCall } from '@/helpers/api';
import type { IUser } from '@/types';

import { useNotificationStore } from './notification';

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

const notificationStore = useNotificationStore();
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
				const response = await apiCall('/api/users');
				this.users = await response.json();
			} catch (e) {
				notificationStore.addNotification('Problem API' , 'error', 'Neviem sa pripojiť na server');
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
				notificationStore.addNotification('Problem API' , 'error', 'Neviem sa pripojiť na server');
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
				notificationStore.addNotification('Problem API' , 'error', 'Neviem sa pripojiť na server');
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
				notificationStore.addNotification('Problem API' , 'error', 'Neviem sa pripojiť na server');
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
				notificationStore.addNotification('Problem API' , 'error', 'Neviem sa pripojiť na server');
				this.loading = false;
				return;
			}
			notificationStore.addNotification(`Zamestnanec uspesne ${soft ? 'archivovany' : 'vymazany'}` , 'success');
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
				
				if (resp.status === 409) {
					notificationStore.addNotification('Pouzivatel existuje' , 'error', 'Pouzivatel s tymto menom uz existuje');
					this.loading = false;
					return;
				}
				const newUser = (await resp.json()) as IUser;
				this.users = [...this.users, newUser];
			} catch (e) {
				notificationStore.addNotification('Problem API' , 'error', 'Neviem sa pripojiť na server');
				this.loading = false;
				return;

			}
			this.loading = false;
			notificationStore.addNotification('Pouzivatel vytvoreny' , 'success');
		},
		/**
		 * Action to update user
		 * @param user - user object
		 */
		async updateUser(user: IUser) {
			this.loading = true;
			try {
				 await apiCall(`/api/users/${user.id}`, {
					method: 'PUT',
					headers: {
						'Content-Type': 'application/json'
					},
					body: JSON.stringify(user)
				});
				this.users = this.users.map(u => (u.id === user.id ? user : u));
			} catch (e) {
				notificationStore.addNotification('Problem API' , 'error', 'Neviem sa pripojiť na server');
				this.loading = false;
				return;
			}
			notificationStore.addNotification('Uspesne upraveny' , 'success');
			this.loading = false;
		}
	}
});
