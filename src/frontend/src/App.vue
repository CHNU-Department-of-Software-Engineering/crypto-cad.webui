<template>
  <v-app>
    <Header/>
    <div class="app-content__wrapper">
      <div class="app-content__container">
        <router-view></router-view>
      </div>
    </div>
  </v-app>
</template>

<script lang="ts">
import Vue from 'vue'
import Header from './components/Header.vue'

export default Vue.extend({
  name: 'App',
  components: {
    Header
  }
})
</script>

<style lang="scss">
  #app {
    font-family: 'Montserrat', sans-serif;
  }

  .app-content__wrapper {
    background-color: #f5f5f5;
    height: calc(100vh - 60px);
    padding: 20px 40px;

    .app-content__container {
      height: 100%;
      padding: 20px 20px 0 20px;
      background-color: #ffffff;
      border-radius: 7px;
      box-shadow: 0 4px 25px 0 rgba(0,0,0,.1);
      overflow: auto;
    }
  }
</style>
