<script lang="ts">
import { mapState } from 'pinia';
import { defineComponent } from 'vue';
import { RouteRecordNormalized } from 'vue-router';

import ErrorComponent from '@/components/ErrorComponent.vue';
import { usePositionsStore } from '@/store/positions';
import { useUsersStore } from '@/store/users';

export default defineComponent({
	name: 'HomeView',
	components: { ErrorComponent },
	data: () => ({
		routes: [] as RouteRecordNormalized[]
	}),
	computed: {
		...mapState(usePositionsStore, {
			networkErrorPos: store => store.networkError
		}),
		...mapState(useUsersStore, {
			userExists: state => state.userExists,
			networkErrorUser: store => store.networkError
		})
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
			<ErrorComponent
				v-if="networkErrorPos || networkErrorUser"
				:error="{
					title: 'API Problém',
					message:
						'Nepodarilo sa spojiť s API, kontaktujte administrátora.'
				}" />
			<ErrorComponent
				v-if="userExists"
				:error="{
					title: 'Vytvorenie používateľa',
					message: userExists
				}" />
		</main>
	</div>
</template>
<style lang="scss"></style>
