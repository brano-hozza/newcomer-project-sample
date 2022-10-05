import { createApp } from "vue";
import App from "./App.vue";
import { createRouter, createWebHistory } from "vue-router";
import { routes } from "./routes";
import { createPinia } from "pinia";
import "bootstrap";
import "./styles/index.scss";
createApp(App)
  .use(createPinia())
  .use(
    createRouter({
      routes: routes,
      history: createWebHistory(),
    })
  )
  .mount("#app");
