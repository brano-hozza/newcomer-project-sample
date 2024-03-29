<script lang="ts">
import { mapActions, mapState } from 'pinia';
import { defineComponent } from 'vue';

import ButtonComponent from '@/components/ButtonComponent.vue';
import CalendarComponent from '@/components/form/CalendarComponent.vue';
import FormInput from '@/components/form/FormTextInput.vue';
import HistoryComponent from '@/components/HistoryComponent.vue';
import { usePositionsStore } from '@/store/positions';
import { useUsersStore } from '@/store/users';
import { IUser } from '@/types';
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
	components: {
		CalendarComponent,
		FormInput,
		HistoryComponent,
		ButtonComponent
	},
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
				this.name.length > 3 &&
				this.surname.length > 3 &&
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
			<h1 class="text-3xl my-4 font-semibold">
				{{
					creation
						? 'Vytvorenie'
						: editing
						? 'Upravenie'
						: 'Zobrazenie'
				}}
				zamestnanca
			</h1>
			<HistoryComponent />

			<FormInput
				id="name"
				v-model="name"
				label="Meno"
				placeholder="Meno zamestnance..."
				:disabled="!creation"
				:required="creation"
				:error="{
					message: 'Meno musí mať aspoň 4 znaky',
					check: val => val.length < 4
				}" />

			<FormInput
				id="surname"
				v-model="surname"
				label="Priezvisko"
				placeholder="Priezvisko zamestnance..."
				:disabled="!creation"
				:required="creation"
				:error="{
					message: 'Priezvisko musí mať aspoň 4 znaky',
					check: val => val.length < 4
				}" />

			<FormInput
				id="address"
				v-model="address"
				label="Adresa"
				placeholder="Adresa zamestnance..."
				:disabled="!creation && !editing" />

			<CalendarComponent
				id="birthDate"
				v-model="birthDate"
				label="Dátum narodenia"
				:required="creation"
				:disabled="!creation"
				:checks="[(val:string) => (new Date(val).getTime() > Date.now()) && 'Neplatny datum']" />

			<CalendarComponent
				id="startDate"
				v-model="startDate"
				label="Dátum nástupu"
				:required="creation"
				:disabled="!creation" />
			<div class="w-full my-2">
				<label
					for="position"
					class="block mb-2 text-lg font-medium text-gray-900">
					<span v-if="creation || editing">*</span>Pozícia:
				</label>
				<select
					id="position"
					:disabled="!creation && !editing"
					class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 disabled:text-gray-400"
					@change="changePosition">
					<option value="-1" disabled :selected="position == -1">
						Zvoľte pozíciu
					</option>

					<option
						v-for="_position in positions"
						:key="_position.id"
						:selected="position == _position.id"
						:value="_position.id">
						{{ _position.name }}
					</option>
				</select>

				<p v-if="position < 0" class="text-red-500">Vyberte pozíciu</p>
			</div>
			<FormInput
				id="salary"
				v-model="salary"
				label="Plat"
				placeholder="Plat zamestnance..."
				type="number"
				:required="creation || editing"
				:disabled="!creation && !editing"
				:error="{
					message: 'Je potrebné zadať plat',
					check: val => val < 1
				}" />
			<i v-if="creation || editing" class="text-gray-300">
				Povinné údaje sú označené hviezdičkou
			</i>
			<span class="w-full flex">
				<ButtonComponent
					v-if="creation || editing"
					type="create"
					text="Uložiť"
					@click="save" />
			</span>
		</div>
	</article>
</template>
