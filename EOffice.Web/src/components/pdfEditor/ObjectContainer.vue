<script>
// import ObjectSignature from './objects/ObjectSignature.vue'
import ObjectImage from "@/components/pdfEditor/objects/ObjectImage";
import TextEditor from "@/components/pdfEditor/Text"
export default {
  components: {ObjectImage, TextEditor},
  props: {
    payload: {required: true},
    x: {required: true},
    y: {required: true},
    file: {required: false},
    width: {required: true},
    height: {required: true},
    pageScale: {required: true},
    opacity: {required: true},
    type: {required: true},
    path: {required: false, default: null},
    object: {
      required: true,
      type: Object,
    },
    imageBase64: {required: false}
  },
  data() {
    return {
      startX: 0,
      startY: 0,
      directions: [],
      dx: 0,
      dy: 0,
      dw: 0,
      dh: 0,
      pannableFunction: null,
      operation: null,
      signature: null,
    }
  },
  computed: {
    moveOperation() {
      return this.operation === 'move'
    },
    textEditor(){
      if(this.object){
        if(this.object.lines){
          return this.object.lines[0]
        }
      }
      return "Hãy nhập dữ liệu ..."
    }
  },
  mounted() {
    this.setCanvas()
  },
  methods: {
    setCanvas() {
      if (this.type == 'image') {
        let cvn = this.$refs.canvasImage;
        // use canvas to prevent img tag's auto resize
        this.$refs.canvasImage.width = this.width
        this.$refs.canvasImage.height = this.height

        // this.$refs.canvasImage.getContext('2d').drawImage(this.payload, 0, 0)
        // // if(this.payload){
        // //   this.$refs.canvasImage.getContext('2d').drawImage(this.payload, 0, 0)
        // // }else{
        // //   var image = new Image();
        // //   image.src = this.imageBase64;
        // //   image.onload = function() {
        // //     this.$refs.canvasImage.getContext('2d').drawImage(image, 0, 0)
        // //   };
        // // }
        let ctx = cvn.getContext("2d");
        let bg = new Image();
        bg.width = this.width;
        bg.height = this.height;
        bg.src = this.imageBase64;
        bg.onload = function() {
          ctx.drawImage(bg, 0 ,0, this.width, this.height);
        };
        //
        //   var image = new Image();
        //
        //   image.onload = function() {
        //   };
        // image.src = this.imageBase64;
        // image.width = this.width;
        // image.height = this.height;
        //
        // this.$refs.canvasImage.getContext('2d').drawImage(image, 0, 0)
        // const canvas = document.getElementById('canvasImage');
        // const context = canvas.getContext('2d');
        // context.drawImage(image, image.width, image.height);

        let scale = 1
        const limit = 500
        if (this.width > limit) {
          scale = limit / this.width
        }
        if (this.height > limit) {
          scale = Math.min(scale, limit / this.height)
        }
        this.$emit('update', {
          width: this.width * scale,
          height: this.height * scale,
        })

        if (!['image/jpeg', 'image/png'].includes(this.file.type)) {
          this.$refs.canvasImage.toBlob((blob) => {
            this.$emit('update', {
              file: blob,
            })
          })
        }
      }else if (this.type == 'text') {
        // use canvas to prevent img tag's auto resize
        // this.$refs.canvasText.width = this.width
        // this.$refs.canvasText.height = this.height

        let scale = 1
        const limit = 500
        if (this.width > limit) {
          scale = limit / this.width
        }
        if (this.height > limit) {
          scale = Math.min(scale, limit / this.height)
        }
        this.$emit('update', {
          width: this.width * scale,
          height: this.height * scale,
        })
      }
      else if (this.type == 'signature') {
        // this.value.setAttribute("viewBox", `0 0 200 200`);
      }
    },

    handlePanMove(event) {
      const _dx = (event.x - this.startX) / this.pageScale
      const _dy = (event.y - this.startY) / this.pageScale
      if (this.operation === 'move') {
        this.dx = _dx
        this.dy = _dy
      } else if (this.operation === 'scale') {
        if (this.directions.includes('left')) {
          this.dx = _dx
          this.dw = -_dx
        }
        if (this.directions.includes('top')) {
          this.dy = _dy
          this.dh = -_dy
        }
        if (this.directions.includes('right')) {
          this.dw = _dx
        }
        if (this.directions.includes('bottom')) {
          this.dh = _dy
        }
      }
    },

    handlePanEnd() {
      console.log("handlePanEnd")
      if (this.operation === 'move') {
        this.$emit('update', {
          x: this.x + this.dx,
          y: this.y + this.dy,
        })
        this.dx = 0
        this.dy = 0
      } else if (this.operation === 'scale') {
        this.$emit('update', {
          x: this.x + this.dx,
          y: this.y + this.dy,
          width: this.width + this.dw,
          height: this.height + this.dh,
        })

        this.dx = 0
        this.dy = 0
        this.dw = 0
        this.dh = 0
        this.directions = []
      }
      this.operation = ''
    },

    handlePanStart(event) {
      console.log("handlePanStart")
      this.startX = event.x
      this.startY = event.y
      if (event.target === event.currentTarget) {
        return (this.operation = 'move')
      }
      this.operation = 'scale'
      this.directions = event.target.dataset.direction.split('-')
    },
    handleEndText(event){
      let value = {...this.object,...{
          lines: event.lines,
          lineHeight: event.lineHeight,
          size: event.size,
          fontFamily: event.fontFamily,
          height: event.height,
          width: event.width,
        }}

      this.$emit('update', value)
    },
  }

}

</script>
<template>
  <div
      class="absolute left-0 top-0 select-none"
      :style="{ width: `${width + dw}px`, height: `${height + dh}px`, transform: `translate(${x + dx}px, ${y + dy}px)` }"
  >
    <object-image v-if="type== 'image'" :operation="operation" @panstart="handlePanStart" @panmove="handlePanMove" @panend="handlePanEnd"/>
    <TextEditor v-else-if="type == 'text'" :text="textEditor" :operation="operation" @panstart="handlePanStart" @panmove="handlePanMove" @panend="handlePanEnd" @textEnd="handleEndText"/>
    <!-- <object-signature v-else-if="type == 'signature'"
        :operation="operation"
        @panstart="handlePanStart"
        @panmove="handlePanMove"
        @panend="handlePanEnd" /> -->
    <div
        @click="$emit('delete')"
        class="absolute left-0 top-0 right-0 w-6 h-6 m-auto rounded-full bg-red-100 cursor-pointer transform -translate-y-1/2 md:scale-25 text-center border border-black"
    >
      <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-x-circle"
           viewBox="0 0 16 16">
        <path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14zm0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16z"/>
        <path
            d="M4.646 4.646a.5.5 0 0 1 .708 0L8 7.293l2.646-2.647a.5.5 0 0 1 .708.708L8.707 8l2.647 2.646a.5.5 0 0 1-.708.708L8 8.707l-2.646 2.647a.5.5 0 0 1-.708-.708L7.293 8 4.646 5.354a.5.5 0 0 1 0-.708z"
        />
      </svg>
    </div>
    <canvas v-if="type == 'image'" class="w-full h-full" ref="canvasImage" id="canvasImage"/>
  </div>
</template>
