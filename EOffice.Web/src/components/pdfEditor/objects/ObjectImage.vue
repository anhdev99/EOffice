<script>
export default {
  name: "ObjectImage",
  props: {
    operation: {required: true}
  },
  data() {
    return {
      signatureContainer: null,
      data:{
        x: 0,
        y: 0,
      }
    }
  },
  mounted() {
     this.setupEvent()
  },
  computed:{
    moveOperation: function () {
      return this.operation == "move"
    }
  },
  methods: {
    setupEvent() {
      this.$refs.signatureContainer.addEventListener('mousedown', this.handleMousedown)
      this.$refs.signatureContainer.addEventListener('touchstart', this.handleTouchStart)
    },

    handleMousedown(event) {
     this.data.x = event.clientX
      this.data.y = event.clientY
      const target = event.target
      this.$emit('panstart', {
        x:   this.data.x,
        y:   this.data.y,
        target,
        currentTarget:   this.$refs.signatureContainer
      })
      this.$refs.signatureContainer.addEventListener('mousemove', this.handleMousemove)
      this.$refs.signatureContainer.addEventListener('mouseup', this.handleMouseup)
    },

    handleMousemove(event) {
      const dx = event.clientX -   this.data.x
      const dy = event.clientY -   this.data.y
      this.data.x = event.clientX
      this.data.y = event.clientY
      this.$emit('panmove', {
        x:   this.data.x,
        y:   this.data.y,
        dx,
        dy
      })
    },

    handleMouseup(event) {
      this.data.x = event.clientX
      this.data.y = event.clientY

      this.$emit('panend', {x:   this.data.x, y:   this.data.y})

      this.$refs.signatureContainer.removeEventListener('mousemove', this.handleMousemove)
      this.$refs.signatureContainer.removeEventListener('mouseup', this.handleMouseup)
    },

    handleTouchStart(event) {
      if (event.touches.length > 1) return
      const touch = event.touches[0]
      this.data.x = touch.clientX
      this.data.y = touch.clientY
      const target = touch.target

      this.$emit('panstart', {x:   this.data.x, y:   this.data.y, target})
      this.$refs.signatureContainer.addEventListener('touchmove',   this.handleTouchmove) // { passive: false }
      this.$refs.signatureContainer.addEventListener('touchend',   this.handleTouchend)
    },

    handleTouchmove(event) {
      event.preventDefault()
      if (event.touches.length > 1) return
      const touch = event.touches[0]
      const dx = touch.clientX -   this.data.x
      const dy = touch.clientY -   this.data.y
      this.data.x = touch.clientX
      this.data.y = touch.clientY

      this.$emit('panmove', {x:   this.data.x, y:   this.data.y, dx, dy})
    },

    handleTouchend(event) {
      const touch = event.changedTouches[0]
      this.data.x = touch.clientX
      this.data.y = touch.clientY

      this.$emit('panend', {x:   this.data.x, y:   this.data.y})
      this.$refs.signatureContainer.removeEventListener('touchmove',   this.handleTouchmove)
      this.$refs.signatureContainer.removeEventListener('touchend',   this.handleTouchend)
    }

  }
}
</script>
<template>
  <div ref="signatureContainer"
       @mousedown.passive="handleMousedown"
       @touchstart.passive="handleTouchStart"
       :class="['tw-absolute tw-w-full tw-h-full cursor-grab operation',
            { 'cursor-grabbing': moveOperation }]">
    <div data-direction="left"
         class="resize-border tw-h-full tw-w-1 tw-left-0 tw-top-0 tw-border-l cursor-ew-resize" />
    <div data-direction="top"
         class="resize-border tw-w-full tw-h-1 tw-left-0 tw-top-0 tw-border-t cursor-ns-resize" />
    <div data-direction="bottom"
         class="resize-border tw-w-full tw-h-1 tw-left-0 tw-bottom-0 tw-border-b cursor-ns-resize" />
    <div data-direction="right"
         class="resize-border tw-h-full tw-w-1 tw-right-0 tw-top-0 tw-border-r cursor-ew-resize" />
    <div data-direction="left-top"
         class="resize-corner tw-left-0 tw-top-0 cursor-nwse-resize transform-translate-x-1/2 tw--translate-y-1/2 md:scale-25" />
    <div data-direction="right-top"
         class="resize-corner tw-right-0 tw-top-0 cursor-nesw-resize tw-transform
                tw-translate-x-1/2 tw--translate-y-1/2 md:scale-25" />
    <div data-direction="left-bottom"
         class="resize-corner tw-left-0 tw-bottom-0 cursor-nesw-resize tw-transform
                tw--translate-x-1/2 tw-translate-y-1/2 md:scale-25" />
    <div data-direction="right-bottom"
         class="resize-corner tw-right-0 tw-bottom-0 cursor-nwse-resize tw-transform
            tw-translate-x-1/2 tw-translate-y-1/2 md:scale-25" />
  </div>
</template>
<style>
    .operation {
        background-color: rgba(0, 0, 0, 0.3)
    }
    .resize-border {
        position: absolute;
        border-style: dashed;
        border-color: #718096;
    }
    .resize-corner {
        position: absolute;
        width: .7rem;
        height: .7rem;
        background-color: #3475E0;
        border-radius: 9999px;
    }
</style>
