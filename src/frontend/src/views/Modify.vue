<template>
  <div class="modify-page-wrapper">
    <ModifyForm></ModifyForm>
  </div>
</template>

<script lang="ts">
import ModifyForm from '../components/ModifyForm/ModifyForm.vue'

export default {
  name: 'Modify',
  components: {
    ModifyForm
  }
}
</script>

<style scoped>
  .modify-page-wrapper {
    background-color: #f5f5f5;
    height: calc(100vh - 60px);
    padding: 20px 50px;
    font-family: 'Montserrat', sans-serif;
  }
</style>
