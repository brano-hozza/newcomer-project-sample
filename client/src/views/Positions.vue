<script lang="ts">
import { mapActions, mapState, mapStores } from "pinia";
import { defineComponent } from "vue";
import { usePositionsStore } from "../store/positions";
export default defineComponent({
  name: "Positions",
  data: () => ({}),
  async mounted() {
    await this.fetchPositions();
  },
  computed: {
    ...mapState(usePositionsStore, ["positions", "loading"]),
  },
  methods: {
    ...mapActions(usePositionsStore, ["fetchPositions"]),
  },
});
</script>
<template>
  <h1 class="text-xl m-2 font-semibold">Positions</h1>
  <div v-if="loading" class="text-center">
    <div class="lds-ring">
      <div></div>
      <div></div>
      <div></div>
      <div></div>
    </div>
  </div>
  <div v-else>
    {{ positions }}
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
