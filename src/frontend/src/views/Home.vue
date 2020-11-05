<template>
  <div class="home-page-wrapper">
    <CiphersTabs></CiphersTabs>
  </div>
</template>

<script lang="ts">
import CiphersTabs from '@/components/CiphersTabs/CiphersTabs.vue'

export default {
  name: 'Home',
  components: {
    CiphersTabs
  }
}
</script>

<style scoped>
  .home-page-wrapper {
    background-color: #f5f5f5;
    height: calc(100vh - 60px);
    padding: 0 30px;
  }
</style>
