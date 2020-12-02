<template>
 <div class="permutation-table__wrapper">
   <div class="permutation-table__title">{{ title }}</div>
   <div class="permutation-table__container">
     <table>
       <tr v-for="(row, rowIndex) in formattedTableData" :key="rowIndex">
         <td v-for="(column, columnIndex) in row" :key="columnIndex">
           <input
             :class="`permutation-table__cell ${column.edited ? 'edited-cell': ''}`"
             type="text"
             v-model="column.value"
             @change="onInputChange(rowIndex, columnIndex)"
           >
         </td>
       </tr>
     </table>
   </div>
   <div class="permutation-table__footer-buttons">
     <button @click="populateDefaultValues">Populate</button>
     <button @click="clearTableValues">Clear</button>
   </div>
 </div>
</template>

<script>
import _ from 'lodash'

export default {
  name: 'PermutationTable',
  props: ['tableData', 'columnsNumber', 'title'],
  data () {
    return {
      formattedTableData: _.chunk(this.tableData.map(() => ({ value: null, edited: false })), this.columnsNumber)
    }
  },
  methods: {
    populateDefaultValues () {
      const formattedData = this.tableData.map(item => ({ value: item, edited: false }))

      this.formattedTableData = _.chunk(formattedData, this.columnsNumber)
    },
    clearTableValues () {
      const formattedData = this.tableData.map(() => ({ value: null, edited: false }))

      this.formattedTableData = _.chunk(formattedData, this.columnsNumber)
    },
    onInputChange (rowIndex, columnIndex) {
      this.formattedTableData[rowIndex][columnIndex].edited = true
    }
  }
}
</script>

<style lang="scss" scoped>
  .permutation-table__wrapper {
    display: flex;
    flex-direction: column;

    .permutation-table__title {
      font-weight: bold;
    }

    .permutation-table__container {
      display: flex;
      flex-direction: column;
      margin: 15px 0;

      .permutation-table__cell {
        border: 1px solid black;
        width: 45px;
        text-align: center;
      }

      .permutation-table__cell.edited-cell {
        border: 1px solid red;
        background-color: #ffcccb;
      }
    }

    .permutation-table__footer-buttons {
      margin: 0 10px;
      display: flex;
      justify-content: space-between;
    }
  }
</style>
