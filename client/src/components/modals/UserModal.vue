<script lang="ts">
import { defineComponent } from 'vue';

import ButtonComponent from '../ButtonComponent.vue';

export default defineComponent({
	name: 'PositionModalComponent',
	components: { ButtonComponent },
	props: {
		type: {
			type: String as () => 'soft' | 'delete',
			required: true
		},
		user: {
			type: Object as () => { id: number; name: string },
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
		<div class="bg-white w-80 h-44 flex justify-evenly flex-col p-4">
			<h3 class="m-2 text-xl font-bold">Vymazanie záznamu</h3>

			<p class="m-3">
				Naozaj chcete vymazať záznam o používateľovi
				<b>{{ user?.name }}</b>
				?
			</p>
			<span class="flex justify-evenly gap-2 px-4">
				<ButtonComponent
					type="cancel"
					text="Zrusiť"
					@click="$emit('cancel')" />

				<ButtonComponent
					v-if="type === 'soft'"
					type="danger"
					text="Archívovať "
					@click="$emit('delete', true)" />

				<ButtonComponent
					type="delete"
					text="Zmazať"
					@click="$emit('delete')" />
			</span>
		</div>
	</div>
</template>
