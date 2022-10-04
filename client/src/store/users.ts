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
      try {
        const response = await fetch("http://localhost:3030/users");
        this.users = await response.json();
      } catch (e) {
        this.users = [];
      }
      setTimeout(() => (this.loading = false), 5000);
    },
  },
});
