import { defineStore } from 'pinia';

import { apiCall } from '@/helpers/api';
import { IPosition } from '@/types/position';

type TPositionState = {
	positions: IPosition[];
	loading: boolean;
	referenceExists: boolean;
	networkError: boolean;
};

export const usePositionsStore = defineStore('positions', {
	state: (): TPositionState => ({
		positions: [],
		loading: false,
		referenceExists: false,
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
		 * Action to fetch positions from the API
		 */
		async fetchPositions() {
			this.loading = true;
			try {
				const response = await apiCall('/api/positions');
				this.positions = await response.json();
			} catch (e) {
				this.setNetworkError();
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
				} else if (resp.status === 409) {
					this.referenceExists = true;
					setTimeout(() => {
						this.referenceExists = false;
					}, 3000);
				}
			} catch (e) {
				this.setNetworkError();
			}
			this.loading = false;
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
				this.setNetworkError();
			}
			this.loading = false;
		}
	}
});
