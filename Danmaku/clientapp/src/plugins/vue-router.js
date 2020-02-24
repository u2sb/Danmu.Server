import Vue from 'vue'
import VueRouter from 'vue-router'

import Home from '../components/Home'
import Login from '../components/Login'

Vue.use(VueRouter)

const routes = [
  { path: '/', component: Home, meta: { title: '首页' } },
  { path: '/login', component: Login, meta: { title: '登录' }, props: (route) => ({ query: route.query.url }) }
]

const router = new VueRouter({
  routes,
  mode: 'history'
})

router.beforeEach((to, from, next) => {
  if (to.meta.title) {
    document.title = to.meta.title
  }
  next()
})

export default router
