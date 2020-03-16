/*路由表：弹幕列表*/
import Layout from '@/layout'

const router = {
    path: '/danmu',
    component: Layout,
    meta: { title: '弹幕管理', auth: ['SuperAdmin', 'Admin'] },
    children: [
        {
            path: 'index',
            name: 'danmuList',
            component: () => import('@/views/danmu'),
            meta: { title: '弹幕列表', icon: 'list' }
        }
    ]
}

export default router
