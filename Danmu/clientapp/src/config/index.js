module.exports = {
    title: 'Danmu.Server',

    //全局axios的baseUrl
    apiPrefix: '/api',

    //路由模式
    routerMode: 'history',

    logo: '/favicon.png',

    //路由切换时的过渡动画名称,关联transition.css
    rightSideRouteTransition: 'left-out',
    leftSideRouteTransition: 'right-out',

    /*sessionStorage的存储键名*/
    sessionUserKey: 'SESS:USER',

    /*cookie的存储键名*/
    cookieRoleKey: 'ClientAuth'
}
