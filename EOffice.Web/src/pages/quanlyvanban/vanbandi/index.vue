<script>
import Layout from "../../../layouts/main";
import PageHeader from "@/components/page-header";
import appConfig from "../../../../app.config.json";
import {data} from "./data";
import {vanBanDiModel} from "@/models/vanBanDiModel";

import Treeselect from "vue3-treeselect";
// import the styles
import "vue3-treeselect/dist/vue3-treeselect.css";
import flatPickr from "vue-flatpickr-component";
import "flatpickr/dist/flatpickr.css";
import {linhVucModel} from "@/models/linhVucModel";

import PhanCong from "./phancong";

export default {
  page: {
    title: "Văn bản đến",
    meta: [
      {
        name: "van-ban-den",
        content: appConfig.description,
      },
    ],
  },
  data() {
    return {
      title: "Văn bản đi",
      items: [
        {
          text: "Quản lý văn bản đi",
          href: "/",
        },
        {
          text: "Văn bản đi",
          active: true,
        },
      ],
      data: data,
      model:vanBanDiModel.baseJson(),
      optionLoaiVanBan: [
        {
          id: "",
          label: "",
        },
      ],
      optionTrangThai: [
        {
          id: "",
          label: "",
        },
      ],
      optionKhoiCoQuanNhan: [
        {
          id: "",
          label: "",
        },
      ],
      optionCoQuanNhan: [
        {
          id: "",
          label: "",
        },
      ],
      optionDonVi: [
        {
          id: "",
          label: "",
        },
      ],
      optionCanBoSoan: [
        {
          id: "",
          label: "",
          children: [
            {
              id: "",
              label: "",
            },
          ],
        },
      ],
      optionHinhThucGui: [
        {
          id: "",
          label: "",
        },
      ],
      optionHoSoDonVi: [
        {
          id: "",
          label: "",
        },
      ],
      optionMucDoTinhChat: [
        {
          id: "",
          label: "",
        }
      ],
      optionMucDoBaoMat: [
        {
          id: "",
          label: "",
        }
      ],
      config: {
        wrap: true, // set wrap to true only when using 'input-group'
        altFormat: "M j, Y",
        dateFormat: "d/m/Y",
      },
    };
  },
  components: {Layout, PageHeader,PhanCong, Treeselect, flatPickr},
  created() {
    this.myProvider()
    this.getLoaiVanBan()
    this.getTrangThai()
    this.getDonVi()
    this.getUser()
  },
  methods: {
    HandleSubmit(e){
      e.preventDefault();
      console.log("handle submit", this.model);
      if(
          this.model.id != 0 &&
          this.model.id != null &&
          this.model.id
      ){
        //Update model
        // await this.$store.dispatch("linhVucStore/update", this.model).then((res) => {
        //   if (res.resultCode === 'SUCCESS') {
        //     this.showModal = false;
        //     this.model = linhVucModel.baseJson()
        //     this.myProvider()
        //   }
        // })
      }else{
        //Create model
        // await this.$store.dispatch("linhVucStore/create", linhVucModel.toJson(this.model)).then((res) => {
        //   if (res.resultCode === 'SUCCESS') {
        //     this.showModal = false;
        //     this.model = linhVucModel.baseJson()
        //     this.myProvider()
        //   }
        // });
      }
    },
    myProvider (ctx) {


    },
    getLoaiVanBan() {
      try {
        let promise =  this.$store.dispatch("loaiVanBanStore/getLoaiVanBan")
        return promise.then(resp => {
          if(resp.resultCode == "SUCCESS"){
            let items = resp.data
            this.loading = false
            this.optionLoaiVanBan = items;
            return items || []


          }
          return [];
        })
      } finally {
        this.loading = false
      }
    },
    getTrangThai(){
      try {
        let promise =  this.$store.dispatch("trangThaiStore/getTrangThai")
        return promise.then(resp => {
          if(resp.resultCode == "SUCCESS"){
            let items = resp.data
            this.loading = false
            this.optionTrangThai = items;
            return items || []
          }
          return [];
        })
      } finally {
        this.loading = false
      }
    },
    getDonVi() {
      try {
        let promise =  this.$store.dispatch("donViStore/getDonViCha")
        return promise.then(resp => {
          if(resp.resultCode == "SUCCESS"){
            let items = resp.data
            this.loading = false
            this.optionDonVi = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },
    getUser() {
      try {
        let promise =  this.$store.dispatch("userStore/getUserTree")
        return promise.then(resp => {
          if(resp.resultCode == "SUCCESS"){
            let items = resp.data
            this.loading = false
            this.optionCanBoSoan = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    }
  },
};
</script>

<template>
  <Layout>
    <PageHeader :title="title" :items="items"/>

    <div class="row page-vanbanden">
      <div class="col-xl-12">
        <div class="card">
          <div class="card-header align-items-center d-flex">
            <h4 class="card-title mb-0 flex-grow-1">Danh sách văn bản đến</h4>

            <div class="flex-shrink-0">
              <div
                  class="form-check form-switch form-switch-right form-switch-md"
              >
                <button
                    class="btn btn-primary add-btn btn-sm"
                    data-bs-toggle="modal"
                    data-bs-target="#them-moi"
                >
                  <i class="ri-add-line align-bottom me-1"></i> Thêm mới
                </button>
              </div>
            </div>
          </div>
          <div class="row">
            <div class="col-12">
              <!--  Table -->
              <table class="table align-middle table-nowrap mb-0">
                <thead class="table-light">
                <tr>
                  <th scope="col">#</th>
                  <th scope="col">Số Lưu CV</th>
                  <th scope="col">Số CB đi</th>
                  <th scope="col">Trích yếu</th>
                  <th scope="col">Loại văn bản</th>
                  <th scope="col">Trạng thái</th>
                  <th scope="col">Cơ quan nhận</th>
                  <th scope="col">Ngày nhập</th>
                  <th scope="col">Thao tác</th>
                </tr>
                </thead>
                <tbody>
                <tr v-if="data.length <= 0" class="text-center">
                  <td colspan="6">Không tìm thấy dữ liệu</td>
                </tr>
                <tr v-else v-for="(item, index) in data" :key="item.id">
                  <td>{{ ++index }}</td>
                  <td>{{ item.soLuuCV }}</td>
                  <td>{{ item.soCVDi }}</td>
                  <td>{{ item.trichYeu }}</td>
                  <td>{{ item.loaiVanBan }}</td>
                  <td v-if="item.trangThai == 0">
                      <span class="badge badge-soft-secondary"
                      >Vừa tiếp nhận</span
                      >
                  </td>
                  <td v-if="item.trangThai == 1">
                    <span class="badge badge-soft-primary">Đã tiếp nhận</span>
                  </td>
                  <td v-if="item.trangThai == 2">
                    <span class="badge badge-soft-success">Hoàn thành</span>
                  </td>
                  <td>{{ item.coQuanNhan }}</td>
                  <td>{{ item.ngayNhap }}</td>
                  <td>
                    <div class="hstack gap-3 fs-15">
                      <a href="javascript:void(0);" class="link-info"
                      ><i class="ri-newspaper-line"></i
                      ></a>
                      <a href="javascript:void(0);" class="link-info"
                      ><i class="ri-download-2-line"></i
                      ></a>
                      <a
                          href="javascript:void(0);"
                          class="link-info"
                          data-bs-toggle="modal"
                          data-bs-target="#phancong"
                      ><i class="ri-user-add-line"></i
                      ></a>
                      <a
                          class="link-primary edit-btn"
                          data-bs-toggle="modal"
                          data-bs-target="#chinh-sua"
                      ><i class="ri-edit-2-line"></i
                      ></a>
                      <a href="javascript:void(0);" class="link-danger"
                      ><i class="ri-delete-bin-5-line"></i
                      ></a>
                    </div>
                  </td>
                </tr>
                </tbody>
              </table>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!--  create modal form  -->
    <div
        class="modal fade zoomIn"
        id="them-moi"
        tabindex="-1"
        aria-labelledby="CreateModalLabel"
        aria-hidden="true"
    >
      <div class="modal-dialog modal-dialog-centered modal-fullscreen">
        <div class="modal-content border-0">
          <form class="" @submit="HandleSubmit" novalidate>
            <div class="modal-header p-3 bg-primary-dark">
              <h5 class="modal-title" id="CreateModalLabel">
                Thêm mới văn bản đi
              </h5>
              <div class="d-flex">
                <button
                    type="submit"
                    class="btn btn-sm btn-primary waves-effect waves-light me-2 d-flex align-items-center"
                >
                  <i class="ri-save-3-fill me-1"></i>
                  Lưu
                </button>

                <button
                    type="button"
                    class="btn btn-sm btn-danger waves-effect waves-light me-2 d-flex align-items-center"
                    data-bs-dismiss="modal"
                    aria-label="Close"
                    id="close-modal"
                >
                  <i class="ri-close-line me-1"></i>
                  Đóng
                </button>
              </div>
            </div>
            <!-- start content -->
            <div class="row p-4">
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
                        v-model="model.loaiVanBan"
                        :options="optionLoaiVanBan"
                    >
                    </treeselect>
                    <treeselect-value :value="model.loaiVanBan" />
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
                        v-model="model.trangThai"
                        :options="optionTrangThai"
                    >
                    </treeselect>
                    <treeselect-value :value="model.trangThai" />
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
                        v-model="model.soLuuCV"
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
                        v-model="model.soVBDen"
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
                        v-model="model.ngayNhap"
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
                        v-model="model.traLoiCVSo"
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
                        v-model="model.ngayTraLoi"
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
                        v-model="model.soBan"
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
                        v-model="model.trichYeu"
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
                        v-model="model.khoiCoQuanNhan"
                        :options="optionDonVi"
                    >
                    </treeselect>
                    <treeselect-value :value="model.khoiCoQuanNhan" />
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
                        v-model="model.coQuanNhan"
                        :options="optionDonVi"
                    >
                    </treeselect>
                    <treeselect-value :value="model.coQuanNhan" />
                  </div>
                  <!-- file đính kèm-->
                  <div class="col-md-12">
                    <label
                        for="validationCoQuanNhan"
                        class="col-form-label col-form-label-sm"
                    >File đính kèm</label
                    >
                    <div class="input-group">
                      <button class="btn btn-outline-primary" type="button" id="inputGroupFileAddon03">
                        <i class=" ri-attachment-2 text-primary"></i>
                      </button>
                      <input type="file" class="form-control" id="inputGroupFile03" aria-describedby="inputGroupFileAddon03" aria-label="Upload">
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
                        v-model="model.donViSoan"
                        :options="optionDonVi"
                    >
                    </treeselect>
                    <treeselect-value :value="model.donViSoan" />
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
                        v-model="model.canBoSoan"
                        :options="optionCanBoSoan"
                    >
                    </treeselect>
                    <treeselect-value :value="model.canBoSoan" />
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
                        v-model="model.hinhThucGui"
                        :options="optionHinhThucGui"
                    >
                    </treeselect>
                    <treeselect-value :value="model.hinhThucGui" />
                  </div>
                  <!--        Lĩnh vực-->
                  <div class="col-md-6">
                    <label
                        for="validationLinhVuc"
                        class="col-form-label col-form-label-sm"
                    >Lĩnh Vực</label
                    >
                    <input
                        v-model="model.linhVuc"
                        type="text"
                        class="form-control"
                        id="validationLinhVuc"
                    />
                  </div>
                  <!--        Mức độ tính chất-->
                  <div class="col-md-6">
                    <label
                        for="validationHinhThucGui"
                        class="col-form-label col-form-label-sm"
                    >Mức độ tính chất</label
                    >
                    <treeselect
                        placeholder="Chọn hình thức nhận"
                        v-model="model.mucDoTinhChat"
                        :options="optionMucDoTinhChat"
                    >
                    </treeselect>
                    <treeselect-value :value="model.mucDoTinhChat" />
                  </div>
                  <!--        Mức độ bảo mật-->
                  <div class="col-md-6">
                    <label
                        for="validationHinhThucGui"
                        class="col-form-label col-form-label-sm"
                    >Mức độ bảo mật</label
                    >
                    <treeselect
                        placeholder="Chọn hình thức nhận"
                        v-model="model.mucDoBaoMat"
                        :options="optionMucDoBaoMat"
                    >
                    </treeselect>
                    <treeselect-value :value="model.mucDoBaoMat" />
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
                        v-model="model.hoSoDonVi"
                        :options="optionHoSoDonVi"
                    >
                    </treeselect>
                    <treeselect-value :value="model.hoSoDonVi" />
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
                        v-model="model.noiLuuTru"
                    />
                  </div>
                  <!-- Nơi lưu trữ -->
                  <div class="col-md-12">
                    <label
                        for="validationNoiLuuTru"
                        class="col-form-label col-form-label-sm"
                    >Ghi chú</label
                    >
                    <textarea v-model="model.ghiChu" rows="2" class="form-control" />
                  </div>
                </div>
              </div>
            </div>
            <!-- end content -->
          </form>
        </div>
      </div>
    </div>

    <!-- Edit modal form -->
    <div
        class="modal fade zoomIn"
        id="chinh-sua"
        tabindex="-1"
        aria-labelledby="EditModalLabel"
        aria-hidden="true"
    >
      <div class="modal-dialog modal-dialog-centered modal-fullscreen">
        <div class="modal-content border-0">
          <div class="modal-header p-3 bg-primary-dark">
            <h5 class="modal-title" id="EditModalLabel">
              Chỉnh sửa văn bản đi
            </h5>
            <div class="d-flex">
              <button
                  type="button"
                  class="btn btn-sm btn-primary waves-effect waves-light me-2 d-flex align-items-center"
              >
                <i class="ri-save-3-fill me-1"></i>
                Lưu
              </button>

              <button
                  type="button"
                  class="btn btn-sm btn-danger waves-effect waves-light me-2 d-flex align-items-center"
                  data-bs-dismiss="modal"
                  aria-label="Close"
                  id="close-modal"
              >
                <i class="ri-close-line me-1"></i>
                Đóng
              </button>
            </div>
          </div>
          <!-- start content -->
          <div class="row p-4">
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
                      v-model="model.loaiVanBan"
                      :options="optionLoaiVanBan"
                  >
                  </treeselect>
                  <treeselect-value :value="model.loaiVanBan" />
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
                      v-model="model.trangThai"
                      :options="optionTrangThai"
                  >
                  </treeselect>
                  <treeselect-value :value="model.trangThai" />
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
                      v-model="model.soLuuCV"
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
                      v-model="model.soLuuCV"
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
                      v-model="model.ngayNhap"
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
                      v-model="model.traLoiCVSo"
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
                      v-model="model.ngayTraLoi"
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
                      v-model="model.soBan"
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
                      v-model="model.trichYeu"
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
                      v-model="model.khoiCoQuanNhan"
                      :options="optionDonVi"
                  >
                  </treeselect>
                  <treeselect-value :value="model.khoiCoQuanNhan" />
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
                      v-model="model.coQuanNhan"
                      :options="optionDonVi"
                  >
                  </treeselect>
                  <treeselect-value :value="model.coQuanNhan" />
                </div>
                <!-- file đính kèm-->
                <div class="col-md-12">
                  <label
                      for="validationCoQuanNhan"
                      class="col-form-label col-form-label-sm"
                  >File đính kèm</label
                  >
                  <div class="input-group">
                    <button class="btn btn-outline-primary" type="button" id="inputGroupFileAddon03">
                      <i class=" ri-attachment-2 text-primary"></i>
                    </button>
                    <input type="file" class="form-control" id="inputGroupFile03" aria-describedby="inputGroupFileAddon03" aria-label="Upload">
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
                      v-model="model.donViSoan"
                      :options="optionDonVi"
                  >
                  </treeselect>
                  <treeselect-value :value="model.donViSoan" />
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
                      v-model="model.canBoSoan"
                      :options="optionCanBoSoan"
                  >
                  </treeselect>
                  <treeselect-value :value="model.canBoSoan" />
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
                      v-model="model.hinhThucGui"
                      :options="optionHinhThucGui"
                  >
                  </treeselect>
                  <treeselect-value :value="model.hinhThucGui" />
                </div>
                <!--        Lĩnh vực-->
                <div class="col-md-6">
                  <label
                      for="validationLinhVuc"
                      class="col-form-label col-form-label-sm"
                  >Lĩnh Vực</label
                  >
                  <input
                      v-model="model.linhVuc"
                      type="text"
                      class="form-control"
                      id="validationLinhVuc"
                  />
                </div>
                <!--        Mức độ tính chất-->
                <div class="col-md-6">
                  <label
                      for="validationHinhThucGui"
                      class="col-form-label col-form-label-sm"
                  >Mức độ tính chất</label
                  >
                  <treeselect
                      placeholder="Chọn hình thức nhận"
                      v-model="model.mucDoTinhChat"
                      :options="optionMucDoTinhChat"
                  >
                  </treeselect>
                  <treeselect-value :value="model.mucDoTinhChat" />
                </div>
                <!--        Mức độ bảo mật-->
                <div class="col-md-6">
                  <label
                      for="validationHinhThucGui"
                      class="col-form-label col-form-label-sm"
                  >Mức độ bảo mật</label
                  >
                  <treeselect
                      placeholder="Chọn hình thức nhận"
                      v-model="model.mucDoBaoMat"
                      :options="optionMucDoBaoMat"
                  >
                  </treeselect>
                  <treeselect-value :value="model.mucDoBaoMat" />
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
                      v-model="model.hoSoDonVi"
                      :options="optionHoSoDonVi"
                  >
                  </treeselect>
                  <treeselect-value :value="model.hoSoDonVi" />
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
                      v-model="model.noiLuuTru"
                  />
                </div>
                <!-- Nơi lưu trữ -->
                <div class="col-md-12">
                  <label
                      for="validationNoiLuuTru"
                      class="col-form-label col-form-label-sm"
                  >Ghi chú</label
                  >
                  <textarea v-model="model.ghiChu" rows="2" class="form-control" />
                </div>
              </div>
            </div>
          </div>
          <!-- end content -->
        </div>
      </div>
    </div>
    <!-- Modal phân công -->
    <div
        class="modal fade zoomIn"
        id="phancong"
        tabindex="-1"
        aria-labelledby="phancongLabel"
        aria-hidden="true"
    >
      <div class="modal-dialog modal-dialog-centered modal-lg">
        <div class="modal-content border-0">
          <div class="modal-header p-3 bg-primary-dark">
            <h5 class="modal-title" id="phancongLabel">Phân công</h5>
            <button
                type="button"
                class="btn-close"
                data-bs-dismiss="modal"
                aria-label="Close"
            ></button>
          </div>
          <div class="modal-body p-3">
            <phan-cong/>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-light" data-bs-dismiss="modal">
              Đóng
            </button>
            <button type="button" class="btn btn-primary">Lưu</button>
          </div>
        </div>
      </div>
    </div>
  </Layout>
</template>
<style>
.modal-title {
  color: #fff;
}

.bg-primary-dark {
  background: linear-gradient(135deg, #06548e, #ffffff);
  box-shadow: 0px 3px 0px #06548e;
}
</style>
