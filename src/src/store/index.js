import Vue from 'vue'
import Vuex from 'vuex'

import { auth } from './auth.module.js'
import { method } from './method.module.js'

Vue.use(Vuex)

export default new Vuex.Store({
  modules: {
    auth,
    method
  }
})
