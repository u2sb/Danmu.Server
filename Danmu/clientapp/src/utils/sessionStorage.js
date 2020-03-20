import { sessionUserKey } from '@/config'
import { isEmpty } from '@/utils'

export function isUserExist() {
    return !isEmpty(sessionStorage.getItem(sessionUserKey))
}

export function getUser() {
    let obj = sessionStorage.getItem(sessionUserKey)
    if (isEmpty(obj)) return {}
    try {
        obj = JSON.parse(obj)
    }
    catch (e) {
        console.error('用户数据异常！', e)
        obj = {}
        removeUser()
    }
    return obj
}

export function setUser(user) {
    if (isEmpty(user)) return removeUser()
    sessionStorage.setItem(sessionUserKey, JSON.stringify(user))
}

export function removeUser() {
    sessionStorage.removeItem(sessionUserKey)
}
