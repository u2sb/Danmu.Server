import { apiPrefix } from '@/config'
import request from '@/config/request'

const baseUrl = '/admin'

export function login({ name, password }) {
    return request.post(baseUrl + '/login', { name, password }).then(({ data }) => data.data)
}

export function logout() {
    window.location.href = apiPrefix + baseUrl + '/logout'
}

export function getUserInfo(id) {
    return request.get(baseUrl + '/user/user', { params: { id } }).then(({ data }) => data.data)
}

export function changePwd({ uid, oldP, newP }) {
    return request.post(baseUrl + '/user/changepassword', { uid, oldP, newP }).then(({ data }) => data.data)
}

export function changeInfo({ id, name, email, phoneNumber }) {
    return request.post(baseUrl + '/user/changeinfo', { id, name, email, phoneNumber }).then(({ data }) => data.data)
}
