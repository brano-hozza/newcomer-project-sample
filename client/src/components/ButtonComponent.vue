<script lang="ts">
import { defineComponent } from 'vue';
export default defineComponent({
	name: 'ButtonComponent',
	props: {
		text: {
			type: String,
			default: null
		},
		type: {
			// eslint-disable-next-line @typescript-eslint/indent
			type: String as () => // eslint-disable-next-line @typescript-eslint/indent
			| 'confirm' // eslint-disable-next-line @typescript-eslint/indent
				| 'delete' // eslint-disable-next-line @typescript-eslint/indent
				| 'cancel' // eslint-disable-next-line @typescript-eslint/indent
				| 'create' // eslint-disable-next-line @typescript-eslint/indent
				| 'danger' // eslint-disable-next-line @typescript-eslint/indent
				| 'reload',
			default: 'create'
		},
		disabled: {
			type: Boolean,
			default: false
		}
	}
});
</script>
<template>
	<button
		class="text-white py-1 px-2 rounded my-2 h-10 flex justify-center items-center"
		:class="{
			'opacity-50 cursor-not-allowed': disabled,
			'bg-red-500 hover:bg-red-700': type === 'delete',
			'bg-blue-500 hover:bg-blue-700': type === 'create',
			'bg-gray-500 hover:bg-gray-700':
				type === 'cancel' || type === 'reload',
			'bg-green-500 hover:bg-green-700': type === 'confirm',
			'bg-yellow-500 hover:bg-yellow-700 text-black': type === 'danger',
			'flex-1': type !== 'reload',
			'w-10': type === 'reload'
		}">
		<template v-if="type !== 'reload'">
			{{ text }}
		</template>
		<svg
			v-else
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
</template>
