<script>
import Treeselect from "vue3-treeselect";
// import the styles
import "vue3-treeselect/dist/vue3-treeselect.css";

export default {
  components: { Treeselect },
  data() {
    return {
      nguoiThucHien: "",
      optionNguoiThucHien: [
        {
          id: "1",
          label: "Khoa CNTT",
          children: [
            {
              id: "2",
              label: "Người số 1",
            },
            {
              id: "3",
              label: "Người số 2",
            },
          ],
        },
        {
          id: "4",
          label: "Khoa SP & KHXH",
          children: [
            {
              id: "5",
              label: "Người số 3",
            },
            {
              id: "6",
              label: "Người số 4",
            },
          ],
        },
        {
          id: "7",
          label: "Khoa chính trị",
          children: [
            {
              id: "8",
              label: "Người số 5",
            },
            {
              id: "9",
              label: "Người số 6",
            },
          ],
        },
      ],
      tempPhanCongData: [{ id: 1 }],
    };
  },
  methods: {
    addTempPhanCong() {
      this.tempPhanCongData.push({ nguoiThucHien: "", ghiChu: "" });
    },
    deleteTempPhanCong(index) {
      this.tempPhanCongData.splice(index, 1);
    },
  },
};
</script>
<template>
  <from id="PhanCong" class="repeater p-3" @submit.prevent="PhanCong">
    <div class="row">
      <div class="col-12">
        <div class="inner-repeater mb-4">
          <div class="inner mb-3">
            <div
              v-for="(data, index) in tempPhanCongData"
              :key="data.id"
              class="inner mb-3 row"
            >
              <div class="col-md-11">
                <!-- Người thực hiện -->
                <div class="row mb-2">
                  <div class="col-lg-3">
                    <label class="form-label">Người thực hiện</label>
                  </div>
                  <div class="col-lg-9">
                    <treeselect
                      placeholder="Chọn lãnh đạo bút phê"
                      v-model="data.nguoiThucHien"
                      :options="optionNguoiThucHien"
                    >
                    </treeselect>
                    <treeselect-value :value="data.nguoiThucHien" />
                  </div>
                </div>
                <!-- Ghi chú -->
                <div class="row mb-2">
                  <div class="col-lg-3">
                    <label for="" class="form-label">Ghi chú</label>
                  </div>
                  <div class="col-lg-9">
                    <textarea
                      v-model="data.ghiChu"
                      placeholder="Nhập ghi chú..."
                      name="note"
                      id="note"
                      rows="2"
                      class="form-control"
                    >
                    </textarea>
                  </div>
                </div>
              </div>
              <div class="col-1 d-flex align-items-center">
                <div class="d-grid">
                  <button
                    type="button"
                    class="btn btn-outline-danger btn-icon waves-effect waves-light border-0"
                    @click="deleteTempPhanCong(index)"
                  >
                    <i class="ri-delete-bin-line"></i>
                  </button>
                </div>
              </div>
            </div>
          </div>
          <div class="d-flex justify-content-end">
            <button
              type="button"
              class="btn rounded-pill waves-effect waves-light btn-add p-0"
              @click="addTempPhanCong"
            >
              <i class="ri-add-circle-fill fs-1 me-2"></i>
            </button>
          </div>
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
