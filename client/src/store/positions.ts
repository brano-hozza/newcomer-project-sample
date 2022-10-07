import { defineStore } from 'pinia';

import { IPosition } from '../types/position';

type TPositionState = {
	positions: IPosition[];
	loading: boolean;
};

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
				const response = await fetch(
					'http://localhost:5000/api/positions'
				);
				this.positions = await response.json();
			} catch (e) {
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
				await fetch(`http://localhost:5000/api/positions/${id}`, {
					method: 'DELETE'
				});
				this.positions = this.positions.filter(
					position => position.id !== id
				);
			} catch (e) {
				// eslint-disable-next-line no-console
				console.error(e);
			}
			this.loading = false;
		}
	}
});
