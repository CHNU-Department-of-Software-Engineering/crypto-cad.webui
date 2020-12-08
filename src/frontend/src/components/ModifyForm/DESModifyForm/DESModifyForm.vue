<template>
  <div class="des-modify-form__wrapper">
    <v-expansion-panels multiple>
      <PermutationTable
        :configuration="configuration['InitialPermutationTable']"
        :default-configuration="defaultConfiguration['InitialPermutationTable']"
        :columns-number="8"
        title="Initial Permutation Table"
        configuration-name="InitialPermutationTable"
      ></PermutationTable>
      <RoundTables
        :configuration="configuration"
        :default-configuration="defaultConfiguration"
      ></RoundTables>
      <PermutationTable
        :configuration="configuration['FinalPermutationTable']"
        :default-configuration="defaultConfiguration['FinalPermutationTable']"
        :columns-number="8"
        title="Final Permutation Table"
        configuration-name="FinalPermutationTable"
      ></PermutationTable>
      <PermutationTable
        :configuration="configuration['RotationsTable']"
        :default-configuration="defaultConfiguration['RotationsTable']"
        :columns-number="8"
        title="Rotations Table"
        configuration-name="RotationsTable"
      ></PermutationTable>
      <PermutedChoiceTables
        :configuration="configuration"
        :default-configuration="defaultConfiguration"
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
  props: ['configuration', 'defaultConfiguration']
}
</script>

<style lang="scss" scoped>
  .des-modify-form__wrapper {
    display: flex;
    justify-content: center;
  }
</style>
