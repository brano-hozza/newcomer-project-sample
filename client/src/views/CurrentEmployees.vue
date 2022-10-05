<script lang="ts">
import { mapActions, mapState } from 'pinia';
import { defineComponent } from 'vue';

import { useUsersStore } from '../store/users';
export default defineComponent({
	name: 'CurrentEmployees',
	data: () => ({}),
	computed: {
		...mapState(useUsersStore, ['users', 'loading'])
	},
	async mounted() {
		await this.fetchUsers();
	},
	methods: {
		...mapActions(useUsersStore, ['fetchUsers'])
	}
});
</script>
<template>
  <div>
    <h1 class="text-xl m-2 font-semibold">
      Current employees
    </h1>
    <table class="table mx-2">
      <thead>
        <tr>
          <th scope="col">
            ID
          </th>
          <th scope="col">
            Name
          </th>
          <th scope="col">
            Surname
          </th>
          <th scope="col">
            Address
          </th>
          <th scope="col">
            Position
          </th>
          <th scope="col">
            Birth Date
          </th>
          <th scope="col">
            Start Date
          </th>
          <th scope="col">
            Salary
          </th>
        </tr>
      </thead>
      <tbody v-if="!loading">
        <tr
          v-for="user in users"
          :key="user.id">
          <th scope="row">
            {{ user.id }}
          </th>
          <td>{{ user.name }}</td>
          <td>{{ user.surname }}</td>
          <td>{{ user.address }}</td>
          <td>{{ user.position }}</td>
          <td>{{ user.birthDate }}</td>
          <td>{{ user.startDate }}</td>
          <td>{{ user.salary }}</td>
        </tr>
      </tbody>
      <tbody v-else>
        <tr
          v-for="_ in [1, 2, 3, 4, 5]"
          :key="_">
          <th><span class="placeholder col-11" /></th>
          <th><span class="placeholder col-11" /></th>
          <th><span class="placeholder col-11" /></th>
          <th><span class="placeholder col-11" /></th>
          <th><span class="placeholder col-11" /></th>
          <th><span class="placeholder col-11" /></th>
          <th><span class="placeholder col-11" /></th>
          <th><span class="placeholder col-11" /></th>
        </tr>
      </tbody>
    </table>
  </div>
</template>
<style lang="scss">
// Source: https://loading.io/css/
.lds-ring {
  display: inline-block;
  position: relative;
  width: 80px;
  height: 80px;
  div {
    box-sizing: border-box;
    display: block;
    position: absolute;
    width: 64px;
    height: 64px;
    margin: 8px;
    border: 8px solid rgb(0, 0, 0);
    border-radius: 50%;
    animation: lds-ring 1.2s cubic-bezier(0.5, 0, 0.5, 1) infinite;
    border-color: rgb(0, 0, 0) transparent transparent transparent;
    &:nth-child(1) {
      animation-delay: -0.45s;
    }
    &:nth-child(2) {
      animation-delay: -0.3s;
    }
    &:nth-child(3) {
      animation-delay: -0.15s;
    }
  }
}
@keyframes lds-ring {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}
</style>
