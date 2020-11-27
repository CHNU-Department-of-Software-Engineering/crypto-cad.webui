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
          label="Select Cipher"
          outlined
        ></v-select>
      </v-col>
      <v-col offset="4" cols="4">
        <v-select
          v-model="selectedOperation"
          :items="cipherOperations"
          label="Select Cipher Operation"
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
           :rules="[publicKeyRules.required]"
           outlined
         ></v-text-field>
       </v-col>
     </v-row>
   </div>
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
</template>

<script>
import ciphers from '../../assets/mocks/ciphers'
import axios from 'axios'
import vueDropzone from 'vue2-dropzone'

export default {
  name: 'CipherForm',
  components: {
    vueDropzone
  },
  data () {
    return {
      ciphers,
      selectedCipherId: 1,
      isEncrypt: true,
      isSignedIn: localStorage.getItem('isSignedIn'),
      fileText: '',
      cipherOperations: [
        {
          value: 'encrypt',
          text: 'Encrypt'
        },
        {
          value: 'decrypt',
          text: 'Decrypt'
        }
      ],
      selectedOperation: 'encrypt',
      publicKeyInput: '',
      publicKeyRules: {
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
    onFileAdded (file) {
      const fileReader = new FileReader()

      fileReader.readAsText(file)
      fileReader.onload = (event) => {
        this.fileText = event.target.result
      }
    },
    download (filename, text) {
      const element = document.createElement('a')
      element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(text))
      element.setAttribute('download', filename)

      element.style.display = 'none'
      document.body.appendChild(element)

      element.click()

      document.body.removeChild(element)
    },
    onSubmitForm () {
      axios({
        url: 'myuploadurl',
        method: 'POTS',
        data: {
          text: this.fileText.trim(),
          cipher: this.ciphers.find(cipher => cipher.id === this.selectedCipherId),
          encrypt: this.selectedOperation === 'encrypt',
          publicKey: this.publicKeyInput.trim()
        }
      }).then().catch(() => {
        const currentDate = new Date()
        this.download(`encrypted_${currentDate.getUTCMilliseconds()}`, this.fileText.trim())
      })
    }
  },
  computed: {
    formattedCiphers () {
      return this.ciphers.map(cipher => ({
        text: cipher.name,
        value: cipher.id
      }))
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

    .dropzone-custom-content {
      height: 170px;
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
      margin-bottom: 50px;
    }

    .cipher-form__submit-button {
      text-align: center;
      margin: 50px 0 30px 0;
    }

    .cipher-form__inputs {
      height: calc(100% - 610px);
      margin-bottom: 15px;
      overflow-y: auto;
      overflow-x: hidden;
    }
  }
</style>
