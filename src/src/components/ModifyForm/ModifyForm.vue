<template>
  <div class="modify-form__wrapper">
    <v-expansion-panels>
      <v-expansion-panel>
        <v-expansion-panel-header
          :class="`expanded-section-header modify-form__expansion-header ${isModified ? 'expanded-section-header--edited': ''}`"
        >
          Modify
        </v-expansion-panel-header>
        <v-expansion-panel-content>
          <div class="modify-form__container">
            <ModifyDESForm
              :default-configuration="defaultConfiguration"
              :configuration='currentConfiguration'
            />
          </div>
        </v-expansion-panel-content>
      </v-expansion-panel>
    </v-expansion-panels>
  </div>
</template>

<script>
import ModifyDESForm from './DESModifyForm/DESModifyForm.vue'
import { mapState } from 'vuex'

export default {
  name: 'ModifyForm',
  props: ['defaultConfiguration', 'isModified'],
  components: {
    ModifyDESForm
  },
  computed: {
    ...mapState('method', ['currentConfiguration'])
  }
}
</script>

<style lang="scss">
  .modify-form__wrapper {
    margin-bottom: 10px;
    padding: 2px;

    .modify-form__container {
      height: 100%;
      overflow: auto;
      padding: 3px;
    }

    .modify-form__expansion-header {
      height: 56px;
    }
  }
</style>
