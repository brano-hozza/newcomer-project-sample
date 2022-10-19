<script lang="ts">
import {  mapState } from 'pinia';
import { defineComponent } from 'vue';

import { usePositionsStore } from '@/store/positions';
import { useUsersStore } from '@/store/users';
export default defineComponent({
	name: 'HistoryComponent',
	computed:{
		...mapState(useUsersStore, ['history']),
		...mapState(usePositionsStore, ['positions'])
	}
});
</script>
<template>
    <div>
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
                                ?.name ?? '<zmazané>'
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
</template>
