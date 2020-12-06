<template>
  <v-expansion-panel>
    <v-expansion-panel-header
      :class="`expanded-section-header ${isPermutedChoiceTablesEdited ? 'expanded-section-header--edited' : ''}`"
    >
      <span>Permuted Choice Tables</span>
    </v-expansion-panel-header>
    <v-expansion-panel-content>
      <div class="permuted-choice-tables__container">
        <v-expansion-panels multiple>
          <PermutationTable
            @checkForTableEdit="onFirstPermutedChoiceTableEdit"
            :table-data="configuration['Pc1PermutationTable']"
            :columns-number="8"
            title="Permuted Choice Table #1"
          ></PermutationTable>
          <PermutationTable
            @checkForTableEdit="onSecondPermutedChoiceTableEdit"
            :table-data="configuration['Pc2PermutationTable']"
            :columns-number="8"
            title="Permuted Choice Table #2"
          ></PermutationTable>
        </v-expansion-panels>
      </div>
    </v-expansion-panel-content>
  </v-expansion-panel>
</template>

<script>
import PermutationTable from './PermutationTable'

export default {
  name: 'PermutedChoiceTables',
  props: ['configuration'],
  components: {
    PermutationTable
  },
  data () {
    return {
      isFirstPermutedTableEdited: false,
      isSecondPermutedTableEdited: false
    }
  },
  methods: {
    onFirstPermutedChoiceTableEdit (value) {
      this.isFirstPermutedTableEdited = value
      this.checkForTableEdit()
    },
    onSecondPermutedChoiceTableEdit (value) {
      this.isSecondPermutedTableEdited = value
      this.checkForTableEdit()
    },
    checkForTableEdit () {
      this.$emit('checkForTableEdit', this.isPermutedChoiceTablesEdited)
    }
  },
  computed: {
    isPermutedChoiceTablesEdited () {
      return this.isFirstPermutedTableEdited || this.isSecondPermutedTableEdited
    }
  }
}
</script>
