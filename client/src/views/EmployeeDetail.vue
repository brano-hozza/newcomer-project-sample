<script lang="ts">
import { mapActions, mapState } from 'pinia';
import { defineComponent } from 'vue';

import { usePositionsStore } from '../store/positions';
import { useUsersStore } from '../store/users';
type State = {
	editing?: boolean;
	creation?: boolean;
	name: string;
	surname: string;
	address?: string;
	birthDate: string;
	position: number;
	salary: number;
};
export default defineComponent({
	name: 'EmployeeDetail',
	data: (): State => ({
		name: '',
		surname: '',
		address: '',
		birthDate: '',
		position: -1,
		salary: 0
	}),
	computed: {
		...mapState(useUsersStore, ['loading', 'userDetails']),
		...mapState(usePositionsStore, ['positions'])
	},
	async mounted() {
		this.editing = this.$route.meta.edit as boolean;
		this.creation = this.$route.meta.new as boolean;
		await this.fetchPositions();
		if (!this.creation) {
			await this.fetchUserDetails(Number(this.$route.params.id));
			this.name = this.userDetails?.name as string;
			this.surname = this.userDetails?.surname as string;
			this.address = this.userDetails?.address as string;
			this.birthDate = this.userDetails?.birthDate as string;
			this.position = this.userDetails?.position as number;
			this.salary = this.userDetails?.salary as number;
		}
	},
	methods: {
		...mapActions(useUsersStore, ['fetchUserDetails']),
		...mapActions(usePositionsStore, ['fetchPositions']),
		changeBirthDate(e: Event) {
			this.birthDate = (e.target as HTMLInputElement).value;
		},
		changePosition(e: Event) {
			this.position = Number((e.target as HTMLInputElement).value);
		}
	}
});
</script>
<template>
	<article class="flex justify-center items-center">
		<div v-if="loading">Loading...</div>
		<div v-else class="ml-4 w-1/4">
			<h1 class="text-2xl m-2 font-semibold">
				{{
					creation
						? 'Vytvorenie'
						: editing
						? 'Upravenie'
						: 'Zobrazenie'
				}}
				zamestnanca
			</h1>
			<div class="w-full my-2">
				<label
					for="name"
					class="block mb-2 text-lg font-medium text-gray-900">
					*Meno:
				</label>
				<input
					id="name"
					v-model="name"
					:disabled="!creation && !editing"
					class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
					placeholder="Meno zamestnanca..." />
			</div>
			<div class="w-full my-2">
				<label
					for="surname"
					class="block mb-2 text-lg font-medium text-gray-900">
					*Priezvisko:
				</label>
				<input
					id="surname"
					v-model="surname"
					:disabled="!creation && !editing"
					class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
					placeholder="Priezvisko zamestnanca..." />
			</div>
			<div class="w-full my-2">
				<label
					for="address"
					class="block mb-2 text-lg font-medium text-gray-900">
					Adresa:
				</label>
				<input
					id="address"
					v-model="address"
					:disabled="!creation && !editing"
					class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
					placeholder="Adresa zamestnanca..." />
			</div>
			<div class="w-full my-2">
				<label
					for="birthDate"
					class="block mb-2 text-lg font-medium text-gray-900">
					*Datum narodenia:
				</label>
				<input
					id="birthDate"
					class="bg-gray-50 border border-gray-300 text-gray-900 sm:text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full pl-10 p-2.5"
					:disabled="!creation && !editing"
					type="date"
					@change="changeBirthDate" />
			</div>
			<div class="w-full my-2">
				<label
					for="position"
					class="block mb-2 text-lg font-medium text-gray-900">
					*Pozicia:
				</label>
				<select
					id="position"
					:disabled="!creation && !editing"
					class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
					@change="changePosition">
					<option value="-1" disabled :selected="position == 0">
						Zvolte poziciu
					</option>

					<option
						v-for="_position in positions"
						:key="_position.id"
						:selected="position == _position.id"
						:value="_position.id">
						{{ _position.name }}
					</option>
				</select>
			</div>
			<div class="w-full my-2">
				<label
					for="salary"
					class="block mb-2 text-lg font-medium text-gray-900">
					*Plat:
				</label>
				<input
					id="salary"
					v-model="salary"
					:disabled="!creation && !editing"
					type="number"
					min="0"
					class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
					placeholder="Plat zamestnanca..." />
			</div>
			<i class="text-gray-300">Povinne udaje su oznacene hviezdickou</i>
			<button
				v-if="creation || editing"
				class="bg-blue-500 hover:bg-blue-700 text-white py-1 px-2 rounded my-2 w-full">
				Ulozit
			</button>
		</div>
	</article>
</template>
