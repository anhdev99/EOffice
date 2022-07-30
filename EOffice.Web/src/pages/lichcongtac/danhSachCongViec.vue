<script>
import Layout from "../../layouts/main";

import PageHeader from "@/components/page-header";
import {required} from "vuelidate/lib/validators";
import appConfig from "@/app.config";
import {notifyModel} from "@/models/notifyModel";
import {pagingModel} from "@/models/pagingModel";
import {lichCongTacModel} from "@/models/lichCongTacModel";
import {CONSTANTS} from "@/helpers/constants";
import DatePicker from "vue2-datepicker";
import Multiselect from "vue-multiselect";
import {congViecModel} from "@/models/congViecModel";
import CKEditor from "@ckeditor/ckeditor5-vue";
import ClassicEditor from "@ckeditor/ckeditor5-build-classic";

export default {
  page: {
    title: "Công việc",
    meta: [{name: "description", content: appConfig.description}],
  },
  components: {Layout, PageHeader, DatePicker, Multiselect,     ckeditor: CKEditor.component,},
  data() {
    return {
      title: "Công việc",
      items: [
        {
          text: "Công việc",
          href: "#",
          // active: true,
        },
        {
          text: "Danh sách",
          active: true,
        }
      ],
      data: [],
      showModal: false,
      showDetail: false,
      showDeleteModal: false,
      submitted: false,
      model: lichCongTacModel.baseJson(),
      modelCongViec: congViecModel.baseJson(),
      listCoQuan: [],
      listRole: [],
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
      isBusy: false,
      sortBy: "age",
      sortDesc: false,
      fields: [
        {key: 'STT', label: 'STT', class: 'td-stt', sortable: false, thClass: 'hidden-sortable'},
        {
          key: "tuNgay",
          label: "Ngày",
          sortable: true,
          thStyle: {width: '100px', minWidth: '100px'},
          class: "text-center"
        },
        {
          key: "thoiGian",
          label: "Thời gian",
          thClass: 'hidden-sortable',
          thStyle: {width: '100px', minWidth: '100px'},
          sortable: false,
          class: "text-center"
        },
        {
          key: 'noiDung',
          label: "Nội dung",
          thClass: 'hidden-sortable',
          sortable: false,
          class: "px-2",
        },
        {
          key: 'diaDiem',
          label: "Địa điểm",
          thClass: 'hidden-sortable',
          sortable: false,
          class: "px-2",
        },
        {
          key: 'thanhPhan',
          label: "Thành phần",
          thClass: 'hidden-sortable',
          sortable: false,
          class: "px-2",
        },
        {
          key: 'ghiChu',
          label: "Ghi chú",
          thClass: 'hidden-sortable',
          sortable: false,
          class: "px-2",
        },
        {
          key: 'process',
          label: 'Xử lý',
          class: 'td-xuly',
          thClass: 'hidden-sortable',
          sortable: false
        }
      ],
      optionsUser: [],
      showCongViecModel: false,
      editor: ClassicEditor,
      editorConfig: {
        height: "200px"
      }
    };
  },
  validations: {
    model: {
      ngayXepLich: {required},
      chuTri: {required}
    },
    modelCongViec:{
      tuNgay: {required},
    }
  },
  created() {
    // this.fnGetList();
    let lichCongTacId = this.$route.params.id;
    if(lichCongTacId == null){
      this.$router.push('/lich-cong-tac-ca-nhan');
    }
    this.handleDetail(this.$route.params.id)
    this.getUser();
  },
  watch: {
    model: {
      deep: true,
      handler(val) {
        // addCoQuanToModel()
        // this.saveValueToLocalStorage()
      }
    },
  },
  methods: {
    async fnGetList() {
      await this.onPageChange();
    },
    async getUser() {
      try {
        await this.$store.dispatch("userStore/getAll").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionsUser = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },
    async onPageChange() {
      this.$refs.tblList?.refresh()
    },
    async handleUpdate(value) {
      await this.$store.dispatch("lichCongTacStore/getByIdCongViec", value).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.modelCongViec = congViecModel.fromJson(res.data);

          this.showCongViecModel = true;
        } else {
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        }
      });
    },
    async handleDetail(id) {
      await this.$store.dispatch("lichCongTacStore/getById", id).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.model = lichCongTacModel.fromJson(res.data);
          this.modelCongViec.tuNgay = this.model.ngayXepLich;
          this.showDetail = true;
        } else {
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
          this.$router.push("/lich-cong-tac-ca-nhan")
        }
      });
    },
    async handleDelete() {
      if (this.modelCongViec.id != 0 && this.modelCongViec.id != null && this.modelCongViec.id) {
        await this.$store.dispatch("lichCongTacStore/deleteCongViec", this.modelCongViec).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            this.modelCongViec = congViecModel.baseJson();
            this.handleDetail(this.$route.params.id)
            this.showDeleteModal = false;
          }
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
          // });
        });
      }
    },
    handleShowDeleteModal(value) {
      this.modelCongViec = value;
      this.showDeleteModal = true;
    },
    async handleSubmit(e) {
      e.preventDefault();
      this.submitted = true;
      this.$v.$touch();
      if (this.$v.modelCongViec.$invalid) {
        return;
      } else {
        let loader = this.$loading.show({
          container: this.$refs.formContainer,
        });
        if (
            this.modelCongViec.id != 0 &&
            this.modelCongViec.id != null &&
            this.modelCongViec.id
        ) {
          // Update model
          await this.$store.dispatch("lichCongTacStore/updateCongViec", this.modelCongViec).then((res) => {
            if (res.resultCode === 'SUCCESS') {
              this.modelCongViec = congViecModel.baseJson();
              this.handleDetail(this.$route.params.id)
              this.showCongViecModel = false;
            }
            this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
          });
        } else {
          // Create model
          await this.$store.dispatch("lichCongTacStore/createCongViec", this.modelCongViec).then((res) => {
            if (res.resultCode === 'SUCCESS') {
              this.modelCongViec = congViecModel.baseJson();
              this.handleDetail(this.$route.params.id)
              this.showCongViecModel = false;

            }
            this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
          });
        }
        loader.hide();
      }
      this.submitted = false;
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
        let promise = this.$store.dispatch("lichCongTacStore/getPagingParamsCaNhan", params)
        return promise.then(resp => {
          if (resp.resultCode == CONSTANTS.SUCCESS) {
            let data = resp.data;
            console.log("data", data);
            this.totalRows = data.totalRows
            let items = data.data
            this.numberOfElement = items.length
            this.loading = false
            return items || []
          } else {
            return [];
          }
        })
      } finally {
        this.loading = false
      }
    },
    handleShowCongViecModal(data) {
      this.modelCongViec.lichCongTacId = this.$route.params.id;
      this.showCongViecModel = true;
    },
  }
}
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
                  <div class="position-relative" style="display: flex; flex-direction: column;">
                  <h5>{{model.ngayXepLich}}</h5>
                    <h6>Người chủ trì: </h6>
                    <span v-for="(value, index) in model.chuTri" :key="index">
                      {{index+1}}.{{value.fullName}} - {{value.donVi.ten}}
                    </span>
                  </div>
                </div>
              </div>
              <div class="col-sm-8">
                <div class="text-sm-end">
                  <b-button
                      variant="primary"
                      type="button"
                      class="btn w-md btn-primary"
                      @click="handleShowCongViecModal"
                      size="sm"
                  >
                    <i class="mdi mdi-plus me-1"></i> Thêm mới
                  </b-button>
                  <!--                  Modal create -->
                  <b-modal
                      v-model="showCongViecModel"
                      title="Thông tin công việc"
                      title-class="text-black font-18"
                      body-class="p-3"
                      hide-footer
                      centered
                      no-close-on-backdrop
                      size="xl"
                  >
                    <template #modal-header="{ close }">
                      <!-- Emulate built in modal header close button action -->
                      <h5> Thông tin công việc</h5>
                      <div class="text-end">
                        <b-button variant="light" class="w-md" size="sm" @click="showCongViecModel = false">
                          Đóng
                        </b-button>
                        <b-button @click="handleSubmit" variant="primary" size="sm" class="ms-1 w-md">
                          Lưu
                        </b-button>
                      </div>
                    </template>
                    <form @submit.prevent="handleSubmit"
                          ref="formContainer">
                      <div class="row">
                        <div class="col-3">
                          <div class="mb-3">
                            <label class="text-left">Ngày bắt đầu</label>
                            <span style="color: red">&nbsp;*</span>
                            <input type="hidden" v-model="modelCongViec.id"/>
                            <date-picker v-model="modelCongViec.tuNgay"
                                         format="DD/MM/YYYY"
                                         value-type="format"
                                         readonly=""
                            >
                              <div slot="input">
                                <input v-model="modelCongViec.tuNgay"
                                       v-mask="'##/##/####'" type="text" class="form-control"
                                       placeholder="Nhập ngày bắt đầu"
                                readonly
                                />
                              </div>
                            </date-picker>
                          </div>
                        </div>
                        <div class="col-3">
                          <div class="mb-3">
                            <label class="text-left">Ngày kết thúc</label>
                            <date-picker v-model="modelCongViec.denNgay"
                                         format="DD/MM/YYYY"
                                         value-type="format"
                            >
                              <div slot="input">
                                <input v-model="modelCongViec.denNgay"
                                       v-mask="'##/##/####'" type="text" class="form-control"
                                       placeholder="Nhập ngày kết thúc"/>
                              </div>
                            </date-picker>
                          </div>
                        </div>
                        <div class="col-3">
                          <div class="mb-3">
                            <label class="text-left">Thời gian</label>
                            <input v-model.trim="modelCongViec.thoiGian" v-mask="'##:##'" type="text" class="form-control" placeholder="##:##" />
                          </div>
                        </div>
                        <div class="col-md-3">
                          <div class="mb-3">
                            <label class="text-left">Màu sắc</label>
                            <input
                                type="color"
                                class="form-control-color mw-100 form-control"
                                id="example-color-input"
                                v-model="modelCongViec.mauSac"
                            />
                          </div>
                        </div>
                        <div class="col-md-6">
                          <div class="mb-2">
                            <label class="form-label" for="validationCustom01">Nội dung</label>
                            <span style="color: red">&nbsp;*</span>
                            <ckeditor
                                v-model="modelCongViec.noiDung"
                                :editor="editor"
                                :config="editorConfig"
                            ></ckeditor>
                          </div>
                        </div>
                        <div class="col-md-6">
                          <div class="mb-2">
                            <label class="form-label" for="validationCustom01">Địa điểm</label>
                            <span style="color: red">&nbsp;*</span>
                            <ckeditor
                                v-model="modelCongViec.diaDiem"
                                :editor="editor"
                                :config="editorConfig"
                            ></ckeditor>
                          </div>
                        </div>

                        <div class="col-12">
                          <div class="mb-3">
                            <label class="text-left">Thành phần tham dự</label>
                            <multiselect
                                :multiple="true"
                                v-model="modelCongViec.thanhPhanThamDu"
                                :options="optionsUser"
                                track-by="id"
                                label="fullName"
                                placeholder="Chọn người tham dự"
                                deselect-label="Nhấn để xoá"
                                selectLabel="Nhấn enter để chọn"
                                selectedLabel="Đã chọn"
                            ></multiselect>
                          </div>
                        </div>
                        <div class="col-md-6">
                          <div class="mb-2">
                            <label class="form-label" for="validationCustom01">Thành phần khác</label>
                            <ckeditor
                                v-model="modelCongViec.thanhPhan"
                                :editor="editor"
                                :config="editorConfig"
                            ></ckeditor>
                          </div>
                        </div>
                        <div class="col-md-6">
                        <div class="mb-2">
                          <label class="form-label" for="validationCustom01">Ghi chú</label>
                          <ckeditor
                              v-model="modelCongViec.ghiChu"
                              :editor="editor"
                              :config="editorConfig"
                          ></ckeditor>
                        </div>
                      </div>
                      </div>

                    </form>
                  </b-modal>
                </div>
              </div>
            </div>
            <div class="row">
              <div class="col-12">
                <div class="table-responsive-sm">
                  <b-table
                      class="datatables"
                      :items="model.congViecs"
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
                    <template v-slot:cell(tuNgay)="data">
                      <div>{{data.item.tuNgay}}</div>
                      <div v-if="data.item.denNgay">
                        -{{data.item.denNgay}}
                      </div>
                      <div v-else>
                        -...
                      </div>
                    </template>
                    <template v-slot:cell(noiDung)="data">
                      <div v-if="data.item.noiDung" :inner-html.prop="data.item.noiDung">
                      </div>
                    </template>
                    <template v-slot:cell(diaDiem)="data">
                      <div v-if="data.item.diaDiem" :inner-html.prop="data.item.diaDiem">
                      </div>
                    </template>
                    <template v-slot:cell(ghiChu)="data">
                      <div v-if="data.item.ghiChu" :inner-html.prop="data.item.ghiChu">
                      </div>
                    </template>
                    <template v-slot:cell(thanhPhan)="data">
                      <div v-if="data.item.thanhPhanThamDu && data.item.thanhPhanThamDu.length">
                        <div class="title-capso">Thành phần tham dự: </div>
                            <div class="badge font-size-small min-width-30 p-2 btn-xs font-weight-semibold bg-success" v-for="(value, index) in data.item.thanhPhanThamDu" :key="index">
                              {{value.fullName}}-<span style="color: #560404">{{value.donVi.ten}}</span>
                            </div>
                      </div>
                      <div  v-if="data.item.thanhPhan">
                        <div class="title-capso">Thành phần khác: </div>
                        <div :inner-html.prop="data.item.thanhPhan">
                        </div>
                      </div>
                    </template>
                    <template v-slot:cell(process)="data">
                      <button
                          type="button"
                          size="sm"
                          class="btn btn-outline btn-sm"
                          data-toggle="tooltip" data-placement="bottom" title="Cập nhật"
                          v-on:click="handleUpdate(data.item)">
                        <i class="fas fa-pencil-alt text-success me-1"></i>
                      </button>
                      <button
                          type="button"
                          size="sm"
                          class="btn btn-outline btn-sm"
                          data-toggle="tooltip" data-placement="bottom" title="Xóa"
                          v-on:click="handleShowDeleteModal(data.item)">
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

.ck-editor__editable{
  min-height: 100px !important;
}
.title-capso{
  font-weight: 500; color: #00568C; margin-right: 10px;

}
</style>
