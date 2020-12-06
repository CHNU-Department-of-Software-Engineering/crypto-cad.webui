<template>
  <div class="file-dropzone__wrapper">
    <vue-dropzone
      id="file-dropzone"
      ref="fileDropzone"
      :options="dropzoneOptions"
      :useCustomSlot=true
      @vdropzone-file-added="onFileAdded"
    >
      <div class="file-dropzone__content">
        <h3 class="file-dropzone__title">Drag and drop to upload .txt file</h3>
        <div class="file-dropzone__subtitle">...or click to select a .txt file from your computer</div>
      </div>
    </vue-dropzone>
  </div>
</template>

<script>
import VueDropzone from 'vue2-dropzone'

export default {
  name: 'FileDropzone',
  components: {
    VueDropzone
  },
  data () {
    return {
      uploadFileData: '',
      dropzoneOptions: {
        url: 'fake_url', // Don`t need real url because we won`t send file to backend, only text
        acceptedFiles: 'text/plain',
        maxFilesize: 12.5, // MB
        maxFiles: 1,
        addRemoveLinks: true,
        autoProcessQueue: false,
        previewTemplate: this.template()
      }
    }
  },
  methods: {
    template () {
      return (`
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
          <div class="dz-remove-custom" data-dz-remove="">
            <i class="fa fa-close dz-remove-custom-icon"></i>
          </div>
        </div>
      `)
    },
    onFileAdded (file) {
      const fileReader = new FileReader()

      fileReader.readAsText(file)
      fileReader.onload = (event) => {
        this.uploadFileData = event.target.result
      }
    },
    removeAllFiles () {
      this.$refs.fileDropzone.removeAllFiles(true)
    }
  }
}
</script>

<style lang="scss">
  .file-dropzone__wrapper {
    .vue-dropzone {
      padding: 0;
    }

    #file-dropzone {
      height: 154px;

      .file-dropzone__content {
        height: 150px;
        display: flex;
        align-items: center;
        justify-content: center;
        flex-direction: column;

        .file-dropzone__title {
          color: #00b782;
        }

        .file-dropzone__subtitle {
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
  }
</style>
