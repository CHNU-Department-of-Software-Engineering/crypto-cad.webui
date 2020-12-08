<template>
  <v-expansion-panel>
    <v-expansion-panel-header
      :class="`expanded-section-header ${configuration.edited ? 'expanded-section-header--edited' : ''}`"
    >
      <span>{{ title }}</span>
    </v-expansion-panel-header>
    <v-expansion-panel-content>
      <div class="permutation-table__wrapper">
        <div class="permutation-table__container">
          <table>
            <tr v-for="(row, rowIndex) in formattedTableData" :key="rowIndex">
              <td v-for="(column, columnIndex) in row" :key="columnIndex">
                <input
                  :class="`permutation-table__cell ${column.edited ? 'edited-cell': ''}`"
                  type="text"
                  v-model.number="column.value"
                  @change="(event) => onInputChange(rowIndex, columnIndex, event.target.value)"
                >
              </td>
            </tr>
          </table>
          <div class="permutation-table__footer-buttons">
            <v-btn
              class="permutation-table__default-values-button"
              width="175"
              color="primary"
              outlined
              @click.prevent="populateDefaultValues"
            >
              Default Values
            </v-btn>
          </div>
        </div>
      </div>
    </v-expansion-panel-content>
  </v-expansion-panel>
</template>

<script>
import _ from 'lodash'
import { mapMutations } from 'vuex'

export default {
  name: 'PermutationTable',
  props: ['configuration', 'defaultConfiguration', 'columnsNumber', 'title', 'configurationName'],
  data () {
    return {
      currentTableData: this.configuration?.data.map(item => ({ value: item, edited: false }))
    }
  },
  methods: {
    ...mapMutations('method', ['updateMethodConfiguration']),
    populateDefaultValues () {
      this.currentTableData = this.getDefaultTableData()

      const tableData = this.currentTableData.map(item => item.value)
      this.updateMethodConfiguration({ configurationName: this.configurationName, data: tableData, edited: false })
    },
    onInputChange (rowIndex, columnIndex, value) {
      const rowNumber = rowIndex + 1
      const columnNumber = columnIndex + 1
      const itemIndex = ((this.columnsNumber * (rowNumber - 1)) + columnNumber) - 1
      const defaultTableData = this.getDefaultTableData()

      this.currentTableData[itemIndex].edited = defaultTableData[itemIndex].value !== +value

      const tableData = this.currentTableData.map(item => item.value)
      console.log('')
      this.updateMethodConfiguration({ configurationName: this.configurationName, data: tableData, edited: this.isTableEdited })
    },
    getDefaultTableData () {
      return this.defaultConfiguration.map(item => ({ value: item, edited: false }))
    }
  },
  computed: {
    isTableEdited () {
      return _.flatten(this.currentTableData).some((tableCell) => tableCell.edited)
    },
    formattedTableData () {
      return _.chunk(this.currentTableData, this.columnsNumber)
    }
  }
}
</script>

<style lang="scss" scoped>
  .permutation-table__wrapper {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;

    .permutation-table__container {
      display: flex;
      flex-direction: column;
      margin: 15px 0;
      width: max-content;

      .permutation-table__cell {
        border: 1px solid black;
        width: 55px;
        text-align: center;
      }

      .permutation-table__cell.edited-cell {
        background-color: rgba(245, 206, 66, 0.7);
      }

      .permutation-table__footer-buttons {
        margin: 10px 0 0 10px;
        display: flex;
        justify-content: flex-end;

        .permutation-table__default-values-button {
          outline: none;
        }
      }
    }
  }
</style>
