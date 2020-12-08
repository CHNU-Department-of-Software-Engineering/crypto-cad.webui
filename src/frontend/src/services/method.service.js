import axios from 'axios'

const API_URL = 'https://localhost:5001/api/methods'

class MethodService {
  getMethods () {
    return axios.get(API_URL)
  }

  processMethod (method) {
    const data = {
      id: method.id,
      type: method.type,
      data: method.data,
      ...(method.configuration ? { configuration: JSON.stringify(method.configuration) } : null),
      ...(method.type === 'cipher' ? { cipher: { mode: method.mode, key: method.key } } : { hash: { salt: method.key } })
    }
    return axios
      .post(API_URL + '/process', data)
  }
}

export default new MethodService()
