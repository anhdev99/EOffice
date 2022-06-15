<script>
import Layout from "../../../layouts/main";
import PageHeader from "@/components/page-header";
import appConfig from "../../../../app.config.json";
import {data} from "./data";
import {vanBanDenModel} from '@/models/vanBanDenModel';
import Suavanbanden from "./suavanbanden.vue";
// import the component
import Treeselect from "vue3-treeselect";
// import the styles
import "vue3-treeselect/dist/vue3-treeselect.css";
import flatPickr from "vue-flatpickr-component";
import "flatpickr/dist/flatpickr.css";
import 'dropzone-vue/dist/dropzone-vue.common.css';
import vueDropzone from 'vue2-dropzone-vue3'

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
      title: "Văn bản đến",
      items: [
        {
          text: "Quản lý văn bản đến",
          href: "/",
        },
        {
          text: "Văn bản đến",
          active: true,
        },
      ],
      data: data,
      model: vanBanDenModel.baseJson(),
      optionLoaiVanBan: [
        {
          id: "",
          label: "",
        }
      ],
      optionTrangThai: [
        {
          id: "",
          label: "",
        }
      ],
      optionDonVi: [
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
      optionLinhVuc: [
        {
          id: "",
          label: "",
        }
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
      optionMucDoTinhChat: [
        {
          id: "THAP",
          label: "Thấp",
        },
        {
          id: "TRUNG BINH",
          label: "Trung Bình",
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
          id: "TRUNG BINH",
          label: "Trung Bình",
        },
        {
          id: "CAO",
          label: "Cao",
        },
      ],
      optionHinhThucNhan: [
        {
          id: "1",
          label: "Trực tiếp",
        },
        {
          id: "2",
          label: "Nhận qua file",
        },
      ],
      dropzoneOptions: {
        url: `${process.env.VUE_APP_API_URL}files/upload`,
        thumbnailWidth: 100,
        thumbnailHeight: 100,
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
    };
  },
  components: {Layout, PageHeader, Suavanbanden, Treeselect, flatPickr, vueDropzone},
  created() {
    this.getLoaiVanBan()
    this.getTrangThai()
    this.getDonVi()
    this.getUser()
    this.getLinhVuc()
  },
  methods: {
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
      console.log("file", items,re);
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
    removeHinhAnh(file, error, xhr){
      let fileHinhAnh = JSON.parse( file.xhr.response);
      if(fileHinhAnh.data && fileHinhAnh.data.id){
        let idFile = fileHinhAnh.data.id;
        let resultData =   this.model.uploadFilesHinhAnh.filter(x => {
          return x.fileId != idFile;
        })
        this.model.uploadFilesHinhAnh= resultData;
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
                  <th scope="col">Số Lưu</th>
                  <th scope="col">Số VB đến</th>
                  <th scope="col">Trích yếu</th>
                  <th scope="col">Hạn xử lý</th>
                  <th scope="col">Trạng thái VB</th>
                  <th scope="col">Thao tác</th>
                </tr>
                </thead>
                <tbody>
                <tr v-if="data.length <= 0" class="text-center">
                  <td colspan="6">Không tìm thấy dữ liệu</td>
                </tr>
                <tr v-else v-for="(item, index) in data" :key="item.id">
                  <td>{{ ++index }}</td>
                  <td>{{ item.soluu }}</td>
                  <td>{{ item.sovanbanden }}</td>
                  <td>{{ item.trichyeu }}</td>
                  <td>
                      <span class="badge badge-soft-danger">{{
                          item.hanxuxly
                        }}</span>
                  </td>
                  <td v-if="item.trangthaivanban == 0">
                      <span class="badge badge-soft-secondary"
                      >Vừa tiếp nhận</span
                      >
                  </td>
                  <td v-if="item.trangthaivanban == 1">
                    <span class="badge badge-soft-primary">Đã tiếp nhận</span>
                  </td>
                  <td v-if="item.trangthaivanban == 2">
                    <span class="badge badge-soft-success">Hoàn thành</span>
                  </td>
                  <td>
                    <div class="hstack gap-3 fs-15">
                      <a href="javascript:void(0);" class="link-info"
                      ><i class="ri-newspaper-line"></i
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

          <form action="">
            <!--  ./start header -->
            <div class="modal-header p-3 bg-primary-dark">
              <h5 class="modal-title" id="CreateModalLabel">
                Thêm mới văn bản đến
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
            <!--  ./end header -->
            <div class="modal-body">
              <ul class="nav nav-tabs mb-3" role="tablist">
                <li class="nav-item">
                  <a
                      class="nav-link active"
                      data-bs-toggle="tab"
                      href="#thongtinchinh"
                      role="tab"
                      aria-selected="false"
                  >
                    Thông tin chính
                  </a>
                </li>
                <li class="nav-item">
                  <a
                      class="nav-link"
                      data-bs-toggle="tab"
                      href="#butphelanhdao"
                      role="tab"
                      aria-selected="false"
                  >
                    Bút phê lãnh đạo
                  </a>
                </li>
                <li class="nav-item">
                  <a
                      class="nav-link"
                      data-bs-toggle="tab"
                      href="#xulyvanban"
                      role="tab"
                      aria-selected="false"
                  >
                    Đơn vị nhận/ Xử lý văn bản
                  </a>
                </li>
              </ul>

              <!--  tabs panes -->

              <div class="tab-content text-muted">
                <div class="tab-pane active" id="thongtinchinh" role="tabpanel">
                  <div class="row">
                    <div class="col-md-7">
                      <div class="row">
                        <!-- số lưu-->
                        <div class="col-md-3">
                          <label
                              for="validationSoLuu"
                              class="col-form-label col-form-label-sm"
                          >Số lưu</label
                          >
                          <span class="text-danger">*</span>
                          <input
                              type="text"
                              class="form-control form-control-sm"
                              id="validationSoLuu"
                              v-model="model.soLuuCV"
                              required
                          />
                          <div class="valid-feedback">Vui lòng thêm số lưu.</div>
                        </div>
                        <!-- số văn bản đến-->
                        <div class="col-md-5">
                          <label
                              for="validationSoVanBanDen"
                              class="col-form-label col-form-label-sm"
                          >Số VB đến</label
                          >
                          <span class="text-danger">*</span>
                          <input
                              type="text"
                              class="form-control form-control-sm"
                              id="validationSoVanBanDen"
                              v-model="model.soVBDen"
                              required
                          />
                          <div class="valid-feedback">Vui lòng thêm số văn bản đến.</div>
                        </div>
                        <!--loai văn bản-->
                        <div class="col-md-4">
                          <label
                              for="validationLoaiVanBan"
                              class="col-form-label col-form-label-sm"
                          >Loại văn bản</label
                          >
                          <treeselect
                              placeholder="Chọn loại văn bản"
                              v-model="model.loaiVanBan"
                              :options="optionLoaiVanBan"
                          >
                          </treeselect>
                          <treeselect-value :value="model.loaiVanBan" />
                          <div class="invalid-feedback">Vui lòng chọn loại văn bản</div>
                        </div>
                        <!-- trích yếu-->
                        <div class="col-md-12">
                          <label
                              for="validationTrichYeu"
                              class="col-form-label col-form-label-sm"
                          >Trích yếu</label
                          >
                          <span class="text-danger">*</span>
                          <textarea
                              class="form-control form-control-sm"
                              id="validationTrichYeu"
                              rows="4"
                              v-model="model.trichYeu"
                              required
                          />
                          <div class="valid-feedback">Vui lòng thêm trích yếu.</div>
                        </div>
                        <!--          Ngày ban hành-->
                        <div class="col-md-6">
                          <label
                              for="validationNgayBanHanh"
                              class="col-form-label col-form-label-sm"
                          >Ngày ban hành</label
                          >
                          <span class="text-danger">*</span>
                          <flat-pickr
                              v-model="model.ngayBanHanh"
                              :config="config"
                              class="form-control form-control-sm"
                          ></flat-pickr>
                          <div class="valid-feedback">Vui lòng thêm ngày ban hành.</div>
                        </div>
                        <!--          Ngày nhận-->
                        <div class="col-md-6">
                          <label
                              for="validationNgayNhan"
                              class="col-form-label col-form-label-sm"
                          >Ngày nhận</label
                          >
                          <span class="text-danger">*</span>
                          <flat-pickr
                              v-model="model.ngayNhan"
                              :config="config"
                              class="form-control form-control-sm"
                          ></flat-pickr>
                          <div class="valid-feedback">Vui lòng thêm ngày nhận</div>
                        </div>
                        <!--          Trạng thái-->
                        <div class="col-md-6">
                          <label
                              for="validationTrangThai"
                              class="col-form-label col-form-label-sm"
                          >Trạng thái văn bản</label
                          >
                          <treeselect
                              placeholder="Chọn loại văn bản"
                              v-model="model.trangThaiVanBan"
                              :options="optionTrangThai"
                          >
                          </treeselect>
                          <treeselect-value :value="model.trangThaiVanBan" />
                        </div>
                        <!--          Ngày ký-->
                        <div class="col-md-6">
                          <label
                              for="validationNgayKy"
                              class="col-form-label col-form-label-sm"
                          >Ngày ký</label
                          >
                          <span class="text-danger">*</span>
                          <flat-pickr
                              v-model="model.ngayKy"
                              :config="config"
                              class="form-control form-control-sm"
                          ></flat-pickr>
                        </div>
                        <!--          Người ký-->
                        <div class="col-md-6">
                          <label
                              for="validationTrangThai"
                              class="col-form-label col-form-label-sm"
                          >Người ký</label
                          >
                          <treeselect
                              :multiple="true"
                              placeholder="Chọn người ký"
                              v-model="model.nguoiKy"
                              :options="optionUser"
                          >
                          </treeselect>
                          <treeselect-value :value="model.nguoiKy" />
                        </div>
                        <!--          Thời hạn xử lý-->
                        <div class="col-md-6">
                          <label
                              for="validationThoiHanXuLy"
                              class="col-form-label col-form-label-sm"
                          >Thời hạn xử lý</label
                          >
                          <span class="text-danger">*</span>
                          <flat-pickr
                              v-model="model.thoiHanXuLy"
                              :config="config"
                              class="form-control form-control-sm"
                          ></flat-pickr>
                        </div>
                        <!--          fiel đính kèm-->
                        <div class="col-md-12">
                          <label
                              for="validationCoQuanNhan"
                              class="col-form-label col-form-label-sm"
                          >File đính kèm</label
                          >
                          <vue-dropzone
                              ref="myVueDropzone"
                              id="dropzone"
                              :use-custom-slot="true"
                              :options="dropzoneOptions"
                              v-on:vdropzone-success="addThisFile"
                              v-on:vdropzone-removed-file="removeHinhAnh"
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
                      </div>
                    </div>
                    <div class="col-md-5">
                      <div class="row">
                        <!--        Khối cơ quan gửi-->
                        <div class="col-md-6">
                          <label
                              for="validationKhoiCoQuanGui"
                              class="col-form-label col-form-label-sm"
                          >Khối cơ quan gửi</label
                          >
                          <treeselect
                              placeholder="Chọn khối cơ quan gửi"
                              v-model="model.khoiCoQuanGui"
                              :options="optionDonVi"
                          >
                          </treeselect>
                          <treeselect-value :value="model.khoiCoQuanGui" />
                        </div>
                        <!--        Cơ quan gửi-->
                        <div class="col-md-6">
                          <label
                              for="validationCoQuanGui"
                              class="col-form-label col-form-label-sm"
                          >Cơ quan gửi</label
                          >
                          <treeselect
                              placeholder="Chọn cơ quan gửi"
                              v-model="model.coQuanGui"
                              :options="optionDonVi"
                          >
                          </treeselect>
                          <treeselect-value :value="model.coQuanGui" />
                        </div>
                        <!--        Hình thức nhận-->
                        <div class="col-md-6">
                          <label
                              for="validationHinhThucNhan"
                              class="col-form-label col-form-label-sm"
                          >Hình thức nhận</label
                          >
                          <treeselect
                              placeholder="Chọn hình thức nhận"
                              v-model="model.hinhThucNhan"
                              :options="optionHinhThucNhan"
                          >
                          </treeselect>
                          <treeselect-value :value="model.hinhThucNhan" />
                        </div>
                        <!--        Lĩnh vực-->
                        <div class="col-md-6">
                          <label
                              for="validationLinhVuc"
                              class="col-form-label col-form-label-sm"
                          >Lĩnh Vực</label
                          >
                          <span class="text-danger">*</span>
                          <treeselect
                              placeholder="Chọn hình thức nhận"
                              v-model="model.linhVuc"
                              :options="optionLinhVuc"
                          >
                          </treeselect>
                          <treeselect-value :value="model.linhVuc" />
                        </div>
                        <!--        Mức độ tính chất-->
                        <div class="col-md-12">
                          <label for="mucDoTinhChat" class="col-form-label col-form-label-sm"
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
                        <div class="col-md-12">
                          <label for="mucDoBaoMat" class="col-form-label col-form-label-sm"
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
                        <div class="col-md-12">
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
                        <div class="col-md-12">
                          <label
                              for="validationNoiLuuTru"
                              class="col-form-label col-form-label-sm"
                          >Nơi lưu trữ</label
                          >
                          <span class="text-danger">*</span>
                          <input
                              type="text"
                              class="form-control form-control-sm"
                              id="validationNoiLuuTru"
                              v-model="model.noiLuuTru"
                              required
                          />
                        </div>
                        <div class="col-md-12 mt-3">
                          <div class="form-check form-switch">
                            <input
                                class="form-check-input"
                                type="checkbox"
                                role="switch"
                                id="congVanChiDoc"
                                v-model="model.congVanChiDoc"
                            />
                            <label class="form-check-label" for="congVanChiDoc"
                            >Là công văn chỉ đọc</label
                            >
                          </div>
                          <div class="form-check form-switch">
                            <input
                                class="form-check-input"
                                type="checkbox"
                                role="switch"
                                id="banChinh"
                                v-model="model.banChinh"
                            />
                            <label class="form-check-label" for="banChinh">Bản chính</label>
                          </div>
                          <div class="form-check form-switch">
                            <input
                                class="form-check-input"
                                type="checkbox"
                                role="switch"
                                id="thongBao"
                                v-model="model.thongBao"
                            />
                            <label class="form-check-label" for="thongBao"
                            >Hiện thị mục thông báo</label
                            >
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
                <div class="tab-pane" id="butphelanhdao" role="tabpanel">
                  <div class="row card-body">
                    <div class="col-md-12">
                      <!-- Bút phê -->
                      <div>
                        <label for="validationButPhe" class="col-form-label col-form-label-sm"
                        >Bút phê</label
                        >
                        <span class="text-danger">*</span>
                        <textarea
                            type="text"
                            class="form-control form-control-sm"
                            id="validationButPhe"
                            v-model="model.butphe"
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
                            v-model="model.ngayButPhe"
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
                            v-model="model.nguoiButPhe"
                            :options="optionUser"
                        >
                        </treeselect>
                        <treeselect-value :value="model.loaiVanBan" />
                        <div class="invalid-feedback">Vui lòng chọn lãnh đạo bút phê.</div>
                      </div>
                    </div>
                  </div>
                </div>
                <div class="tab-pane" id="xulyvanban" role="tabpanel">
                  <for-xu-ly-van-ban/>
                </div>
              </div>
            </div>
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
              Chỉnh sửa văn bản đến
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
          <suavanbanden/>
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

.dropzone {
  height: 100px;
  min-height: 0px !important;
  display: flex;
  flex-direction: row;
  align-items: center;
}
</style>
