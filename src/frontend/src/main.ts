import Vue from 'vue'
import App from './App.vue'
import './registerServiceWorker'
import router from './router'
import store from './store'
import vuetify from './plugins/vuetify'
import Vuetify from 'vuetify'
import 'font-awesome/css/font-awesome.min.css'

import 'vue2-dropzone/dist/vue2Dropzone.min.css'

Vue.config.productionTip = false
Vue.use(Vuetify, {
  iconfont: 'fa4'
})

new Vue({
  router,
  store,
  vuetify,
  render: h => h(App)
}).$mount('#app')
