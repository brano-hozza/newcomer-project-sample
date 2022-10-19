import { defineStore } from 'pinia';

import { apiCall } from '@/helpers/api';
import { IPosition } from '@/types';

import { useNotificationStore } from './notification';

type TPositionState = {
	positions: IPosition[];
	loading: boolean;
};

const notificationStore = useNotificationStore();

export const usePositionsStore = defineStore('positions', {
	state: (): TPositionState => ({
		positions: [],
		loading: false
	}),
	actions: {
		/**
		 * Action to fetch positions from the API
		 */
		async fetchPositions() {
			this.loading = true;
			try {
				const response = await apiCall('/api/positions');
				this.positions = await response.json();
			} catch (e) {
				notificationStore.addNotification('Problem API' , 'error', 'Neviem sa pripojiť na server');
				this.positions = [];
			}
			this.loading = false;
		},
		/**
		 * Action to delete a position
		 * @param id - ID of the position to delete
		 */
		async deletePosition(id: number) {
			this.loading = true;
			try {
				const resp = await apiCall(`/api/positions/${id}`, {
					method: 'DELETE'
				});
				if (resp.status === 204) {
					this.positions = this.positions.filter(
						position => position.id !== id
					);
				} else if (resp.status === 400) {
					notificationStore.addNotification('Nemozem vymazat' , 'error', 'Tato pozicia sa aktualne pouziva');
					this.loading = false;
					return;
				}
			} catch (e) {
				notificationStore.addNotification('Problem API' , 'error', 'Neviem sa pripojiť na server');
				this.loading = false;
				return;
			}
			this.loading = false;
			notificationStore.addNotification('Pozicia vymazana' , 'success');
		},
		/**
		 * Action to create a new position
		 * @param name - name of the position
		 */
		async createPosition(name: string) {
			this.loading = true;
			try {
				const resp = await apiCall('/api/positions', {
					method: 'POST',
					headers: {
						'Content-Type': 'application/json'
					},
					body: JSON.stringify({
						name
					})
				});
				const position = await resp.json();
				this.positions.push(position);
			} catch (e) {
				notificationStore.addNotification('Problem API' , 'error', 'Neviem sa pripojiť na server');
				this.loading = false;
				return;
			}
			this.loading = false;
			notificationStore.addNotification('Pozicia vytvorena' , 'success');
		}
	}
});
