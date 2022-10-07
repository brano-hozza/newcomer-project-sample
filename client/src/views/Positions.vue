<script lang="ts">
import { mapActions, mapState } from 'pinia';
import { defineComponent } from 'vue';

import PositionRow from '../components/PositionRow.vue';
import { usePositionsStore } from '../store/positions';
import { IPosition } from '../types/position';
type State = {
	selectedPosition?: IPosition;
	showDelete: boolean;
	showNewPosition: boolean;
	positionName: string;
};
export default defineComponent({
	name: 'PositionsView',
	components: { PositionRow },
	data: (): State => ({
		showDelete: false,
		showNewPosition: false,
		positionName: ''
	}),
	computed: {
		...mapState(usePositionsStore, [
			'positions',
			'loading',
			'referenceExists'
		])
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
		promptDelete(id: number) {
			this.selectedPosition = this.positions.find(
				position => position.id === id
			) as IPosition;
			this.showDelete = true;
		},
		cancelDelete() {
			this.showDelete = false;
			delete this.selectedPosition;
		},
		async confirmDelete() {
			await this.deletePosition(this.selectedPosition?.id as number);
			this.showDelete = false;
			delete this.selectedPosition;
		},
		newPosition() {
			this.showNewPosition = true;
		},
		async savePosition() {
			this.showNewPosition = false;
			await this.createPosition(this.positionName);
			this.positionName = '';
		},
		cancelCreation() {
			this.showNewPosition = false;
			this.positionName = '';
		}
	}
});
</script>
<template>
	<div>
		<span class="flex justify-between px-4">
			<h1 class="text-xl m-2 font-semibold">{{ $route.name }}</h1>
			<button
				class="bg-blue-500 hover:bg-blue-700 text-white py-1 px-2 rounded"
				@click="newPosition">
				Vytvorit novu poziciu
			</button>
		</span>
		<table class="table mx-2">
			<thead>
				<tr>
					<th class="px-4 py-2">ID</th>
					<th class="px-4 py-2">Name</th>
					<th class="px-4 py-2">Actions</th>
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
				<tr v-for="_ in [1, 2, 3, 4, 5]" :key="_">
					<th><span class="placeholder col-11" /></th>
					<th><span class="placeholder col-11" /></th>
					<th><span class="placeholder col-11" /></th>
				</tr>
			</tbody>
		</table>

		<div
			v-if="showDelete"
			class="fixed top-0 left-0 right-0 bottom-0 w-full h-screen z-50 overflow-hidden bg-gray-700 opacity-75 flex flex-col items-center justify-center"
			@click.self="cancelDelete">
			<div class="bg-white w-80 h-44 flex justify-evenly flex-col p-4">
				<h3 class="m-2 text-xl font-bold">Vymazanie zaznamu</h3>
				<p class="m-4">
					Naozaj chcete vymazat poziciu
					<b>{{ selectedPosition?.name }}</b>
					?
				</p>
				<span class="flex justify-evenly mb-3">
					<button
						class="bg-gray-500 hover:bg-gray-700 text-white py-1 px-4 rounded"
						@click="cancelDelete">
						Zrusit
					</button>
					<button
						class="bg-red-700 hover:bg-red-700 text-white py-1 px-4 rounded"
						@click="confirmDelete()">
						Potvrdit
					</button>
				</span>
			</div>
		</div>

		<div
			v-if="showNewPosition"
			class="fixed top-0 left-0 right-0 bottom-0 w-full h-screen z-50 overflow-hidden bg-gray-700 opacity-75 flex flex-col items-center justify-center"
			@click.self="cancelCreation">
			<div class="bg-white w-80 h-44 flex justify-evenly flex-col p-4">
				<h3 class="m-2 text-xl font-bold">Nova pozicia</h3>
				<div class="w-full my-2">
					<label
						for="name"
						class="block mb-2 text-lg font-medium text-gray-900">
						Nazov pozicie:
					</label>
					<input
						id="name"
						v-model="positionName"
						class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
						placeholder="Nazov pozicie..." />
				</div>
				<span class="flex justify-evenly mb-3">
					<button
						class="bg-gray-500 hover:bg-gray-700 text-white py-1 px-4 rounded"
						@click="cancelCreation">
						Zrusit
					</button>
					<button
						class="bg-red-700 hover:bg-red-700 text-white py-1 px-4 rounded"
						@click="savePosition()">
						Ulozit
					</button>
				</span>
			</div>
		</div>
		<div
			v-if="referenceExists"
			class="fixed right-0 bottom-0 bg-gray-100 w-80 h-44 flex justify-evenly flex-col p-4 border-red-400 border-solid border-2">
			<h3 class="m-2 text-xl font-bold">Vymazanie zaznamu zlyhalo</h3>
			<p class="m-4">
				Nie je mozne vymazat poziciu
				<b>{{ selectedPosition?.name }}</b>
				, pretoze je priradena k nejakej osobe.
			</p>
		</div>
	</div>
</template>
<style lang="scss"></style>
