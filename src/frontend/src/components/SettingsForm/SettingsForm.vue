<template>
  <div class="settings-form__wrapper">
    <div class="settings-form__container">
      <div class="settings-form__header">
        <div class="settings-form__header-title">Settings</div>
        <!--//TODO Uncomment when add new method functionality will be ready on backend-->
        <!--        <div class="settings-form__header-buttons-container">-->
        <!--          <v-tooltip top :disabled="isSignedIn">-->
        <!--            <template v-slot:activator="{ on, attrs }">-->
        <!--              <div class="settings-form__header-button" v-on="on" v-bind="attrs">-->
        <!--                <v-btn-->
        <!--                  width="250"-->
        <!--                  color="success"-->
        <!--                  outlined-->
        <!--                  :disabled="isSignedIn"-->
        <!--                  @click="$router.push('method/new')"-->
        <!--                >-->
        <!--                  Add New Method-->
        <!--                </v-btn>-->
        <!--              </div>-->
        <!--            </template>-->
        <!--            <span>Only Signed In Users can create New Method</span>-->
        <!--          </v-tooltip>-->
        <!--        </div>-->
      </div>
      <div class="settings-form__content">
        <v-form v-model="isFormValid" ref="settingsForm">
          <v-row>
            <v-col cols="4">
              <v-select
                v-model="methodId"
                :items="formattedMethods"
                :rules="[inputRules.required]"
                label="Select Method"
                outlined
                required
              ></v-select>
            </v-col>
            <v-col v-if="showOperationSelector" offset="4" cols="4">
              <v-select
                v-model="operationId"
                :items="operations"
                :rules="[inputRules.required]"
                label="Select Operation"
                outlined
                required
              ></v-select>
            </v-col>
          </v-row>
          <v-row v-if="selectedMethod">
            <v-col class="settings-form__content-item" cols="12">
              <v-text-field
                v-model="secretKeyValue"
                :value="secretKey"
                :label="keyInputLabel"
                :rules="keyInputRequired
                  ? [
                    inputRules.required,
                    (value) => inputRules.maxLength(value, selectedMethod.secretLength),
                    (value) => inputRules.minLength(value, selectedMethod.secretLength)
                  ]
                  : []
                "
                :required="keyInputRequired"
                outlined
              ></v-text-field>
            </v-col>
          </v-row>
          <v-row v-if="Object.values(currentConfiguration).length">
            <v-col class="settings-form__content-item" cols="12">
              <ModifyForm :defaultConfiguration="selectedMethod.configuration ? JSON.parse(selectedMethod.configuration) : {}"></ModifyForm>
            </v-col>
          </v-row>
          <v-row v-if="Object.values(currentConfiguration).length">
            <v-col class="settings-form__content-item" cols="12">
              <v-checkbox
                v-model="downloadIntermediateResults"
                label="Download Intermediate Results"
                color="success"
                outlined
              ></v-checkbox>
            </v-col>
          </v-row>
        </v-form>
      </div>
    </div>
    <div class="settings-form__footer">
      <FileDropzone ref="fileDropzone"></FileDropzone>
      <div :class="`settings-form__footer-buttons ${selectedMethod.relation === 'parent' ? 'settings-form__footer-center': ''}`">
        <div v-if="selectedMethod && selectedMethod.relation !== 'parent'">
          <v-btn
            class="settings-form__footer-button"
            :disabled="!isFormValid"
            outlined
            width="200px"
            color="error"
            @click="showDeleteDialog=true"
          >
            Delete
          </v-btn>
        </div>
       <div>
         <v-btn
           class="settings-form__footer-button"
           :disabled="!isFormValid"
           outlined
           width="200px"
           color="success"
           @click="onSubmitForm"
         >
           {{ submitButtonLabel }}
         </v-btn>
         <v-btn
           @click.stop=" selectedMethod.relation === 'parent' ? showSaveDialog=true : saveMethod()"
           v-if="selectedMethod &&  (selectedMethod.isModifiable || selectedMethod.relation === 'child')"
           class="settings-form__footer-button"
           :disabled="!isFormValid"
           outlined
           width="200px"
           color="success"
         >
           {{ selectedMethod.relation === 'parent' ? 'Save' : 'Update'}}
         </v-btn>
       </div>
      </div>
    </div>
    <SaveMethodDialog v-model="showSaveDialog" @onSave="saveMethod"></SaveMethodDialog>
    <DeleteMethodDialog v-model="showDeleteDialog" @onDelete="deleteMethod"></DeleteMethodDialog>
  </div>
</template>

<script>
import _ from 'lodash'
import moment from 'moment'
import fileSaver from 'file-saver'
import ModifyForm from '../ModifyForm/ModifyForm'
import SaveMethodDialog from './SaveMethodDialog'
import DeleteMethodDialog from './DeleteMethodDialog'
import FileDropzone from './FileDropzone'
import { mapActions, mapState, mapMutations } from 'vuex'

export default {
  name: 'SettingsForm',
  components: {
    ModifyForm,
    FileDropzone,
    SaveMethodDialog,
    DeleteMethodDialog
  },
  mounted () {
    this.getMethods()
  },
  data () {
    return {
      showSaveDialog: false,
      showDeleteDialog: false,
      downloadIntermediateResults: true,
      isFormValid: true,
      showTitle: true,
      isEncrypt: true,
      fileText: '',
      publicKeyInput: '',
      inputRules: {
        required: value => !!value || 'Required',
        maxLength: (value, length) => (value && value.length >= length) || `Key length must be ${length} characters. Now ${value.length} characters`,
        minLength: (value, length) => (value && value.length <= length) || `Key length must be ${length} characters. Now ${value.length} characters`
      }
    }
  },
  methods: {
    ...mapActions('method', ['getMethods', 'processMethod', 'deleteMethod', 'saveMethod']),
    ...mapMutations('method', ['selectMethod', 'selectOperation', 'changeSecretKey']),
    onSubmitForm () {
      this.processMethod().then((data) => {
        this.$refs.fileDropzone.removeAllFiles()
        this.downloadFile(data.data)
      }).catch((e) => {
        console.log('error', e)
      })
    },
    downloadFile (processedMethod) {
      const dataBlob = new Blob([processedMethod.data], { type: 'text/plain;charset=utf-8' })
      const currentDate = moment().format('YYYY-MM-DD_h-mm-ss')
      const operation = this.selectedOperation === 'encryption' ? 'encrypted' : 'decrypted'

      if (this.downloadIntermediateResults) {
        const intermediateResultsBlob = new Blob([processedMethod.intermediateResults], { type: 'text/plain;charset=utf-8' })
        fileSaver.saveAs(intermediateResultsBlob, `${this.selectedMethod.name}_Intermediate_Results_${currentDate}.txt`)
      }

      fileSaver.saveAs(dataBlob, `${this.selectedMethod.name}_${operation}_${currentDate}.txt`)
    }
  },
  computed: {
    ...mapState(
      'method',
      ['methods', 'selectedMethodId', 'currentConfiguration', 'operations', 'selectedOperationId', 'secretKey']
    ),
    methodId: {
      set (methodId) {
        this.selectMethod(methodId)
      },
      get () {
        return this.selectedMethodId
      }
    },
    operationId: {
      set (operationId) {
        this.selectOperation(operationId)
      },
      get () {
        return this.selectedOperationId
      }
    },
    secretKeyValue: {
      set (secretKeyValue) {
        this.changeSecretKey(secretKeyValue)
      },
      get () {
        return this.secretKey
      }
    },
    isSignedIn () {
      return this.$store.state.auth.status.signedIn
    },
    formattedMethods () {
      const formattedMethods = this.methods.map(method => ({
        text: method.name,
        value: method.id,
        type: method.type
      }))
      const groupedMethods = _.groupBy(formattedMethods, 'type')

      return Object.keys(groupedMethods).reduce((formattedMethods, methodType) => {
        return [
          ...formattedMethods,
          {
            divider: true
          },
          {
            header: _.startCase(methodType)
          },
          {
            divider: true
          },
          ...groupedMethods[methodType]
        ]
      }, [])
    },
    selectedMethod () {
      return this.methods.find(method => method.id === this.selectedMethodId)
    },
    selectedMethodType () {
      return this.selectedMethod ? this.selectedMethod.type : null
    },
    submitButtonLabel () {
      let label = 'Submit'
      switch (this.selectedMethodType) {
        case 'cipher':
          label = this.selectedOperationId ? this.selectedOperationId === 'encryption' ? 'Encrypt' : 'Decrypt' : 'Submit'
          break
        case 'hash':
          label = 'Hash'
          break
        default:
          label = 'Submit'
      }

      return label
    },
    keyInputLabel () {
      let label
      switch (this.selectedMethodType) {
        case 'cipher':
          label = 'Key'
          break
        case 'hash':
          label = 'Salt'
          break
        default:
          label = 'Key'
      }

      return label
    },
    keyInputRequired () {
      let required
      switch (this.selectedMethodType) {
        case 'cipher':
          required = true
          break
        case 'hash':
          required = false
          break
        default:
          required = true
      }

      return required
    },
    showOperationSelector () {
      let show
      switch (this.selectedMethodType) {
        case 'cipher':
          show = true
          break
        case 'hash':
          show = false
          break
        default:
          show = false
      }

      return show
    }
  }
}
</script>

<style lang="scss">
  .settings-form__wrapper {
    display: flex;
    flex-direction: column;
    height: 100%;

    .settings-form__container {
      flex: 1 0 auto;

      .settings-form__header {
        display: flex;
        justify-content: space-between;
        margin-bottom: 20px;

        .settings-form__header-title {
          font-size: 20px;
        }

        .settings-form__header-buttons-container {
          display: flex;

          .settings-form__header-button {
            margin-left: 20px;

            a {
              text-decoration: none;
            }
          }
        }
      }

      .settings-form__content-item {
        padding: 0 12px;
      }
    }

    .settings-form__footer {
      flex-shrink: 0;

      .settings-form__footer-buttons {
        display: flex;
        justify-content: space-between;
        margin: 20px 0;
        cursor: pointer;

        .settings-form__footer-button {
          margin: 0 5px;
        }
      }

      .settings-form__footer-center {
        justify-content: center;
      }
    }
  }
</style>
