import Vue from 'vue'
import App from './App.vue'
import router from './plugins/vue-router'
import store from './plugins/store'
import './plugins/element'
import './plugins/vue-axios'
Vue.config.productionTip = false

new Vue({
  el: '#app',
  store,
  render: h => h(App),
  router
}).$mount('#app')


