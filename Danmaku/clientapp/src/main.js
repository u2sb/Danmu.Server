import Vue from 'vue'
import App from './App.vue'
import router from './plugins/vue-router'
import './plugins/element.js'
import './plugins/vue-axios'
Vue.config.productionTip = false

new Vue({
  el: '#app',
  render: h => h(App),
  router
}).$mount('#app')


