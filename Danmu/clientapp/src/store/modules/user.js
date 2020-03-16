import cookie from 'js-cookie'
import { cookieRoleKey } from '@/config'
import { login, logout } from '@/api/admin/account'
import { createMutations, isEmpty } from '@/utils'
import { getUser, setUser } from '@/utils/sessionStorage'

//刷新时从本地存储中获取用户信息
const user = getUser()

const state = {
    //是否正在退出的过程中，避免重复弹框
    prepareLogout: false,

    /*用户基本信息*/
    id: !isEmpty(user.id) ? user.id : null,
    name: !isEmpty(user.name) ? user.name : null,
    role: !isEmpty(user.role) ? user.role : null
}

const mutations = createMutations(state)

const actions = {
    login({ commit, dispatch }, { name, password }) {
        return new Promise((resolve, reject) => {
            login({ name, password })
                .then(user => {
                    user = {
                        id: user.uid,
                        name,
                        role: cookie.get(cookieRoleKey)
                    }
                    commit('setAll', user)
                    setUser(user)
                    resolve()
                })
                .catch(error => reject(error))
        })
    },

    logout({ commit, state, dispatch }) {
        return new Promise((resolve, reject) => {
            if (state.prepareLogout) return Promise.reject()
            commit('setPrepareLogout', true)
            logout()
            /*logout(state.token)
                .then(() => {
                    dispatch('removeUser')
                    dispatch('tagsView/delAllViews', null, { root: true })
                    commit('resource/setHasInitRoutes', false, { root: true })
                    resolve()
                    window.location.reload()
                })
                .catch(error => reject(error))
                .finally(() => commit('setPrepareLogout', false))*/
        })
    }
}

export default {
    namespaced: true,
    state,
    mutations,
    actions
}
