<template>
  <div>
    <v-dialog
      v-model="dialog"
      ripple
      width="500"
    >
      <template v-slot:activator="{ attrs }">
        <div v-bind="attrs" @click="isSignedIn ? handleSignOut(): openDialog()">
          <span>{{ isSignedIn ? 'Signed In' : 'Sign In'}}</span>
        </div>
      </template>

      <v-card>
        <div class="login-dialog__wrapper">
          <h3 class="login-dialog__title">{{ isSignInView ? 'Sign In' : 'Sign Up'}}</h3>
          <span v-if="isSignInView">Welcome back, please sign in to your account.</span>
          <span v-else>Welcome, please create your account.</span>
          <div class="login-dialog__form-container">
            <v-form v-model="isFormValid">
              <v-text-field
                v-model="user.username"
                label="Username"
                :rules="[usernameRules.required]"
                outlined
                required
              ></v-text-field>
              <v-text-field
                v-model="user.password"
                :append-icon="showPassword ? 'mdi-eye' : 'mdi-eye-off'"
                :rules="[passwordRules.required, passwordRules.min]"
                :type="showPassword ? 'text' : 'password'"
                label="Password"
                hint="At least 6 characters"
                outlined
                required
                @click:append="showPassword = !showPassword"
              ></v-text-field>
              <v-text-field
                v-if="!isSignInView"
                v-model="user.confirmPassword"
                :append-icon="showConfirmPassword ? 'mdi-eye' : 'mdi-eye-off'"
                :rules="[passwordRules.required, passwordRules.min]"
                :type="showConfirmPassword ? 'text' : 'password'"
                label="Confirm Password"
                hint="At least 6 characters"
                outlined
                required
                @click:append="showConfirmPassword = !showConfirmPassword"
              ></v-text-field>
              <div class="login-dialog__buttons-container">
                <v-btn
                  class="login-dialog__submit-button"
                  @click="isSignInView ? handleSignIn() : handleSignUp()"
                  :disabled="!isFormValid"
                >
                  {{ isSignInView ? 'Sign In' : 'Sign Up'}}
                </v-btn>
                <a
                  class="login-dialog__switch-tab-link"
                  @click="changeTab"
                >
                  {{ isSignInView ? 'Do not have account yet ? Sign Up' : 'Already have account ? Sign In'}}
                </a>
              </div>
            </v-form>
          </div>
        </div>
      </v-card>
    </v-dialog>
    <v-snackbar
      v-model="snackbar.show"
      :right="true"
      :top="true"
      :color="snackbar.color"
    >
      {{ snackbar.message }}
    </v-snackbar>
  </div>
</template>

<script>
import User from '../models/user'

export default {
  name: 'LoginDialog',
  data () {
    return {
      snackbar: {
        show: false,
        message: null,
        color: null
      },
      isSignInView: true,
      isSuccessful: false,
      isFormValid: true,
      user: new User('', ''),
      loading: false,
      dialog: false,
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
  computed: {
    isSignedIn () {
      return this.$store.state.auth.status.loggedIn
    }
  },
  methods: {
    changeTab () {
      this.isSignInView = !this.isSignInView
    },
    handleSignIn () {
      if (this.isFormValid) {
        this.$store.dispatch('auth/signIn', this.user).then(
          data => {
            this.snackbar.message = data.message
            this.snackbar.show = true
            this.snackbar.color = 'green'
            this.isSuccessful = true
            this.dialog = false
          },
          error => {
            this.snackbar.message =
              (error.response && error.response.data && error.response.data.message) ||
              error.message ||
              error.toString()
            this.snackbar.show = true
            this.snackbar.color = 'red'
            this.isSuccessful = false
            console.log('this.isSuccessful', this.isSuccessful)
          }
        )
      }
    },
    handleSignUp () {
      if (this.isFormValid) {
        this.$store.dispatch('auth/signUp', this.user).then(
          data => {
            this.snackbar.message = data.message
            this.snackbar.color = 'green'
            this.snackbar.show = true
            this.isSuccessful = true
            this.isSignInView = true
          },
          error => {
            this.snackbar.message =
              (error.response && error.response.data && error.response.data.message) ||
              error.message ||
              error.toString()
            this.snackbar.color = 'red'
            this.snackbar.show = true
            this.isSuccessful = false
          }
        )
      }
    },
    handleSignOut () {
      this.$store.dispatch('auth/signOut', this.user).then(
        data => {
          this.snackbar.message = data
          this.snackbar.color = 'red'
          this.snackbar.show = true
          this.isSuccessful = true
        }
      )
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
      padding: 30px 30px 0 30px;
    }

    .login-dialog__buttons-container {
      display: flex;
      flex-direction: column;
      justify-content: space-between;
      padding: 15px 25px 0 25px;

      .login-dialog__submit-button {
        margin-bottom: 10px;
      }

      .login-dialog__switch-tab-link {
        text-align: center;
        font-size: 13px;
      }
    }
  }
</style>
