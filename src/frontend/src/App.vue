<template>
  <v-app>
    <Header/>
    <router-view></router-view>
  </v-app>
</template>

<script lang="ts">
import Vue from 'vue'
import Header from './components/Header.vue'

export default Vue.extend({
  name: 'App',

  components: {
    Header
  },

  data: () => ({
    //
  })
})
</script>

<style>
  body {
    font-family: 'Montserrat', sans-serif;
  }
</style>
