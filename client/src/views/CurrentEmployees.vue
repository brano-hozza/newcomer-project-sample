<script lang="ts">
import { mapActions, mapState } from 'pinia';
import { defineComponent } from 'vue';

import ButtonComponent from '@/components/ButtonComponent.vue';
import UserRow from '@/components/table/UserRow.vue';
import { usePositionsStore } from '@/store/positions';
import { useUsersStore } from '@/store/users';

import UserModal from '../components/modals/UserModal.vue';
type State = {
	selectedUser?: {
		id: number;
		name: string;
	};
	showDelete: boolean;
};

export default defineComponent({
	name: 'CurrentEmployees',
	components: { UserRow, ButtonComponent, UserModal },
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
		/**
		 * Method to change route to user editor
		 * @param {number} id - ID of user
		 *  */
		editUser(id: number) {
			this.$router.push({ name: 'Details edit', params: { id } });
		},
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
		 * @param {boolean?} [soft] - true if should be soft delete
		 */
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
		<span class="flex justify-between px-4">
			<h1 class="text-2xl m-2 font-semibold">{{ $route.name }}</h1>
			<span class="flex justify-between p-2 gap-2 w-1/5">
				<ButtonComponent type="reload" @click="fetchUsers" />
				<ButtonComponent
					type="create"
					text="Vytvoriť nového zamestnanca"
					@click="$router.push({ name: 'New Employee' })" />
			</span>
		</span>

		<table v-if="users.length > 0 || loading" class="table-auto mx-2">
			<thead>
				<tr>
					<th class="px-4 py-2">ID</th>
					<th class="px-4 py-2">Meno</th>
					<th class="px-4 py-2">Pozícia</th>
					<th />
					<th />
				</tr>
			</thead>
			<tbody v-if="!loading">
				<UserRow
					v-for="user in users"
					:key="user.id"
					:user="user"
					@edit="editUser"
					@delete="promptDelete" />
			</tbody>
			<tbody v-else>
				<UserRow v-for="_ in [1, 2, 3, 4, 5]" :key="_" placeholder />
			</tbody>
		</table>
		<h2 v-else class="text-center text-4xl">
			Nie sú registrovaní žiadny zamestnanci
		</h2>
		<UserModal
			v-if="showDelete"
			:user="selectedUser"
			type="soft"
			@cancel="cancelDelete"
			@delete="confirmDelete" />
		>
	</article>
</template>
<style lang="scss"></style>
