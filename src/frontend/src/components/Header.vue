<template>
  <div class="header-wrapper">
    <h3>CryptoCAD</h3>
    <div>
      <LoginDialog/>
    </div>
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

<style scoped>
.header-wrapper {
  display: flex;
  align-items: center;
  justify-content: space-between;
  height: 60px;
  padding: 10px 30px;
  background-color: #fff;
}
</style>
