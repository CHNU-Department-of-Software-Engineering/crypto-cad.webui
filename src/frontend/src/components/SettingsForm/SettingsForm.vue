<template>
  <div class="settings-form__wrapper">
    <div class="settings-form__container">
      <div class="settings-form__header">
        <div class="settings-form__header-title">Settings</div>
        <div class="settings-form__header-buttons-container">
          <v-tooltip top :disabled="isSignedIn">
            <template v-slot:activator="{ on, attrs }">
              <div class="settings-form__header-button" v-on="on" v-bind="attrs">
                <v-btn
                  width="250"
                  color="success"
                  outlined
                  :disabled="isSignedIn"
                  @click="$router.push('method/new')"
                >
                  Add New Method
                </v-btn>
              </div>
            </template>
            <span>Only Signed In Users can create New Method</span>
          </v-tooltip>
        </div>
      </div>
      <div class="settings-form__content">
        <v-form v-model="isFormValid">
          <v-row>
            <v-col cols="4">
              <v-select
                v-model="selectedCipherId"
                :items="formattedMethods"
                :rules="[inputRules.required]"
                label="Select Method"
                outlined
                required
              ></v-select>
            </v-col>
            <v-col offset="4" cols="4">
              <v-select
                v-model="selectedOperation"
                :items="cipherOperations"
                label="Select Operation"
                outlined
              ></v-select>
            </v-col>
          </v-row>
          <v-row>
            <v-col class="settings-form__content-item" cols="12">
              <v-text-field
                v-model="publicKeyInput"
                :label="'Key'"
                :rules="[
                inputRules.required,
                (value) => inputRules.maxLength(value, selectedCipher.key.lenght),
                (value) => inputRules.minLength(value, selectedCipher.key.lenght)
              ]"
                required
                solo
              ></v-text-field>
            </v-col>
          </v-row>
          <v-row v-if="selectedCipher">
            <v-col class="settings-form__content-item" cols="12">
              <ModifyForm :configuration="JSON.parse(selectedCipher.configuration)"></ModifyForm>
            </v-col>
          </v-row>
        </v-form>
      </div>
    </div>
    <div class="settings-form__footer">
      <FileDropzone ref="fileDropzone"></FileDropzone>
      <div class="settings-form__submit-button">
        <v-btn :disabled="!isFormValid" outlined width="400px" color="success" @click="onSubmitForm">
          {{ selectedOperation === 'encryption' ? 'Encrypt' : 'Decrypt' }}
        </v-btn>
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios'
import moment from 'moment'
import fileSaver from 'file-saver'
import ModifyForm from '../ModifyForm/ModifyForm'
import FileDropzone from './FileDropzone'
import { mapActions, mapState } from 'vuex'

export default {
  name: 'SettingsForm',
  components: {
    ModifyForm,
    FileDropzone
  },
  mounted () {
    this.getMethods()
  },
  data () {
    return {
      isFormValid: true,
      showTitle: true,
      ciphers: [],
      selectedCipherId: '',
      isEncrypt: true,
      fileText: '',
      cipherOperations: [
        {
          value: 'encryption',
          text: 'Encryption'
        },
        {
          value: 'decryption',
          text: 'Decryption'
        }
      ],
      selectedOperation: 'encryption',
      publicKeyInput: '',
      inputRules: {
        required: value => !!value || 'Required',
        maxLength: (value, length) => (value && value.length >= length) || `Key length must be ${length} characters. Now ${value.length} characters`,
        minLength: (value, length) => (value && value.length <= length) || `Key length must be ${length} characters. Now ${value.length} characters`
      }
    }
  },
  methods: {
    ...mapActions('method', ['getMethods']),
    onSubmitForm () {
      axios.post('https://localhost:5001/api/ciphers/process', {
        data: this.fileText.trim(),
        name: this.selectedCipher.name,
        id: this.selectedCipher.id,
        mode: this.selectedOperation,
        key: this.publicKeyInput.trim()
      }).then((data) => {
        this.$refs.fileDropzone.removeAllFiles()
        this.downloadFile(data.data.data)
      }).catch((e) => {
        console.log('error', e)
      })
    },
    downloadFile (text) {
      const blob = new Blob([text], { type: 'text/plain;charset=utf-8' })
      const currentDate = moment().format('YYYY-MM-DD_h-mm-ss')
      const operation = this.selectedOperation === 'encryption' ? 'encrypted' : 'decrypted'

      fileSaver.saveAs(blob, `${operation}_${currentDate}.txt`)
    }
  },
  computed: {
    ...mapState('method', ['methods']),
    isSignedIn () {
      return this.$store.state.auth.status.signedIn
    },
    formattedMethods () {
      return this.methods.map(method => ({
        text: method.name,
        value: method.id
      }))
    },
    selectedCipher () {
      return this.ciphers.find(cipher => cipher.id === this.selectedCipherId)
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

      .settings-form__submit-button {
        text-align: center;
        margin-top: 20px;
        cursor: pointer;
      }
    }
  }
</style>
