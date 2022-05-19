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
        lanhDaoButPhe: "",
        ngayButPhe: new Date(),
        file: "",
        mucDoTinhChat: "",
        nguoiPhuTrach: "",
        nguoiChuTri: "",
        nguoiPhoiHopXuLy: "",
        donViXuLy: "",
        donViPhoiHop: "",
        nguoiXemDeBiet: "",
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
      optionNguoiPhuTrach: [
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
      optionNguoiChuTri: [
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
      optionPhoiHopXuLy: [
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
      optionNguoiXemDeBiet: [
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
    <div class="col-md-6">
      <div class="row">
        <div class="col-md-6">
          <!-- Số lưu -->
          <label class="col-form-label col-form-label-sm">Số lưu</label>
          <span class="text-danger">*</span>
          <input
            type="text"
            class="form-control border-0"
            id="SoLuu"
            value="123-2020"
            disabled
          />
        </div>
        <div class="col-md-6">
          <!-- Số văn bản đến -->
          <label class="col-form-label col-form-label-sm">Số văn bản đến</label>
          <span class="text-danger">*</span>
          <input
            type="text"
            class="form-control border-0"
            id="SoVanBanDen"
            value="123/DTHU"
            disabled
          />
        </div>
      </div>
      <!-- Trích yếu -->
      <div>
        <div class="col-md-12">
          <label class="col-form-label col-form-label-sm">Trích yếu</label>
          <span class="text-danger">*</span>
          <textarea
            rows="2"
            class="form-control border-0"
            id="TrichYeu"
            disabled
          >
Báo cáo công tác 
          </textarea>
        </div>
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
      </div>
      <!-- Ngày bút phê -->
      <div>
        <label
          for="validationNgayButPhe"
          class="col-form-label col-form-label-sm"
          >Ngày bút phê</label
        >
        <flat-pickr
          v-model="form.ngayButPhe"
          :config="config"
          class="form-control"
        ></flat-pickr>
      </div>
      <!-- Mức độ tính chất -->
      <div>
        <label class="col-form-label col-form-label-sm">Mức độ tính chất</label>
        <div>
          <div class="form-check form-check-inline">
            <input
              class="form-check-input"
              type="radio"
              name="mucDoTinhChatOptions"
              id="MucDoTinhChatOptions"
              value="option1"
              checked
            />
            <label>Thấp</label>
          </div>
          <div class="form-check form-check-inline">
            <input
              class="form-check-input"
              type="radio"
              name="mucDoTinhChatOptions"
              id="MucDoTinhChatOptions"
              value="option2"
            />
            <label for="">Trung bình</label>
          </div>
          <div class="form-check form-check-inline">
            <input
              class="form-check-input"
              type="radio"
              name="mucDoTinhChatOptions"
              id="MucDoTinhChatOptions"
              value="option2"
            />
            <label for="">Cao</label>
          </div>
        </div>
      </div>
      <!-- file đính kèm -->
      <div class="input-group mt-1">
        <label class="input-group-text" for="inputGroupFile01"
          ><i class="ri-file-upload-line"></i
        ></label>
        <input type="file" class="form-control" id="inputGroupFile01" />
      </div>
    </div>
    <div class="col-md-6">
      <!-- Người phụ trách -->
      <div>
        <label for="nguoiPhuTrach" class="col-form-label col-form-label-sm"
          >Lãnh đạo bút phê</label
        >
        <treeselect
          placeholder="Chọn người phụ trách"
          v-model="form.nguoiPhuTrach"
          :options="optionNguoiPhuTrach"
        >
        </treeselect>
        <treeselect-value :value="form.loaiVanBan" />
      </div>
      <!-- Người chủ trì -->
      <div>
        <label for="nguoiChuTri" class="col-form-label col-form-label-sm"
          >Người chủ trì</label
        >
        <treeselect
          placeholder="Chọn người chủ trì"
          v-model="form.nguoiChuTri"
          :options="optionNguoiChuTri"
        >
        </treeselect>
        <treeselect-value :value="form.nguoiChuTri" />
      </div>
      <!-- Người phối hợp xử lý -->
      <div>
        <label
          for="validationLoaiVanBan"
          class="col-form-label col-form-label-sm"
          >Người phối hợp xử lý</label
        >
        <treeselect
          placeholder="Chọn người phối hợp xử lý"
          v-model="form.nguoiPhoiHopXuLy"
          :options="optionPhoiHopXuLy"
        >
        </treeselect>
        <treeselect-value :value="form.nguoiPhoiHopXuLy" />
      </div>
      <!-- Đơn vị xử lý -->
      <div>
        <label class="col-form-label col-form-label-sm">Đơn vị xử lý</label>
        <input
          type="text"
          class="form-control"
          id="DonViXuLy"
          v-model="form.donViXuLy"
        />
      </div>
      <!-- Đơn vị phối hợp -->
      <div>
        <label class="col-form-label col-form-label-sm">Đơn vị phối hợp</label>
        <input
          type="text"
          class="form-control"
          id="DonViPhoiHop"
          v-model="form.donViPhoiHop"
        />
      </div>
      <!-- Người xem để biết -->
      <div>
        <label for="nguoiXemDeBiet" class="col-form-label col-form-label-sm"
          >Lãnh đạo bút phê</label
        >
        <treeselect
          placeholder="Chọn người xem"
          v-model="form.nguoiXemDeBiet"
          :options="optionNguoiXemDeBiet"
        >
        </treeselect>
        <treeselect-value :value="form.nguoiXemDeBiet" />
      </div>
    </div>
  </div>
</template>

<style>
.card-body {
  min-height: 70vh;
}
</style>
