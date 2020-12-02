<template>
    <div class="modify-form__wrapper">
      <ModifyDESForm/>
    </div>
</template>

<script>
import ModifyDESForm from './DESModifyForm/DESModifyForm.vue'
export default {
  name: 'ModifyForm',
  components: {
    ModifyDESForm
  }
}
</script>

<style lang="scss" scoped>
  .modify-form__wrapper {
    height: 100%;
    padding: 20px 60px;
    background-color: #ffffff;
    border-radius: 7px;
    box-shadow: 0 4px 25px 0 rgba(0,0,0,.1);
    overflow: auto;
  }
</style>
