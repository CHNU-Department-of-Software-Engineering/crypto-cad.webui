<template>
  <v-expansion-panel>
    <v-expansion-panel-header
      :class="`configuration-expanded-section-header ${isRoundTablesEdited
        ? 'configuration-expanded-section-header--edited-table'
        : ''
      }`"
    >
      <span>Round Tables</span>
    </v-expansion-panel-header>
    <v-expansion-panel-content>
      <div class="des-round-tables__container">
        <v-expansion-panels multiple>
          <PermutationTable
            @checkForTableEdit="onExpansionPermutationTableEdit"
            :table-data="configuration['ExpansionPermutationTable']"
            :columns-number="8"
            title="Expansion Permutation Table"
          ></PermutationTable>
          <v-expansion-panel>
            <v-expansion-panel-header :class="`section-header ${isSubstitutionBoxTableEdited ? 'section-header--edited-table': ''}`">
              <span>Substitution Boxes</span>
            </v-expansion-panel-header>
            <v-expansion-panel-content>
              <v-expansion-panels multiple>
                <v-expansion-panel v-for="(substitutionBox, boxIndex) in configuration['SubstitutionBoxes']" :key="boxIndex">
                  <PermutationTable
                    @checkForTableEdit="onSubstitutionBoxTableEdit"
                    :table-data="substitutionBox"
                    :columns-number="8"
                    :title="`Substitution Box #${boxIndex + 1}`"
                  ></PermutationTable>
                </v-expansion-panel>
              </v-expansion-panels>
            </v-expansion-panel-content>
          </v-expansion-panel>
          <PermutationTable
            @checkForTableEdit="onPermutationTableEdit"
            :table-data="configuration['PermutationTable']"
            :columns-number="8"
            title="Permutation Table"
          ></PermutationTable>
        </v-expansion-panels>
      </div>
    </v-expansion-panel-content>
  </v-expansion-panel>
</template>

<script>
import PermutationTable from './PermutationTable'

export default {
  name: 'RoundTables',
  props: ['configuration'],
  components: {
    PermutationTable
  },
  data () {
    return {
      isPermutationTableEdited: false,
      isSubstitutionBoxTableEdited: false,
      isExpansionPermutationTableEdited: false
    }
  },
  methods: {
    onSubstitutionBoxTableEdit (value) {
      this.isSubstitutionBoxTableEdited = value
      this.checkForTableEdit()
    },
    onExpansionPermutationTableEdit (value) {
      this.isExpansionPermutationTableEdited = value
      this.checkForTableEdit()
    },
    onPermutationTableEdit (value) {
      this.isPermutationTableEdited = value
      this.checkForTableEdit()
    },
    checkForTableEdit () {
      this.$emit('checkForTableEdit',
        this.isSubstitutionBoxTableEdited ||
        this.isExpansionPermutationTableEdited ||
        this.isPermutationTableEdited
      )
    }
  },
  computed: {
    isRoundTablesEdited () {
      return this.isSubstitutionBoxTableEdited || this.isExpansionPermutationTableEdited || this.isPermutationTableEdited
    }
  }
}
</script>

<style scoped lang="scss">
</style>
