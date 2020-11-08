<template>
  <div>
    <v-dialog
      v-model="dialog"
      ripple
      width="500"
    >
      <template v-slot:activator="{ attrs }">
        <div v-bind="attrs" @click="isSignedIn ? signOut(): openDialog()">
          <span>{{ isSignedIn ? 'Signed In' : 'Sign In'}}</span>
        </div>
      </template>

      <v-card>
        <div class="login-dialog__wrapper">
          <h3 class="login-dialog__title">{{ isSignIn ? 'Sign In' : 'Sign Up'}}</h3>
          <span v-if="isSignIn">Welcome back, please sign in to your account.</span>
          <span v-else>Welcome, please create your account.</span>
          <div class="login-dialog__form-container">
            <v-text-field
              v-model="username"
              label="Username"
              :rules="[usernameRules.required]"
              outlined
            ></v-text-field>
            <v-text-field
              v-model="password"
              :append-icon="showPassword ? 'mdi-eye' : 'mdi-eye-off'"
              :rules="[passwordRules.required, passwordRules.min]"
              :type="showPassword ? 'text' : 'password'"
              label="Password"
              hint="At least 6 characters"
              outlined
              @click:append="showPassword = !showPassword"
            ></v-text-field>
            <v-text-field
              v-if="!isSignIn"
              v-model="confirmPassword"
              :append-icon="showConfirmPassword ? 'mdi-eye' : 'mdi-eye-off'"
              :rules="[passwordRules.required, passwordRules.min]"
              :type="showConfirmPassword ? 'text' : 'password'"
              label="Confirm Password"
              hint="At least 6 characters"
              outlined
              @click:append="showConfirmPassword = !showConfirmPassword"
            ></v-text-field>
          </div>
          <div class="login-dialog__buttons-container">
            <v-btn
              color="#ffffff"
              class="login-dialog__register-button"
              @click="changeTab"
            >
              {{ isSignIn ? 'Sign Up' : 'Sign In'}}
            </v-btn>
            <v-btn
              color="#7266f0"
              dark
              @click="isSignIn ? signIn() : signUp()"
            >
              {{ isSignIn ? 'Sign In' : 'Sign Up'}}
            </v-btn>
          </div>
        </div>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
export default {
  name: 'LoginDialog',
  mounted () {
    console.log('localStorage.getItem(\'isSignedIn\')', localStorage.getItem('isSignedIn'))
  },
  data () {
    return {
      isSignIn: true,
      isSignedIn: localStorage.getItem('isSignedIn'),
      dialog: false,
      username: '',
      password: '',
      confirmPassword: '',
      showPassword: false,
      showConfirmPassword: false,
      passwordRules: {
        required: value => !!value || 'Required',
        min: value => value.length >= 6 || 'Min 6 characters'
      },
      usernameRules: {
        required: value => !!value || 'Required'
      }
    }
  },
  watch: {
    isSignedIn () {
      console.log('this.isSignedIn', this.isSignedIn)
    }
  },
  methods: {
    changeTab () {
      this.isSignIn = !this.isSignIn
    },
    signIn () {
      console.log('signIn')
      localStorage.setItem('isSignedIn', true)
      this.isSignedIn = true
      this.dialog = false
    },
    signUp () {
      console.log('signUp')
      localStorage.setItem('isSignedIn', true)
      this.isSignedIn = true
      this.dialog = false
    },
    signOut () {
      console.log('signOut')
      localStorage.setItem('isSignedIn', false)
      this.isSignedIn = false
      this.dialog = false
    },
    openDialog () {
      this.dialog = true
    }
  }
}
</script>

<style scoped lang="scss">
  .login-dialog__wrapper {
    padding: 30px;

    .login-dialog__title {
      margin-bottom: 5px;
    }

    .login-dialog__form-container {
      padding: 30px 30px 10px 30px;
    }

    .login-dialog__buttons-container {
      display: flex;
      justify-content: space-between;
      padding: 0 25px;
    }
  }
</style>
