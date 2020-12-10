import axios from 'axios'

const API_URL = 'https://localhost:5001/api/methods'

class MethodService {
  getMethods () {
    return axios.get(API_URL)
  }

  deleteMethod (methodId) {
    return axios.post(API_URL + '/delete', { id: methodId })
  }

  saveMethod (method) {
    const data = {
      id: method.id,
      name: method.name,
      parentId: method.parentId,
      ...(method.configuration ? { configuration: JSON.stringify(method.configuration) } : null)
    }
    return axios.post(API_URL + '/savechanges', data)
  }

  processMethod (method) {
    const data = {
      id: method.id,
      type: method.type,
      data: method.data,
      family: method.family,
      mode: method.type === 'cipher' ? method.mode : null,
      secret: method.secret,
      ...(method.configuration ? { configuration: JSON.stringify(method.configuration) } : null)
    }
    return axios
      .post(API_URL + '/process', data)
  }
}

export default new MethodService()
