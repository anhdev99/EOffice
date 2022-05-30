<template>
  <form class="">
    <input type="hidden" id="id" name="id" />
    <div class="mb-3">
      <label for="validationName" class="form-label">Tên nhóm quyền</label>
      <input type="text" class="form-control" id="validationName" name="name" />
    </div>
    <div class="mb-3">
      <label for="description" class="form-label">Thứ tự</label>
      <input
        type="number"
        class="form-control"
        id="validationNunber"
        name="number"
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
