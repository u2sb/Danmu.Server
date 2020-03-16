/*
* 路由配置
*
* 需要鉴权的路由：meta && Array.isArray(meta.auth) && meta.auth.length>0
* 左侧菜单显示：name && !hidden && meta.title
* 左侧菜单不折叠只有一个children的路由：alwaysShow
* tab栏固定显示：meta.affix
* 页面不缓存：!name || meta.noCache
* */
import Vue from 'vue'
import Router from 'vue-router'
import NProgress from 'nprogress'
import 'nprogress/nprogress.css'
import { isUserExist } from '@/utils/sessionStorage'
import { auth } from '@/utils/auth'
import { isEmpty } from '@/utils'
import { title } from '@/config'
import constantRoutes from '@/router/constantRoutes'
import authorityRoutes from '@/router/authorityRoutes'
import store from '@/store'

Vue.use(Router)

NProgress.configure({ showSpinner: false })

const endRoute = [{ path: '*', redirect: '/404', hidden: true }]
const urlNoNeedLogin = {
    '/welcome': 1,
    '/login': 1,
    '/404': 1
}

metaExtend(constantRoutes)
metaExtend(authorityRoutes)

const createRouter = () => new Router({
    scrollBehavior: () => ({ y: 0 }),
    routes: constantRoutes.concat(authorityRoutes, endRoute)
})

const router = createRouter()

router.beforeEach(async (to, from, next) => {
    NProgress.start()

    document.title = getPageTitle(to.meta.title)

    //不需要登录的页面也不需要进行权限控制
    if (urlNoNeedLogin[to.path]) return next()

    const isLogin = isUserExist()

    //未登录时返回登录页
    if (!isLogin) return next({ path: '/login', query: { redirect: to.fullPath } })

    //初始化菜单
    await store.dispatch('resource/init', store.state.user)

    //已登录时访问登录页则跳转至首页
    if (to.path === '/login') return next({ path: '/' })

    //页面不需要鉴权或有访问权限时通过
    if (auth(to)) return next()

    //用户无权限访问时的动作
    next({ path: '/login' })
})

router.afterEach(() => NProgress.done())

//拼接页面标题
function getPageTitle(pageTitle) {
    return pageTitle ? `${pageTitle} - ${title}` : title
}

//子路由继承父路由meta上的{affix,noCache}，优先使用子路由的值
function metaExtend(routes, meta) {
    routes.forEach(route => {
        if (meta) {
            const keys = ['affix', 'noCache']
            Object.keys(meta).forEach(key => {
                if (keys.includes(key) && !isEmpty(meta[key]) && isEmpty(route.meta[key])) {
                    route.meta[key] = meta[key]
                }
            })
        }
        if (route.children) {
            metaExtend(route.children, route.meta)
        }
    })
}

export default router
