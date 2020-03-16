import { isEmpty } from '@/utils/index'

export function timeFormat(fmt = 'yyyy-MM-dd HH:mm:ss', date = new Date()) {
    if (isEmpty(fmt)) fmt = 'yyyy-MM-dd HH:mm:ss'
    let o = {
        'M+': date.getMonth() + 1, //月份
        'd+': date.getDate(), //日
        'H+': date.getHours(), //小时
        'm+': date.getMinutes(), //分
        's+': date.getSeconds(), //秒
        'q+': Math.floor((date.getMonth() + 3) / 3), //季度
        'S': date.getMilliseconds() //毫秒
    }
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (date.getFullYear() + '').substr(4 - RegExp.$1.length))
    for (let k in o)
        if (new RegExp('(' + k + ')').test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length === 1) ? (o[k]) : (('00' + o[k]).substr(('' + o[k]).length)))
    return fmt
}

export function utc2local(utcTime) {
    return new Date(utcTime + 'Z')
}

export function getLocalTime(utcTime) {
    if (isEmpty(utcTime)) return null
    return timeFormat(null, utc2local(utcTime))
}
