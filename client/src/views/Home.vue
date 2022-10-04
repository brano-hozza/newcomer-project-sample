<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
  name: "Home",
  state: () => ({}),
});
</script>
<template>
  <div>
    <header>
      <nav>
        <ul>
          <li><router-link to="/positions">Positions</router-link></li>
          <li>
            <router-link to="/">Current Employees</router-link>
          </li>
          <li><router-link to="/old">Old Employees</router-link></li>
        </ul>
      </nav>
    </header>
    <main>
      <router-view></router-view>
    </main>
  </div>
</template>
<style lang="scss"></style>
