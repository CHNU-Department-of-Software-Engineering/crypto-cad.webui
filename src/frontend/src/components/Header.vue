<template>
  <div class="main-header__wrapper">
    <h3 class="main-header__login" @click="$router.push('/settings')">CryptoCAD</h3>
    <LoginDialog/>
  </div>
</template>

<script>
import LoginDialog from './LoginDialog'

export default {
  name: 'Header',
  components: {
    LoginDialog
  }
}
</script>

<style lang="scss" scoped>
  .main-header__wrapper {
    display: flex;
    align-items: center;
    justify-content: space-between;
    height: 60px;
    padding: 10px 30px;
    background-color: #fff;

    .main-header__login {
      cursor: pointer;
    }
  }
</style>
