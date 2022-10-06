<script lang="ts">
import { mapActions, mapState } from 'pinia';
import { defineComponent } from 'vue';

import { usePositionsStore } from '../store/positions';
import { useUsersStore } from '../store/users';
export default defineComponent({
	name: 'CurrentEmployees',
	data: () => ({}),
	computed: {
		...mapState(useUsersStore, ['users', 'loading']),
		...mapState(usePositionsStore, {
			positions: state =>
				state.positions.reduce((acc, val) => {
					acc[val.id] = val.name;
					return acc;
				}, [] as string[])
		})
	},
	async mounted() {
		await this.fetchPositions(), await this.fetchUsers();
	},
	methods: {
		...mapActions(useUsersStore, ['fetchUsers']),
		...mapActions(usePositionsStore, ['fetchPositions'])
	}
});
</script>
<template>
	<article>
		<h1 class="text-xl m-2 font-semibold">Current employees</h1>
		<table class="table-auto mx-2">
			<thead>
				<tr>
					<th class="px-4 py-2">ID</th>
					<th class="px-4 py-2">Meno</th>
					<th class="px-4 py-2">Position</th>
					<th />
					<th />
				</tr>
			</thead>
			<tbody v-if="!loading">
				<tr v-for="user in users" :key="user.id">
					<th class="border px-4 py-2">
						{{ user.id }}
					</th>
					<td class="border px-4 py-2">
						<router-link :to="'/details/' + user.id">
							{{ user.name }} {{ user.surname }}
						</router-link>
					</td>
					<td class="border px-4 py-2">
						{{ positions[user.position] }}
					</td>
					<td class="border px-4 py-2">
						<button
							class="bg-blue-500 hover:bg-blue-700 text-white py-1 px-2 rounded"
							@click="
								$router.push({
									name: 'Details',
									params: { id: user.id, edit: 1 }
								})
							">
							Upravit
						</button>
					</td>
					<td class="border px-4 py-2">
						<button
							class="bg-red-500 hover:bg-red-700 text-white py-1 px-2 rounded">
							Zmazat
						</button>
					</td>
				</tr>
			</tbody>
			<tbody v-else>
				<tr v-for="_ in [1, 2, 3, 4, 5]" :key="_">
					<th><span class="placeholder col-11" /></th>
					<th><span class="placeholder col-11" /></th>
					<th><span class="placeholder col-11" /></th>
					<th><span class="placeholder col-11" /></th>
					<th><span class="placeholder col-11" /></th>
					<th><span class="placeholder col-11" /></th>
					<th><span class="placeholder col-11" /></th>
					<th><span class="placeholder col-11" /></th>
				</tr>
			</tbody>
		</table>
	</article>
</template>
<style lang="scss">
// Source: https://loading.io/css/
.lds-ring {
	display: inline-block;
	position: relative;
	width: 80px;
	height: 80px;
	div {
		box-sizing: border-box;
		display: block;
		position: absolute;
		width: 64px;
		height: 64px;
		margin: 8px;
		border: 8px solid rgb(0, 0, 0);
		border-radius: 50%;
		animation: lds-ring 1.2s cubic-bezier(0.5, 0, 0.5, 1) infinite;
		border-color: rgb(0, 0, 0) transparent transparent transparent;
		&:nth-child(1) {
			animation-delay: -0.45s;
		}
		&:nth-child(2) {
			animation-delay: -0.3s;
		}
		&:nth-child(3) {
			animation-delay: -0.15s;
		}
	}
}
@keyframes lds-ring {
	0% {
		transform: rotate(0deg);
	}
	100% {
		transform: rotate(360deg);
	}
}
</style>
