<script lang="ts">
import { mapState } from 'pinia';
import { defineComponent, PropType } from 'vue';

import ButtonComponent from '@/components/ButtonComponent.vue';
import { usePositionsStore } from '@/store/positions';
import { IUser } from '@/types';

type ChangedPositions = {
	[key: number]: string;
};
export default defineComponent({
	name: 'UserRow',
	components: { ButtonComponent },
	props: {
		user: {
			type: Object as PropType<IUser>,
			// eslint-disable-next-line @typescript-eslint/consistent-type-assertions
			default: (): IUser => ({} as IUser)
		},
		placeholder: {
			type: Boolean
		},
		old: {
			type: Boolean
		}
	},
	computed: {
		...mapState(usePositionsStore, {
			positions: state =>
				state.positions.reduce(
					(acc: ChangedPositions, val): ChangedPositions => {
						acc[val.id] = val.name;
						return acc;
					},
					{}
				)
		})
	}
});
</script>
<template>
	<tr>
		<th class="border px-4 py-2">
			<p v-if="!placeholder" class="w-5">{{ user?.id }}</p>
			<span
				v-else
				class="inline-block col-11 bg-gradient-to-r from-gray-300 via-gray-100 to-gray-200 w-5 h-5" />
		</th>
		<td class="border px-4 py-2">
			<router-link
				v-if="!placeholder"
				:to="'/details/' + user?.id"
				class="cursor-pointer text-blue-600">
				{{ user?.name }} {{ user?.surname }}
			</router-link>
			<span
				v-else
				class="inline-block col-11 bg-gradient-to-r from-gray-300 via-gray-100 to-gray-200 w-40 h-5" />
		</td>
		<td v-if="!old" class="border px-4 py-2">
			<p v-if="!placeholder">{{ positions[user?.position] }}</p>
			<span
				v-else
				class="inline-block col-11 bg-gradient-to-r from-gray-300 via-gray-100 to-gray-200 w-40 h-5" />
		</td>
		<td v-else class="border px-4 py-2">
			<p v-if="!placeholder">
				{{ new Date(user?.resignedDate!).toUTCString() }}
			</p>
			<span
				v-else
				class="inline-block col-11 bg-gradient-to-r from-gray-300 via-gray-100 to-gray-200 w-40 h-5" />
		</td>
		<td v-if="!old" class="border px-4 py-2">
			<ButtonComponent
				v-if="!placeholder"
				text="Upraviť"
				type="create"
				@click="$emit('edit', user?.id)" />

			<span
				v-else
				class="inline-block col-11 bg-gradient-to-r from-gray-300 via-gray-100 to-gray-200 w-40 h-5" />
		</td>
		<td v-else class="border px-4 py-2" />

		<td class="border px-4 py-2">
			<ButtonComponent
				v-if="!placeholder"
				text="Zmazať"
				type="delete"
				@click="
					$emit('delete', user?.id, `${user?.name} ${user?.surname}`)
				" />

			<span
				v-else
				class="inline-block col-11 bg-gradient-to-r from-gray-300 via-gray-100 to-gray-200 w-40 h-5" />
		</td>
	</tr>
</template>
