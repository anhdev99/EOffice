<script>
// import ObjectSignature from './objects/ObjectSignature.vue'
import ObjectImage from "@/components/pdfEditor/objects/ObjectImage";

export default {
  components: {ObjectImage},
  props: {
    payload: {required: true},
    x: {required: true},
    y: {required: true},
    file: {required: true},
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
    }
  },
  mounted() {
    this.setCanvas()
  },
  methods: {
    setCanvas() {
      if (this.type == 'image') {

        // use canvas to prevent img tag's auto resize
        this.$refs.canvasImage.width = this.width
        this.$refs.canvasImage.height = this.height
        this.$refs.canvasImage.getContext('2d').drawImage(this.payload, 0, 0)

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
      } else if (this.type == 'signature') {
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
    }
  }

}

</script>
<template>
  <div
      class="absolute left-0 top-0 select-none"
      :style="{ width: `${width + dw}px`, height: `${height + dh}px`, transform: `translate(${x + dx}px, ${y + dy}px)` }"
  >
    <object-image :operation="operation" @panstart="handlePanStart" @panmove="handlePanMove" @panend="handlePanEnd"/>

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
    <canvas v-if="type == 'image'" class="w-full h-full" ref="canvasImage"/>
    <svg v-else-if="type == 'signature'" ref="signature" :viewBox="`0 0 ${width} ${height}`" width="100%" height="100%">
      <path stroke-width="5" stroke-linejoin="round" stroke-linecap="round" stroke="black" fill="none"
            :d="object.path"/>
    </svg>
  </div>
</template>
