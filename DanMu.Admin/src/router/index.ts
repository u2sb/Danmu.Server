import { createRouter, createWebHistory, RouteRecordRaw } from "vue-router";
import HomeView from "@/views/HomeView.vue";
import LoginView from "@/views/LoginView.vue";
import DanMuListView from "@/views/DanMuListView.vue";

const routes: Array<RouteRecordRaw> = [
  {
    path: "/",
    name: "home",
    component: HomeView,
    meta: {
      title: "弹幕服务器",
    },
  },
  {
    path: "/login",
    name: "login",
    component: LoginView,
    meta: {
      title: "登录",
    },
  },
  {
    path: "/danmulist",
    name: "danmulist",
    component: DanMuListView,
    meta: {
      title: "弹幕列表",
    },
  },
];

const router = createRouter({
  history: createWebHistory(""),
  routes,
});

router.beforeEach((to, from, next) => {
  /* 路由发生变化修改页面title */
  if (to.meta.title) {
    document.title = to.meta.title as string;
  }
  if (to.name === "login") {
    to.meta.url = from.fullPath;
  }
  next();
});

export default router;
