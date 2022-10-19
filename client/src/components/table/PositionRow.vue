<script lang="ts">
import { defineComponent, PropType } from 'vue';

import { IPosition } from '@/types';

import ButtonComponent from '../ButtonComponent.vue';

export default defineComponent({
	name: 'PositionRow',
	components: { ButtonComponent },
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
		<th class="border px-4 py-1">
			<p v-if="!placeholder">
				{{ position?.id }}
			</p>

			<span
				v-else
				class="inline-block col-11 bg-gradient-to-r from-gray-300 via-gray-100 to-gray-200 w-5 h-5" />
		</th>
		<td class="border px-4 py-1">
			<p v-if="!placeholder">{{ position?.name }}</p>
			<span
				v-else
				class="inline-block col-11 bg-gradient-to-r from-gray-300 via-gray-100 to-gray-200 w-40 h-5" />
		</td>

		<td class="border px-4 py-1">
			<ButtonComponent
				v-if="!placeholder"
				text="Zmazať"
				type="delete"
				@click="$emit('delete', position?.id, position?.name)" />
			<span
				v-else
				class="inline-block col-11 bg-gradient-to-r from-gray-300 via-gray-100 to-gray-200 w-40 h-5" />
		</td>
	</tr>
</template>
