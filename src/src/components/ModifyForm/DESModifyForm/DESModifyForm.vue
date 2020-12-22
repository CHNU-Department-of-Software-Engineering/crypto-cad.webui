<template>
  <div class="des-modify-form__wrapper">
    <v-expansion-panels multiple>
      <PermutationTable
        :configuration="configuration['InitialPermutation']"
        :default-configuration="defaultConfiguration['InitialPermutation']"
        :columns-number="8"
        title="Initial Permutation Table"
        configuration-name="InitialPermutation"
      ></PermutationTable>
      <RoundTables
        :configuration="configuration"
        :default-configuration="defaultConfiguration"
      ></RoundTables>
      <PermutationTable
        :configuration="configuration['FinalPermutation']"
        :default-configuration="defaultConfiguration['FinalPermutation']"
        :columns-number="8"
        title="Final Permutation Table"
        configuration-name="FinalPermutation"
      ></PermutationTable>
      <PermutationTable
        :configuration="configuration['Rotations']"
        :default-configuration="defaultConfiguration['Rotations']"
        :columns-number="8"
        title="Rotations Table"
        configuration-name="Rotations"
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
