<script>
export default {
  components: {},
  data() {
    return {};
  },
};
</script>
<template>
  <from id="xacthuc" class="repeater">
    <div class="row">
      <div class="col-12">
        <div class="inner-repeater mb-4">
          <form class="">
            <div class="mb-3">
              <label for="validationCode" class="form-label">Mã PIN</label>
              <span class="text-danger">*</span>
              <input
                type="password"
                class="form-control"
                id="validationCode"
                name="name"
                placeholder="Nhập mã pin..."
              />
            </div>
            <div class="mb-3">
              <label for="Note" class="form-label">Ghi chú</label>
              <sapn class="text-danger">*</sapn>
              <textarea
                type="text"
                class="form-control"
                id="Note"
                name="Note"
                row="2"
              ></textarea>
            </div>
          </form>
        </div>
      </div>
    </div>
  </from>
</template>
<style>
.btn-add {
  color: #06548e;
}
.btn-add:hover {
  color: #03406f;
}
</style>
