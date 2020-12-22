import _ from 'lodash'
import methodService from '../services/method.service'
import { getOnlyEditedPartOfConfig, populateEditedFieldToConfig } from '../utils/method-helper'

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
    getMethods ({ commit }, selectMethodId) {
      return methodService.getMethods().then(
        methods => {
          const data = methods.data
          commit('getMethodsSuccess', data)
          if (selectMethodId) {
            commit('selectMethod', selectMethodId)
          } else {
            commit('selectFirstMethod')
          }
          return Promise.resolve(data)
        },
        error => {
          commit('getMethodsFailure')
          return Promise.reject(error)
        }
      )
    },
    deleteMethod ({ dispatch, state }) {
      return methodService.deleteMethod(state.selectedMethodId).then(
        data => {
          dispatch('getMethods')
          return Promise.resolve(data)
        },
        error => {
          return Promise.reject(error)
        }
      )
    },
    saveMethod ({ dispatch, commit, state }, payload) {
      const selectedMethod = state.methods.find(method => method.id === state.selectedMethodId)
      return methodService.saveMethod({
        ...(payload ? {} : { id: state.selectedMethodId }),
        name: !payload ? selectedMethod.name : payload,
        parentId: payload ? selectedMethod.id : selectedMethod.parentId,
        configuration: selectedMethod.isModifiable ? getOnlyEditedPartOfConfig(state.currentConfiguration) : null
      }).then(
        data => {
          dispatch('getMethods', state.selectedMethodId || null)
          return Promise.resolve(data)
        },
        error => {
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
        family: selectedMethod.family,
        mode: state.selectedOperationId,
        secret: state.secretKey,
        configuration: selectedMethod.isModifiable ? getOnlyEditedPartOfConfig(state.currentConfiguration) : null
      }

      return new Promise((resolve, reject) => {
        methodService.processMethod(processMethodInfo).then(
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
    selectFirstMethod (state, methodId) {
      const firstMethodInArray = state.methods.length ? state.methods[0] : null
      const methodConfiguration = firstMethodInArray ? firstMethodInArray.configuration : null
      state.selectedMethodId = firstMethodInArray ? firstMethodInArray.id : null
      state.selectedOperationId = firstMethodInArray?.type === 'cipher' ? 'encryption' : null
      state.currentConfiguration = firstMethodInArray.isModifiable ? populateEditedFieldToConfig(methodConfiguration) : {}
    },
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

      state.currentConfiguration = selectedMethod.isModifiable || selectedMethod.relation === 'child'
        ? populateEditedFieldToConfig(methodConfiguration)
        : {}
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
