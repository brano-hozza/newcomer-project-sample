<script lang="ts">
import { defineComponent, PropType } from 'vue';

import { IPosition } from '../types/position';

export default defineComponent({
	name: 'PositionRow',
	props: {
		position: {
			type: Object as PropType<IPosition>,
			required: true
		}
	}
});
</script>
<template>
	<tr>
		<th class="border px-4 py-2">
			{{ position.id }}
		</th>
		<td class="border px-4 py-2">
			{{ position.name }}
		</td>

		<td class="border px-4 py-2">
			<button
				class="bg-red-500 hover:bg-red-700 text-white py-1 px-2 rounded"
				@click="$emit('delete', position.id, position.name)">
				Zmazat
			</button>
		</td>
	</tr>
</template>
