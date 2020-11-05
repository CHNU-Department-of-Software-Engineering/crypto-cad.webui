<template>
  <div class="cipher-tab__wrapper">
    {{ cipher.name }}
  </div>
</template>

<script>

export default {
  name: 'CipherTab',
  props: ['cipher']
}
</script>

<style scoped lang="scss">
  .cipher-tab__wrapper {
    padding: 10px 20px;
    cursor: pointer;
  }
</style>
