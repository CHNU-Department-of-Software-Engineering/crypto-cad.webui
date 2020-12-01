<template>
  <div class="cipher-form__wrapper">
    <div class="cipher-form__header">
      <h2 class="cipher-form__title">Settings</h2>
      <v-tooltip top :disabled="isSignedIn">
        <template v-slot:activator="{ on, attrs }">
          <div v-on="on" v-bind="attrs">
            <v-btn
              color="success"
              outlined
              :disabled="!isSignedIn"
            >
              Add new Cipher
            </v-btn>
          </div>
        </template>
        <span>Only Signed In Users can create new cipher</span>
      </v-tooltip>
    </div>
    <v-form v-model="isFormValid">
      <v-row>
        <v-col cols="4">
          <v-select
            v-model="selectedCipherId"
            :items="formattedCiphers"
            :rules="[inputRules.required]"
            label="Select Cipher"
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
      <div class="cipher-form__inputs">
        <v-row>
          <v-col cols="12">
            <v-text-field
              v-model="publicKeyInput"
              :label="'Public Key'"
              :rules="[inputRules.required]"
              required
              outlined
            ></v-text-field>
          </v-col>
        </v-row>
      </div>
      <div class="cipher-form__footer">
        <div class="cipher-form__file-dropzone">
          <v-row>
            <v-col align-self="center">
              <vue-dropzone
                id="file-dropzone"
                ref="fileDropzone"
                :options="dropzoneOptions"
                :useCustomSlot=true
                @vdropzone-queue-complete="onFileUploadComplete"
              >
                <div class="dropzone-custom-content">
                  <h3 class="dropzone-custom-title">Drag and drop to upload .txt file</h3>
                  <div class="subtitle">...or click to select a .txt file from your computer</div>
                </div>
              </vue-dropzone>
            </v-col>
          </v-row>
        </div>
        <div class="cipher-form__submit-button">
          <v-btn :disabled="!isFormValid || !this.fileText" outlined width="400px" color="success" @click="onSubmitForm">
            Submit
          </v-btn>
        </div>
      </div>
    </v-form>
  </div>
</template>

<script>
import axios from 'axios'
import fileSaver from 'file-saver'
import moment from 'moment'
import vueDropzone from 'vue2-dropzone'

export default {
  name: 'EncryptDecryptForm',
  components: {
    vueDropzone
  },
  created () {
    this.fetchCiphersData()
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
        required: value => !!value || 'Required'
      },
      dropzoneOptions: {
        url: 'fake_url',
        acceptedFiles: 'text/plain, application/rtf',
        maxFilesize: 12.5, // MB
        maxFiles: 1,
        addRemoveLinks: true,
        autoProcessQueue: false,
        dictDefaultMessage: `
            <div>
              <div>test</div>
              <div>test 2</div>
            </div>`,
        previewTemplate: `
          <div class="dz-preview dz-file-preview">
            <div class="dz-image">
              <div data-dz-thumbnail-bg></div>
            </div>
            <div class="dz-details">
              <div class="dz-filename"><span data-dz-name></span></div>
              <div class="dz-size"><span data-dz-size></span></div>
              <div class="dz-error-mark">Cannot upload file </br> (hover to see details)</i></div>
            </div>
            <div class="dz-error-message"><span data-dz-errormessage></span></div>
            <div class="dz-remove-custom" href="javascript:undefined;" data-dz-remove="">
              <i class="fa fa-close dz-remove-custom-icon"></i>
            </div>
          </div>
        `
      }
    }
  },
  methods: {
    fetchCiphersData () {
      axios.get('https://localhost:5001/api/ciphers').then((data) => {
        this.ciphers = data.data
      })
        .catch((e) => {
          console.log('error fetch', e)
        })
    },
    onFileUploadComplete (file) {
      const fileReader = new FileReader()

      fileReader.readAsText(file)
      fileReader.onload = (event) => {
        this.fileText = event.target.result
      }
    },
    downloadFile (text) {
      const blob = new Blob([text], { type: 'text/plain;charset=utf-8' })
      const currentDate = moment().format('YYYY-MM-DD_h-mm-ss')
      const operation = this.selectedOperation === 'encryption' ? 'encrypted' : 'decrypted'

      fileSaver.saveAs(blob, `${operation}_${currentDate}.txt`)
    },
    onSubmitForm () {
      axios.post('https://localhost:5001/api/ciphers/process', {
        data: this.fileText.trim(),
        name: this.selectedCipher.name,
        mode: this.selectedOperation,
        key: this.publicKeyInput.trim()
      }).then((data) => {
        this.$refs.fileDropzone.removeAllFiles(true)
        this.downloadFile(data.data.data)
      }).catch((e) => {
        console.log('error', e)
      })
    }
  },
  computed: {
    isSignedIn () {
      return this.$store.state.auth.status.signedIn
    },
    formattedCiphers () {
      return this.ciphers.map(cipher => ({
        text: cipher.name,
        value: cipher.name
      }))
    },
    selectedCipher () {
      return this.ciphers.find(cipher => cipher.name === this.selectedCipherId)
    }
  }
}
</script>

<style lang="scss">
  .cipher-form__wrapper {
    height: 100%;
    padding: 20px 60px;
    background-color: #ffffff;
    border-radius: 7px;
    box-shadow: 0 4px 25px 0 rgba(0,0,0,.1);
    overflow: auto;

    .vue-dropzone {
      padding: 0;
    }

    #file-dropzone {
      height: 154px;

      .dropzone-custom-content {
        height: 150px;
        display: flex;
        align-items: center;
        justify-content: center;
        flex-direction: column;

        .dropzone-custom-title {
          color: #00b782;
        }

        .subtitle {
          color: #314b5f;
        }
      }

      .dz-message {
        margin: 0;
      }

      .dz-preview {
        position: relative;
        height: 130px;
        width: 130px;
        box-sizing: border-box;
        margin: 10px;

        .dz-details {
          padding: 30px 10px 10px 10px;
        }

        .dz-size {
          margin: 8px 0;
          text-align: center;
        }

        .dz-progress, .dz-remove {
          display: none;
        }

        .dz-error-mark {
          cursor: pointer;
          position: initial;
          text-align: center;
          font-size: 10px;
          color: #be2626;
        }

        .dz-remove-custom {
          position: absolute;
          z-index: 20;
          height: 16px;
          width: 16px;
          top: 5px;
          right: 5px;
          color: white;

          .dz-remove-custom-icon {
            cursor: pointer;
          }
        }
      }
    }

    .cipher-form__header {
      display: flex;
      justify-content: space-between;
      margin-bottom: 20px;
    }

    .cipher-form__submit-button {
      text-align: center;
      margin-top: 20px;
      cursor: pointer;
    }

    .cipher-form__inputs {
      margin-bottom: 15px;
      overflow-y: auto;
      overflow-x: hidden;
    }
  }
</style>
