<script lang="ts">
import { mapState } from 'pinia';
import { defineComponent } from 'vue';
import { RouteRecordNormalized } from 'vue-router';

import Notification from '@/components/Notification.vue';
import { useNotificationStore } from '@/store/notification';

export default defineComponent({
	name: 'HomeView',
	components: { Notification },
	data: () => ({
		routes: [] as RouteRecordNormalized[]
	}),
	computed: {
		...mapState(useNotificationStore, ['notifications'])
	},
	mounted() {
		// Map navigation routes to local state property
		this.routes = this.$router.getRoutes().filter(route => route.meta?.nav);
	},
	methods: {
		/**
		 * Method to check if a route is active
		 * @param path - The path to the route
		 */
		isActiveRoute(path: string) {
			return this.$route.path === path;
		}
	}
});
</script>
<template>
	<div>
		<header class="border-b-4 border-solid py-2 mb-2">
			<nav class="flex">
				<b class="text-xl mx-4 my-1">EmploYer</b>
				<ul class="flex list-none">
					<li
						v-for="route in routes"
						:key="route.path"
						class="mx-2 border-2 border-neutral-600 px-2 cursor-pointer"
						:class="{ 'border-red-500': isActiveRoute(route.path) }"
						@click="$router.push(route.path)">
						{{ route.name }}
					</li>
				</ul>
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
