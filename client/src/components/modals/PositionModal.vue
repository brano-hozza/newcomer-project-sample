<script lang="ts">
import { defineComponent } from 'vue';

import { IPosition } from '@/types';

import ButtonComponent from '../ButtonComponent.vue';

export default defineComponent({
	name: 'PositionModalComponent',
	components: { ButtonComponent },
	props: {
		type: {
			type: String as () => 'create' | 'delete',
			required: true
		},
		position: {
			type: Object as () => IPosition,
			default: null
		}
	},
	data: () => ({
		positionName: ''
	}),
	computed: {
		/**
		 * Computed property to check if position name is saveable
		 */
		canSave() {
			return this.positionName.length > 0;
		}
	}
});
</script>
<template>
	<div
		class="fixed top-0 left-0 right-0 bottom-0 w-full h-screen z-50 overflow-hidden bg-gray-700 opacity-75 flex flex-col items-center justify-center"
		@click.self="$emit('cancel')">
		<div class="bg-white w-1/5 flex justify-evenly flex-col py-2">
			<h3 class="m-2 pl-2 text-2xl font-bold">
				{{ type === 'create' ? 'Nová pozícia' : 'Zmazat poziciu' }}
			</h3>
			<div v-if="type === 'create'" class="w-9/10 m-2 px-2">
				<label
					for="name"
					class="block mb-2 text-lg font-medium text-gray-900">
					Názov pozície:
				</label>
				<input
					id="name"
					v-model="positionName"
					class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
					placeholder="Názov pozície..." />
			</div>

			<p v-if="type === 'delete'" class="m-3">
				Naozaj chcete vymazať pozíciu
				<b>{{ position?.name }}</b>
				?
			</p>
			<span class="flex justify-evenly gap-2 px-4">
				<ButtonComponent
					type="cancel"
					text="Zrusiť"
					@click="$emit('cancel')" />

				<ButtonComponent
					v-if="type === 'create'"
					type="create"
					text="Uložiť"
					@click="$emit('save', positionName)" />

				<ButtonComponent
					v-if="type === 'delete'"
					type="delete"
					text="Zmazať"
					@click="$emit('delete')" />
			</span>
		</div>
	</div>
</template>
