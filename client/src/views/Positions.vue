<script lang="ts">
import { mapActions, mapState } from 'pinia';
import { defineComponent } from 'vue';

import PositionModal from '@/components/PositionModal.vue';
import PositionRow from '@/components/PositionRow.vue';
import { usePositionsStore } from '@/store/positions';
import { IPosition } from '@/types';
type State = {
	selectedPosition?: IPosition;
	showDelete: boolean;
	showNewPosition: boolean;
	positionName: string;
};
export default defineComponent({
	name: 'PositionsView',
	components: { PositionRow, PositionModal },
	data: (): State => ({
		showDelete: false,
		showNewPosition: false,
		positionName: ''
	}),
	computed: {
		...mapState(usePositionsStore, ['positions', 'loading'])
	},
	async mounted() {
		await this.fetchPositions();
	},
	methods: {
		...mapActions(usePositionsStore, [
			'fetchPositions',
			'deletePosition',
			'createPosition'
		]),
		/**
		 * Method to open delete confirmation modal
		 * @param {number} id - ID of position
		 */
		promptDelete(id: number) {
			this.selectedPosition = this.positions.find(
				position => position.id === id
			) as IPosition;
			this.showDelete = true;
		},
		/**
		 * Method to cancel deletion
		 */
		cancelDelete() {
			this.showDelete = false;
			delete this.selectedPosition;
		},
		/**
		 * Method to confirm deletion
		 */
		async confirmDelete() {
			await this.deletePosition(this.selectedPosition?.id as number);
			this.showDelete = false;
			delete this.selectedPosition;
		},
		/**
		 * Method to open new position modal
		 */
		newPosition() {
			this.showNewPosition = true;
		},
		/**
		 * Method to save new position based on local state
		 */
		async savePosition(name: string) {
			this.showNewPosition = false;
			await this.createPosition(name);
		},
		/**
		 * Method to cancel creation
		 */
		cancelCreation() {
			this.showNewPosition = false;
		}
	}
});
</script>
<template>
	<div>
		<span class="flex justify-between px-4">
			<h1 class="text-2xl m-2 font-semibold">{{ $route.name }}</h1>
			<span class="flex justify-between p-2 gap-4">
				<button
					class="bg-gray-400 hover:bg-gray-500 text-white py-1 px-2 rounded"
					title="Prenačítaj pozície"
					@click="fetchPositions">
					<svg
						xmlns="http://www.w3.org/2000/svg"
						fill="none"
						viewBox="0 0 24 24"
						stroke-width="1.5"
						stroke="currentColor"
						class="w-5 h-5">
						<path
							stroke-linecap="round"
							stroke-linejoin="round"
							d="M16.023 9.348h4.992v-.001M2.985 19.644v-4.992m0 0h4.992m-4.993 0l3.181 3.183a8.25 8.25 0 0013.803-3.7M4.031 9.865a8.25 8.25 0 0113.803-3.7l3.181 3.182m0-4.991v4.99" />
					</svg>
				</button>
				<button
					class="bg-blue-500 hover:bg-blue-700 text-white py-1 px-2 rounded"
					@click="newPosition">
					Vytvoriť novú pozíciu
				</button>
			</span>
		</span>
		<table v-if="positions.length > 0 || loading" class="table mx-2">
			<thead>
				<tr>
					<th class="px-4 py-2">ID</th>
					<th class="px-4 py-2">Názov pozície</th>
					<th class="px-4 py-2" />
				</tr>
			</thead>
			<tbody v-if="!loading">
				<PositionRow
					v-for="position in positions"
					:key="position.id"
					:position="position"
					@delete="promptDelete" />
			</tbody>
			<tbody v-else>
				<PositionRow
					v-for="_ in [1, 2, 3, 4, 5]"
					:key="_"
					placeholder />
			</tbody>
		</table>

		<h2 v-else class="text-center text-4xl">Neexistujú žiadne pozície</h2>

		<PositionModal
			v-if="showNewPosition"
			type="create"
			@cancel="cancelCreation"
			@save="savePosition" />
		<PositionModal
			v-if="showDelete"
			type="delete"
			@cancel="cancelDelete"
			@delete="confirmDelete" />
	</div>
</template>
<style lang="scss"></style>
