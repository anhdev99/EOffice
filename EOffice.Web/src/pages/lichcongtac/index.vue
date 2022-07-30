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
import {FunctionalCalendar} from 'vue-functional-calendar';

export default {
  page: {
    title: "Lịch công tác",
    meta: [{name: "description", content: appConfig.description}],
  },
  components: {Layout, PageHeader, DatePicker, Multiselect, FunctionalCalendar},
  data() {
    return {
      title: "Lịch công tác",
      items: [
        {
          text: "Lịch công tác",
          href: "/lich-cong-tac",
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
      todofilterOn: [],
      isBusy: false,
      sortBy: "age",
      sortDesc: false,
      optionsUser: [],
      calendarData: {},
      markedDates: ["6-3-2021"],
      calendarConfigs: {
        sundayStart: false,
        dateFormat: 'dd/mm/yyyy',
        isDatePicker: false,
        isDateRange: false
      },
      dayNames: ['T2', 'T3', 'T4', 'T5', 'T6', 'T7', 'CN'],
      monthNames: [
        "Tháng 1",
        "Tháng 2",
        "Tháng 3",
        "Tháng 4",
        "Tháng 5",
        "Tháng 6",
        "Tháng 7",
        "Tháng 8",
        "Tháng 9",
        "Tháng 10",
        "Tháng 11",
        "Tháng 12",
      ],
      dataRange: {
        start: new Date(),
        end: "",
      },
    };
  },
  validations: {
    model: {
      tieuDe: {required},
    },
  },
  created() {
    this.fnGetList();
    this.getUser();
    this.getByDateNow();
  },
  watch: {
    model: {
      deep: true,
      handler(val) {
        // addCoQuanToModel()
        // this.saveValueToLocalStorage()
        console.log("markedDates", this.markedDates);
      },
    },
    calendarData:{
      deep: true,
      handler(val) {
        console.log("getByDate", this.calendarData);
        this.$store.dispatch("lichCongTacStore/getByDate", this.calendarData).then(resp => {
          if (resp.resultCode == CONSTANTS.SUCCESS) {
            this.model = resp.data;
          } else {
            return [];
          }
        })
      }
    },
    showModal(status) {
      if (status == false) this.model = lichCongTacModel.baseJson();
    },
    showDeleteModal(val) {
      if (val == false) {
        this.model.id = null;
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
            console.log("user", resp.data);
            this.loading = false
            this.optionsUser = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
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
    async handleUpdate(id) {
      console.log("LOG HANDLE UPDATE ", id)
      await this.$store.dispatch("lichCongTacStore/getById", id).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.model = res.data;
          this.showModal = true;
        } else {
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        }
      });
    },
    async handleDetail(id) {
      await this.$store.dispatch("lichCongTacStore/getById", id).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.model = lichCongTacModel.fromJson(res.data);
          this.showDetail = true;
        } else {
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        }
      });
    },
    async handleDelete() {
      if (this.model.id != 0 && this.model.id != null && this.model.id) {
        await this.$store.dispatch("lichCongTacStore/delete", this.model.id).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            // this.fnGetList();
            this.showDeleteModal = false;
          }
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
          // });
        });
      }
    },
    handleShowDeleteModal(id) {
      this.model.id = id;
      this.showDeleteModal = true;
    },
    async handleSubmit(e) {
      e.preventDefault();
      this.submitted = true;
      console.log("handleSubmit");
      this.$v.$touch();
      if (this.$v.$invalid) {
        return;
      } else {
        let loader = this.$loading.show({
          container: this.$refs.formContainer,
        });
        if (
            this.model.id != 0 &&
            this.model.id != null &&
            this.model.id
        ) {
          // Update model
          await this.$store.dispatch("lichCongTacStore/update", this.model).then((res) => {
            if (res.resultCode === 'SUCCESS') {
              // this.fnGetList();
              this.showModal = false;
              this.model = {};
            }
            this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
          });
        } else {
          // Create model
          await this.$store.dispatch("lichCongTacStore/create", lichCongTacModel.toJson(this.model)).then((res) => {
            if (res.resultCode === 'SUCCESS') {
              // this.fnGetList();
              this.showModal = false;
              this.model = {};
            }
            this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
          });
        }
        loader.hide();
      }
      this.submitted = false;
    },
    markDates() {
      let someDate = new Date();
      someDate.setDate(someDate.getDate() - 1);
      let yesterday = this.getFormatedDate(someDate);
      someDate.setDate(someDate.getDate() + 2);
      let tommorow = this.getFormatedDate(someDate);

      this.markedDates = [yesterday, tommorow];

      // this.$set(this.$data, "markedDates", ["7-3-2021"]);
    },
    async getByDateNow(){
      try {
        await this.$store.dispatch("lichCongTacStore/getByDateNow").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            this.model = resp.data;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
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
        let promise = this.$store.dispatch("lichCongTacStore/getPagingParams", params)
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
    }
  }
}
</script>
<template>
  <Layout>
    <PageHeader :title="title" :items="items"/>
    <div class="row">
      <div class="col-xl-4 col-lg-5 col-md-6 col-sm-12">
        <div class="card">
          <div class="card-body">
            <div class="text-lg-start mb-3">
              <b-button
                  variant="primary"
                  type="button"
                  class="btn w-100 btn-primary"
                  @click="showModal = true"
              >
                <i class="mdi mdi-plus me-1"></i> Thêm mới
              </b-button>
              <b-modal
                  v-model="showModal"
                  title="Thông tin lịch công tác"
                  title-class="text-black font-18"
                  body-class="p-3"
                  hide-footer
                  centered
                  no-close-on-backdrop
                  size="lg"
              >
                <form @submit.prevent="handleSubmit"
                      ref="formContainer">
                  <div class="row">
                    <div class="col-12">
                      <div class="mb-3">
                        <label class="text-left">Tiêu đề</label>
                        <span style="color: red">&nbsp;*</span>
                        <input type="hidden" v-model="model.id"/>
                        <input
                            id="ten"
                            v-model.trim="model.tieuDe"
                            type="text"
                            class="form-control"
                            placeholder="Nhập tiêu đề "
                            :class="{
                                'is-invalid':
                                  submitted && $v.model.tieuDe.$error,
                              }"
                        />
                        <div
                            v-if="submitted && !$v.model.tieuDe.required"
                            class="invalid-feedback"
                        >
                          Tiêu đề không được để trống.
                        </div>
                      </div>
                    </div>
                    <div class="col-12">
                      <div class="mb-3">
                        <label class="text-left">Ngày bắt đầu</label>
                        <span style="color: red">&nbsp;*</span>
                        <input type="hidden" v-model="model.id"/>
                        <date-picker v-model="model.tuNgay"
                                     format="DD/MM/YYYY"
                                     value-type="format"
                        >
                          <div slot="input">
                            <input v-model="model.tuNgay"
                                   v-mask="'##/##/####'" type="text" class="form-control"
                                   placeholder="Nhập ngày bắt đầu"/>
                          </div>
                        </date-picker>
                      </div>
                    </div>
                    <div class="col-12">
                      <div class="mb-3">
                        <label class="text-left">Ngày kết thúc</label>
                        <span style="color: red">&nbsp;*</span>
                        <input type="hidden" v-model="model.id"/>
                        <date-picker v-model="model.denNgay"
                                     format="DD/MM/YYYY"
                                     value-type="format"
                        >
                          <div slot="input">
                            <input v-model="model.denNgay"
                                   v-mask="'##/##/####'" type="text" class="form-control"
                                   placeholder="Nhập ngày kết thúc"/>
                          </div>
                        </date-picker>
                      </div>
                    </div>
                    <div class="col-12">
                      <div class="mb-3">
                        <label class="text-left">Thời gian</label>
                        <input
                            id="ten"
                            v-model.trim="model.thoiGian"
                            type="time"
                            class="form-control"
                            placeholder="Nhập thời gian"
                        />
                      </div>
                    </div>
                    <div class="col-12">
                      <div class="mb-3">
                        <label class="text-left">Địa điểm</label>
                        <span style="color: red">&nbsp;*</span>
                        <input
                            id="ten"
                            v-model.trim="model.diaDiem"
                            type="text"
                            class="form-control"
                            placeholder="Nhập địa điểm"
                        />
                      </div>
                    </div>
                    <div class="col-md-12">
                      <div class="mb-3">
                        <label class="text-left">Màu sắc</label>
                        <input
                            type="color"
                            class="form-control-color mw-100 form-control"
                            id="example-color-input"
                            v-model="model.mauSac"
                        />
                      </div>
                    </div>
                    <div class="col-12">
                      <div class="mb-3">
                        <label class="text-left">Chủ trì</label>
                        <multiselect
                            v-model="model.chuTri"
                            :options="optionsUser"
                            track-by="id"
                            label="fullName"
                            placeholder="Chọn người chủ trì"
                            deselect-label="Nhấn để xoá"
                            selectLabel="Nhấn enter để chọn"
                            selectedLabel="Đã chọn"
                        ></multiselect>
                      </div>
                    </div>
                    <div class="col-12">
                      <div class="mb-3">
                        <label class="text-left">Thành phần tham dự</label>
                        <multiselect
                            :multiple="true"
                            v-model="model.thanhPhanThamDu"
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
                    <div class="col-12">
                      <div class="mb-3">
                        <label class="text-left">Ghi chú</label>
                        <input
                            id="ghichu"
                            v-model.trim="model.ghiChu"
                            type="text"
                            class="form-control"
                            placeholder="Nhập ghi chú"
                        />
                      </div>
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
                <form @submit.prevent="handleSubmit"
                      ref="formContainer">
                  <div class="row">
                    <div class="col-12">
                      <div class="mb-3">
                        <label class="text-left">Tên lĩnh vực : </label>
                        <input
                            v-model="model.ten"
                            type="text"
                            class="form-control"
                        />
                      </div>
                    </div>
                    <div class="col-12">
                      <div class="mb-3">
                        <label class="text-left">Thứ tự : </label>
                        <input
                            v-model="model.thuTu"
                            type="number"
                            min="0"
                            oninput="validity.valid||(value='');"
                            class="form-control"
                        />
                      </div>
                    </div>
                    <div class="col-12">
                      <div class="mb-3">
                        <label class="text-left">Người tạo : </label>
                        <input
                            v-model="model.createdBy"
                            type="text"
                            class="form-control"
                        />
                      </div>
                    </div>
                    <div class="col-12">
                      <div class="mb-3">
                        <label class="text-left">Ngày tạo: </label>
                        <input
                            v-model="model.createdAtShow"
                            type="text"
                            class="form-control"
                        />
                      </div>
                    </div>
                    <div class="col-12">
                      <div class="mb-3">
                        <label class="text-left">Người cập nhật : </label>
                        <input
                            v-model="model.modifiedBy"
                            type="text"
                            class="form-control"
                        />
                      </div>
                    </div>
                    <div class="col-12">
                      <div class="mb-3">
                        <label class="text-left">Ngày cập nhật : </label>
                        <input
                            v-model="model.lastModifiedShow"
                            type="text"
                            class="form-control"
                        />
                      </div>
                    </div>
                  </div>
                  <div class="text-end pt-2 mt-3">
                    <b-button variant="light" @click="showDetail = false">
                      Đóng
                    </b-button>
                  </div>
                </form>
              </b-modal>
            </div>
            <functional-calendar
                v-model="calendarData"
                :day-names="dayNames"
                :month-names="monthNames"
                :is-date-range="true"
                :date-format="'dd-mm-yyyy'"
                :change-month-function="true"
                :change-year-function="true"
                :markedDates="markedDates"
                class="calendar multiple"
                ref="Calendar"
            >
            </functional-calendar>
          </div>
        </div>
      </div>
      <div class="col-xl-8 col-lg-7 col-md-6 col-sm-12">
        {{model.lenght}}
        <div v-for="(item, index) in model" :key="index">
          <b-card>
            <b-card-header
                class="fw-bold mb-3 text-white"
                :style="`background: ${item.mauSac}`"
            >
              <i class="fas fa-calendar-alt me-2"></i>
              {{item.tuNgay}} - {{item.denNgay}}
            </b-card-header>
            <div class="row d-flex px-3">
              <div class="col-xxl-2 col-md-4 text-primary">
                Tiêu đề:
              </div>
              <div class="col-xxl-10 col-md-8 d-flex">
                <b-card-text
                    class="bg-info px-3 text-white"
                    style="border-radius: 99px;"
                >
                  {{item.tieuDe}}
                </b-card-text>
              </div>
            </div>
            <hr class="bg-blue-grey m-2">
            <div class="row px-3">
              <div class="col-xxl-2 col-md-4 text-primary">
                Địa điểm:
              </div>
              <div class="col-xxl-10 col-md-8">
                <b-card-text>
                  {{item.diaDiem}}
                </b-card-text>
              </div>
            </div>
            <hr class="bg-blue-grey m-2">
            <div v-if="item.ghiChu" class="row px-3">
              <div class="col-xxl-2 col-md-4 text-primary">
                Ghi chú:
              </div>
              <div class="col-xxl-10 col-md-8">
                <b-card-text>
                  {{item.ghiChu}}
                </b-card-text>
              </div>
            </div>
            <hr class="bg-blue-grey m-2">
            <div v-if="item.ghiChu" class="row px-3">
              <div class="col-xxl-2 col-md-4 text-primary">
                Thành phần tham dự:
              </div>
              <div class="col-xxl-10 col-md-8 d-flex mb-3 justify-content-start flex-row flex-wrap">
                <div
                    v-for="(i, index) in item.thanhPhanThamDu"
                    :key="index"
                    class="me-1 mb-1"
                >
                  <b-card-text
                      class="bg-info px-3 text-white w-100"
                      style="border-radius: 99px;"
                  >
                    {{i.fullName}} - {{i.donVi.ten}}
                  </b-card-text>
                </div>
              </div>
            </div>
            <hr class="bg-blue-grey m-2">
            <div class="row">
              <div class="col-12 d-flex justify-content-end">
                <b-button
                    pill
                    variant="primary"
                    class="me-2"
                    size="sm"
                    @click="handleUpdate(item.id)"
                >
                  <i class="fas fa-pencil-alt"></i>
                  Chỉnh sửa
                </b-button>
                <b-button
                    pill
                    variant="danger"
                    class="me-2"
                    size="sm"
                    @click="HandleDelete(item.id)"
                >
                  <i class="fas fa-trash"></i>
                  Xoá
                </b-button>
              </div>
            </div>
          </b-card>
        </div>

      </div>
    </div>
  </Layout>
</template>
<style lang="scss">
$white: #ffffff !default;
$black: #000000 !default;
$mirage: #1a202c !default;
$ebony: #0a0c19 !default;
$mineShaft: #333333 !default;
$tundora: #464646 !default;
$licorice: #2d3748 !default;
$lightgrey: #d9d9d9 !default;
$lightgreyHover: #dadada !default;
$powderblue: #b0e0e6 !default;
$lightskyblue: #8fd8ec !default;
$royalblue: #66b3cc !default;
$steelblue: #4682b4 !default;
$pictionBlue: #4299e1 !default;
$astronaut: #28456c !default;
$keppel: #38b2ac !default;
$gainsboro: #bfbfbf !default;
$lightred: #ff8498 !default;
$charade: #292d36 !default;
$silver_chalice: #aaaaaa !default;
$silver: #cccccc !default;
$gallery: #efefef !default;
$alto: #dbdbdb !default;
$trout: #495057 !default;
$porcelain: #f0f1f2 !default;
$boulder: #7d7d7d !default;

.green-line {
  width: 20px;
  position: absolute;
  height: 2px;
  background-color: #45cc0d;
  bottom: 1px;
  left: calc(50% - 10px);
}

.green-point {
  position: absolute;
  width: 4px;
  height: 4px;
  border-radius: 50%;
  background-color: #45cc0d;
  bottom: 1px;
  left: calc(50% - 4px);
}

.orange-point {
  position: absolute;
  width: 4px;
  height: 4px;
  border-radius: 50%;
  background-color: #eb9800;
  bottom: 1px;
  left: calc(50% - 4px);
}
</style>
