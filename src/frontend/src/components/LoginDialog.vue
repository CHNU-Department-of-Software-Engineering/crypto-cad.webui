<template>
  <div>
    <v-dialog
      v-model="dialog"
      ripple
      width="500"
    >
      <template v-slot:activator="{ on, attrs }">
        <v-btn
          color="red lighten-2"
          dark
          v-bind="attrs"
          v-on="on"
        >
          Login
        </v-btn>
      </template>

      <v-card>
        <div class="login-dialog__wrapper">
          <h3 class="login-dialog__title">Login</h3>
          <span>Welcome back, please login to your account.</span>
          <div class="login-dialog__form-container">
            <v-text-field label="Login"></v-text-field>
            <v-text-field label="Password"></v-text-field>
          </div>
          <div class="login-dialog__buttons-container">
            <v-btn
              color="#ffffff"
              class="login-dialog__register-button"
            >
              Register
            </v-btn>
            <v-btn
              color="#7266f0"
              dark
            >
              Login
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
  data () {
    return {
      dialog: false,
      tab: 0,
      tabs: [
        { name: 'Login', icon: 'mdi-account' },
        { name: 'Register', icon: 'mdi-account-outline' }
      ]
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
      padding: 10px 30px;
    }

    .login-dialog__buttons-container {
      display: flex;
      justify-content: space-between;
      padding: 0 25px;
    }
  }
</style>
