<script>
import Treeselect from "vue3-treeselect";
// import the styles
import "vue3-treeselect/dist/vue3-treeselect.css";
import flatPickr from "vue-flatpickr-component";
import "flatpickr/dist/flatpickr.css";

export default {
  components: { Treeselect, flatPickr },
  data() {
    return {
      form: {
        butPhe: "",
        ngayButPhe: new Date(),
        lanhDaoButPhe: "",
      },
      config: {
        wrap: true, // set wrap to true only when using 'input-group'
        altFormat: "M j, Y",
        dateFormat: "d.m.y",
      },
      optionLanhDaoButPhe: [
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
    };
  },
};
</script>

<template>
  <div class="row card-body">
    <div class="col-md-12">
      <!-- Bút phê -->
      <div>
        <label for="validationButPhe" class="col-form-label col-form-label-sm"
          >Bút phê</label
        >
        <span class="text-danger">*</span>
        <input
          type="text"
          class="form-control form-control-sm"
          id="validationButPhe"
          value=""
          required
        />
        <div class="valid-feedback">Vui lòng thêm bút phê.</div>
      </div>
      <!-- Ngày bút phê -->
      <div>
        <label
          for="validationNgayButPhe"
          class="col-form-label col-form-label-sm"
          >Ngày ban hành</label
        >
        <span class="text-danger">*</span>
        <flat-pickr
          v-model="form.ngayButPhe"
          :config="config"
          class="form-control form-control-sm"
        ></flat-pickr>
        <div class="valid-feedback">Vui lòng thêm ngày bút phê.</div>
      </div>
      <!-- Lãnh đạo bút phê -->
      <div>
        <label
          for="validationLoaiVanBan"
          class="col-form-label col-form-label-sm"
          >Lãnh đạo bút phê</label
        >
        <treeselect
          placeholder="Chọn lãnh đạo bút phê"
          v-model="form.lanhDaoButPhe"
          :options="optionLanhDaoButPhe"
        >
        </treeselect>
        <treeselect-value :value="form.loaiVanBan" />
        <div class="invalid-feedback">Vui lòng chọn lãnh đạo bút phê.</div>
      </div>
    </div>
  </div>
</template>

<style>
.card-body {
  min-height: 70vh;
}
</style>
