<script>
import {Fonts} from "@/components/pdfEditor/utils/prepareAssets";
import {timeout} from "@/components/pdfEditor/utils/helper";

export default {
  name: "TextEditor",
  props: {
    size: {required: false},
    text: {required: false},
    lineHeight: {required: false},
    fontFamily: {required: false},
    pageScale: {required: false, default: 1},
    operation:{required: true},
    lines:{required: false}
  },
  data() {
    return {
      Families: Object.keys(Fonts),
      startX: 0,
      startY: 0,
      dx: 0,
      dy: 0,
      data:{
        x: 0,
        y: 0,
      },
      timeout: null
    }
  },
  computed:{
    moveOperation: function () {
      return this.operation == "move"
    },
  },
  mounted() {
    this.render();
    this.$refs.editable.addEventListener('mousedown', this.handleMousedown)
    this.$refs.editable.addEventListener('keydown', this.onKeydown)
  },
  methods: {
    handlePanMove(event) {
      this.dx = (event.detail.x - this.startX) / this.pageScale;
      this.dx = (event.detail.x - this.startX) / this.pageScale;
    },
    handlePanEnd() {
      if (this.dx === 0 && this.dy === 0) {
        return this.$refs.editable.focus();
      }
      this.$emit('update', {
        x: this.data.x ,
        y: this.data.y
      })
      this.dx = 0;
      this.dy = 0;
      // eslint-disable-next-line vue/no-mutating-props
      this.operation = "";
    },
    handlePanStart(event) {
      this.startX = event.detail.x;
      this.startY = event.detail.y;
      // eslint-disable-next-line vue/no-mutating-props
      this.operation = "move";
    },
    onFocus() {
      // eslint-disable-next-line vue/no-mutating-props
      this.operation = "edit";
      console.log("onFocus")
    },
    async onBlur() {
      if (this.operation !== "edit" || this.operation === "tool") return;
      this.$refs.editable.blur();
      this.sanitize();
      this.$emit('update', {
        lines: this.extractLines(),
        width: this.$refs.editable.clientWidth
      });
      // eslint-disable-next-line vue/no-mutating-props
      this.operation = "";
    },
    async onPaste(e) {
      // get text only
      const pastedText = e.clipboardData.getData("text");
      document.execCommand("insertHTML", false, pastedText);
      // await tick() is not enough
      await timeout();
      this.sanitize();
    },
    onKeydown(e) {
      const childNodes = Array.from(this.$refs.editable.childNodes);
      if (e.keyCode === 13) {
        // prevent default adding div behavior
        e.preventDefault();
        const selection = window.getSelection();
        const focusNode = selection.focusNode;
        const focusOffset = selection.focusOffset;
        // the caret is at an empty line
        if (focusNode === this.$refs.editable) {
          this.$refs.editable.insertBefore(
              document.createElement("br"),
              childNodes[focusOffset]
          );
        } else if (focusNode instanceof HTMLBRElement) {
          this.$refs.editable.insertBefore(document.createElement("br"), focusNode);
        }
        // the caret is at a text line but not end
        else if (focusNode.textContent.length !== focusOffset) {
          document.execCommand("insertHTML", false, "<br>");
          // the carat is at the end of a text line
        } else {
          let br = focusNode.nextSibling;
          if (br) {
            this.$refs.editable.insertBefore(document.createElement("br"), br);
          } else {
            br = this.$refs.editable.appendChild(document.createElement("br"));
            br = this.$refs.editable.appendChild(document.createElement("br"));
          }
          // set selection to new line
          selection.collapse(br, 0);
        }
      }
      this.searchTimeOut()
    },

    onFocusTool() {
      // eslint-disable-next-line vue/no-mutating-props
      this.operation = "tool";
    },
    onBlurTool() {
      console.log("this.extractLines()", this.extractLines())
      this.$emit('textEnd', {
        lines: this.extractLines(),
        lineHeight: 1,
        size: 16,
        fontFamily:"Times-Roman",
        height: this.$refs.editable.clientHeight,
        width: this.$refs.editable.clientWidth,
      })
      // eslint-disable-next-line vue/no-mutating-props
      this.operation = "";
    },
    onChangeFont() {
      this.$emit('selectFont', {
        name: this.fontFamily
      })
    },
    render() {
      this.$refs.editable.innerHTML = this.text;
      clearTimeout(this.timeout);

      // Make a new timeout set to go off in 800ms
      this.timeout = setTimeout(() => {
        this.$emit('textEnd', {
          lines: this.extractLines(),
          lineHeight: 1,
          size: 16,
          fontFamily:"Times-Roman",
          height: this.$refs.editable.clientHeight,
          width: this.$refs.editable.clientWidth,
        })

      }, 400);

      // this.$refs.editable.focus();
    },
    onDelete() {
      this.$emit('delete')
    },
    sanitize() {
      let weirdNode;
      while (
          (weirdNode = Array.from(this.$refs.editable.childNodes).find(
              node => !["#text", "BR"].includes(node.nodeName)
          ))
          ) {
        this.$refs.editable.removeChild(weirdNode);
      }
    },
    extractLines() {
      const nodes = this.$refs.editable.childNodes;
      const lines = [];
      let lineText = "";
      for (let index = 0; index < nodes.length; index++) {
        const node = nodes[index];
        if (node.nodeName === "BR") {
          lines.push(lineText);
          lineText = "";
        } else {
          lineText += node.textContent;
        }
      }
      lines.push(lineText);
      return lines;
    },
    handleMousedown(event) {
      this.data.x = event.clientX
      this.data.y = event.clientY
      const target = event.target
      this.$emit('panstart', {
        x:   this.data.x,
        y:   this.data.y,
        target,
        currentTarget:   this.$refs.editable
      })
      this.$refs.editable.addEventListener('mousemove', this.handleMousemove)
      this.$refs.editable.addEventListener('mouseup', this.handleMouseup)
    },
    handleMouseup(event) {
      this.data.x = event.clientX
      this.data.y = event.clientY
      this.$emit('panend', {x:   this.data.x, y:   this.data.y})

      this.$refs.editable.removeEventListener('mousemove', this.handleMousemove)
      this.$refs.editable.removeEventListener('mouseup', this.handleMouseup)
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
    handleTouchStart(event) {
      if (event.touches.length > 1) return
      const touch = event.touches[0]
      this.data.x = touch.clientX
      this.data.y = touch.clientY
      const target = touch.target

      this.$emit('panstart', {x:   this.data.x, y:   this.data.y, target})
      this.$refs.editable.addEventListener('touchmove',   this.handleTouchmove) // { passive: false }
      this.$refs.editable.addEventListener('touchend',   this.handleTouchend)
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
      this.$refs.editable.removeEventListener('touchmove',   this.handleTouchmove)
      this.$refs.editable.removeEventListener('touchend',   this.handleTouchend)
    },
    searchTimeOut() {

      clearTimeout(this.timeout);

      // Make a new timeout set to go off in 800ms
      this.timeout = setTimeout(() => {
        this.onBlurTool()

      }, 400);

    }
  }
}
</script>
<template>
  <div>
    <div
        ref="editable"
        @focus={onFocus}
        @keyup={onKeydown}
        contenteditable="true"
        spellcheck="false"
        @mousedown.passive="handleMousedown"
        @touchstart.passive="handleTouchStart"
        class="tw-cursor-grab tw-border tw-border-dotted tw-outline-none tw-whitespace-nowrap"
        style="padding: 10px; -webkit-user-select: text; display: inline-block; white-space: nowrap; cursor: pointer"
        :class="['tw-cursor-grab ',
            { 'tw-cursor-grabbing': moveOperation }]"
    />
  </div>
</template>