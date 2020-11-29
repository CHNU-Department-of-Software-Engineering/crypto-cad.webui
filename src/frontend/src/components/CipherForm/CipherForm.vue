<template>
  <div class="cipher-form__wrapper">
    <div class="cipher-form__header">
      <h2 class="cipher-form__title">Cipher Settings</h2>
      <v-tooltip top>
        <template v-slot:activator="{ on, attrs }">
          <v-btn
            color="success"
            v-bind="attrs"
            v-on="isSignedIn ? on : null"
            outlined
            :disabled="!isSignedIn"
          >
            Add new Cipher
          </v-btn>
        </template>
        <span>Tooltip</span>
      </v-tooltip>
    </div>
    <v-row>
      <v-col cols="4">
        <v-select
          v-model="selectedCipherId"
          :items="formattedCiphers"
          :rules="[inputRules.required]"
          label="Select Cipher"
          outlined
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
              @vdropzone-file-added="onFileAdded"
            >
              <div class="dropzone-custom-content">
                <h3 class="dropzone-custom-title">Drag and drop to upload content</h3>
                <div class="subtitle">...or click to select a file from your computer</div>
              </div>
            </vue-dropzone>
          </v-col>
        </v-row>
      </div>
      <div class="cipher-form__submit-button">
        <v-btn outlined width="400px" color="success" @click="onSubmitForm">
          Submit
        </v-btn>
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios'
import fileSaver from 'file-saver'
import moment from 'moment'
import vueDropzone from 'vue2-dropzone'

export default {
  name: 'CipherForm',
  components: {
    vueDropzone
  },
  created () {
    this.fetchCiphersData()
  },
  data () {
    return {
      ciphers: [],
      selectedCipherId: '',
      isEncrypt: true,
      isSignedIn: localStorage.getItem('isSignedIn'),
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
        manuallyAddFile: true,
        autoProcessQueue: false
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
    onFileAdded (file) {
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

<style scoped lang="scss">
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

    .cipher-form__header {
      display: flex;
      justify-content: space-between;
      margin-bottom: 20px;
    }

    .cipher-form__submit-button {
      text-align: center;
      margin-top: 20px;
    }

    .cipher-form__inputs {
      margin-bottom: 15px;
      overflow-y: auto;
      overflow-x: hidden;
    }
  }
</style>
