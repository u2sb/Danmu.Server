import store from '@/store'

export function auth(route) {
    if (!needAuth(route)) return true
    return route.meta.auth.includes(store.state.user.role)
}

function needAuth(route) {
    const { meta, path } = route
    if (path.startsWith('/redirect')) return false
    return meta && Array.isArray(meta.auth) && meta.auth.length > 0
}
