<template>
  <form class="">
    <div class="mb-3">
      <label for="validationName" class="form-label">Tên cấp cơ quan</label>
      <input type="text" class="form-control" id="validationName" name="name" />
    </div>
    <div class="mb-3">
      <label for="description" class="form-label">Thứ tự</label>
      <input
        type="number"
        class="form-control"
        id="validationNumber"
        name="number"
        min="1"
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
