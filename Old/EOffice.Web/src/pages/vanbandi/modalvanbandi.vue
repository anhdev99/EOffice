<script>
import { required } from "@vuelidate/validators";
// import the component
import Treeselect from "vue3-treeselect";
// import the styles
import "vue3-treeselect/dist/vue3-treeselect.css";
import flatPickr from "vue-flatpickr-component";
import "flatpickr/dist/flatpickr.css";

export default {
  components: { Treeselect, flatPickr },
  props: ['data'],
  data() {
    return {
      form: {
        soLuuCV: "",
        soCVDi: "",
        loaiVanBan: "",
        ngayNhap: new Date(),
        trangThai: "",
        traLoiCVSo: "",
        ngayTraLoi: new Date(),
        soBan: "",
        trichYeu: "",
        khoiCoQuanNhan: "",
        coQuanNhan: "",
        file: "",
        donViSoan: "",
        canBoSoan: "",
        hinhThucGui: "",
        linhVuc: "",
        mucDoTinhChat: "",
        mucDoBaoMat: "",
        hoSoDonVi: "",
        noiLuuTru: "",
        ghiChu: "",
      },
      optionLoaiVanBan: [
        {
          id: "1",
          label: "Văn bản hành chính",
        },
        {
          id: "2",
          label: "Văn bản thông hành",
        },
        {
          id: "3",
          label: "Văn bản thông báo",
        },
      ],
      optionTrangThai: [
        {
          id: "1",
          label: "Văn bản hành chính",
        },
        {
          id: "2",
          label: "Văn bản thông báo",
        },
        {
          id: "3",
          label: "Văn bản thông hành",
        },
      ],
      optionKhoiCoQuanNhan: [
        {
          id: "1",
          label: "Khối cơ quan 1",
        },
        {
          id: "2",
          label: "Khối cơ quan 2",
        },
        {
          id: "3",
          label: "Khối cơ quan 3",
        },
      ],
      optionCoQuanNhan: [
        {
          id: "1",
          label: "Cơ quan 1",
        },
        {
          id: "2",
          label: "Cơ quan 2",
        },
        {
          id: "3",
          label: "Cơ quan 3",
        },
      ],
      optionDoanViSoan: [
        {
          id: "1",
          label: "Đơn vị soạn 1",
        },
        {
          id: "2",
          label: "Đơn vị soạn 2",
        },
        {
          id: "3",
          label: "Đơn vị soạn 3",
        },
      ],
      optionCanBoSoan: [
        {
          id: "1",
          label: "Khoa CNTT",
          children: [
            {
              id: "8",
              label: "Cán bộ 1",
            },
            {
              id: "9",
              label: "Cán bộ 2",
            },
          ],
        },
        {
          id: "2",
          label: "Khoa KHXH",
          children: [
            {
              id: "8",
              label: "Cán bộ 1",
            },
            {
              id: "9",
              label: "Cán bộ 2",
            },
          ],
        },
        {
          id: "3",
          label: "Khoa công nghệ - Kỹ thuật",
          children: [
            {
              id: "8",
              label: "Cán bộ 1",
            },
            {
              id: "9",
              label: "Cán bộ 2",
            },
          ],
        },
      ],
      optionHinhThucGui: [
        {
          id: "1",
          label: "Hình thức gửi 1",
        },
        {
          id: "2",
          label: "Hình thức gửi 2",
        },
        {
          id: "3",
          label: "Hình thức gửi 3",
        },
      ],
      optionHoSoDonVi: [
        {
          id: "1",
          label: "Hồ sơ đơn vị 1",
        },
        {
          id: "2",
          label: "Hồ sơ đon vị 2",
        },
        {
          id: "3",
          label: "Hồ sơ đơn vị 3",
        },
      ],
      config: {
        wrap: true, // set wrap to true only when using 'input-group'
        altFormat: "M j, Y",
        dateFormat: "d.m.y",
      },
    };
  },
  validations: {
    form: {
      soLuu: { required },
      soVanBanDen: { required },
      loaiVanBan: { required },
      trichYeu: { required },
      nguoiKy: { required },
      file: { required },
    },
  },
  watch:{
    form: {
      deep: true,
          handler(val) {
        this.update('model')
        console.log("model", val);
      }
    }
  }
};
</script>

<template>
  <div class="p-4">
    <form class="row g-3 needs-validation" novalidate>
      <div class="col-md-7">
        <div class="row">
          <!-- Loại văn bản -->
          <div class="col-md-6">
            <label
              for="validationLoaiVanBan"
              class="col-form-label col-form-label-sm"
              >Loại văn bản</label
            >
            <span class="text-danger">*</span>
            <treeselect
              placeholder="Chọn loại văn bản"
              v-model="form.loaiVanBan"
              :options="optionLoaiVanBan"
            >
            </treeselect>
            <treeselect-value :value="form.loaiVanBan" />
            <div class="valid-feedback">Vui lòng chọn loại văn bản.</div>
          </div>
          <!-- Trạng thái -->
          <div class="col-md-6">
            <label
              for="validationTrangThai"
              class="col-form-label col-form-label-sm"
              >Trạng thái</label
            >
            <span class="text-danger">*</span>
            <treeselect
              placeholder="Chọn trạng thái"
              v-model="form.trangThai"
              :options="optionTrangThai"
            >
            </treeselect>
            <treeselect-value :value="form.trangThai" />
            <div class="valid-feedback">Vui lòng chọn trạng thái.</div>
          </div>
          <!-- số lưu CV-->
          <div class="col-md-4">
            <label
              for="validationSoLuuCV"
              class="col-form-label col-form-label-sm"
              >Số lưu CV</label
            >
            <input
              v-model="form.soLuuCV"
              type="text"
              class="form-control"
              id="validationSoLuuCV"
            />
          </div>
          <!-- Số CV đi-->
          <div class="col-md-4">
            <label
              for="validationSoVanBanDen"
              class="col-form-label col-form-label-sm"
              >Số VB đến</label
            >
            <input
              v-model="form.soVanBanDen"
              type="text"
              class="form-control"
              id="validationSoVanBanDen"
              required
            />
          </div>
          <!--Ngày nhập-->
          <div class="col-md-4">
            <label
              for="validationNgayNhap"
              class="col-form-label col-form-label-sm"
              >Ngày nhập</label
            >
            <flat-pickr
              v-model="form.ngayNhap"
              :config="config"
              class="form-control"
            ></flat-pickr>
          </div>
          <!-- Trả lời CV số -->
          <div class="col-md-4">
            <label
              for="validationTraLoiCVSo"
              class="col-form-label col-form-label-sm"
              >Trả lời CV số</label
            >
            <input
              type="text"
              class="form-control"
              id="validationTraLoiCVSo"
              v-model="form.traLoiCVSo"
            />
          </div>
          <!-- Ngày trả lời -->
          <div class="col-md-4">
            <label
              for="validationNgayNhap"
              class="col-form-label col-form-label-sm"
              >Ngày trả lời</label
            >
            <flat-pickr
              v-model="form.ngayTraLoi"
              :config="config"
              class="form-control"
            ></flat-pickr>
          </div>
          <!-- Số bản -->
          <div class="col-md-4">
            <label
              for="validationSoBan"
              class="col-form-label col-form-label-sm"
              >Số bản</label
            >
            <input
              type="number"
              class="form-control"
              id="validationSoBan"
              v-model="form.soBan"
            />
          </div>
          <!-- Trích yếu  -->
          <div class="col-md-12">
            <label
              for="validationTrichYeu"
              class="col-form-label col-form-label-sm"
              >Trích yếu</label
            >
            <span class="text-danger">*</span>
            <textarea
              v-model="form.trichYeu"
              class="form-control"
              id="validationTrichYeu"
              rows="3"
              required
            />
            <div class="valid-feedback">Vui lòng thêm trích yếu.</div>
          </div>
          <!-- Khối cơ quan nhận -->
          <div class="col-md-6">
            <label
              for="validationKhoiCoQuanNhan"
              class="col-form-label col-form-label-sm"
              >Khối cơ quan nhận</label
            >
            <treeselect
              placeholder="Chọn khối cơ quan nhận"
              v-model="form.khoiCoQuanNhan"
              :options="optionKhoiCoQuanNhan"
            >
            </treeselect>
            <treeselect-value :value="form.khoiCoQuanNhan" />
          </div>
          <!-- Cơ quan nhận  -->
          <div class="col-md-6">
            <label
              for="validationCoQuanNhan"
              class="col-form-label col-form-label-sm"
              >Cơ quan nhận</label
            >
            <treeselect
              placeholder="Chọn cơ quan nhận"
              v-model="form.coQuanNhan"
              :options="optionCoQuanNhan"
            >
            </treeselect>
            <treeselect-value :value="form.coQuanNhan" />
          </div>
          <!-- file đính kèm-->
          <div class="col-md-12">
            <label
              for="validationCoQuanNhan"
              class="col-form-label col-form-label-sm"
              >File đính kèm</label
            >
            <div class="input-group">
              <label class="input-group-text" for="inputGroupFile01"
                ><i class="ri-file-upload-line"></i
              ></label>
              <input type="file" class="form-control" id="inputGroupFile01" />
            </div>
          </div>
        </div>
      </div>
      <div class="col-md-5">
        <div class="row">
          <!-- Đơn vị soạn -->
          <div class="col-md-12">
            <label
              for="validationDonViSoan"
              class="col-form-label col-form-label-sm"
              >Đơn vị soạn</label
            >
            <treeselect
              placeholder="Chọn đơn vị soạn"
              v-model="form.donViSoan"
              :options="optionDoanViSoan"
            >
            </treeselect>
            <treeselect-value :value="form.donViSoan" />
          </div>
          <!-- Cán bộ soạn -->
          <div class="col-md-12">
            <label
              for="validationCanBoSoan"
              class="col-form-label col-form-label-sm"
              >Cán bộ soạn</label
            >
            <treeselect
              placeholder="Chọn cán bộ soạn"
              v-model="form.canBoSoan"
              :options="optionCanBoSoan"
            >
            </treeselect>
            <treeselect-value :value="form.canBoSoan" />
          </div>
          <!-- Hình thức gửi -->
          <div class="col-md-6">
            <label
              for="validationHinhThucGui"
              class="col-form-label col-form-label-sm"
              >Hình thức gửi</label
            >
            <treeselect
              placeholder="Chọn hình thức nhận"
              v-model="form.hinhThucGui"
              :options="optionHinhThucGui"
            >
            </treeselect>
            <treeselect-value :value="form.hinhThucGui" />
          </div>
          <!--        Lĩnh vực-->
          <div class="col-md-6">
            <label
              for="validationLinhVuc"
              class="col-form-label col-form-label-sm"
              >Lĩnh Vực</label
            >
            <input
              v-model="form.linhVuc"
              type="text"
              class="form-control"
              id="validationLinhVuc"
            />
          </div>
          <!--        Mức độ tính chất-->
          <div class="col-md-12">
            <label for="mucDoTinhChat" class="col-form-label col-form-label-sm"
              >Mức độ tính chất</label
            >
            <div>
              <div class="form-check form-check-inline">
                <input
                  class="form-check-input"
                  type="radio"
                  name="mucDoTinhChatOption"
                  id="mucDoTinhChat1"
                  v-model="form.mucDoTinhChat"
                />
                <label>Thấp</label>
              </div>
              <div class="form-check form-check-inline">
                <input
                  class="form-check-input"
                  type="radio"
                  name="mucDoTinhChatOption"
                  id="mucDoTinhChat2"
                  v-model="form.mucDoTinhChat"
                />
                <label>Trung bình</label>
              </div>
              <div class="form-check form-check-inline">
                <input
                  class="form-check-input"
                  type="radio"
                  name="mucDoTinhChatOption"
                  id="mucDoTinhChat3"
                  v-model="form.mucDoTinhChat"
                />
                <label>Cao</label>
              </div>
            </div>
          </div>
          <!--        Mức độ bảo mật-->
          <div class="col-md-12">
            <label for="mucDoBaoMat" class="col-form-label col-form-label-sm"
              >Mức độ bảo mật</label
            >
            <div>
              <div class="form-check form-check-inline">
                <input
                  class="form-check-input"
                  type="radio"
                  name="mucDoBaoMat"
                  id="mucDoBaoMat"
                  v-model="form.mucDoBaoMat"
                />
                <label class="form-check-label" for="mucDoBaoMat1">Thấp</label>
              </div>
              <div class="form-check form-check-inline">
                <input
                  class="form-check-input"
                  type="radio"
                  name="mucDoBaoMat"
                  id="mucDoBaoMat"
                  v-model="form.mucDoBaoMat"
                />
                <label class="form-check-label" for="mucDoBaoMat2"
                  >Trung bình</label
                >
              </div>
              <div class="form-check form-check-inline">
                <input
                  class="form-check-input"
                  type="radio"
                  name="mucDoBaoMat"
                  id="mucDoBaoMat"
                  v-model="form.mucDoBaoMat"
                />
                <label class="form-check-label" for="mucDoBaoMat3">Cao</label>
              </div>
            </div>
          </div>
          <!--        Hồ sơ đơn vị-->
          <div class="col-md-6">
            <label
              for="validationHoSoDonVi"
              class="col-form-label col-form-label-sm"
              >Hồ sơ đơn vị</label
            >
            <treeselect
              placeholder="Chọn hồ sơ đơn vị"
              v-model="form.hoSoDonVi"
              :options="optionHoSoDonVi"
            >
            </treeselect>
            <treeselect-value :value="form.hoSoDonVi" />
          </div>
          <!--        Nơi lưu trữ-->
          <div class="col-md-6">
            <label
              for="validationNoiLuuTru"
              class="col-form-label col-form-label-sm"
              >Nơi lưu trữ</label
            >
            <span class="text-danger">*</span>
            <input
              type="text"
              class="form-control"
              id="validationNoiLuuTru"
              v-model="form.noiLuuTru"
            />
          </div>
          <!-- Nơi lưu trữ -->
          <div class="col-md-12">
            <label
              for="validationNoiLuuTru"
              class="col-form-label col-form-label-sm"
              >Ghi chú</label
            >
            <textarea v-model="form.ghiChu" rows="2" class="form-control" />
          </div>
        </div>
      </div>
    </form>
  </div>
</template>

<style></style>
