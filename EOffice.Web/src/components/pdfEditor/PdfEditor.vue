<script>
import {readAsImage, readAsPDF, readAsDataURL} from './utils/asyncReader'
import prepareAssets, {fetchFont} from './utils/prepareAssets'
import {save} from './utils/PDF'
import {generateId} from './utils/helper'
import PdfPage from "@/components/pdfEditor/PdfPage";
 import ObjectContainer from "@/components/pdfEditor/ObjectContainer";

export default {
  components: { ObjectContainer, PdfPage},
  props: {
    pdf: {
      required: true,
      type: String,
    },
  },
  data() {
    return {
      pages: [],
      allObjects: [],
      pagesScale: [],
      selectedPageIndex:0,
      signatureCanvas:{
        isShow: false
      },
      opacity: 1,
      pdfFile: null,
    }
  },
  computed:{
    isShow() {
      if(this.$refs.signatureCanvas){
        return this.$refs.signatureCanvas.isShow;
      }
      return false;

    }
  },
  watch:{
    selectedPageIndex(value){
      console.log(value)
    }
  },
  mounted() {
    this.mountPdf()
  },
  methods: {
    async mountPdf() {
      try {
        const res = await fetch(this.pdf)
        const pdfBlob = await res.blob()
        await this.addPDF(pdfBlob)

        this.selectedPageIndex = 0

        setTimeout(() => {
          fetchFont('Times-Roman')
          prepareAssets()
        }, 5000)
      } catch (e) {
        console.log('Error:', e)
      }
    },

    async addPDF(file) {
      try {
        const pdf = await readAsPDF(file)
        this.pdfFile = file
        const numPages = pdf.numPages

        // this.pages = pdf;
        //  this.pages.splice(0, numPages)
        if(this.pages){
          this.pages.splice(0, this.pages.length)
        }
        this.pages = Array(numPages).fill().map((_, i) => pdf.getPage(i + 1))

        if(this.allObjects){
          this.allObjects.splice(0, this.allObjects.length)
        }
        this.pagesScale = Array(numPages).fill(1)
      } catch (e) {
        console.log('Failed to add pdf. Please try again.', e)
      }
    },

    onMeasure(scale, pageIndex) {
      this.pagesScale[pageIndex] = scale
    },

    selectPage(index) {
      this.selectedPageIndex = index;
      console.log(this.pages)
      console.log(this.allObjects)
      if(this.allObjects && this.allObjects.length > 0){
        this.allObjects[0].page = index;
      }
      // if(
      //
      // )
      // this.allObjects[0].page = index;
    },

    updateObject(objectId, payload) {
      this.allObjects = this.allObjects.map((object) => {
        if (object.page == this.selectedPageIndex && object.id === objectId) {
          return {...object, ...payload}
        } else {
          return object
        }
      })
    },

    deleteObject(objectId) {
      this.allObjects = this.allObjects.filter((object) => object.page == this.selectedPageIndex && object.id !== objectId)
    },

    async addImage(file) {
      try {
        // get dataURL to prevent canvas from tainted
        const url = await readAsDataURL(file)
        const img = await readAsImage(url)
        const id = Math.floor(Math.random() * 100)
        const {width, height} = img

        const object = {
          id,
          type: 'image',
          width,
          height,
          x: 0,
          y: 0,
          payload: img,
          file: file,
          page: this.selectedPageIndex,
        }
        this.allObjects = [...this.allObjects,object];
      } catch (e) {
        console.log('Failed to add image.', e)
      }
    },

    async download() {
      try {
        console.log(this.allObjects)
        await save(this.pdfFile, this.allObjects, 'PDF Copy', this.pagesScale)
      } catch (e) {
        console.log('Error on saving, please try again.', e)
      }
    },

    uploadImage(e) {
      const file = e.target.files[0]
      if (file && this.selectedPageIndex >= 0) {
        this.addImage(file)
      }
      e.target.value = null
    },

    addSignature() {
      // @TODO check selectedPageIndex
      this.$refs.signatureCanvas.isShow = true
    },

    pasteSignature({width, height, path, scale}) {
      const id = generateId()

      const signatureObject = {
        id,
        path,
        type: 'signature',
        x: 0,
        y: 0,
        height,
        width: width * scale,
        scale: 500,
        page: this.selectedPageIndex,
      }
      this.allObjects = [signatureObject]
      this.$refs.signatureCanvas.isShow = false
    }

  }
}


</script>
<template>
  <div class="mb-4 flex flex-col relative">
    <div class="">
      <div style="display: flex; justify-content: space-between ">
        <div>
          <input type="file" id="image" name="image" class="hidden" @change="uploadImage"/>
          <label for="image"
                 class="text-black border border-black cursor-pointer font-medium text-sm px-5 py-2.5 text-center mr-2 mb-2">
            Chữ ký
          </label>
        </div>

        <button
            class="text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium text-sm px-5 py-2.5 text-center mr-2 mb-2 dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
            @click="download"
        >
        Thiết lập ký số
        </button>
      </div>

    </div>
    <div class="w-full overflow-auto">
      <div
          v-for="(page, pageIndex) in pages"
          :key="pageIndex"
          class="p-1 w-full flex flex-col items-center overflow-hidden"
          @mousedown="() => selectPage(pageIndex)"
          @touchstart="() => selectPage(pageIndex)"
      >
        <div  :class="['relative shadow-lg mb-4', { 'selected-pdf': pageIndex == selectedPageIndex }]">
          <pdf-page :page="pages[pageIndex]" @measure="(payload) => onMeasure(payload, pageIndex)"/>
          <div
              v-if="pageIndex == selectedPageIndex"
              class="absolute top-0 left-0 transform origin-top-left"
              :style="{ transform: `scale(${pagesScale[pageIndex]})`, touchAction: 'none' }"
          >
            <div v-for="(object, objectIndex) in allObjects"
                 :key="objectIndex">
              <object-container
                  @update="(payload) => updateObject(object.id, payload)"
                  @delete="() => deleteObject(object.id)"
                  :file="object.file"
                  :payload="object.payload"
                  :x="object.x"
                  :y="object.y"
                  :width="object.width"
                  :height="object.height"
                  :opacity="opacity"
                  :pageScale="pagesScale[pageIndex]"
                  :type="object.type"
                  :path="object.type"
                  :object="object"
              />
            </div>

          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<style >
.selected-pdf {
  box-shadow: 0 0 0 1px rgba(52, 117, 224, 0.5);
  border: 1px #14b0ef solid;
}

.slider-blue {
  --slider-connect-bg: #3475e0;
  --slider-tooltip-bg: #3475e0;
  --slider-handle-ring-color: #3475e0;
}
</style>
