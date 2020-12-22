<template>
  <v-expansion-panel>
    <v-expansion-panel-header
      :class="`expanded-section-header ${isRoundTablesEdited ? 'expanded-section-header--edited' : ''}`"
    >
      <span>Round Tables</span>
    </v-expansion-panel-header>
    <v-expansion-panel-content>
      <div class="round-tables__container">
        <v-expansion-panels multiple>
          <PermutationTable
            :configuration="configuration['ExpansionPermutation']"
            :default-configuration="defaultConfiguration['ExpansionPermutation']"
            :columns-number="8"
            title="Expansion Permutation Table"
            configuration-name="ExpansionPermutation"
          ></PermutationTable>
          <v-expansion-panel>
            <v-expansion-panel-header
              :class="`expanded-section-header ${isSBoxTablesEdited ? 'expanded-section-header--edited' : ''}`"
            >
              <span>Substitution Boxes</span>
            </v-expansion-panel-header>
            <v-expansion-panel-content>
              <v-expansion-panels multiple>
                <v-expansion-panel v-for="boxIndex in 8" :key="boxIndex">
                  <PermutationTable
                    :configuration="configuration[`SBox${boxIndex}`]"
                    :default-configuration="defaultConfiguration[`SBox${boxIndex}`]"
                    :columns-number="8"
                    :title="`Substitution Box #${boxIndex}`"
                    :configuration-name="`SBox${boxIndex}`"
                  ></PermutationTable>
                </v-expansion-panel>
              </v-expansion-panels>
            </v-expansion-panel-content>
          </v-expansion-panel>
          <PermutationTable
            :configuration="configuration['Permutation']"
            :default-configuration="defaultConfiguration['Permutation']"
            :columns-number="8"
            title="Permutation Table"
            configuration-name="Permutation"
          ></PermutationTable>
        </v-expansion-panels>
      </div>
    </v-expansion-panel-content>
  </v-expansion-panel>
</template>

<script>
import PermutationTable from './PermutationTable'
import { mapState, mapMutations } from 'vuex'

export default {
  name: 'RoundTables',
  props: ['configuration', 'defaultConfiguration'],
  components: {
    PermutationTable
  },
  computed: {
    isRoundTablesEdited () {
      return this.isSBoxTablesEdited || this.configuration.Permutation.edited || this.configuration.ExpansionPermutation.edited
    },
    isSBoxTablesEdited () {
      let edited = false

      for (let boxIndex = 1; boxIndex < 9; boxIndex++) {
        if (this.configuration[`SBox${boxIndex}`].edited) {
          edited = true
        }
      }

      return edited
    }
  }
}
</script>
