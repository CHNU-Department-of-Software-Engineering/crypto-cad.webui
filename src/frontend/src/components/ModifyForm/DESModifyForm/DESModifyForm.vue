<template>
  <div class="des-container">
    <v-expansion-panels multiple>
      <PermutationTable
        @checkForTableEdit="onInitialPermutationTableEdit"
        :table-data="configuration['InitialPermutationTable']"
        :columns-number="8"
        title="Initial Permutation Table"
      ></PermutationTable>
      <RoundTables
        @checkForTableEdit="onRoundTablesEdit"
        :configuration="configuration"
      ></RoundTables>
      <PermutationTable
        @checkForTableEdit="onFinalPermutationTableEdit"
        :table-data="configuration['FinalPermutationTable']"
        :columns-number="8"
        title="Final Permutation Table"
      ></PermutationTable>
      <PermutationTable
        @checkForTableEdit="onRotationsTableEdit"
        :table-data="configuration['RotationsTable']"
        :columns-number="8"
        title="Rotations Table"
      ></PermutationTable>
      <PermutedChoiceTables
        @checkForTableEdit="onPermutedChoiceTablesEdit"
        :configuration="configuration"
      ></PermutedChoiceTables>
    </v-expansion-panels>
  </div>
</template>

<script>
import PermutationTable from './PermutationTable'
import RoundTables from './RoundTables'
import PermutedChoiceTables from './PermutedChoiceTables'

export default {
  name: 'DESModifyForm',
  components: {
    PermutationTable,
    RoundTables,
    PermutedChoiceTables
  },
  props: ['configuration'],
  data () {
    return {
      isInitialPermutationTableEdited: false,
      isRoundTablesEdited: false,
      isFinalPermutationTableEdited: false,
      isRotationsTableEdited: false,
      isPermutedChoiceTablesEdited: false
    }
  },
  methods: {
    onInitialPermutationTableEdit (value) {
      this.isInitialPermutationTableEdited = value
      this.checkForModifications()
    },
    onRoundTablesEdit (value) {
      this.isRoundTablesEdited = value
      this.checkForModifications()
    },
    onFinalPermutationTableEdit (value) {
      this.isFinalPermutationTableEdited = value
      this.checkForModifications()
    },
    onRotationsTableEdit (value) {
      this.isRotationsTableEdited = value
      this.checkForModifications()
    },
    onPermutedChoiceTablesEdit (value) {
      this.isPermutedChoiceTablesEdited = value
      this.checkForModifications()
    },
    checkForModifications () {
      this.$emit('checkForModifications',
        this.isInitialPermutationTableEdited ||
        this.isRoundTablesEdited ||
        this.isFinalPermutationTableEdited ||
        this.isRotationsTableEdited ||
        this.isPermutedChoiceTablesEdited
      )
    }
  }
}
</script>

<style lang="scss" scoped>
  .des-container {
    display: flex;
    justify-content: center;
  }
  h2 {
    margin-bottom: 50px;
  }

  .table-header {
    margin-bottom: 10px;
  }
</style>
