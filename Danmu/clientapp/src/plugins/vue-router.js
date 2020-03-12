import Vue from 'vue'
import VueRouter from 'vue-router'
import VueCookies from 'vue-cookies'

import Home from '../components/Home'
import Login from '../components/Login'
import Admin from '../components/Admin/Admin'
import DanmuList from '../components/Admin/DanmuList'
import DanmuListByVid from '../components/Admin/DanmuListByVid'

Vue.use(VueCookies)
Vue.use(VueRouter)

const routesBase = [
  { path: '/', name: 'home', component: Home, meta: { title: '首页' } },
  {
    path: '/login',
    name: 'login',
    component: Login,
    meta: { title: '登录' },
    props: route => ({ query: route.query.url })
  },
  {
    path: '/admin',
    name: 'admin',
    component: Admin,
    meta: { title: '弹幕列表', auth: ['SuperAdmin', 'Admin'] },
    children: [
      {
        path: 'danmulist',
        name: 'danmulist',
        component: DanmuList,
        meta: { title: '弹幕列表', auth: ['SuperAdmin', 'Admin'] }
      },
      {
        path: 'danmulistbyvid',
        name: 'danmulistbyvid',
        component: DanmuListByVid,
        meta: { title: '弹幕列表', auth: ['SuperAdmin', 'Admin'] }
      }
    ]
  }
]

const routesReg = [{ path: '*', redirect: '/' }]

const routes = [...routesBase, ...routesReg]

const router = new VueRouter({
  routes,
  mode: 'history'
})

router.beforeEach((to, from, next) => {
  if (
    /^\/admin/.test(to.path) &&
    (!Vue.$cookies.isKey('ClientAuth') ||
      (to.meta.auth && to.meta.auth.indexOf(Vue.$cookies.get('ClientAuth')) < 0))
  ) {
    next({
      path: '/login',
      query: { url: to.fullPath }
    })
  }
  if (to.path == '/login' && !to.query.url) {
    next({
      path: to.fullPath,
      query: { url: from.fullPath }
    })
  }
  if (to.meta.title) {
    document.title = to.meta.title
  }
  next()
})

export default router
