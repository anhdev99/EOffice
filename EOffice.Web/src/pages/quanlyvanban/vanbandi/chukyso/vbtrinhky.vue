<script>
import pdf from "vue-pdf";

export default {
  components: { pdf },
  data() {
    return {};
  },
};
</script>

<template>
  <div class="row p-4">
    <div class="col-md-6">
      <label class="col-form-label col-form-label-sm">Thông tin văn bản</label>
      <pdf src="../../../../../assets/file/DH_GDCT.pdf"> </pdf>
    </div>
    <div class="col-md-6"></div>
  </div>
</template>

<style></style>
