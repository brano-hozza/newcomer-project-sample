import { defineStore } from 'pinia';

import { Position } from '../types/position';

export const usePositionsStore = defineStore('positions', {
	state: () => ({
		positions: [] as Position[],
		loading: false
	}),
	actions: {
		/**
     * Action to fetch positions from the API
     */
		async fetchPositions() {
			this.loading = true;
			try {
				const response = await fetch('http://localhost:5000/api/positions');
				this.positions = await response.json();
			}
			catch (e) {
				this.positions = [];
			}
			this.loading = false;
		}
	}
});
