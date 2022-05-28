<template>
  <form class="">
    <div class="mb-3">
      <label for="validationName" class="form-label">Tên loại nguồn vốn</label>
      <input type="text" class="form-control" id="validationName" name="name" />
    </div>
    <div class="mb-3">
      <label for="description" class="form-label">Mô tả</label>
      <textarea
        type="text"
        class="form-control"
        id="description"
        name="description"
        rows="2"
      />
    </div>
  </form>
</template>

<script>
export default {
  data() {
    return {};
  },
};
</script>

<style></style>
