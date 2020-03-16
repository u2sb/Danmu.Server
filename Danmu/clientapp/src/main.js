import Vue from 'vue'
import 'normalize.css/normalize.css' // a modern alternative to CSS resets
import Element from 'element-ui'
import '@/assets/styles/index.scss' // global css
import App from '@/App'
import store from '@/store'
import router from '@/router'
import '@/assets/icons'
import '@/directive'

Vue.use(Element)

Vue.config.productionTip = false

new Vue({
    el: '#app',
    router,
    store,
    render: h => h(App)
})
