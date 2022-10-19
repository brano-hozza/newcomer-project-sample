<script lang="ts">
import { mapState } from 'pinia';
import { defineComponent } from 'vue';
import { RouteRecordNormalized } from 'vue-router';

import Navigation from '@/components/layout/Navigation.vue';
import Notification from '@/components/NotificationComponent.vue';
import { useNotificationStore } from '@/store/notification';

export default defineComponent({
	name: 'HomeView',
	components: { Notification, Navigation },
	data: () => ({
		routes: [] as RouteRecordNormalized[]
	}),
	computed: {
		...mapState(useNotificationStore, ['notifications'])
	},
	mounted() {
		// Map navigation routes to local state property
		this.routes = this.$router.getRoutes().filter(route => route.meta?.nav);
	}
});
</script>
<template>
	<div>
		<header class="border-b-4 border-solid py-2 mb-2">
			<nav class="flex">
				<b class="text-xl mx-4 my-1">EmploYer</b>
				<Navigation :routes="routes" />
			</nav>
		</header>
		<main>
			<router-view />
			<div class="fixed right-0 bottom-0 flex flex-col">
				<Notification
					v-for="notification in notifications"
					:key="notification.id"
					:notification="notification" />
			</div>
		</main>
	</div>
</template>
<style lang="scss"></style>
