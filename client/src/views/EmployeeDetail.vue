<script lang="ts">
import { mapActions, mapState } from 'pinia';
import { defineComponent } from 'vue';

import { useUsersStore } from '../store/users';

export default defineComponent({
	name: 'EmployeeDetail',
	data: () => ({
		editing: false
	}),
	computed: {
		...mapState(useUsersStore, ['loading', 'userDetails'])
	},
	async mounted() {
		this.editing = this.$route.meta.edit as boolean;
		await this.fetchUserDetails(Number(this.$route.params.id));
	},
	methods: {
		...mapActions(useUsersStore, ['fetchUserDetails'])
	}
});
</script>
<template>
	<article>
		<div v-if="loading">Loading...</div>
		<div v-else>{{ userDetails?.name }} {{ editing ? 'Editing' : '' }}</div>
	</article>
</template>
