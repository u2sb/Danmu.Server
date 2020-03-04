import Vue from 'vue'
import VueRouter from 'vue-router'
import VueCookies from 'vue-cookies'

import Home from '../components/Home'
import Login from '../components/Login'
import Admin from '../components/Admin/Admin'
import DanmakuList from '../components/Admin/DanmakuList'

Vue.use(VueCookies)
Vue.use(VueRouter)

const routesBase = [
  { path: '/', component: Home, meta: { title: '首页' } },
  {
    path: '/login',
    component: Login,
    meta: { title: '登录' },
    props: route => ({ query: route.query.ReturnUrl })
  },
  { path: '/admin', component: Admin, meta : { title : '弹幕列表' }, children: [{
    path: 'danmakulist', component: DanmakuList, meta : { title : '弹幕列表' }
  }] }
]

const routesReg = [{ path: '*', redirect: '/' }]

const routes = [...routesBase, ...routesReg]

const router = new VueRouter({
  routes,
  mode: 'history'
})

router.beforeEach((to, from, next) => {
  if ((/^\/admin/).test(to.path) && !Vue.$cookies.isKey('ClientAuth')) {
    next({
      path: '/login',
      query: { ReturnUrl: to.fullPath }
    })
  }
  if (to.path == '/login' && !to.query.ReturnUrl) {
    next({
      path: to.fullPath,
      query: { ReturnUrl: from.fullPath }
    })
  }
  if (to.meta.title) {
    document.title = to.meta.title
  }
  next()
})

export default router
