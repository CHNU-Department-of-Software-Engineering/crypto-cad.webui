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
            :configuration="configuration['Pc1Permutation']"
            :default-configuration="defaultConfiguration['Pc1Permutation']"
            :columns-number="8"
            title="Permuted Choice Table #1"
            configuration-name="Pc1Permutation"
          ></PermutationTable>
          <PermutationTable
            :configuration="configuration['Pc2Permutation']"
            :default-configuration="defaultConfiguration['Pc2Permutation']"
            :columns-number="8"
            title="Permuted Choice Table #2"
            configuration-name="Pc2Permutation"
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
  props: ['configuration', 'defaultConfiguration'],
  components: {
    PermutationTable
  },
  computed: {
    isPermutedChoiceTablesEdited () {
      return this.configuration.Pc1Permutation.edited || this.configuration.Pc2Permutation.edited
    }
  }
}
</script>
