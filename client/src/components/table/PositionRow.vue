<script lang="ts">
import { defineComponent, PropType } from 'vue';

import { IPosition } from '@/types';

export default defineComponent({
	name: 'PositionRow',
	props: {
		position: {
			type: Object as PropType<IPosition>,
			// eslint-disable-next-line @typescript-eslint/consistent-type-assertions
			default: (): IPosition => ({} as IPosition)
		},
		placeholder: {
			type: Boolean
		}
	}
});
</script>
<template>
	<tr>
		<th class="border px-4 py-2">
			<p v-if="!placeholder">
				{{ position?.id }}
			</p>

			<span
				v-else
				class="inline-block col-11 bg-gradient-to-r from-gray-300 via-gray-100 to-gray-200 w-5 h-5" />
		</th>
		<td class="border px-4 py-2">
			<p v-if="!placeholder">{{ position?.name }}</p>
			<span
				v-else
				class="inline-block col-11 bg-gradient-to-r from-gray-300 via-gray-100 to-gray-200 w-40 h-5" />
		</td>

		<td class="border px-4 py-2">
			<button
				v-if="!placeholder"
				class="bg-red-500 hover:bg-red-700 text-white py-1 px-2 rounded"
				@click="$emit('delete', position?.id, position?.name)">
				Zmazať
			</button>
			<span
				v-else
				class="inline-block col-11 bg-gradient-to-r from-gray-300 via-gray-100 to-gray-200 w-40 h-5" />
		</td>
	</tr>
</template>
