import { defineStore } from "pinia";
import type { User } from "../types/user";

export const useUsersStore = defineStore("users", {
  state: () => ({
    users: [] as User[],
    loading: false,
  }),
});
