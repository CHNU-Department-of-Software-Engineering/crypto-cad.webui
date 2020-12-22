<template>
  <div class="save-method-dialog__wrapper">
    <v-dialog
      v-model="show"
      ripple
      width="500"
    >
      <v-card>
        <div class="save-method-dialog__container">
          <h3 class="save-method-dialog__title">Save Method</h3>
          <v-form v-model="isFormValid" ref="form">
            <div class="save-method-dialog__form-container">
              <v-text-field
                v-model="methodName"
                label="Method Name"
                :rules="[value => !!value || 'Required']"
                outlined
                required
              ></v-text-field>
            </div>
            <div class="save-method-dialog__buttons-container">
              <v-btn
                class="save-method-dialog__button"
                outlined
                @click="onClose"
              >
                Cancel
              </v-btn>
              <v-btn
                class="save-method-dialog__button"
                outlined
                color="success"
                @click="onSave"
                :disabled="!isFormValid"
              >
                Save
              </v-btn>
            </div>
          </v-form>
        </div>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
export default {
  name: 'SaveMethodDialog',
  props: ['value', 'methodsNames'],
  data () {
    return {
      dialog: false,
      methodName: '',
      isFormValid: true
    }
  },
  methods: {
    onSave () {
      this.show = false
      this.$emit('onSave', this.methodName)
      this.$refs.form.reset()
    },
    onClose () {
      this.show = false
      this.$refs.form.reset()
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
  .save-method-dialog__container {
    padding: 30px;

    .save-method-dialog__title {
      margin-bottom: 5px;
    }

    .save-method-dialog__form-container {
      padding: 30px 30px 0 30px;
    }

    .save-method-dialog__buttons-container {
      display: flex;
      justify-content: center;
      padding: 15px 25px 0 25px;
      margin-bottom: 10px;

      .save-method-dialog__button {
        margin: 0 10px;
        min-width: 150px;
      }
    }
  }
</style>
