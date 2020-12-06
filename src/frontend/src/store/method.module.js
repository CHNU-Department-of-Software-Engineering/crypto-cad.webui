import MethodService from '../services/method.service'

export const method = {
  namespaced: true,
  state: {
    methods: [],
    currentConfiguration: null,
    loading: {
      global: false
    }
  },
  actions: {
    getMethods ({ commit }) {
      return MethodService.getMethods().then(
        methods => {
          const data = methods.data
          commit('getMethodsSuccess', data)
          return Promise.resolve(data)
        },
        error => {
          commit('getMethodsFailure')
          return Promise.reject(error)
        }
      )
    }
  },
  mutations: {
    getMethodsSuccess (state, methods) {
      state.methods = methods
      state.global = false
    },
    getMethodsFailure (state) {
      state.status.loggedIn = false
      state.global = false
    }
  }
}
