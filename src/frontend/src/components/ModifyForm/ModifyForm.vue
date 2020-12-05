<template>
  <v-expansion-panels>
    <v-expansion-panel>
      <v-expansion-panel-header
        :class="`configuration-expanded-section-header ${isModified
          ? 'configuration-expanded-section-header--edited-table'
          : ''
        }`"
      >
        Modify
      </v-expansion-panel-header>
      <v-expansion-panel-content>
        <div class="modify-form__wrapper">
          <ModifyDESForm
            @checkForModifications="checkForModifications"
            :configuration='configuration'
          />
        </div>
      </v-expansion-panel-content>
    </v-expansion-panel>
  </v-expansion-panels>
</template>

<script>
import ModifyDESForm from './DESModifyForm/DESModifyForm.vue'
export default {
  name: 'ModifyForm',
  props: ['configuration'],
  components: {
    ModifyDESForm
  },
  data () {
    return {
      isModified: false
    }
  },
  methods: {
    checkForModifications (value) {
      this.isModified = value
    }
  }
}
</script>

<style lang="scss" scoped>
  .modify-form__wrapper {
    height: 100%;
    overflow: auto;
  }
</style>
