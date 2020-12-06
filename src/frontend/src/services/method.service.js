import axios from 'axios'

const API_URL = 'https://localhost:5001/api/ciphers'

class MethodService {
  getMethods () {
    return axios.get(API_URL)
  }

  processMethod (method) {
    return axios
      .post(API_URL + '/process', {
        data: method.data,
        name: method.name,
        id: method.id,
        mode: method.mode,
        key: method.key
      })
  }
}

export default new MethodService()
