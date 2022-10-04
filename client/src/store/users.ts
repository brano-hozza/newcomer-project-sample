import { defineStore } from "pinia";
import type { User } from "../types/user";

export const useUsersStore = defineStore("users", {
  state: () => ({
    users: [] as User[],
    loading: false,
  }),
  actions: {
    /**
     * Action to fetch users from the API
     */
    async fetchUsers() {
      this.loading = true;
      const response = await fetch("http://localhost:3000/users");
      this.users = await response.json();
      this.loading = false;
    },
  },
});
