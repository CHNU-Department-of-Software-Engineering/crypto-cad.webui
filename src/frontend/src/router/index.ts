import Vue from 'vue'
import VueRouter, { RouteConfig } from 'vue-router'
import Settings from '../views/Settings.vue'
import AddNewMethod from '../views/AddNewMethod.vue'

Vue.use(VueRouter)

const routes: Array<RouteConfig> = [
  {
    path: '/',
    redirect: '/settings'
  },
  {
    path: '/settings',
    name: 'Settings',
    component: Settings
  },
  {
    path: '/method/add',
    name: 'AddNewMethod',
    component: AddNewMethod
  }
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

export default router
