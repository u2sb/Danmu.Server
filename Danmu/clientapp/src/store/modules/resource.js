import path from 'path'
import constantRoutes from '@/router/constantRoutes'
import authorityRoutes from '@/router/authorityRoutes'
import { auth } from '@/utils/auth'

const finalConstantRoutes = transformOriginRoutes(constantRoutes)
const finalAuthorityRoutes = transformOriginRoutes(authorityRoutes)
clean(finalConstantRoutes, false)
clean(finalAuthorityRoutes, false)


const state = {
    sidebarMenus: [],
    hasInitRoutes: false
}

const mutations = {
    setRoutes: (state, routes) => {
        let tempConstantRoutes = JSON.parse(JSON.stringify(finalConstantRoutes))
        let tempAuthorityRoutes = JSON.parse(JSON.stringify(routes))
        clean(tempConstantRoutes)
        clean(tempAuthorityRoutes)
        state.sidebarMenus = tempConstantRoutes.concat(tempAuthorityRoutes)
    },
    setHasInitRoutes: (state, sign) => {
        state.hasInitRoutes = sign
    }
}

const actions = {
    init({ state, commit }) {
        if (state.hasInitRoutes) return Promise.resolve()
        return new Promise(resolve => {
            const accessedRoutes = getAuthorizedRoutes()
            commit('setRoutes', accessedRoutes)
            commit('setHasInitRoutes', true)
            resolve()
        })
    }
}

//在原始路由数组基础上添加全路径
function transformOriginRoutes(routes) {
    let res = JSON.parse(JSON.stringify(routes))

    addFullPath(res)

    return res
}

//删除不显示的路由(没有children且没有meta.title，左侧菜单需清除hidden=true)
function clean(routes, cleanHidden = true) {
    for (let i = routes.length - 1; i >= 0; i--) {
        if (cleanHidden && routes[i].hidden) {
            routes.splice(i, 1)
            continue
        }
        if (!routes[i].children && (!routes[i].meta || !routes[i].meta.title)) {
            routes.splice(i, 1)
            continue
        }
        if (routes[i].children) {
            clean(routes[i].children, cleanHidden)
            if (routes[i].children.length < 1 && routes[i].alwaysShow !== true) {
                routes.splice(i, 1)
            }
        }
    }
}

//路由添加全路径
function addFullPath(routes, basePath = '/') {
    routes.forEach(route => {
        delete route.components
        route.fullPath = path.resolve(basePath, route.path)
        route.children && addFullPath(route.children, route.fullPath)
    })
}

//获取经过权限控制后的路由
function getAuthorizedRoutes() {
    const arr = JSON.parse(JSON.stringify(finalAuthorityRoutes))
    filter(arr, i => auth(i))
    return arr
}

//若没有children且未通过，则删除，若有，当children长度为0时删除
function filter(arr, fun) {
    for (let i = 0; i < arr.length; i++) {
        if (!arr[i].children && !fun(arr[i])) {
            arr.splice(i, 1)
            i--
            continue
        }

        if (!arr[i].children) continue

        filter(arr[i].children, fun)

        if (arr[i].children.length <= 0) {
            arr.splice(i, 1)
            i--
        }
    }
}

export default {
    namespaced: true,
    state,
    mutations,
    actions
}
