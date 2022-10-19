<script lang="ts">
import { defineComponent } from 'vue';

export default defineComponent({
	name: 'FormTextInputComponent',
	props: {
		id: {
			type: String,
			default: null
		},
		modelValue: {
			type: String,
			default: ''
		},
		required: {
			type: Boolean,
			default: false
		},
		disabled: {
			type: Boolean,
			default: false
		},
		placeholder: {
			type: String,
			default: ''
		},
		label: {
			type: String,
			default: ''
		},
		error: {
			type: Object as () => {
				message: string;
				check: (val: string) => boolean;
			},
			default: null
		}
	},

	data: () => ({ value: '' }),
	watch: {
		modelValue: function (val) {
			if (val !== this.value) {
				this.value = val;
			}
		},
		value: function (val) {
			if (val !== this.modelValue) {
				this.$emit('update:modelValue', val);
			}
		}
	}
});
</script>
<template>
	<div class="w-full my-2">
		<label :for="id" class="block mb-2 text-lg font-medium text-gray-900">
			<span v-if="required">*</span>{{ label }}:
		</label>
		<input
			:id="id"
			v-model="value"
			:disabled="disabled"
			class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 disabled:text-gray-400"
			:placeholder="placeholder" />
		<p v-if="error?.check(value)" class="text-red-500">
			{{ error?.message }}
		</p>
	</div>
</template>
