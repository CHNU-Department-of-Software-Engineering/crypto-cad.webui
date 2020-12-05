<template>
  <div class="home-page-wrapper">
    <SettingsForm></SettingsForm>
  </div>
</template>

<script lang="ts">
import SettingsForm from '../components/SettingsForm/SettingsForm.vue'

export default {
  name: 'Home',
  components: {
    SettingsForm
  }
}
</script>

<style scoped>
  .home-page-wrapper {
    background-color: #f5f5f5;
    height: calc(100vh - 60px);
    padding: 20px 40px;
    font-family: 'Montserrat', sans-serif;
  }
</style>
