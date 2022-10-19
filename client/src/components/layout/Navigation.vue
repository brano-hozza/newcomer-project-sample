<script lang="ts">
import { defineComponent, PropType } from 'vue';
import { RouteRecordNormalized } from 'vue-router';
export default defineComponent({
	name: 'NavigationComponent',
	props: {
		routes: {
			type: Array as PropType<RouteRecordNormalized[]>,
			required: true
		}
	},
	data: () => ({ lRoutes: [] as RouteRecordNormalized[] }),
	watch: {
		routes: function (val) {
			this.lRoutes = val;
		}
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
	<ul class="flex list-none">
		<li
			v-for="route in lRoutes"
			:key="route.path"
			class="mx-2 border-2 border-neutral-600 px-2 cursor-pointer"
			:class="{ 'border-red-500': isActiveRoute(route.path) }"
			@click="$router.push(route.path)">
			{{ route.name }}
		</li>
	</ul>
</template>
