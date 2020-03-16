import request from '@/config/request'

const baseUrl = '/admin'

export function getAllVids() {
    return request.get(baseUrl + '/danmulist/vids').then(({ data }) => data.data)
}

export function search(searchForm) {
    return request.get(baseUrl + '/danmulist/baseselect', { params: searchForm }).then(({ data }) => data.data)
}

export function del(id) {
    return request.get(baseUrl + '/danmuedit/delete', { params: { id } }).then(({ data }) => data.data)
}

export function getById(id) {
    return request.get(baseUrl + '/danmuedit', { params: { id } }).then(({ data }) => data.data)
}

export function update(data) {
    return request.post(baseUrl + '/danmuedit/edit', data).then(({ data }) => data.data)
}
