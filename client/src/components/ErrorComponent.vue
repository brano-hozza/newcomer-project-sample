<script lang="ts">
import { defineComponent } from 'vue';
type Error = {
	title: string;
	message: string;
};
export default defineComponent({
	name: 'ErrorComponent',
	props: {
		error: {
			type: Object as () => Error,
			required: true
		}
	}
});
</script>
<template>
	<div
		class="fixed right-0 bottom-0 bg-gray-100 w-80 h-44 flex justify-evenly flex-col p-4 border-red-400 border-solid border-2">
		<h3 class="m-2 text-xl font-bold">{{ error.title }}</h3>
		<p class="m-4">{{ error.message }}</p>
	</div>
</template>
