/*
* 个性化设置
* */

import { isEmpty } from '@/utils'

const localSettings = {}

const state = {
    //是否显示logo
    showLogo: false,
    //左侧菜单手风琴效果
    sidebarUniqueOpen: true,
    //左侧菜单折叠时，弹出菜单是否显示上级
    sidebarShowParent: false,
    //是否显示返回顶部按钮
    showBackToTop: true
}

//state填入初始值
Object.keys(state).forEach(key => {
    if (!isEmpty(localSettings[key])) state[key] = localSettings[key]
})

const mutations = createMutations(state)

function createMutations(state) {
    return Object.keys(state).reduce((mutations, key) => {
        mutations[key] = (ref, data) => {
            ref[key] = data
        }
        return mutations
    }, {})
}

export default {
    namespaced: true,
    state,
    mutations
}
