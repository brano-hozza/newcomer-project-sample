<script lang="ts">
import { defineComponent } from 'vue';

import { INotification } from '@/types';

export default defineComponent({
	name: 'NotificationComponent',
	props: {
		notification: {
			type: Object as () => INotification,
			required: true
		}
	}
});
</script>
<template>
	<div
		class="bg-gray-100 w-80 h-44 flex justify-evenly flex-col p-4 border-solid border-2"
		:class="{
			'border-red-400': notification.type === 'error',
			'border-green-400': notification.type === 'success',
			'h-20': !notification.message
		}">
		<h3 class="m-2 text-xl font-bold">{{ notification.title }}</h3>
		<p v-if="notification.message" class="m-4">
			{{ notification.message }}
		</p>
	</div>
</template>
