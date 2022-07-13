<script>
import Layout from "../../layouts/main";
import PageHeader from "@/components/page-header";
import appConfig from "@/app.config";
import {pagingModel} from "@/models/pagingModel";
import {required} from "vuelidate/lib/validators";
import {vanBanDenModel} from "@/models/vanBanDenModel";
import Multiselect from "vue-multiselect";
import DatePicker from "vue2-datepicker";
import Switches from "vue-switches";


/**
 * Advanced table component
 */
export default {
  page: {
    title: "Văn bản đến",
    meta: [{name: "description", content: appConfig.description}]
  },
  components: {Layout, PageHeader,Multiselect},
  data() {
    return {
      title: "Văn bản đến",
      items: [
        {
          text: "E-Office",
          href: "/"
        },
        {
          text: "Văn bản đến",
          href: "/van-ban-den"
        },
        {
          text: "Danh sách",
          active: true
        }
      ],
      data: [],
      showModal: false,
      showDetail: false,
      showDeleteModal: false,
      submitted: false,
      listCoQuan: [],
      listRole: [],
      model: vanBanDenModel.baseJson(),
      pagination: pagingModel.baseJson(),
      totalRows: 1,
      todoTotalRows: 1,
      currentPage: 1,
      numberOfElement: 1,
      perPage: 10,
      pageOptions: [5, 10, 25, 50, 100],
      filter: null,
      todoFilter: null,
      filterOn: [],
      todofilterOn: [],
      isBusy: false,
      sortBy: "age",
      sortDesc: false,
      fields: [
        {
          key: 'STT',
          label: 'STT',
          thStyle: {width: '50px', minWidth: '50px'},
          class: "text-center"
        },
        {
          key: "soLuuCV",
          label: "Số lưu CV",
          thStyle: {width: '10px', minWidth: '160px'},
          class: "px-1",
          sortable: true,
        },
        {
          key: "soVBDen",
          label: "Số văn bản đến",
          thStyle: {width: '160px', minWidth: '160px'},
          class: "px-1",
        },
        {
          key: "trichYeu",
          label: "Trích yếu",
          thStyle: {width: '100px', minWidth: '100px'},
          class: "px-1",
        },
        {
          key: "hanXuLy",
          label: "Hạn xử lý",
          thStyle: {width: '100px', minWidth: '100px'},
          class: "px-1",
        },
        {
          key: "trangThai",
          label: "Trạng thái",
          thStyle: {width: '100px', minWidth: '100px'},
          class: "px-1",
        },
        {
          key: 'process',
          label: 'Xử lý',
          thStyle: {width: '110px', minWidth: '110px'},
        }
      ],
      optionsLoaiVanBan: null,
      optionDonVi: null,
      optionUser: null,
      optionHinhThucNhan: null,
      optionMucDoTinhChat: null,
      optionMucDoBaoMat: null,
    };
  },
  validations: {
    model: {
      soLuuCV: {required},
      soVBDen: {required},
      loaiVanBan: {required},
      trichYeu: {required},
      ngayBanHanh: {required},
      ngayNhan: {required},
    }
  },
  computed: {
    /**
     * Total no. of records
     */
    rows() {
      return this.data.length;
    },
  },
  async created(){
    this.getLoaiVanBan();
  },
  mounted() {
    // Set the initial number of items
    this.totalRows = this.items.length;
  },
  methods: {
    /**
     * Search the table data with search input
     */
    async fnGetList() {
      await this.onPageChange();
    },
    async onPageChange(page = 1) {
      console.log("LOG ON PAGE CHAGNE : ")
      this.pagination.currentPage = page;
      const params = {
        pageNumber: this.pagination.currentPage,
        pageSize: this.pagination.pageSize,
      }
      this.$refs.tblList?.refresh()
    },
    onFiltered(filteredItems) {
      // Trigger pagination to update the number of buttons/pages due to filtering
      this.totalRows = filteredItems.length;
      this.currentPage = 1;
    },

    handleSubmit() {
      this.submitted = true;
      this.$v.$touch();
      console.log("handleSubmit", this.model);
    },

    handleDetail(id) {

    },
    handleUpdate(id) {

    },
    handlePhanCong(id) {

    },
    handleButPhe(id) {

    },
    handleShowDeleteModal(id) {

    },
    async getLoaiVanBan() {
      await this.$store.dispatch("loaiVanBanStore/getAll").then((res) => {
        if (res.resultCode === "SUCCESS") {
          this.optionsLoaiVanBan = res.data;
        } else {
          this.optionsLoaiVanBan = [];
        }
      });
    },
    myProvider(ctx) {
      const params = {
        start: ctx.currentPage,
        limit: ctx.perPage,
        content: this.filter,
        sortBy: ctx.sortBy,
        sortDesc: ctx.sortDesc,
      }
      this.loading = true

      try {
        let promise = this.$store.dispatch("vanBanDenStore/getPagingParams", params)
        return promise.then(resp => {
          let items = resp.data.data
          this.totalRows = resp.data.totalRows
          this.numberOfElement = resp.data.data.length
          this.loading = false
          console.log("data", items);
          return items || []
        })
      } finally {
        this.loading = false
      }
    }
  }
};
</script>

<template>
  <Layout>
    <PageHeader :title="title" :items="items"/>
    <div class="row">
      <div class="col-12">
        <div class="card">
          <div class="card-body">
            <div class="row mb-2">
              <div class="col-sm-4">
                <div class="search-box me-2 mb-2 d-inline-block">
                  <div class="position-relative">
                    <input
                        v-model="filter"
                        type="text"
                        class="form-control"
                        placeholder="Tìm kiếm ..."
                    />
                    <i class="bx bx-search-alt search-icon"></i>
                  </div>
                </div>
              </div>
              <div class="col-sm-8">
                <div class="text-sm-end">
                  <b-button
                      variant="primary"
                      type="button"
                      class="btn w-md btn-primary"
                      @click="showModal = true"
                      size="sm"
                  >
                    <i class="mdi mdi-plus me-1"></i> Thêm mới
                  </b-button>
                  <!-- Model create -->
                  <b-modal
                      v-model="showModal"
                      title="Tiếp nhận văn bản đến"
                      title-class="text-black font-18"
                      body-class="p-3"
                      hide-footer
                      centered
                      no-close-on-backdrop
                      size="xl"
                  >
                    <form @submit.prevent="handleSubmit"
                          ref="formContainer">
                      <div class="row">
                        <div class="col-md-7">
                          <div class="row">
                            <div class="col-md-3">
                              <div class="mb-3">
                                <label class="form-label" for="validationCustom01">Số lưu CV</label> <span
                                  class="text-danger">*</span>
                                <input
                                    id="validationCustom01"
                                    v-model="model.soLuuCV"
                                    type="text"
                                    class="form-control"
                                    placeholder="eg: 123-dthu"
                                    :class="{
                                      'is-invalid': submitted && $v.model.soLuuCV.$error,
                                    }"
                                />
                                <div
                                    v-if="submitted && $v.model.soLuuCV.$error"
                                    class="invalid-feedback"
                                >
                                <span v-if="!$v.model.soLuuCV.required"
                                >Vui lòng thêm số lưu CV.</span
                                >
                                </div>
                              </div>
                            </div>
                            <div class="col-md-5">
                              <div class="mb-3">
                                <label class="form-label" for="validationCustom01">Số văn bản đến</label> <span
                                  class="text-danger">*</span>
                                <input
                                    id="validationSoVBDen"
                                    v-model="model.soVBDen"
                                    type="text"
                                    class="form-control"
                                    placeholder="eg: 123-dthu"
                                    :class="{
                                      'is-invalid': submitted && $v.model.soVBDen.$error,
                                    }"
                                />
                                <div
                                    v-if="submitted && $v.model.soVBDen.$error"
                                    class="invalid-feedback"
                                >
                                <span v-if="!$v.model.soVBDen.required"
                                >Vui lòng thêm số văn bản đến.</span
                                >
                                </div>
                              </div>
                            </div>
                            <div class="col-md-4">
                              <div class="mb-3">
                                <label class="form-label" for="validationCustom01">Loại văn bản</label> <span
                                  class="text-danger">*</span>
                                <multiselect
                                    v-model="model.loaiVanBan"
                                    :options="optionsLoaiVanBan"
                                    track-by="id"
                                    label="ten"
                                ></multiselect>
                                <div
                                    v-if="submitted && $v.model.soVBDen.$error"
                                    class="invalid-feedback"
                                >
                                <span v-if="!$v.model.soVBDen.required"
                                >Vui lòng thêm số văn bản đến.</span
                                >
                                </div>
                              </div>
                            </div>
                          </div>
                        </div>
                        <div class="col-md-5">

                        </div>
                      </div>
                      <div class="text-end pt-2 mt-3">
                        <b-button variant="light" class="w-md" size="sm" @click="showModal = false">
                          Đóng
                        </b-button>
                        <b-button type="submit" variant="primary" size="sm" class="ms-1 w-md">
                          Lưu
                        </b-button>
                      </div>
                    </form>
                  </b-modal>
                  <!-- Model detail -->
                  <b-modal
                      v-model="showDetail"
                      title="Thông tin chi tiết lĩnh vực"
                      title-class="text-black font-18"
                      body-class="p-3"
                      hide-footer
                      centered
                      no-close-on-backdrop
                      size="lg"
                  >
                    <form @submit.prevent="handleSubmit" ref="formContainer">
                      <div class="row">

                      </div>
                      <div class="text-end pt-2 mt-3">
                        <b-button variant="light" @click="showDetail = false">
                          Đóng
                        </b-button>
                      </div>
                    </form>
                  </b-modal>
                </div>
              </div>
            </div>
            <div class="row">
              <div class="col-12">
                <div class="row mb-3">
                  <div class="col-sm-12 col-md-6">
                    <div id="tickets-table_length" class="dataTables_length">
                      <label class="d-inline-flex align-items-center">
                        Hiện
                        <b-form-select
                            class="form-select form-select-sm"
                            v-model="perPage"
                            size="sm"
                            :options="pageOptions"
                        ></b-form-select
                        >&nbsp;dòng
                      </label>
                    </div>
                  </div>
                </div>
                <!-- Table -->
                <div class="table-responsive-sm">
                  <b-table
                      class="datatables"
                      :items="myProvider"
                      :fields="fields"
                      striped
                      bordered
                      responsive="sm"
                      :per-page="perPage"
                      :current-page="currentPage"
                      :sort-by.sync="sortBy"
                      :sort-desc.sync="sortDesc"
                      :filter="filter"
                      :filter-included-fields="filterOn"
                      ref="tblList"
                      primary-key="id"
                      :busy.sync="isBusy"
                  >
                    <template v-slot:cell(STT)="data">
                      {{ data.index + ((currentPage - 1) * perPage) + 1 }}
                    </template>
                    <template v-slot:cell(hanXuLy)="data">
                      <span class="badge bg-danger"> {{ data.item.hanXuLy }}</span>
                    </template>
                    <template v-slot:cell(process)="data">
                      <button
                          type="button"
                          size="sm"
                          class="btn btn-outline btn-sm"
                          data-toggle="tooltip" data-placement="bottom" title="Chi tiết"
                          v-on:click="handleDetail(data.item.id)">
                        <i class="fas fa-eye  text-warning me-1"></i>
                      </button>
                      <button
                          type="button"
                          size="sm"
                          class="btn btn-outline btn-sm"
                          data-toggle="tooltip" data-placement="bottom" title="Cập nhật"
                          v-on:click="handleUpdate(data.item.id)">
                        <i class="fas fa-pencil-alt text-success me-1"></i>
                      </button>
                      <button
                          type="button"
                          size="sm"
                          class="btn btn-outline btn-sm"
                          data-toggle="tooltip" data-placement="bottom" title="Cập nhật"
                          v-on:click="handlePhanCong(data.item.id)">
                        <i class="fas fa-user-plus text-info me-1"></i>
                      </button>
                      <button
                          type="button"
                          size="sm"
                          class="btn btn-outline btn-sm"
                          data-toggle="tooltip" data-placement="bottom" title="Cập nhật"
                          v-on:click="handleButPhe(data.item.id)">
                        <i class="fas fa-feather-alt text-primary me-1"></i>
                      </button>
                      <button
                          type="button"
                          size="sm"
                          class="btn btn-outline btn-sm"
                          data-toggle="tooltip" data-placement="bottom" title="Xóa"
                          v-on:click="handleShowDeleteModal(data.item.id)">
                        <i class="fas fa-trash-alt text-danger me-1"></i>
                      </button>
                    </template>
                    <template v-slot:cell(ten)="data">&nbsp;&nbsp;
                      {{ data.item.ten }}
                    </template>
                  </b-table>
                  <template v-if="isBusy">
                    <div align="center">Đang tải dữ liệu</div>
                  </template>
                  <template v-if="totalRows <= 0 && !isBusy">
                    <div align="center">Không có dữ liệu</div>
                  </template>
                </div>
                <!-- Paginnation -->
                <div class="row">
                  <b-col>
                    <div>Hiển thị {{ numberOfElement }} trên tổng số {{ totalRows }} dòng</div>
                  </b-col>
                  <div class="col">
                    <div
                        class="dataTables_paginate paging_simple_numbers float-end">
                      <ul class="pagination pagination-rounded mb-0">
                        <!-- pagination -->
                        <b-pagination
                            v-model="currentPage"
                            :total-rows="totalRows"
                            :per-page="perPage"
                        ></b-pagination>
                      </ul>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
        <b-modal
            v-model="showDeleteModal"
            centered
            title="Xóa dữ liệu"
            title-class="font-18"
            no-close-on-backdrop
        >
          <p>
            Dữ liệu xóa sẽ không được phục hồi!
          </p>
          <template #modal-footer>
            <b-button v-b-modal.modal-close_visit
                      size="sm"
                      class="btn btn-outline-info w-md"
                      v-on:click="showDeleteModal = false">
              Đóng
            </b-button>
            <b-button v-b-modal.modal-close_visit
                      size="sm"
                      variant="danger"
                      type="button"
                      class="w-md"
                      v-on:click="handleDelete">
              Xóa
            </b-button>
          </template>
        </b-modal>
      </div>
    </div>
  </Layout>
</template>
<style>
.td-stt {
  text-align: center;
  width: 50px;
}

.td-xuly {
  text-align: center;
  width: 130px;
}

.table > tbody > tr > td {
  padding: 0px;
  line-height: 30px;
}

.hidden-sortable:after, .hidden-sortable:before {
  display: none !important;
}
</style>