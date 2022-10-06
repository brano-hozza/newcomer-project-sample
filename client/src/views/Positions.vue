<script lang="ts">
import { mapActions, mapState } from 'pinia';
import { defineComponent } from 'vue';

import { usePositionsStore } from '../store/positions';
export default defineComponent({
	name: 'PositionsView',
	data: () => ({}),
	computed: {
		...mapState(usePositionsStore, ['positions', 'loading'])
	},
	async mounted() {
		await this.fetchPositions();
	},
	methods: {
		...mapActions(usePositionsStore, ['fetchPositions'])
	}
});
</script>
<template>
	<div>
		<h1 class="text-xl m-2 font-semibold">{{ $route.name }}</h1>
		<div v-if="loading" class="text-center">
			<div class="lds-ring">
				<div />
				<div />
				<div />
				<div />
			</div>
		</div>
		<div v-else>
			<table class="table mx-2">
				<thead>
					<tr>
						<th scope="col">ID</th>
						<th scope="col">Name</th>
					</tr>
				</thead>
				<tbody>
					<tr v-for="position in positions" :key="position.id">
						<th scope="row">
							{{ position.id }}
						</th>
						<td>{{ position.name }}</td>
					</tr>
				</tbody>
			</table>
		</div>
	</div>
</template>
<style lang="scss"></style>
