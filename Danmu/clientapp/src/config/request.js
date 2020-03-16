import axios from 'axios'
import { apiPrefix } from '@/config'
import { Message, MessageBox, Notification } from 'element-ui'
import store from '@/store'

const service = axios.create({
    baseURL: apiPrefix,
    // withCredentials: true, // send cookies when cross-domain requests
    timeout: 60000 // request timeout
})

service.interceptors.response.use(
    response => {
        const res = response.data

        //当返回类型非{code}的接口请求时，不使用code来判断请求是否成功
        if (!('code' in res) || res.code === 0) {
            return response
        }

        //服务器异常
        if (res.code === 1) {
            Message.error(res.msg || '操作失败')
            return Promise.reject(res.msg)
        }

        //未登录或无权限
        if (res.code === 401) {
            if (store.state.user.prepareLogout) return Promise.reject()
            return MessageBox.alert('请登录后重试', {
                type: 'warning',
                beforeClose: (action, instance, done) => {
                    store.dispatch('user/logout').then(() => done())
                }
            })
        }

        //其他错误
        Message.error(res.msg || '接口有误')
        return Promise.reject(res)
    },
    error => {
        console.error(error)
        error && Notification.error({
            title: '错误',
            message: '请求错误，请稍后重试'
        })
        return Promise.reject(error)
    }
)

export default service
