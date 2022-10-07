<script lang="ts">
import { mapActions, mapState } from 'pinia';
import { defineComponent } from 'vue';

import UserRow from '../components/UserRow.vue';
import { usePositionsStore } from '../store/positions';
import { useUsersStore } from '../store/users';
type State = {
	selectedUser?: {
		id: number;
		name: string;
	};
	showDelete: boolean;
};

export default defineComponent({
	name: 'CurrentEmployees',
	components: { UserRow },
	data: (): State => ({
		showDelete: false
	}),
	computed: {
		...mapState(useUsersStore, ['users', 'loading'])
	},
	async mounted() {
		await this.fetchPositions(), await this.fetchUsers();
	},
	methods: {
		...mapActions(useUsersStore, ['fetchUsers', 'deleteUser']),
		...mapActions(usePositionsStore, ['fetchPositions']),
		editUser(id: number) {
			this.$router.push({ name: 'Details edit', params: { id } });
		},
		promptDelete(id: number, name: string) {
			this.selectedUser = { id, name };
			this.showDelete = true;
		},
		cancelDelete() {
			this.showDelete = false;
			delete this.selectedUser;
		},
		async confirmDelete(soft = false) {
			await this.deleteUser(this.selectedUser?.id as number, soft);
			this.showDelete = false;
			delete this.selectedUser;
		}
	}
});
</script>
<template>
	<article>
		<h1 class="text-xl m-2 font-semibold">{{ $route.name }}</h1>
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
				<user-row
					v-for="user in users"
					:key="user.id"
					:user="user"
					@edit="editUser"
					@delete="promptDelete" />
			</tbody>
			<tbody v-else>
				<tr v-for="_ in [1, 2, 3, 4, 5]" :key="_">
					<th><span class="placeholder col-11" /></th>
					<th><span class="placeholder col-11" /></th>
					<th><span class="placeholder col-11" /></th>
					<th><span class="placeholder col-11" /></th>
					<th><span class="placeholder col-11" /></th>
				</tr>
			</tbody>
		</table>
		<div
			v-if="showDelete"
			class="fixed top-0 left-0 right-0 bottom-0 w-full h-screen z-50 overflow-hidden bg-gray-700 opacity-75 flex flex-col items-center justify-center"
			@click.self="cancelDelete">
			<div class="bg-white w-80 h-44 flex justify-evenly flex-col p-4">
				<h3 class="m-2 text-xl font-bold">Vymazanie zaznamu</h3>
				<p class="m-4">
					Naozaj chcete vymazat zaznam o pouzivatelovi
					<b>{{ selectedUser?.name }}</b>
					?
				</p>
				<span class="flex justify-evenly mb-3">
					<button
						class="bg-gray-500 hover:bg-gray-700 text-white py-1 px-4 rounded"
						@click="cancelDelete">
						Zrusit
					</button>
					<button
						class="bg-yellow-500 hover:bg-yellow-700 text-black py-1 px-4 rounded"
						@click="confirmDelete(true)">
						Archivovat
					</button>
					<button
						class="bg-red-700 hover:bg-red-700 text-white py-1 px-4 rounded"
						@click="confirmDelete()">
						Potvrdit
					</button>
				</span>
			</div>
		</div>
	</article>
</template>
<style lang="scss"></style>
