<template>
  <div class="home-page-wrapper">
    <CipherForm></CipherForm>
  </div>
</template>

<script lang="ts">
import CipherForm from '../components/CipherForm/CipherForm.vue'

export default {
  name: 'Home',
  components: {
    CipherForm
  }
}
</script>

<style scoped>
  .home-page-wrapper {
    background-color: #f5f5f5;
    height: calc(100vh - 60px);
    padding: 20px 50px;
    font-family: 'Montserrat', sans-serif;
  }
</style>
