<script>
import {required} from "@vuelidate/validators"
// import the component
import Treeselect from 'vue3-treeselect'
// import the styles
import 'vue3-treeselect/dist/vue3-treeselect.css'
import flatPickr from "vue-flatpickr-component";
import "flatpickr/dist/flatpickr.css";


export default {
  components: {Treeselect, flatPickr},
  data() {
    return {
      loaivanban: "",
      form: {
        soLuu: "",
        soVanBanDen: "",
        loaiVanBan: "",
        trichYeu: "",
        ngayBanHanh: new Date(),
        ngayNhan: new Date(),
        nguoiKy: "",
        trangThai: "",
        ngayKy: new Date(),
        thoiHanXuLy: new Date(),
        khoiCoQuanGui: "",
        coQuanGui: "",
        hinhThucNhan: "",
        linhVuc: "",
        hoSoDonVi: "",
        noiLuuTru: "",
        file: "",
      },
      optionLoaiVanBan: [
        {
          id: "1",
          label: "Văn bản hành chính",
        },
        {
          id: "2",
          label: "Văn bản ban hành",
        },
        {
          id: "3",
          label: "Văn bản thông báo",
        },
      ],
      optionTrangThai: [
        {
          id: "1",
          label: "Vừa tiếp nhận",
        },
        {
          id: "2",
          label: "Đã tiếp nhận",
        },
        {
          id: "3",
          label: "Hoàn thành",
        },
      ],
      optionKhoiCoQuanGui: [
        {
          id: "1",
          label: "Khối cơ quan gửi 1",
        },
        {
          id: "2",
          label: "Khối cơ quan gửi 2",
        },
        {
          id: "3",
          label: "Khối cơ quan gửi 3",
        },
      ],
      optionCoQuanGui: [
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
      optionHinhThucNhan: [
        {
          id: "1",
          label: "Hình thức nhận 1",
        },
        {
          id: "2",
          label: "Hình thức nhận 2",
        },
        {
          id: "3",
          label: "Hình thức nhận 3",
        },
      ],
      optionHoSoDonVi: [
        {
          id: "1",
          label: "Hồ sơ đươn vị 1",
        },
        {
          id: "2",
          label: "Hồ sơ đươn vị 2",
        },
        {
          id: "3",
          label: "Hồ sơ đươn vị 3",
        },
      ],
      optionNguoiKy: [
        {
          id: "1",
          label: "Khoa CNTT",
          children: [{
            id: '2',
            label: 'Người số 1',
          }, {
            id: '3',
            label: 'Người số 2',
          },],
        },
        {
          id: "4",
          label: "Khoa SP & KHXH",
          children: [{
            id: '5',
            label: 'Người số 3',
          }, {
            id: '6',
            label: 'Người số 4',
          },],
        },
        {
          id: "7",
          label: "Khoa chính trị",
          children: [{
            id: '8',
            label: 'Người số 5',
          }, {
            id: '9',
            label: 'Người số 6',
          },],
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
      soLuu: {required},
      soVanBanDen: {required},
      loaiVanBan: {required},
      trichYeu: {required},
      nguoiKy: {required},
      file: {required},
    },
  },
};
</script>

<template>
  <div>
    <form class="row g-3 needs-validation" novalidate>
      <div class="col-md-7">
        <div class="row">
          <!-- số lưu-->
          <div class="col-md-3">
            <label for="validationSoLuu" class="col-form-label col-form-label-sm ">Số lưu</label> <span
              class="text-danger">*</span>
            <input type="text" class="form-control form-control-sm" id="validationSoLuu" value="" required>
            <div class="valid-feedback">
              Vui lòng thêm số lưu.
            </div>
          </div>
          <!-- số văn bản đến-->
          <div class="col-md-5">
            <label for="validationSoVanBanDen" class="col-form-label col-form-label-sm">Số VB đến</label> <span
              class="text-danger">*</span>
            <input type="text" class="form-control form-control-sm" id="validationSoVanBanDen" value="" required>
            <div class="valid-feedback">
              Vui lòng thêm số văn bản đến.
            </div>
          </div>
          <!--loai văn bản-->
          <div class="col-md-4">
            <label for="validationLoaiVanBan" class="col-form-label col-form-label-sm">Loại văn bản</label>
            <treeselect
                placeholder="Chọn loại văn bản"
                v-model="form.loaiVanBan"
                :options="optionLoaiVanBan"
            >
            </treeselect>
            <treeselect-value :value="form.loaiVanBan"/>
            <div class="invalid-feedback">
              Vui lòng chọn loại văn bản
            </div>
          </div>
          <!-- trích yếu-->
          <div class="col-md-12">
            <label for="validationTrichYeu" class="col-form-label col-form-label-sm ">Trích yếu</label> <span
              class="text-danger">*</span>
            <textarea
                class="form-control form-control-sm"
                id="validationTrichYeu"
                rows="4"
                required
            />
            <div class="valid-feedback">
              Vui lòng thêm trích yếu.
            </div>
          </div>
          <!--          Ngày ban hành-->
          <div class="col-md-6">
            <label for="validationNgayBanHanh" class="col-form-label col-form-label-sm ">Ngày ban hành</label> <span
              class="text-danger">*</span>
            <flat-pickr
                v-model="form.ngayBanHanh"
                :config="config"
                class="form-control form-control-sm"
            ></flat-pickr>
            <div class="valid-feedback">
              Vui lòng thêm ngày ban hành.
            </div>
          </div>
          <!--          Ngày nhận-->
          <div class="col-md-6">
            <label for="validationNgayNhan" class="col-form-label col-form-label-sm ">Ngày nhận</label> <span
              class="text-danger">*</span>
            <flat-pickr
                v-model="form.ngayNhan"
                :config="config"
                class="form-control form-control-sm"
            ></flat-pickr>
            <div class="valid-feedback">
              Vui lòng thêm ngày nhận
            </div>
          </div>
          <!--          Trạng thái-->
          <div class="col-md-6">
            <label for="validationTrangThai" class="col-form-label col-form-label-sm">Trạng thái văn bản</label>
            <treeselect
                placeholder="Chọn loại văn bản"
                v-model="form.trangThai"
                :options="optionTrangThai"
            >
            </treeselect>
            <treeselect-value :value="form.trangThai"/>
          </div>
          <!--          Ngày ký-->
          <div class="col-md-6">
            <label for="validationNgayKy" class="col-form-label col-form-label-sm ">Ngày ký</label> <span
              class="text-danger">*</span>
            <flat-pickr
                v-model="form.ngayKy"
                :config="config"
                class="form-control form-control-sm"
            ></flat-pickr>
          </div>
          <!--          Người ký-->
          <div class="col-md-6">
            <label for="validationTrangThai" class="col-form-label col-form-label-sm">Người ký</label>
            <treeselect
                placeholder="Chọn người ký"
                v-model="form.nguoiKy"
                :options="optionNguoiKy"
            >
            </treeselect>
            <treeselect-value :value="form.nguoiKy"/>
          </div>
          <!--          Thời hạn xử lý-->
          <div class="col-md-6">
            <label for="validationThoiHanXuLy" class="col-form-label col-form-label-sm ">Thời hạn xử lý</label> <span
              class="text-danger">*</span>
            <input type="date" class="form-control form-control-sm" id="validationThoiHanXuLy" value="" required>
          </div>
          <!--          fiel đính kèm-->
          <div class="col-md-12">
            <div class="input-group mt-3">
              <label class="input-group-text" for="inputGroupFile01"><i class="ri-file-upload-line"></i></label>
              <input type="file" class="form-control" id="inputGroupFile01">
            </div>
          </div>
        </div>
      </div>
      <div class="col-md-5">
        <div class="row">
          <!--        Khối cơ quan gửi-->
          <div class="col-md-6">
            <label for="validationKhoiCoQuanGui" class="col-form-label col-form-label-sm">Khối cơ quan gửi</label>
            <treeselect
                placeholder="Chọn khối cơ quan gửi"
                v-model="form.khoiCoQuanGui"
                :options="optionKhoiCoQuanGui"
            >
            </treeselect>
            <treeselect-value :value="form.khoiCoQuanGui"/>
          </div>
          <!--        Cơ quan gửi-->
          <div class="col-md-6">
            <label for="validationCoQuanGui" class="col-form-label col-form-label-sm">Cơ quan gửi</label>
            <treeselect
                placeholder="Chọn cơ quan gửi"
                v-model="form.coQuanGui"
                :options="optionCoQuanGui"
            >
            </treeselect>
            <treeselect-value :value="form.coQuanGui"/>
          </div>
          <!--        Hình thức nhận-->
          <div class="col-md-6">
            <label for="validationHinhThucNhan" class="col-form-label col-form-label-sm">Hình thức nhận</label>
            <treeselect
                placeholder="Chọn hình thức nhận"
                v-model="form.hinhThucNhan"
                :options="optionHinhThucNhan"
            >
            </treeselect>
            <treeselect-value :value="form.hinhThucNhan"/>
          </div>
          <!--        Lĩnh vực-->
          <div class="col-md-6">
            <label for="validationLinhVuc" class="col-form-label col-form-label-sm">Lĩnh Vực</label> <span
              class="text-danger">*</span>
            <input type="text" class="form-control form-control-sm" id="validationLinhVuc" value="" required>
          </div>
          <!--        Mức độ tính chất-->
          <div class="col-md-12">
            <label for="mucDoTinhChat" class="col-form-label col-form-label-sm">Mức độ tính chất</label>
            <div>
              <div class="form-check form-check-inline">
                <input class="form-check-input" type="radio" name="mucDoTinhChatOption1" id="mucDoTinhChat1" value="option1">
                <label>Thấp</label>
              </div>
              <div class="form-check form-check-inline">
                <input class="form-check-input" type="radio" name="mucDoTinhChatOptiopn2" id="mucDoTinhChat2" value="option2">
                <label>Trung bình</label>
              </div>
              <div class="form-check form-check-inline">
                <input class="form-check-input" type="radio" name="mucDoTinhChatOption3" id="mucDoTinhChat3" value="option2">
                <label>Cao</label>
              </div>
            </div>
          </div>
          <!--        Mức độ bảo mật-->
          <div class="col-md-12">
            <label for="mucDoBaoMat" class="col-form-label col-form-label-sm">Mức độ bảo mật</label>
            <div>
              <div class="form-check form-check-inline">
                <input class="form-check-input" type="radio" name="mucDoBaoMat1" id="mucDoBaoMat1" value="option1">
                <label class="form-check-label" for="mucDoBaoMat1">Thấp</label>
              </div>
              <div class="form-check form-check-inline">
                <input class="form-check-input" type="radio" name="mucDoBaoMat2" id="mucDoBaoMat2" value="option2">
                <label class="form-check-label" for="mucDoBaoMat2">Trung bình</label>
              </div>
              <div class="form-check form-check-inline">
                <input class="form-check-input" type="radio" name="mucDoBaoMat3" id="mucDoBaoMat3" value="option2">
                <label class="form-check-label" for="mucDoBaoMat3">Cao</label>
              </div>
            </div>
          </div>
          <!--        Hồ sơ đơn vị-->
          <div class="col-md-12">
            <label for="validationHoSoDonVi" class="col-form-label col-form-label-sm">Hồ sơ đơn vị</label>
            <treeselect
                placeholder="Chọn hồ sơ đơn vị"
                v-model="form.hoSoDonVi"
                :options="optionHoSoDonVi"
            >
            </treeselect>
            <treeselect-value :value="form.hoSoDonVi"/>
          </div>
          <!--        Nơi lưu trữ-->
          <div class="col-md-12">
            <label for="validationNoiLuuTru" class="col-form-label col-form-label-sm">Nơi lưu trữ</label> <span
              class="text-danger">*</span>
            <input type="text" class="form-control form-control-sm" id="validationNoiLuuTru" value="" required>
          </div>
          <div class="col-md-12 mt-3">
            <div class="form-check form-switch">
              <input class="form-check-input" type="checkbox" role="switch" id="congVanChiDoc" >
              <label class="form-check-label" for="congVanChiDoc">Là công văn chỉ đọc</label>
            </div>
            <div class="form-check form-switch">
              <input class="form-check-input" type="checkbox" role="switch" id="banChinh" >
              <label class="form-check-label" for="banChinh">Bản chính</label>
            </div>
            <div class="form-check form-switch">
              <input class="form-check-input" type="checkbox" role="switch" id="thongBao" >
              <label class="form-check-label" for="thongBao">Hiện thị mục thông báo</label>
            </div>
          </div>
        </div>
      </div>
    </form>
  </div>
</template>

<style>
.vue-treeselect__control {
  height: 26px;
  border-radius: 3px;
  border-color: #ced4da;
  font-size: 12px;
}

.vue-treeselect__single-value {
  margin-top: -5px;
}

.vue-treeselect__placeholder {
  margin-top: -5px;
}
</style>