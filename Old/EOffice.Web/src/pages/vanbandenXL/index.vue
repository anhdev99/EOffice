<script>
import Layout from "@/layouts/main";
import PageHeader from "@/components/page-header";
import Treeselect from "vue3-treeselect";
// import the styles
import "vue3-treeselect/dist/vue3-treeselect.css";
import flatPickr from "vue-flatpickr-component";
import "flatpickr/dist/flatpickr.css";
import {nhanXuLyModel} from "../../models/nhanXuLyModel";
import {data} from "./data"

export default {
  page: {
    title: "Văn bản đến",
    meta: [
      {
        name: "van-ban-den",
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
          children: [
            {
              id: "",
              label: "",
            }
          ]
        }
      ],
      optionMucDoQuanTrong: [
        {
          id: "THAP",
          label: "Thấp"
        },
        {
          id: "TRUNG BINH",
          label: "Trung bình"
        },
        {
          id: "CAO",
          label: "Cao"
        },
      ],
      model: nhanXuLyModel.baseJson(),
      showModal: false,
      showModalPCXL: false,
      config: {
        wrap: true, // set wrap to true only when using 'input-group'
        altFormat: "M j, Y",
        dateFormat: "d/m/Y",
      },
      tempPhanCongData: [{id: 1}],
    };
  },
  components: {Layout, PageHeader, Treeselect},
  created() {
    this.getDonVi()
    this.getUser()
  },
  methods: {
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
    addTempPhanCong() {
      this.tempPhanCongData.push({nguoiThucHien: "", ghiChu: ""});
    },
    deleteTempPhanCong(index) {
      this.tempPhanCongData.splice(index, 1);
    },
  },
};
</script>

<template>
  <Layout>
    <PageHeader :title="title" :items="items"/>

    <div class="row">
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
                >
                  <i class="ri-add-line align-bottom me-1"></i> PDF
                </button>
              </div>
              <div
                  class="form-check form-switch form-switch-right form-switch-md"
              >
                <button
                    class="btn btn-primary add-btn btn-sm"
                    data-bs-toggle="modal"
                >
                  <i class="ri-add-line align-bottom me-1"></i> EXCEL
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
                          v-b-modal.v-edit-modal
                          @click="showModal = true"
                      ><i class="ri-git-repository-line"></i
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

    <!--  Modal chuyển xử lý -->
    <b-modal
        class="modal fade zoomIn"
        id="PhanCongXuLy"
        v-model="showModalPCXL"
        size="lg"
        hide-header
        hide-footer
        body-class="p-0"
    >
      <div class="modal-header p-3 bg-primary-dark">
        <h5 class="modal-title" id="PhanCongXuLyModalLabel">
          Phân công xử lý
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
      <div class="p-3">
        <from id="PhanCong" class="repeater" @submit.prevent="PhanCong">
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
                              v-model="model.nguoiThucHien"
                              :options="optionUser"
                          >
                          </treeselect>
                          <treeselect-value :value="model.nguoiThucHien"/>
                        </div>
                      </div>
                      <!-- Ghi chú -->
                      <div class="row mb-2">
                        <div class="col-lg-3">
                          <label for="" class="form-label">Ghi chú</label>
                        </div>
                        <div class="col-lg-9">
                    <textarea
                        v-model="model.ghiChu"
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
      </div>
    </b-modal>

    <!-- Modal chỉnh sửa -->
    <b-modal
        v-model="showModal"
        size="lg"
        hide-header
        hide-footer
        body-class="p-0"
    >
      <div class="modal-header pb-3 bg-primary-dark m-0">
        <h5 class="modal-title" id="EditModalLabel">
          Báo cáo xử lý văn bản
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
      <!-- start content  -->
      <div class="row card-body p-4">
        <div class="col-12">
          <div class="row">
            <!-- Số lưu - disable -->
            <div class="col-sm-12 col-md-6">
              <label class="col-form-label col-form-label-sm">Số lưu</label>
              <span class="text-danger">*</span>
              <input
                  type="text"
                  class="form-control border-0"
                  id="SoLuu"
                  v-model="model.soLuuCV"
                  disabled
              />
            </div>
            <!-- Số văn bản đến -->
            <div class="col-sm-12 col-md-6">

              <label class="col-form-label col-form-label-sm">Số văn bản đến</label>
              <span class="text-danger">*</span>
              <input
                  type="text"
                  class="form-control border-0"
                  id="SoVanBanDen"
                  v-model="model.soVanBanDen"
                  disabled
              />
            </div>
            <!-- Trích yếu - disable -->
            <div class="col-12">
              <label class="col-form-label col-form-label-sm">Trích yếu</label>
              <span class="text-danger">*</span>
              <textarea
                  rows="2"
                  class="form-control border-0"
                  id="TrichYeu"
                  v-model="model.trichYeu"
                  disabled
              >
                </textarea>
            </div>
          </div>
          <!-- Trạng thái -->
          <div class="mb-2">
            <label class="col-form-label col-form-label-sm">Chọn trạng thái</label>
            <div>
              <div class="form-check form-switch form-check-inline" dir="ltr">
                <input type="radio" class="form-check-input" name="TrangThai" id="TrangThaiHoanThanh" value="HOANTHANH">
                <label class="form-check-label" for="TrangThaiHoanThanh">Đã hoàn thành</label>
              </div>
              <div class="form-check form-switch form-check-inline" dir="ltr">
                <input type="radio" class="form-check-input" name="TrangThai" id="TrangThaiChuaHoanThanh" value="CHUAHOANTHANH">
                <label class="form-check-label" for="TrangThaiChuaHoanThanh">Chưa hoàn thành</label>
              </div>
              <div class="form-check form-switch form-check-inline" dir="ltr">
                <input type="radio" class="form-check-input" name="TrangThai" id="TrangThaiChuyenXuLy" value="CHUYENXULY">
                <label class="form-check-label" for="TrangThaiChuyenXuLy">Phân công xử lý</label>
              </div>
            </div>


          </div>


          <!-- Lãnh đạo bút phê -->
          <div >
            <label
                class="col-form-label col-form-label-sm"
            >Người nhhận xử lý</label
            >
            <treeselect
                placeholder="Chọn người nhận xử lý"
                v-model="model.nguoiNhanXuLy"
                :options="optionUser"
            >
            </treeselect>
            <treeselect-value :value="model.lanhDaoButPhe"/>
          </div>

          <!-- Mức độ tính chất -->
          <div>
            <label class="col-form-label col-form-label-sm">Mức độ quan trọng</label>
            <treeselect
                placeholder="Chọn mức độ quan trọng"
                v-model="model.mucDoQuanTrong"
                :options="optionMucDoQuanTrong"
            >
            </treeselect>
            <treeselect-value :value="model.lanhDaoButPhe"/>
          </div>
          <!-- file đính kèm -->
          <div class="input-group mt-1">
            <label class="input-group-text" for="inputGroupFile01"
            ><i class="ri-file-upload-line"></i
            ></label>
            <input type="file" class="form-control" id="inputGroupFile01"/>
          </div>
        </div>

      </div>
      <!-- end content  -->
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
</style>
