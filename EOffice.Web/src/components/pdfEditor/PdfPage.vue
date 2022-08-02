<script>

export default {
  name: "PdfPage",
  props: {
    page: {
      required: true
    }
  },
  data() {
    return {
      width: 0,
      height: 0
    }
  },
  mounted() {
    this.setupPage()
  },
  beforeMount() {
    window.removeEventListener("resize", this.measure)
  },
  methods: {
    async setupPage() {
      const _page = await this.page
      const context = this.$refs.pdfCanvas.getContext("2d")
      const viewport = _page.getViewport({scale: 1, rotation: 0})
      this.width = viewport.width
      this.height = viewport.height
      await _page.render({
        canvasContext: context,
        viewport
      }).promise
      this.measure()
      window.addEventListener("resize", this.measure)
    },

    measure() {
      this.$emit('measure', this.$refs.pdfCanvas.clientWidth / this.width)
      return;
    }
  }
}
</script>
<template>
  <div>
    <canvas
        ref="pdfCanvas"
        class="max-w-full"
        :style="{width: `${width}px`}"
        :width="width"
        :height="height"/>
  </div>
</template>
