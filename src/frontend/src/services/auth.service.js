import axios from 'axios'

const API_URL = 'http://localhost:5001/api/auth/'

class AuthService {
  signIn (user) {
    return axios
      .post(API_URL + 'signin', {
        username: user.username,
        password: user.password
      })
      .then(response => {
        if (response.data.accessToken) {
          localStorage.setItem('user', JSON.stringify(response.data))
        }

        return response.data
      })
  }

  signOut () {
    localStorage.removeItem('user')
  }

  signUp (user) {
    return axios.post(API_URL + 'signup', {
      username: user.username,
      password: user.password
    })
  }
}

export default new AuthService()
