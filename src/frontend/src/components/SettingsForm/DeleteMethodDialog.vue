<template>
  <div class="delete-method-dialog__wrapper">
    <v-dialog
      v-model="show"
      ripple
      width="500"
    >
      <v-card>
        <div class="delete-method-dialog__container">
          <h3 class="delete-method-dialog__title">Delete Method</h3>
          <div class="delete-method-dialog__buttons-container">
            <v-btn
              class="delete-method-dialog__button"
              outlined
              @click="onClose"
            >
              Cancel
            </v-btn>
            <v-btn
              class="delete-method-dialog__button"
              outlined
              color="error"
              @click="onDelete"
            >
              Delete
            </v-btn>
          </div>
        </div>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
export default {
  name: 'DeleteMethodDialog',
  props: {
    value: Boolean
  },
  data () {
    return {
      dialog: false
    }
  },
  methods: {
    onDelete () {
      this.show = false
      this.$emit('onDelete')
    },
    onClose () {
      this.show = false
    }
  },
  computed: {
    show: {
      get () {
        return this.value
      },
      set (value) {
        this.$emit('input', value)
      }
    }
  }
}
</script>

<style lang="scss" scoped>
  .delete-method-dialog__container {
    padding: 30px;

    .delete-method-dialog__title {
      margin-bottom: 5px;
    }

    .delete-method-dialog__buttons-container {
      display: flex;
      justify-content: center;
      padding: 15px 25px 0 25px;
      margin-bottom: 10px;

      .delete-method-dialog__button {
        margin: 0 10px;
        min-width: 150px;
      }
    }
  }
</style>
