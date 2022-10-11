<script lang="ts">
import { mapActions, mapState } from 'pinia';
import { defineComponent } from 'vue';

import { usePositionsStore } from '../store/positions';
import { useUsersStore } from '../store/users';
import { IUser } from '../types/user';
type State = {
	editing?: boolean;
	creation?: boolean;
	name: string;
	surname: string;
	address?: string;
	birthDate: string;
	startDate: string;
	position: number;
	salary: number;
};
export default defineComponent({
	name: 'EmployeeDetail',
	data: (): State => ({
		editing: false,
		creation: false,
		name: '',
		surname: '',
		address: '',
		birthDate: new Date('2000-09-22T00:00:00').toISOString().slice(0, 10),
		startDate: new Date().toISOString().slice(0, 10),
		position: -1,
		salary: 0
	}),
	computed: {
		...mapState(useUsersStore, ['loading', 'userDetails', 'history']),
		...mapState(usePositionsStore, ['positions']),
		/**
		 * Computed property to check if user can be saved
		 */
		canSave() {
			return (
				this.name.length > 0 &&
				this.surname.length > 0 &&
				new Date(this.birthDate).getTime() < Date.now() &&
				this.position > -1 &&
				this.salary > 0
			);
		}
	},
	async mounted() {
		this.editing = this.$route.meta.edit as boolean;
		this.creation = this.$route.meta.new as boolean;
		await this.fetchPositions();
		// if we are not creating new user, fetch already existing data and history
		if (!this.creation) {
			await this.fetchUserDetails(Number(this.$route.params.id));
			await this.fetchUserHistory(Number(this.$route.params.id));
			this.name = this.userDetails?.name as string;
			this.surname = this.userDetails?.surname as string;
			this.address = this.userDetails?.address as string;
			this.birthDate = (this.userDetails?.birthDate as string).slice(
				0,
				10
			);
			this.position = this.userDetails?.position as number;
			this.salary = this.userDetails?.salary as number;
		}
	},
	methods: {
		...mapActions(useUsersStore, [
			'fetchUserDetails',
			'fetchUserHistory',
			'createUser',
			'updateUser'
		]),
		...mapActions(usePositionsStore, ['fetchPositions']),
		/**
		 * Method to change current user position
		 * @param {Event} e - event from selection HTML element
		 */
		changePosition(e: Event) {
			this.position = Number((e.target as HTMLInputElement).value);
		},
		/**
		 * Method to save current user based on component state
		 */
		save() {
			const user: IUser = {
				id: (this.userDetails?.id as number) ?? 0,
				name: this.name,
				surname: this.surname,
				address: this.address,
				birthDate: this.birthDate,
				position: this.position,
				salary: this.salary,
				startDate: this.startDate
			};
			if (this.editing) {
				user.startDate = this.userDetails?.startDate as string;
				this.updateUser(user);
			} else {
				this.createUser(user);
			}
			this.$router.push('/');
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
			<div v-if="!creation">
				<h1 class="text-xl m-2 font-semibold">Historia</h1>
				<table>
					<thead>
						<tr>
							<th class="border px-4 py-2">Pozicia</th>
							<th class="border px-4 py-2">Od</th>
							<th class="border px-4 py-2">Do</th>
						</tr>
					</thead>
					<tbody>
						<tr v-for="item in history" :key="item.id">
							<td class="border px-4 py-2">
								{{
									positions.find(p => p.id == item.positionId)
										?.name
								}}
							</td>
							<td class="border px-4 py-2">
								{{ item.startDate?.slice(0, 10) }}
							</td>
							<td class="border px-4 py-2">
								{{ item.endDate?.slice(0, 10) }}
							</td>
						</tr>
					</tbody>
				</table>
			</div>

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
					v-model="birthDate"
					class="bg-gray-50 border border-gray-300 text-gray-900 sm:text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full pl-3 p-2.5"
					:disabled="!creation && !editing"
					type="date" />
			</div>
			<div class="w-full my-2">
				<label
					for="startDate"
					class="block mb-2 text-lg font-medium text-gray-900">
					*Datum nastupu:
				</label>
				<input
					id="startDate"
					v-model="startDate"
					class="bg-gray-50 border border-gray-300 text-gray-900 sm:text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full pl-3 p-2.5"
					:disabled="!creation"
					type="date" />
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
					<option value="-1" disabled :selected="position == -1">
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
				:disabled="!canSave"
				class="bg-blue-500 hover:bg-blue-700 text-white py-1 px-2 rounded my-2 w-full disabled:bg-blue-100 disabled:text-blue-300 disabled:hover:cursor-not-allowed"
				@click="save">
				Ulozit
			</button>
		</div>
	</article>
</template>
