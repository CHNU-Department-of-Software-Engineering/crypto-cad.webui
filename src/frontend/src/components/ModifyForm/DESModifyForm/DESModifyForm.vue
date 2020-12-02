<template>
  <div>
    <h2>DES Modify</h2>
    <v-row>
      <v-col cols="6">
        <PermutationTable
          title="Initial Permutation"
          :table-data="[
        58,
        50,
        42,
        34,
        26,
        18,
        10,
        2,
        60,
        52,
        44,
        36,
        28,
        20,
        12,
        4,
        62,
        54,
        46,
        38,
        30,
        22,
        14,
        6,
        64,
        56,
        48,
        40,
        32,
        24,
        16,
        8,
        57,
        49,
        41,
        33,
        25,
        17,
        9,
        1,
        59,
        51,
        43,
        35,
        27,
        19,
        11,
        3,
        61,
        53,
        45,
        37,
        29,
        21,
        13,
        5,
        63,
        55,
        47,
        39,
        31,
        23,
        15,
        7
      ]"
          columns-number="8"
        ></PermutationTable>
      </v-col>
      <v-col cols="6">
        <PermutationTable
          title="Final Permutation"
          :table-data="[
        58,
        50,
        42,
        34,
        26,
        18,
        10,
        2,
        60,
        52,
        44,
        36,
        28,
        20,
        12,
        4,
        62,
        54,
        46,
        38,
        30,
        22,
        14,
        6,
        64,
        56,
        48,
        40,
        32,
        24,
        16,
        8,
        57,
        49,
        41,
        33,
        25,
        17,
        9,
        1,
        59,
        51,
        43,
        35,
        27,
        19,
        11,
        3,
        61,
        53,
        45,
        37,
        29,
        21,
        13,
        5,
        63,
        55,
        47,
        39,
        31,
        23,
        15,
        7
      ]"
          columns-number="8"
        ></PermutationTable>
      </v-col>
    </v-row>
  </div>
</template>

<script>
import PermutationTable from '../PermutationTable'

export default {
  name: 'DESModifyForm',
  components: {
    PermutationTable
  }
}
</script>

<style lang="scss" scoped>
  h2 {
    margin-bottom: 50px;
  }

  .table-header {
    margin-bottom: 10px;
  }
</style>
