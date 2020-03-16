import { createMutations } from '@/utils'

const state = {
    //区分pc和移动端（mobile）
    device: 'pc',
    //路由页面滚动高度
    scrollTop: 0,
    //侧边栏折叠
    collapseSidebar: false
}

const mutations = createMutations(state, false)


export default {
    namespaced: true,
    state,
    mutations
}
