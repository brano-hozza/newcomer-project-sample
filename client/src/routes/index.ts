import type { RouteRecordRaw } from "vue-router";
export const routes: RouteRecordRaw[] = [
  {
    path: "/",
    component: () => import("../views/Home.vue"),
    children: [
      {
        path: "/",
        name: "Current employees",
        component: () => import("../views/CurrentEmployees.vue"),
      },
      {
        path: "/old",
        name: "Old employees",
        component: () => import("../views/OldEmployees.vue"),
      },
      {
        path: "/positions",
        name: "Positions",
        component: () => import("../views/Positions.vue"),
      },
    ],
  },
];
