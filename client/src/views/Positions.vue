<script lang="ts">
import { mapActions, mapState } from 'pinia';
import { defineComponent } from 'vue';

import ButtonComponent from '@/components/ButtonComponent.vue';
import PositionModal from '@/components/modals/PositionModal.vue';
import PositionRow from '@/components/table/PositionRow.vue';
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
	components: { PositionRow, PositionModal, ButtonComponent },
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
			<span class="flex justify-between p-2 gap-2 w-1/5">
				<ButtonComponent type="reload" @click="fetchPositions" />
				<ButtonComponent
					type="create"
					text="Vytvoriť novú pozíciu"
					@click="newPosition" />
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
