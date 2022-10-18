<script lang="ts">
import { mapActions, mapState } from 'pinia';
import { defineComponent } from 'vue';

import UserRow from '@/components/UserRow.vue';
import { usePositionsStore } from '@/store/positions';
import { useUsersStore } from '@/store/users';
type State = {
	selectedUser?: {
		id: number;
		name: string;
	};
	showDelete: boolean;
};
export default defineComponent({
	name: 'OldEmployees',
	components: { UserRow },
	data: (): State => ({
		showDelete: false
	}),
	computed: {
		...mapState(useUsersStore, ['users', 'loading']),
		...mapState(usePositionsStore, {
			//map positions from object to array for easier access
			positions: state =>
				state.positions.reduce((acc, val) => {
					acc[val.id] = val.name;
					return acc;
				}, [] as string[])
		})
	},
	async mounted() {
		await this.fetchPositions(), await this.fetchOldUsers();
	},
	methods: {
		...mapActions(useUsersStore, ['fetchOldUsers', 'deleteUser']),
		...mapActions(usePositionsStore, ['fetchPositions']),
		/**
		 * Method to open delete confirmation window
		 * @param {number} id - ID of user
		 * @param {string} name - Name of user
		 */
		promptDelete(id: number, name: string) {
			this.selectedUser = { id, name };
			this.showDelete = true;
		},
		/**
		 * Method to cancel deletion
		 */
		cancelDelete() {
			this.showDelete = false;
			delete this.selectedUser;
		},
		/**
		 * Method to confirm deletion
		 */
		async confirmDelete() {
			await this.deleteUser(this.selectedUser?.id as number, false);
			this.showDelete = false;
			delete this.selectedUser;
		}
	}
});
</script>
<template>
	<article>
		<span class="flex justify-between px-4">
			<h1 class="text-2xl m-2 font-semibold">{{ $route.name }}</h1>
			<span class="flex justify-between p-2 gap-4">
				<button
					class="bg-gray-400 hover:bg-gray-500 text-white py-1 px-2 rounded"
					title="Prenačítaj zamestnancov"
					@click="fetchOldUsers">
					<svg
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
			</span>
		</span>
		<table v-if="users.length > 0 || loading" class="table-auto mx-2">
			<thead>
				<tr>
					<th class="px-4 py-2">ID</th>
					<th class="px-4 py-2">Meno</th>
					<th class="px-4 py-2">Dátum prepustenia</th>
					<th />
					<th />
				</tr>
			</thead>
			<tbody v-if="!loading">
				<user-row
					v-for="user in users"
					:key="user.id"
					:user="user"
					old
					@delete="promptDelete" />
			</tbody>
			<tbody v-else>
				<user-row
					v-for="_ in [1, 2, 3, 4, 5]"
					:key="_"
					placeholder
					old />
			</tbody>
		</table>
		<h2 v-else class="text-center text-4xl">
			Nie sú archívovaní žiadny zamestnanci
		</h2>
		<div
			v-if="showDelete"
			class="fixed top-0 left-0 right-0 bottom-0 w-full h-screen z-50 overflow-hidden bg-gray-700 opacity-75 flex flex-col items-center justify-center"
			@click.self="cancelDelete">
			<div class="bg-white w-80 h-44 flex justify-evenly flex-col p-4">
				<h3 class="m-2 text-xl font-bold">Vymazanie záznamu</h3>
				<p class="m-3">
					Naozaj chcete vymazať záznam o používateľovi
					<b>{{ selectedUser?.name }}</b>
					?
				</p>
				<span class="flex justify-evenly mb-3">
					<button
						class="bg-gray-500 hover:bg-gray-700 text-white py-1 rounded my-2 w-1/3"
						@click="cancelDelete">
						Zrusiť
					</button>
					<button
						class="bg-red-700 hover:bg-red-700 text-white py-1 rounded my-2 w-1/3"
						@click="confirmDelete()">
						Potvrdiť
					</button>
				</span>
			</div>
		</div>
	</article>
</template>
<style lang="scss"></style>
