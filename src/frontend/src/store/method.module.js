import _ from 'lodash'
import MethodService from '../services/method.service'

export const method = {
  namespaced: true,
  state: {
    methods: [],
    operations: [
      {
        value: 'encryption',
        text: 'Encryption'
      },
      {
        value: 'decryption',
        text: 'Decryption'
      }
    ],
    selectedOperationId: null,
    selectedMethodId: null,
    secretKey: '',
    fileData: '',
    currentConfiguration: {},
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
    },
    processMethod ({ state }) {
      const selectedMethod = state.methods.find(method => method.id === state.selectedMethodId)
      const processMethodInfo = {
        data: state.fileData,
        type: selectedMethod.type,
        id: selectedMethod.id,
        mode: state.selectedOperationId,
        key: state.secretKey,
        configuration: selectedMethod.isModifiable
          ? Object.keys(state.currentConfiguration).reduce(
            (result, configItemKey) => {
              if (state.currentConfiguration[configItemKey].edited) {
                return {
                  ...result,
                  [configItemKey]: state.currentConfiguration[configItemKey].data
                }
              }

              return result
            }, {}
          )
          : null
      }

      return new Promise((resolve, reject) => {
        MethodService.processMethod(processMethodInfo).then(
          processedMethod => {
            resolve(processedMethod)
          },
          error => {
            reject(error)
          }
        )
      })
    }
  },
  mutations: {
    getMethodsSuccess (state, methods) {
      state.methods = _.sortBy(methods, 'type')
      state.loading.global = false
    },
    getMethodsFailure (state) {
      state.status.loggedIn = false
      state.loading.global = false
    },
    selectMethod (state, selectedMethodId) {
      const selectedMethod = state.methods.find(method => method.id === selectedMethodId)
      const methodConfiguration = selectedMethod.configuration
      const parsedConfiguration = methodConfiguration ? JSON.parse(methodConfiguration) : {}

      if (selectedMethod.isModifiable) {
        state.currentConfiguration = Object.keys(parsedConfiguration).reduce((result, configurationItemKey) => {
          return {
            ...result,
            [configurationItemKey]: {
              data: parsedConfiguration[configurationItemKey],
              edited: false
            }
          }
        }, {})
      } else {
        state.currentConfiguration = {}
      }

      state.selectedMethodId = selectedMethodId
    },
    selectOperation (state, selectedOperationId) {
      state.selectedOperationId = selectedOperationId
    },
    changeSecretKey (state, secretKey) {
      state.secretKey = secretKey
    },
    changeFileData (state, fileData) {
      state.fileData = fileData
    },
    updateMethodConfiguration (state, payload) {
      const { data, configurationName, edited } = payload

      state.currentConfiguration[configurationName].edited = edited
      state.currentConfiguration[configurationName].data = data
    }
  }
}
