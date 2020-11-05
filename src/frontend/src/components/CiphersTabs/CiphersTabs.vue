<template>
    <div class="ciphers-tabs__wrapper">
      <CipherTab v-for="cipher in ciphers" :key="cipher.id" :cipher="cipher"></CipherTab>
    </div>
</template>

<script>
import ciphers from '@/assets/mocks/ciphers'
import CipherTab from './CipherTab'

export default {
  name: 'CiphersTabs',
  data () {
    return {
      ciphers
    }
  },
  components: {
    CipherTab
  }
}
</script>

<style scoped lang="scss">
  .ciphers-tabs__wrapper {
    display: flex;
    background-color: #ffffff;
    margin: 30px 0;
    padding: 10px 0;
    border-radius: 5px;
    box-shadow: 0 4px 20px 0 rgba(0, 0, 0, .05);
  }
</style>
