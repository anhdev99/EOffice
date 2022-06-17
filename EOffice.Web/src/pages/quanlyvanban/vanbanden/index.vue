<script>
import Layout from "../../../layouts/main";
import PageHeader from "@/components/page-header";
import appConfig from "../../../../app.config.json";
import {data} from "./data";
import {vanBanDiModel} from "@/models/vanBanDiModel";

import DropZone from 'dropzone-vue';
import 'dropzone-vue/dist/dropzone-vue.common.css';

import Treeselect from "vue3-treeselect";
// import the styles
import "vue3-treeselect/dist/vue3-treeselect.css";
import flatPickr from "vue-flatpickr-component";
import "flatpickr/dist/flatpickr.css";
import {linhVucModel} from "@/models/linhVucModel";
import vueDropzone from 'vue2-dropzone-vue3'

import {userModel} from "@/models/userModel";

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
      title: "Văn bản dến",
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
      showModal: false,
      model: vanBanDiModel.baseJson(),
      currentPage: 1,
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
      showDeleteModal: false,
      showPhanCongModal: false,
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
      optionUser: [
        {
          id: "",
          label: "",
          children: [
            {
              id: "",
              label: "",
            }
          ]
        }
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
          id: "0",
          label: "Văn bản giấy",
        },
        {
          id: "1",
          label: "File tài liệu",
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
          id: "THAP",
          label: "Thấp",
        },
        {
          id: "TRUNGBINH",
          label: "Trung bình",
        },
        {
          id: "CAO",
          label: "Cao",
        },
      ],
      optionMucDoBaoMat: [
        {
          id: "THAP",
          label: "Thấp",
        },
        {
          id: "TRUNGBINH",
          label: "Trung bình",
        },
        {
          id: "CAO",
          label: "Cao",
        },
      ],
      optionLinhVuc: [
        {
          id: "",
          label: "",
        }
      ],
      apiUrl: process.env.VUE_APP_API_URL,
      url: `${process.env.VUE_APP_API_URL}files/upload`,
      dropzoneOptions: {
        url: `${process.env.VUE_APP_API_URL}files/upload`,
        thumbnailWidth: 300,
        thumbnailHeight: 160,
        maxFiles: 4,
        maxFilesize: 30,
        headers: { "My-Awesome-Header": "header value" },
        addRemoveLinks: true,
        acceptedFiles: ".jpeg,.jpg,.png,.gif,.doc,.docx,.xlsx,.pptx,.pdf",
      },
      config: {
        wrap: true, // set wrap to true only when using 'input-group'
        altFormat: "M j, Y",
        dateFormat: "d/m/Y",
      },
      tempPhanCongData: []
    };
  },
  components: {Layout, PageHeader, Treeselect, flatPickr,vueDropzone},
  created() {
    this.myProvider()
    this.getLoaiVanBan()
    this.getTrangThai()
    this.getDonVi()
    this.getUser()
    this.getLinhVuc()
    this.myProvider();
  },
  watch: {
    tempPhanCongData: {
      deep: true,
      handler(val) {
        console.log("sdafs");
      }
    },
  },
  methods: {
    async handleUpdate(id) {
      await this.$store.dispatch("vanBanDenStore/getById", id).then((res) => {
        if (res.resultCode == "SUCCESS") {
          console.log("res", res.data)
          this.model = res.data;
          this.showModal = true;
        } else {
          // this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        }
      });
    },
    handleShowDeleteModal(id) {
      this.model.id = id;
      this.showDeleteModal = true;
    },
    async handleDelete() {
      if (this.model.id != 0 && this.model.id != null && this.model.id) {
        await this.$store.dispatch("vanBanDenStore/delete", this.model.id).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            this.showDeleteModal = false;
            this.myProvider()
          }
        });
      }
    },
    myProvider (ctx) {
      const params = {
        start: this.currentPage - 1,
        limit: this.perPage,
        content: "",
        sortBy: "",
        sortDesc: false,
      }
      this.loading = true

      try {
        let promise =  this.$store.dispatch("vanBanDenStore/getPagingParams", params)
        return promise.then(resp => {
          if(resp.resultCode == "SUCCESS"){
            let items = resp.data.data
            this.totalRows = resp.data.totalRows
            this.numberOfElement = resp.data.data.length
            this.loading = false
            this.data = items;
            return items || []
          }
          return [];
        })
      } finally {
        this.loading = false
      }
    },
    async HandleSubmit(e) {
      e.preventDefault();
      console.log("handle submit", this.model);
      if (
          this.model.id != 0 &&
          this.model.id != null &&
          this.model.id
      ) {
        //Update model
        await this.$store.dispatch("vanBanDenStore/update", this.model).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            this.showModal = false;
            this.model = vanBanDiModel.baseJson()
            this.myProvider()
          }
        })
      } else {
        //Create model
        await this.$store.dispatch("vanBanDenStore/create", this.model).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            this.showModal = false;
            this.model = vanBanDiModel.baseJson()
            this.myProvider()
          }
        });
      }
    },
    getLoaiVanBan() {
      try {
        let promise = this.$store.dispatch("loaiVanBanStore/getLoaiVanBan")
        return promise.then(resp => {
          if (resp.resultCode == "SUCCESS") {
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
    getTrangThai() {
      try {
        let promise = this.$store.dispatch("trangThaiStore/getTrangThai")
        return promise.then(resp => {
          if (resp.resultCode == "SUCCESS") {
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
        let promise = this.$store.dispatch("donViStore/getDonViCha")
        return promise.then(resp => {
          if (resp.resultCode == "SUCCESS") {
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
        let promise = this.$store.dispatch("userStore/getAll")
        return promise.then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false

            this.optionUser = items.map(value => {
              return {
                id: value.id,
                label: value.fullName,
              };
            });
            console.log("this.optionUser", this.optionUser);
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },
    getLinhVuc() {
      try {
        let promise = this.$store.dispatch("linhVucStore/get")
        return promise.then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionLinhVuc = items.map(value => {
              return {
                id: value.id,
                label: value.ten,
              };
            });
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },
    addThisFile(items, re ) {
      console.log("file", items, re);
      // console.log("response", response);
      // if (this.model) {
      //   if (this.model.uploadFiles == null || this.model.uploadFiles.length <= 0)
      //   {
      //     this.model.uploadFiles = [];
      //   }
      //   let fileSuccess = response.data;
      //   this.model.uploadFiles.push({fileId: fileSuccess.id,  fileName: fileSuccess.fileName , ext : fileSuccess.ext})
      // }
    },
    removeCommentFile(file, error, xhr) {
      let fileCongViec = JSON.parse(file.xhr.response);
      if (fileCongViec.data && fileCongViec.data.id) {
        let idFile = fileCongViec.data.id;
        let resultData = this.model.uploadFiles =  null;
        this.model.uploadFiles= resultData;
      }
    },
    addCommentFile(file, response) {
      if (this.model) {
        if (this.model.uploadFiles == null || this.model.length <= 0)
        {
          this.model.uploadFiles = [];
        }
        let fileSuccess = response.data;
        this.model.uploadFiles = {fileId: fileSuccess.id,  fileName: fileSuccess.fileName , ext : fileSuccess.ext}
      }
    },
    normalizer(node){
      if(node.children == null || node.children == 'null'){
        delete node.children;
      }
    },
    addTempPhanCong() {
      if(!this.tempPhanCongData )
        this.tempPhanCongData = []
      this.tempPhanCongData.push({ userId: null, Ten: null, ghiChu: null });
      this.model.nguoiPhanCong =  this.tempPhanCongData;
    },
    deleteTempPhanCong(index) {
      this.tempPhanCongData.splice(index, 1);
      this.model.nguoiPhanCong =  this.tempPhanCongData;
    },
    async handlePhanCong(id){
      await this.$store.dispatch("vanBanDenStore/getById", id).then((res) => {
        if (res.resultCode == "SUCCESS") {
          this.model = res.data;
          this.tempPhanCongData = res.data.nguoiPhanCong;
          this.showPhanCongModal = true;
        } else {
          // this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        }
      });
    },
    async handleSubmitPhanCong(e) {

      console.log("handle submit", this.model);
      if (
          this.model.id != 0 &&
          this.model.id != null &&
          this.model.id
      ) {
        //Update model
        await this.$store.dispatch("vanBanDenStore/phanCong", this.model).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            this.showModal = false;
            this.model = vanBanDiModel.baseJson()
            this.myProvider()
          }
        })
      }
    },
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

                    @click="showModal = true"
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
                  <td>{{ item.soVBDi }}</td>
                  <td>{{ item.trichYeu }}</td>
                  <td>{{ item.loaiVanBanTen }}</td>
                  <td>
                    <span class="badge badge-soft-success">{{item.trangThaiTen}}</span>
                  </td>
                  <td>{{ item.coQuanNhanTen }}</td>
                  <td>{{ item.ngayNhap }}</td>
                  <td>
                    <div class="hstack gap-3 fs-15">
                      <a href="javascript:void(0);" class="link-info"
                      ><i class="ri-newspaper-line"></i
                      ></a>
                      <a v-if="item.file"  :href="`${apiUrl}files/view/${item.file.fileId}`" class="link-info"
                      ><i class="ri-download-2-line"></i
                      ></a>
                      <a
                          href="javascript:void(0);"
                          class="link-info"
                          @click="handlePhanCong(item.id)"
                      ><i class="ri-user-add-line"></i
                      ></a>
                      <a
                          class="link-primary edit-btn"
                          @click="handleUpdate(item.id)"
                      ><i class="ri-edit-2-line"></i
                      ></a>
                      <a href="javascript:void(0);" class="link-danger"
                         @click="handleShowDeleteModal(item.id)"
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
    <b-modal
        class="modal fade zoomIn"
        id="them-moi"
        tabindex="-1"
        aria-labelledby="CreateModalLabel"
        aria-hidden="true"
        v-model="showModal"
        size="xl"
        hide-footer
        hide-header
        body-class="modal-body"
        no-close-on-backdrop
    >
      <!--      <div class="modal-dialog modal-dialog-centered modal-fullscreen">-->
      <!--        <div class="modal-content border-0">-->
      <!--      -->
      <!--        </div>-->
      <!--      </div>-->
      <form class="" @submit.prevent="HandleSubmit"
            ref="formContainer"
      >
        <div class="modal-header p-3 bg-primary-dark">
          <h5 class="modal-title" id="CreateModalLabel">
            Thông tin văn bản đến
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
                @click="showModal = false"
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
                    track-by="id"
                >
                </treeselect>
                <treeselect-value :value="model.loaiVanBan"/>
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
                <treeselect-value :value="model.trangThai"/>
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
                    v-model="model.soVBDi"
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
                <treeselect-value :value="model.khoiCoQuanNhan"/>
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
                <treeselect-value :value="model.coQuanNhan"/>
              </div>
              <!-- file đính kèm-->
              <div class="col-md-12">
                <label
                    for="validationCoQuanNhan"
                    class="col-form-label col-form-label-sm"
                >File đính kèm</label
                >
                <vue-dropzone
                    ref="myVueDropzone"
                    id="dropzone"
                    :options="dropzoneOptions"
                    v-on:vdropzone-removed-file="removeCommentFile"
                    v-on:vdropzone-success="addCommentFile"
                />
                <ul class="list-unstyled mb-0" id="dropzone-preview">
                  <div
                      class="border rounded"
                      v-for="(file, index) of files"
                      :key="index"
                  >
                    <div class="d-flex p-2">
                      <div class="flex-grow-1">
                        <div class="pt-1">
                          <h5 class="fs-14 mb-1" data-dz-name="">
                            {{ file.name }}
                          </h5>
                          <p class="fs-13 text-muted mb-0" data-dz-size="">
                            <strong>{{ file.size / 1024 }}</strong> KB
                          </p>
                          <strong
                              class="error text-danger"
                              data-dz-errormessage=""
                          ></strong>
                        </div>
                      </div>
                      <div class="flex-shrink-0 ms-3">
                        <button
                            data-dz-remove=""
                            class="btn btn-sm btn-danger"
                            @click="deleteRecord"
                        >
                          Delete
                        </button>
                      </div>
                    </div>
                  </div>
                </ul>
              </div>


              <!--  end vue dropzone -->

            </div>
          </div>
          <div class="col-md-5">
            <div class="row">
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
                    :normalizer="normalizer"
                >
                </treeselect>
                <treeselect-value :value="model.donViSoan"/>
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
                    :options="optionUser"
                >
                </treeselect>
                <treeselect-value :value="model.canBoSoan"/>
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
                <treeselect-value :value="model.hinhThucGui"/>
              </div>
              <!--        Lĩnh vực-->
              <div class="col-md-6">
                <label
                    for="validationLinhVuc"
                    class="col-form-label col-form-label-sm"
                >Lĩnh Vực</label
                >
                <treeselect
                    placeholder="Chọn hình thức nhận"
                    v-model="model.linhVuc"
                    :options="optionLinhVuc"
                >
                </treeselect>
                <treeselect-value :value="model.linhVuc"/>
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
                <treeselect-value :value="model.mucDoTinhChat"/>
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
                <treeselect-value :value="model.mucDoBaoMat"/>
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
                <treeselect-value :value="model.hoSoDonVi"/>
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
                <textarea v-model="model.ghiChu" rows="2" class="form-control"/>
              </div>
            </div>
          </div>
        </div>
        <!-- end content -->
      </form>
    </b-modal>

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
                  <treeselect-value :value="model.loaiVanBan"/>
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
                  <treeselect-value :value="model.trangThai"/>
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
                  <treeselect-value :value="model.khoiCoQuanNhan"/>
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
                  <treeselect-value :value="model.coQuanNhan"/>
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
                    <input type="file" class="form-control" id="inputGroupFile03"
                           aria-describedby="inputGroupFileAddon03" aria-label="Upload">
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
                  <treeselect-value :value="model.donViSoan"/>
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
                  <treeselect-value :value="model.canBoSoan"/>
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
                  <treeselect-value :value="model.hinhThucGui"/>
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
                  <treeselect-value :value="model.mucDoTinhChat"/>
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
                  <treeselect-value :value="model.mucDoBaoMat"/>
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
                  <treeselect-value :value="model.hoSoDonVi"/>
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
                  <textarea v-model="model.ghiChu" rows="2" class="form-control"/>
                </div>
              </div>
            </div>
          </div>
          <!-- end content -->
        </div>
      </div>
    </div>
    <!-- Modal phân công -->
    <b-modal
        class="modal fade zoomIn"
        id="phancong"
        v-model="showPhanCongModal"
        hide-header
        hide-footer

    >
      <div class="modal-dialog modal-dialog-centered modal-lg">
        <div class="modal-content border-0">
          <div class="modal-header p-3 bg-primary-dark">
            <h5 class="modal-title" id="phancongLabel">Phân công</h5>
          </div>
          <div class="modal-body p-3">
            <from
                ref="formContainer">
              <div class="row">
                <div class="col-12">
                  <div class="inner-repeater mb-4">
                    <div class="inner mb-3">
                      <div
                          v-for="(data, index) in tempPhanCongData"
                          :key="index"
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
                                  v-model="data.userId"
                                  :options="optionUser"
                              >
                              </treeselect>
                              <treeselect-value :value="data.userId" />
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
              <div class="modal-footer">
                <button type="button" class="btn btn-light" data-bs-dismiss="modal">
                  Đóng
                </button>
                <button type="button" @click="handleSubmitPhanCong" class="btn btn-primary">Lưu</button>
              </div>
            </from>
          </div>

        </div>
      </div>
    </b-modal>
    <b-modal
        ref="modal"
        content-class="p-5"
        hide-header
        hide-footer
        v-model="showDeleteModal"
    >
      <div class="text-center">
        <i class="ri-error-warning-line text-warning" style="font-size: 100px;"></i>
        <p class="fs-4">Bạn có chắc muốn xóa không?</p>
      </div>
      <div class="d-flex justify-content-center">
        <b-button
            type="button"
            class="btn btn-danger waves-effect waves-light me-2 d-flex align-items-center"
            data-bs-dismiss="modal"
            aria-label="Close"
            id="close-modal"
            @click="showDeleteModal = false"
        >
          Hủy
        </b-button>
        <b-button
            type="submit"
            class="btn btn-primary waves-effect waves-light me-2 d-flex align-items-center"
            @click="handleDelete"
        >
          Xóa
        </b-button>
      </div>
    </b-modal>
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
.modal-body{
  padding: 0px;
}
</style>
