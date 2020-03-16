/*
* 不需要权限控制的路由表
* */
import Layout from '@/layout'

const constantRoutes = [
    {
        path: '/redirect',
        component: Layout,
        children: [
            { path: '/redirect/:path(.*)', component: () => import('@/views/app/redirect') }
        ]
    },
    { path: '/welcome', component: () => import('@/views/welcome') },
    { path: '/login', component: () => import('@/views/app/login') },
    { path: '/404', component: () => import('@/views/app/404') },
    {
        path: '/',
        component: Layout,
        redirect: '/index',
        children: [
            {
                path: 'index',
                component: () => import('@/views/index'),
                name: 'index',
                meta: { title: '首页', affix: true, icon: 'home' }
            }
        ]
    },
    {
        path: '/user',
        component: Layout,
        redirect: '/user/index',
        children: [
            {
                path: 'index',
                name: 'user',
                component: () => import('@/views/user'),
                meta: { title: '个人中心', noCache: true, icon: 'user' }
            }
        ],
        hidden: true
    }
]

export default constantRoutes
