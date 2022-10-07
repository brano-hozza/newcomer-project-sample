<script lang="ts">
import { mapState } from 'pinia';
import { defineComponent, PropType } from 'vue';

import { usePositionsStore } from '../store/positions';
import { IUser } from '../types/user';

export default defineComponent({
	name: 'UserRow',
	props: {
		user: {
			type: Object as PropType<IUser>,
			required: true
		},
		old: {
			type: Boolean
		}
	},
	computed: {
		...mapState(usePositionsStore, ['positions'])
	}
});
</script>
<template>
	<tr>
		<th class="border px-4 py-2">
			{{ user.id }}
		</th>
		<td class="border px-4 py-2">
			<router-link
				:to="'/details/' + user.id"
				class="cursor-pointer text-blue-600">
				{{ user.name }} {{ user.surname }}
			</router-link>
		</td>
		<td v-if="!old" class="border px-4 py-2">
			{{ positions[user.position].name }}
		</td>
		<td v-else class="border px-4 py-2">
			{{ new Date(user.resignedDate!).toUTCString() }}
		</td>
		<td v-if="!old" class="border px-4 py-2">
			<button
				class="bg-blue-500 hover:bg-blue-700 text-white py-1 px-2 rounded"
				@click="$emit('edit', user.id)">
				Upravit
			</button>
		</td>
		<td v-else class="border px-4 py-2" />

		<td class="border px-4 py-2">
			<button
				class="bg-red-500 hover:bg-red-700 text-white py-1 px-2 rounded"
				@click="
					$emit('delete', user.id, `${user.name} ${user.surname}`)
				">
				Zmazat
			</button>
		</td>
	</tr>
</template>
