<script lang="ts">
import { defineComponent, PropType } from 'vue';

type Condition = (val: string) => string | boolean;

export default defineComponent({
	name: 'CalendarComponent',
	props: {
		modelValue: {
			type: String,
			required: true
		},
		checks: {
			type: Array as PropType<Condition[]>,
			default: () => []
		},
		disabled: {
			type: Boolean
		},
		required: {
			type: Boolean
		},
		label: {
			type: String,
			default: null
		}
	},
	data: () => ({
		date: '',
		errors: ''
	}),
	watch: {
		modelValue: function (val) {
			this.date = val;
		},
		date: function (value) {
			this.errors = this.checks
				.map(check => check(value) || undefined)
				.join('. ');
			if (value !== this.modelValue) {
				this.$emit('update:modelValue', value);
			}
		}
	},
	mounted() {
		this.date = this.modelValue;
	}
});
</script>
<template>
	<div class="w-full my-2">
		<label
			for="startDate"
			class="block mb-2 text-lg font-medium text-gray-900">
			<span v-if="required">*</span>{{ label }}:
		</label>
		<input
			id="birthDate"
			v-model="date"
			class="bg-gray-50 border border-gray-300 text-gray-900 sm:text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full pl-3 p-2.5 disabled:text-gray-400"
			:class="{ 'border-red-300': errors.length > 0 }"
			:disabled="disabled"
			type="date" />
		<p v-if="errors.length > 0" class="text-red-500">{{ errors }}</p>
	</div>
</template>
