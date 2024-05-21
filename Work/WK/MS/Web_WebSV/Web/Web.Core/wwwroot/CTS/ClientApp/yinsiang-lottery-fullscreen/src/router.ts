import { createRouter, createWebHashHistory, RouteRecordRaw } from "vue-router";
import Views from "./views";

const routes: Array<RouteRecordRaw> = [
  {
    path: "/",
    name: "Home",
    redirect: { name: "Bet" },
  },
  {
    path: "/Bet",
    name: "Bet",
    component: Views.BetViews.BetView,
  },
  {
    path: "/Description",
    name: "Description",
    component: Views.DescriptionView,
  },
  {
    path: "/OrderHistory",
    name: "OrderHistory",
    component: Views.OrderHistoryView,
  },
  {
    path: "/IssueHistory",
    name: "IssueHistory",
    component: Views.IssueHistoryView,
  },
  {
    path: "/FollowPlan",
    name: "FollowPlan",
    component: Views.FollowPlanView,
  }
];

const router = createRouter({
  history: createWebHashHistory(),
  routes,
});

export default router;

declare module "vue-router" {
  interface RouteMeta {
    useSmallIcon?: boolean;
  }
}
