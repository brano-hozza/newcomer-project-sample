<script lang="ts">
import { defineComponent } from "vue";
import { RouteRecordNormalized } from "vue-router";

export default defineComponent({
  name: "Home",
  data: () => ({
    routes: [] as RouteRecordNormalized[],
  }),
  mounted() {
    this.routes = this.$router.getRoutes().filter((route) => route.meta?.nav);
  },
  computed: {},
  methods: {
    goTo(path: string) {
      this.$router.push(path);
    },
    isCurrentPath(path: string) {
      return this.$route.path === path;
    },
  },
});
</script>
<template>
  <div>
    <header class="border-b-4 border-solid py-2 mb-2">
      <nav class="flex">
        <b class="text-xl mx-4 my-1">EmploYer</b>
        <ul class="flex list-none">
          <li
            v-for="route in routes"
            :key="route.path"
            class="mx-2 border-2 border-neutral-600 px-2"
            :class="{ 'border-red-500': isCurrentPath(route.path) }"
            @click="goTo(route.path)"
          >
            {{ route.name }}
          </li>
        </ul>
      </nav>
    </header>
    <main>
      <router-view />
    </main>
  </div>
</template>
<style lang="scss"></style>
