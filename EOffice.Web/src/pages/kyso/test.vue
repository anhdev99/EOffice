<template>
  <div >
    <WebViewer
        initialDoc="https://pdftron.s3.amazonaws.com/downloads/pl/demo-annotated.pdf"
    />
    <div> abc</div>
  </div>
</template>

<script>
import WebViewer from "../../components/WebViewer";

export default {
  components: {
    WebViewer,
  },
};
</script>