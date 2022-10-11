<script lang="ts">
import { mapState } from 'pinia';
import { defineComponent, PropType } from 'vue';

import { usePositionsStore } from '../store/positions';
import { IUser } from '../types/user';

type ChangedPositions = {
	[key: number]: string;
};
export default defineComponent({
	name: 'UserRow',
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
			<button
				v-if="!placeholder"
				class="bg-blue-500 hover:bg-blue-700 text-white py-1 px-2 rounded"
				@click="$emit('edit', user?.id)">
				Upraviť
			</button>

			<span
				v-else
				class="inline-block col-11 bg-gradient-to-r from-gray-300 via-gray-100 to-gray-200 w-40 h-5" />
		</td>
		<td v-else class="border px-4 py-2" />

		<td class="border px-4 py-2">
			<button
				v-if="!placeholder"
				class="bg-red-500 hover:bg-red-700 text-white py-1 px-2 rounded"
				@click="
					$emit('delete', user?.id, `${user?.name} ${user?.surname}`)
				">
				Zmazať
			</button>

			<span
				v-else
				class="inline-block col-11 bg-gradient-to-r from-gray-300 via-gray-100 to-gray-200 w-40 h-5" />
		</td>
	</tr>
</template>
