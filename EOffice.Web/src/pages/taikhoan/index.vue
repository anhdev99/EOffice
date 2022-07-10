<!--<script>-->
<!--import Layout from "@/layouts/main";-->
<!--import PageHeader from "@/components/page-header";-->
<!--import appConfig from "../../../app.config.json";-->
<!--import {pagingModel} from "@/models/pagingModel";-->
<!--import {roleModel} from "@/models/roleModel";-->
<!--import {userModel} from "@/models/userModel";-->

<!--export default {-->
<!--  page: {-->
<!--    title: "Quyền",-->
<!--    meta: [-->
<!--      {-->
<!--        content: appConfig.description,-->
<!--      },-->
<!--    ],-->
<!--  },-->
<!--  data() {-->
<!--    return {-->
<!--      title: "Tài khoản",-->
<!--      items: [-->
<!--        {-->
<!--          text: "Trang chủ",-->
<!--          href: "/",-->
<!--        },-->
<!--        {-->
<!--          text: "Tài khoản",-->
<!--          active: true,-->
<!--        },-->
<!--      ],-->
<!--      showModal: false,-->
<!--      showDetail: false,-->
<!--      showDeleteModal: false,-->
<!--      data: [],-->
<!--      model: userModel.baseJson(),-->
<!--      pagination: pagingModel.baseJson(),-->
<!--      totalRows: 1,-->
<!--      currentPage: 1,-->
<!--      numberOfElement: 1,-->
<!--      perPage: 10,-->
<!--      pageOptions: [5,10, 25, 50, 100],-->
<!--      filter: null,-->
<!--      fields: [-->
<!--        { key: 'STT',-->
<!--          label: 'STT',-->
<!--          class: 'ps-4',-->
<!--          thStyle: {width: '80px', minWidth: '80px'},-->
<!--          thClass: 'hidden-sortable py-2'-->
<!--        },-->
<!--        {-->
<!--          key: "userName",-->
<!--          label: "Tài khoản",-->
<!--          class: 'ps-4',-->
<!--          sortable: true,-->
<!--          thClass: 'hidden-sortable py-2'-->
<!--        },-->
<!--        {-->
<!--          key: "lastName",-->
<!--          label: "Họ",-->
<!--          class: 'ps-4',-->
<!--          sortable: true,-->
<!--          thClass: 'hidden-sortable py-2'-->
<!--        },-->
<!--        {-->
<!--          key: "firstName",-->
<!--          label: "Tên",-->
<!--          class: 'ps-4',-->
<!--          sortable: true,-->
<!--          thClass: 'hidden-sortable py-2'-->
<!--        },-->
<!--        {-->
<!--          key: "firstName",-->
<!--          label: "Tên",-->
<!--          class: 'ps-4',-->
<!--          sortable: true,-->
<!--          thClass: 'hidden-sortable py-2'-->
<!--        },-->
<!--        {-->
<!--          key: "thuTu",-->
<!--          label: "Thứ tự",-->
<!--          class: 'ps-4',-->
<!--          thStyle: {width: '100px', minWidth: '100px'},-->
<!--          thClass: 'hidden-sortable py-2'-->
<!--        },-->
<!--        {-->
<!--          key: 'process',-->
<!--          label: 'Xử lý',-->
<!--          class: 'ps-4',-->
<!--          thStyle: {width: '110px', minWidth: '110px'},-->
<!--          thClass: 'hidden-sortable py-2'-->
<!--        }-->
<!--      ],-->
<!--    };-->
<!--  },-->
<!--  components: { Layout, PageHeader },-->
<!--  created() {-->
<!--    this.myProvider()-->
<!--  },-->
<!--  watch:{-->
<!--    currentPage: {-->
<!--      deep: true,-->
<!--      handler(val) {-->
<!--        console.log(val)-->
<!--        this.myProvider();-->
<!--      }-->
<!--    },-->
<!--  },-->
<!--  methods: {-->
<!--    async handleUpdate(id) {-->
<!--      await this.$store.dispatch("roleStore/getById", id).then((res) => {-->
<!--        if (res.resultCode === 'SUCCESS') {-->
<!--          this.model = res.data;-->
<!--          this.showModal = true;-->
<!--          this.myProvider()-->
<!--        } else {-->
<!--          // this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));-->
<!--          this.myProvider()-->
<!--        }-->
<!--      });-->
<!--    },-->
<!--    async handleDetail(id) {-->
<!--      await this.$store.dispatch("roleStore/getById", id).then((res) => {-->
<!--        if (res.resultCode === 'SUCCESS') {-->
<!--          this.model = res.data;-->
<!--          this.showDetail = true;-->
<!--        }-->
<!--      });-->
<!--    },-->
<!--    async handleDelete() {-->
<!--      if (this.model.id != 0 && this.model.id != null && this.model.id) {-->
<!--        await this.$store.dispatch("roleStore/delete", this.model.id).then((res) => {-->
<!--          if (res.resultCode === 'SUCCESS') {-->
<!--            this.showDeleteModal = false;-->
<!--            this.myProvider()-->
<!--          }-->
<!--        });-->
<!--      }-->
<!--    },-->
<!--    handleShowDeleteModal(id) {-->
<!--      this.model.id = id;-->
<!--      this.showDeleteModal = true;-->
<!--    },-->
<!--    async HandleSubmit(e){-->
<!--      e.preventDefault();-->
<!--      if(-->
<!--          this.model.id != 0 &&-->
<!--          this.model.id != null &&-->
<!--          this.model.id-->
<!--      ){-->
<!--        //Update model-->
<!--        await this.$store.dispatch("roleStore/update", this.model).then((res) => {-->
<!--          if (res.resultCode === 'SUCCESS') {-->
<!--            this.showModal = false;-->
<!--            this.model = roleModel.baseJson()-->
<!--            this.myProvider()-->
<!--          }-->
<!--        })-->
<!--      }else{-->
<!--        //Create model-->
<!--        await this.$store.dispatch("roleStore/create",this.model).then((res) => {-->
<!--          if (res.resultCode === 'SUCCESS') {-->
<!--            this.showModal = false;-->
<!--            this.model = roleModel.baseJson()-->
<!--            this.myProvider()-->
<!--          }-->
<!--        });-->
<!--      }-->
<!--    },-->
<!--    myProvider (ctx) {-->
<!--      const params = {-->
<!--        start: this.currentPage - 1,-->
<!--        limit: this.perPage,-->
<!--        content: this.filter,-->
<!--        sortBy: "",-->
<!--        sortDesc: false,-->
<!--      }-->
<!--      this.loading = true-->

<!--      try {-->
<!--        let promise =  this.$store.dispatch("roleStore/getPagingParams", params)-->
<!--        return promise.then(resp => {-->
<!--          if(resp.resultCode == "SUCCESS"){-->
<!--            let items = resp.data.data-->
<!--            this.totalRows = resp.data.totalRows-->
<!--            this.numberOfElement = resp.data.data.length-->
<!--            this.loading = false-->
<!--            this.data = items;-->
<!--            return items || []-->
<!--          }-->
<!--          return [];-->
<!--        })-->
<!--      } finally {-->
<!--        this.loading = false-->
<!--      }-->
<!--    }-->
<!--  },-->
<!--};-->
<!--</script>-->

<!--<template>-->
<!--  <Layout>-->
<!--    <PageHeader :title="title" :items="items" />-->

<!--    <div class="row page-linhvuc">-->
<!--      <div class="col-xl-12">-->
<!--        <div class="card">-->
<!--          <div class="card-header align-items-center d-flex">-->
<!--            <h4 class="card-title mb-0 flex-grow-1"></h4>-->
<!--            &lt;!&ndash; Button &ndash;&gt;-->
<!--            <div class="flex-shrink-0">-->
<!--              <div-->
<!--                  class="form-check form-switch form-switch-right form-switch-md"-->
<!--              >-->
<!--                <b-button-->
<!--                    class="btn btn-primary add-btn btn-sm"-->
<!--                    data-bs-toggle="modal"-->
<!--                    @click="showModal = true"-->
<!--                >-->
<!--                  <i class="ri-add-line align-bottom me-1"></i> Thêm mới-->
<!--                </b-button>-->
<!--              </div>-->
<!--            </div>-->
<!--          </div>-->
<!--          <div class="row">-->
<!--            <div class="col-12">-->
<!--              &lt;!&ndash;  Table &ndash;&gt;-->
<!--              <b-table-->
<!--                  class="table-light align-middle table-nowrap mb-0"-->
<!--                  :items="data"-->
<!--                  small-->
<!--                  :fields="fields"-->
<!--                  responsive="sm"-->
<!--                  :per-page="perPage"-->
<!--                  :current-page="currentPage"-->
<!--                  :filter="filter"-->
<!--                  :head-variant="light"-->
<!--                  ref="tblList"-->
<!--                  primary-key="id"-->
<!--              >-->
<!--                <template v-slot:cell(STT)="data">-->
<!--                  {{ data.index + ((currentPage-1)*perPage) + 1  }}-->
<!--                </template>-->
<!--                <template v-slot:cell(ten)="data">-->
<!--                  <div class="ps-2">-->
<!--                    {{data.item.ten}}-->
<!--                  </div>-->
<!--                </template>-->
<!--                <template v-slot:cell(code)="data">-->
<!--                  <div class="ps-2">-->
<!--                    {{data.item.code}}-->
<!--                  </div>-->
<!--                </template>-->
<!--                <template v-slot:cell(process)="data">-->
<!--                  <button-->
<!--                      type="button"-->
<!--                      size="sm"-->
<!--                      class="btn btn-outline btn-sm"-->
<!--                      data-toggle="tooltip" data-placement="bottom" title="Chi tiết"-->
<!--                      v-on:click="handleDetail(data.item.id)">-->
<!--                    <i class="ri-eye-fill  text-warning me-1"></i>-->
<!--                  </button>-->
<!--                  <button-->
<!--                      type="button"-->
<!--                      size="sm"-->
<!--                      class="btn btn-outline btn-sm"-->
<!--                      data-toggle="tooltip" data-placement="bottom" title="Cập nhật"-->
<!--                      v-on:click="handleUpdate(data.item.id)">-->
<!--                    <i class="ri-edit-2-fill text-success me-1"></i>-->
<!--                  </button>-->
<!--                  <button-->
<!--                      type="button"-->
<!--                      size="sm"-->
<!--                      class="btn btn-outline btn-sm"-->
<!--                      data-toggle="tooltip" data-placement="bottom" title="Xóa"-->
<!--                      v-on:click="handleShowDeleteModal(data.item.id)">-->
<!--                    <i class=" ri-delete-bin-fill text-danger me-1"></i>-->
<!--                  </button>-->
<!--                </template>-->
<!--              </b-table>-->
<!--            </div>-->
<!--          </div>-->
<!--          <div-->
<!--              class="align-items-center mt-2 row g-3 text-center text-sm-start px-3 mb-3"-->
<!--          >-->
<!--            <div class="col-sm">-->
<!--              <div class="text-muted">-->
<!--                Hiển thị<span class="fw-semibold">{{data.length}}</span> of-->
<!--                <span class="fw-semibold">{{totalRows}}</span> kết quả-->
<!--              </div>-->
<!--            </div>-->
<!--            <div class="col-sm-auto">-->
<!--              <div class="col">-->
<!--                <div-->
<!--                    class="dataTables_paginate paging_simple_numbers float-end">-->
<!--                  <ul class="pagination pagination-rounded mb-0">-->
<!--                    &lt;!&ndash; pagination &ndash;&gt;-->
<!--                    <b-pagination-->
<!--                        v-model="currentPage"-->
<!--                        :total-rows="totalRows"-->
<!--                        :per-page="perPage"-->
<!--                    ></b-pagination>-->
<!--                  </ul>-->
<!--                </div>-->
<!--              </div>-->
<!--            </div>-->
<!--          </div>-->
<!--        </div>-->
<!--      </div>-->
<!--    </div>-->
<!--    <b-modal-->
<!--        v-model="showModal"-->
<!--        title="Thông tin tài khoản"-->
<!--        title-class="text-black font-18"-->
<!--        body-class="p-3"-->
<!--        hide-footer-->
<!--        centered-->
<!--        no-close-on-backdrop-->
<!--        size="lg"-->
<!--    >-->
<!--      <form @submit.prevent="handleSubmit"-->
<!--            ref="formContainer"-->
<!--      >-->
<!--        <div class="row">-->
<!--          <div class="col-6">-->
<!--            <div class="mb-3">-->
<!--              <label class="text-left">Tên tài khoản</label>-->
<!--              <span style="color: red">&nbsp;*</span>-->
<!--              <input type="hidden" v-model="model.id"/>-->
<!--              <input-->
<!--                  id="userName"-->
<!--                  v-model.trim="model.userName"-->
<!--                  type="text"-->
<!--                  class="form-control"-->
<!--                  placeholder="Nhập tài khoản"-->
<!--                  :class="{-->
<!--                                'is-invalid':-->
<!--                                  submitted && $v.model.userName.$error,-->
<!--                              }"-->
<!--              />-->
<!--              <div-->
<!--                  v-if="submitted && !$v.model.userName.required"-->
<!--                  class="invalid-feedback"-->
<!--              >-->
<!--                Tên tài khoản không được trống.-->
<!--              </div>-->
<!--            </div>-->
<!--          </div>-->
<!--          <div class="col-6">-->
<!--            <div class="mb-3">-->
<!--              <label class="text-left" >Mật khẩu</label>-->
<!--              <input type="hidden" v-model="model.id"/>-->
<!--              <input-->
<!--                  id="password"-->
<!--                  v-model="model.password"-->
<!--                  type="password"-->
<!--                  class="form-control"-->
<!--                  placeholder="Nhập mật khẩu"-->
<!--              />-->
<!--            </div>-->
<!--          </div>-->
<!--          <div class="col-6">-->
<!--            <div class="mb-3">-->
<!--              <label class="text-left">Họ</label>-->
<!--              <span style="color: red">&nbsp;*</span>-->
<!--              <input type="hidden" v-model="model.id"/>-->
<!--              <input-->
<!--                  id="lastName"-->
<!--                  v-model="model.lastName"-->
<!--                  type="text"-->
<!--                  class="form-control"-->
<!--                  placeholder="Nhập họ"-->
<!--                  :class="{-->
<!--                                'is-invalid':-->
<!--                                  submitted && $v.model.lastName.$error,-->
<!--                              }"-->
<!--              />-->
<!--              <div-->
<!--                  v-if="submitted && !$v.model.lastName.required"-->
<!--                  class="invalid-feedback"-->
<!--              >-->
<!--                Họ không được trống.-->
<!--              </div>-->
<!--            </div>-->
<!--          </div>-->
<!--          <div class="col-6">-->
<!--            <div class="mb-3">-->
<!--              <label class="text-left">Tên</label>-->
<!--              <span style="color: red">&nbsp;*</span>-->
<!--              <input type="hidden" v-model="model.id"/>-->
<!--              <input-->
<!--                  id="firstName"-->
<!--                  v-model="model.firstName"-->
<!--                  type="text"-->
<!--                  class="form-control"-->
<!--                  placeholder="Nhập tên"-->
<!--                  :class="{-->
<!--                                'is-invalid':-->
<!--                                  submitted && $v.model.firstName.$error,-->
<!--                              }"-->
<!--              />-->
<!--              <div-->
<!--                  v-if="submitted && !$v.model.firstName.required"-->
<!--                  class="invalid-feedback"-->
<!--              >-->
<!--                Tên không được trống.-->
<!--              </div>-->
<!--            </div>-->
<!--          </div>-->
<!--          <div class="col-6">-->
<!--            <div class="mb-3">-->
<!--              <label class="text-left">Số điện thoại</label>-->
<!--              <input type="hidden" v-model="model.id"/>-->
<!--              <input-->
<!--                  id="phoneNumber"-->
<!--                  v-model="model.phoneNumber"-->
<!--                  type="tel"-->
<!--                  maxlength="11"-->
<!--                  minlength="9"-->
<!--                  class="form-control"-->
<!--                  placeholder="Nhập số điện thoại"-->
<!--              />-->
<!--            </div>-->
<!--          </div>-->
<!--          <div class="col-6">-->
<!--            <div class="mb-3">-->
<!--              <label class="text-left">Email</label>-->
<!--              <input type="hidden" v-model="model.id"/>-->
<!--              <input-->
<!--                  id="email"-->
<!--                  v-model="model.email"-->
<!--                  type="email"-->
<!--                  pattern="[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$"-->
<!--                  class="form-control"-->
<!--                  placeholder="Nhập email"-->
<!--              />-->
<!--            </div>-->
<!--          </div>-->
<!--          <div class="col-6">-->
<!--            <div class="mb-3">-->
<!--              <label class="text-left">Cơ quan</label>-->
<!--              <span style="color: red">&nbsp;*</span>-->
<!--              <treeselect-->
<!--                  v-on:select="addCoQuanToModel"-->
<!--                  :options="listCoQuan"-->
<!--                  :value="model.coQuan.id"-->
<!--                  :searchable="true"-->
<!--                  :show-count="true"-->
<!--                  :default-expand-level="1"-->
<!--                  :normalizer="normalizer"-->
<!--                  :class="{-->
<!--                                'is-invalid':-->
<!--                                  submitted && $v.model.coQuan.$error,-->
<!--                              }"-->
<!--                  placeholder="Chọn cơ quan"-->
<!--              >-->
<!--                <label slot="option-label" slot-scope="{ node, shouldShowCount, count, labelClassName, countClassName }" :class="labelClassName">-->
<!--                  {{ node.label }}-->
<!--                  <span v-if="shouldShowCount" :class="countClassName">({{ count }})</span>-->
<!--                </label>-->
<!--              </treeselect>-->
<!--              <div-->
<!--                  v-if="submitted && !$v.model.coQuan.required"-->
<!--                  class="invalid-feedback"-->
<!--              >-->
<!--                Cơ quan không được trống.-->
<!--              </div>-->
<!--            </div>-->
<!--          </div>-->
<!--          <div class="col-6">-->
<!--            <div class="mb-3">-->
<!--              <label class="text-left">Vai trò</label>-->
<!--              <span style="color: red">&nbsp;*</span>-->
<!--              <multiselect v-model="model.roles"-->
<!--                           :options="listRole"-->
<!--                           label="ten"-->
<!--                           :multiple="true"-->
<!--                           track-by="id"-->
<!--                           :class="{-->
<!--                                'is-invalid':-->
<!--                                  submitted && $v.model.roles.$error,-->
<!--                              }"-->
<!--                           placeholder="Chọn vai trò"-->
<!--              ></multiselect>-->
<!--              <div-->
<!--                  v-if="submitted && !$v.model.roles.required"-->
<!--                  class="invalid-feedback"-->
<!--              >-->
<!--                Vai trò không được trống.-->
<!--              </div>-->
<!--            </div>-->
<!--          </div>-->

<!--        </div>-->
<!--        <div class="text-end pt-2 mt-3">-->
<!--          <b-button variant="light" @click="showModal = false">-->
<!--            Đóng-->
<!--          </b-button>-->
<!--          <b-button  type="submit" variant="success" class="ms-1">Lưu-->
<!--          </b-button>-->
<!--        </div>-->
<!--      </form>-->
<!--    </b-modal>-->
<!--    &lt;!&ndash; Modal add and edit &ndash;&gt;-->
<!--&lt;!&ndash;    <b-modal&ndash;&gt;-->
<!--&lt;!&ndash;        ref="modal"&ndash;&gt;-->
<!--&lt;!&ndash;        title="Thông tin vai trò"&ndash;&gt;-->
<!--&lt;!&ndash;        header-class="bg-primary-dark modal-title p-3"&ndash;&gt;-->
<!--&lt;!&ndash;        hide-footer&ndash;&gt;-->
<!--&lt;!&ndash;        v-model="showModal"&ndash;&gt;-->
<!--&lt;!&ndash;    >&ndash;&gt;-->
<!--&lt;!&ndash;      <form class="" @submit="HandleSubmit">&ndash;&gt;-->
<!--&lt;!&ndash;        <div class="mb-3">&ndash;&gt;-->
<!--&lt;!&ndash;          <label for="name" class="form-label">&ndash;&gt;-->
<!--&lt;!&ndash;            Code&ndash;&gt;-->
<!--&lt;!&ndash;            <span class="text-danger">*</span>&ndash;&gt;-->
<!--&lt;!&ndash;          </label>&ndash;&gt;-->
<!--&lt;!&ndash;          <input&ndash;&gt;-->
<!--&lt;!&ndash;              type="text"&ndash;&gt;-->
<!--&lt;!&ndash;              class="form-control"&ndash;&gt;-->
<!--&lt;!&ndash;              v-model="model.code"&ndash;&gt;-->
<!--&lt;!&ndash;          />&ndash;&gt;-->
<!--&lt;!&ndash;        </div>&ndash;&gt;-->
<!--&lt;!&ndash;        <div class="mb-3">&ndash;&gt;-->
<!--&lt;!&ndash;          <label for="name" class="form-label">&ndash;&gt;-->
<!--&lt;!&ndash;            Tên vai trò&ndash;&gt;-->
<!--&lt;!&ndash;            <span class="text-danger">*</span>&ndash;&gt;-->
<!--&lt;!&ndash;          </label>&ndash;&gt;-->
<!--&lt;!&ndash;          <input&ndash;&gt;-->
<!--&lt;!&ndash;              type="text"&ndash;&gt;-->
<!--&lt;!&ndash;              class="form-control"&ndash;&gt;-->
<!--&lt;!&ndash;              v-model="model.ten"&ndash;&gt;-->
<!--&lt;!&ndash;          />&ndash;&gt;-->
<!--&lt;!&ndash;        </div>&ndash;&gt;-->
<!--&lt;!&ndash;        <div class="mb-3">&ndash;&gt;-->
<!--&lt;!&ndash;          <label for="name" class="form-label">&ndash;&gt;-->
<!--&lt;!&ndash;            Số thứ tự&ndash;&gt;-->
<!--&lt;!&ndash;            <span class="text-danger">*</span>&ndash;&gt;-->
<!--&lt;!&ndash;          </label>&ndash;&gt;-->
<!--&lt;!&ndash;          <input&ndash;&gt;-->
<!--&lt;!&ndash;              type="number"&ndash;&gt;-->
<!--&lt;!&ndash;              class="form-control"&ndash;&gt;-->
<!--&lt;!&ndash;              v-model="model.thuTu"&ndash;&gt;-->
<!--&lt;!&ndash;          />&ndash;&gt;-->
<!--&lt;!&ndash;        </div>&ndash;&gt;-->
<!--&lt;!&ndash;        <div class="d-flex justify-content-end">&ndash;&gt;-->
<!--&lt;!&ndash;          <b-button&ndash;&gt;-->
<!--&lt;!&ndash;              type="button"&ndash;&gt;-->
<!--&lt;!&ndash;              class="btn btn-danger waves-effect waves-light me-2 d-flex align-items-center"&ndash;&gt;-->
<!--&lt;!&ndash;              data-bs-dismiss="modal"&ndash;&gt;-->
<!--&lt;!&ndash;              aria-label="Close"&ndash;&gt;-->
<!--&lt;!&ndash;              id="close-modal"&ndash;&gt;-->
<!--&lt;!&ndash;          >&ndash;&gt;-->
<!--&lt;!&ndash;            Hủy&ndash;&gt;-->
<!--&lt;!&ndash;          </b-button>&ndash;&gt;-->
<!--&lt;!&ndash;          <b-button&ndash;&gt;-->
<!--&lt;!&ndash;              type="submit"&ndash;&gt;-->
<!--&lt;!&ndash;              class="btn btn-primary waves-effect waves-light me-2 d-flex align-items-center"&ndash;&gt;-->
<!--&lt;!&ndash;          >&ndash;&gt;-->
<!--&lt;!&ndash;            Lưu&ndash;&gt;-->
<!--&lt;!&ndash;          </b-button>&ndash;&gt;-->
<!--&lt;!&ndash;        </div>&ndash;&gt;-->
<!--&lt;!&ndash;      </form>&ndash;&gt;-->
<!--&lt;!&ndash;    </b-modal>&ndash;&gt;-->
<!--    &lt;!&ndash; Modal edit &ndash;&gt;-->
<!--    &lt;!&ndash; Modal delete &ndash;&gt;-->
<!--    <b-modal-->
<!--        ref="modal"-->
<!--        content-class="p-5"-->
<!--        hide-header-->
<!--        hide-footer-->
<!--        v-model="showDeleteModal"-->
<!--    >-->
<!--      <div class="text-center">-->
<!--        <i class="ri-error-warning-line text-warning" style="font-size: 100px;"></i>-->
<!--        <p class="fs-4">Bạn có chắc muốn xóa không?</p>-->
<!--      </div>-->
<!--      <div class="d-flex justify-content-center">-->
<!--        <b-button-->
<!--            type="button"-->
<!--            class="btn btn-danger waves-effect waves-light me-2 d-flex align-items-center"-->
<!--            data-bs-dismiss="modal"-->
<!--            aria-label="Close"-->
<!--            id="close-modal"-->
<!--            @click="showDeleteModal = false"-->
<!--        >-->
<!--          Hủy-->
<!--        </b-button>-->
<!--        <b-button-->
<!--            type="submit"-->
<!--            class="btn btn-primary waves-effect waves-light me-2 d-flex align-items-center"-->
<!--            @click="handleDelete"-->
<!--        >-->
<!--          Xóa-->
<!--        </b-button>-->
<!--      </div>-->
<!--    </b-modal>-->
<!--  </Layout>-->
<!--</template>-->
<!--<style>-->
<!--.modal-title {-->
<!--  color: #fff;-->
<!--}-->
<!--.bg-primary-dark {-->
<!--  background: linear-gradient(135deg, #06548e, #ffffff);-->
<!--  box-shadow: 0px 3px 0px #06548e;-->
<!--}-->
<!--</style>-->


<script>
import Layout from "@/layouts/main";
import PageHeader from "@/components/page-header";
import appConfig from "../../../app.config.json";
import {pagingModel} from "@/models/pagingModel";
import {roleModel} from "@/models/roleModel";
import {userModel} from "@/models/userModel";
import {numeric, required} from "vuelidate/lib/validators";
// import Multiselect from "vue-multiselect";
import Multiselect from '@vueform/multiselect'
import Treeselect from "vue3-treeselect";
import "vue3-treeselect/dist/vue3-treeselect.css";
import "@vueform/multiselect/themes/default.css";
import {notifyModel} from "@/models/notifyModel";

export default {
  page: {
    title: "Tài khoản",
    meta: [{name: "description", content: appConfig.description}],
  },
  components: {Layout, PageHeader, Treeselect, Multiselect},
  data() {
    return {
      title: "Tài khoản",
      items: [
        {
          text: "Tài khoản",
          href: '/tai-khoan'
        },
        {
          text: "Danh sách",
          active: true,
        }
      ],
      data: [],
      fields: [
        { key: 'STT', label: 'STT', class: 'td-stttt', sortable: false,thClass: 'hidden-sortable', },
        {
          key: "userName",
          label: "Tài khoản",
          class: 'td-taikhoannn',
          sortable: true,
          thStyle:"text-align:center"
        },
        {
          key: "fullName",
          label: "Họ và tên",
          class: 'td-tenss',
          sortable: true,
        },
        {
          key: "email",
          label: "Email",
          class: 'td-mail',
          sortable: true,
        },
        {
          key: "phoneNumber",
          label: "Số điện thoại",
          class: 'td-sodienthoai',
          sortable: true
        },
        {
          key: "donVi",
          label: "Đơn vị",
          class: 'td-donvi',
          sortable: true,
        },
        {
          key: 'process',
          label: 'Xử lý',
          class: 'td-xuly',
          thClass: 'hidden-sortable',
          sortable: false
        }
      ],
      currentPage: 1,
      perPage: 10,
      pageOptions: [5, 10, 25, 50, 100],
      showModal: false,
      showDeleteModal: false,
      submitted: false,
      sortBy: "age",
      filter: null,
      sortDesc: false,
      filterOn: [],
      todoFilter: null,
      itemFilter: {
        Coquan : null,
      },
      todofilterOn: [],
      numberOfElement: 1,
      totalRows: 1,
      model: userModel.baseJson(),
      listCoQuan: [],
      listRole: [],
      listChucVu: [],
      pagination: pagingModel.baseJson()
    };
  },
  validations: {
    model: {
      userName:{required},
      firstName: {required},
      lastName: {required},
      coQuan: {
        id:{required}
      },
      roles: {required},
    },
  },
  created() {
    this.fnGetList();
    this.getListRole();
    this.getListChucVu();
    this.getListCoQuan();
    this.myProvider()
  },
  methods: {
    normalizer(node){
      if(node.children == null || node.children == 'null'){
        delete node.children;
      }
    },
    async fnGetList() {
      this.$refs.tblList?.refresh()
    },
    async getListRole(){
      await  this.$store.dispatch("roleStore/getAll").then((res) =>{
        console.log("role", res.data)
        this.listRole = res.data || [];
      })
    },
    async getListChucVu(){
      await  this.$store.dispatch("chucVuStore/getAll").then((res) =>{
        console.log("role", res.data)
        this.listChucVu = res.data || [];
      })
    },
    async getListCoQuan(){
      await  this.$store.dispatch("donViStore/getTree").then((res) =>{
        console.log("getTree",res.data);
        this.listCoQuan = res.data || [];
      })
    },
    async onPageChange(page = 1) {
      this.pagination.currentPage = page;
      const params = {
        pageNumber: this.pagination.currentPage,
        pageSize: this.pagination.pageSize,
      }
      this.$refs.tblList?.refresh()
    },
    // async GetPagingParam(params) {
    //   await this.$store.dispatch("userStore/getPagingParams", params).then((res) => {
    //     this.pagination = pagingModel.fromJson(res.data);
    //     this.data = res.data.data || [];
    //   })
    // },
    async handleDelete() {
      if (this.model.id != 0 && this.model.id != null && this.model.id) {
        await this.$store.dispatch("userStore/delete", this.model.id).then((res) => {
          if (res.resultCode==='SUCCESS') {
            this.myProvider()
            this.showDeleteModal = false;
          }
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        });
      }
    },
    handleShowDeleteModal(id) {
      this.model.id = id;
      this.showDeleteModal = true;
    },
    async handleSubmit(e) {
      e.preventDefault();
      if (
          this.model.id != 0 &&
          this.model.id != null &&
          this.model.id
      ) {
        // Update model
        await this.$store.dispatch("userStore/update", this.model).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            this.showModal = false;
            this.myProvider()
          }
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
        });
      } else {
        // Create model
        await this.$store.dispatch("userStore/create", this.model).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            this.myProvider()
            this.showModal = false;
            this.model={}
          }
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
        });
      }
    },
    async handleUpdate(id) {
      console.log("LOG HANDLE UPDATE ", id)
      await this.$store.dispatch("userStore/getById", id).then((res) => {
        console.log("LOG HAND UPDATE : " , id)
        if (res.resultCode == "SUCCESS") {
          this.model = userModel.toJson(res.data);
          this.showModal = true;
        } else {
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        }
        this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
      });
    },
    addCoQuanToModel(node, instanceId ){
      if(node.id){
        this.model.donVi = {id: node.id, ten: node.label};
      }
    },
    myProvider (ctx) {
      const params = {
        start: this.currentPage - 1,
        limit: this.perPage,
        content: this.filter,
        sortBy: "",
        sortDesc: false,
      }
      this.loading = true
      try {
        let promise =  this.$store.dispatch("userStore/getPagingParams", params)
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
  },
  computed: {

  },
  watch: {
    currentPage: {
      deep: true,
      handler(val) {
        console.log(val)
        this.myProvider();
      }
    },
    model: {
      deep: true,
      handler(val) {
        // addCoQuanToModel()
        console.log("model", val);
        // this.saveValueToLocalStorage()
      }
    },
    itemFilter: {
      deep: true,
      handler() {
        this.$refs.tblList.refresh()
      }
    },
    showModal(status) {
      if (status == false) this.model = userModel.baseJson();
    },
    // model() {
    //   return this.model;
    // },
    showDeleteModal(val) {
      if (val == false) {
        this.model.id = null;
      }
    }
  },
};
</script>

<template>
  <Layout>
    <PageHeader :title="title" :items="items"/>
    <div class="row">
      <div class="col-12">
        <div class="card">
          <div class="card-body">
            <div class="row mb-0">
              <div class="mb-0 col-sm-3">
                <label>Tìm kiếm</label>
                <input
                    size="sm"
                    type="text"
                    name="untyped-input"
                    class="form-control"
                    v-model="filter"
                    placeholder="Nhập thông tin...."
                />
              </div>
              <div class="mb-0 col-sm-3">
                <label>Cơ quan</label>
                <treeselect
                    :options="listCoQuan"
                    :searchable="true"
                    v-model="itemFilter.Coquan"
                    placeholder="Chọn cơ quan"
                    :normalizer="normalizer"
                >
                  <label slot="option-label"
                         slot-scope="{ node, shouldShowCount, count, labelClassName, countClassName }"
                         :class="labelClassName">
                    {{ node.label }}
                    <span v-if="shouldShowCount" :class="countClassName">({{ count }})</span>
                  </label>
                </treeselect>
              </div>
              <div class="col-sm-6 text-end p-4">
                <div class="search-box me-2 mb-2 d-inline-block">
                  <div class="position-relative">
                    <b-button
                        type="button"
                        class="btn btn-success btn-rounded w-md"
                        size="sm"
                        @click="showModal = true"
                    >
                      <i class="mdi mdi-plus me-1"></i> Tạo tài khoản
                    </b-button>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="row">
      <div class="col-12">
        <div class="card">
          <div class="card-body">
            <div class="col-sm-12">
              <div class="text-sm-end">
<!--                                  <b-button-->
<!--                                      type="button"-->
<!--                                      class="btn btn-success btn-rounded w-md"-->
<!--                                      size="sm"-->
<!--                                      @click="showModal = true"-->
<!--                                  >-->
<!--                                    <i class="mdi mdi-plus me-1"></i> Tạo tài khoản-->
<!--                                  </b-button>-->
                <b-modal
                    v-model="showModal"
                    title="Thông tin tài khoản"
                    title-class="text-black font-18"
                    body-class="p-3"
                    hide-footer
                    centered
                    no-close-on-backdrop
                    size="lg"
                >
                  <form @submit.prevent="handleSubmit"
                        ref="formContainer"
                  >
                    <div class="row">
                      <div class="col-6">
                        <div class="mb-3">
                          <label class="text-left">Tên tài khoản</label>
                          <span style="color: red">&nbsp;*</span>
                          <input type="hidden" v-model="model.id"/>
                          <input
                              id="userName"
                              v-model.trim="model.userName"
                              type="text"
                              class="form-control"
                              placeholder="Nhập tài khoản"
                              :class="{
                                'is-invalid':
                                  submitted && $v.model.userName.$error,
                              }"
                          />
                          <div
                              v-if="submitted && !$v.model.userName.required"
                              class="invalid-feedback"
                          >
                            Tên tài khoản không được trống.
                          </div>
                        </div>
                      </div>
                      <div class="col-6">
                        <div class="mb-3">
                          <label class="text-left" >Mật khẩu</label>
                          <input type="hidden" v-model="model.id"/>
                          <input
                              id="password"
                              v-model="model.password"
                              type="password"
                              class="form-control"
                              placeholder="Nhập mật khẩu"
                          />
                        </div>
                      </div>
                      <div class="col-6">
                        <div class="mb-3">
                          <label class="text-left">Họ</label>
                          <span style="color: red">&nbsp;*</span>
                          <input type="hidden" v-model="model.id"/>
                          <input
                              id="lastName"
                              v-model="model.lastName"
                              type="text"
                              class="form-control"
                              placeholder="Nhập họ"
                              :class="{
                                'is-invalid':
                                  submitted && $v.model.lastName.$error,
                              }"
                          />
                          <div
                              v-if="submitted && !$v.model.lastName.required"
                              class="invalid-feedback"
                          >
                            Họ không được trống.
                          </div>
                        </div>
                      </div>
                      <div class="col-6">
                        <div class="mb-3">
                          <label class="text-left">Tên</label>
                          <span style="color: red">&nbsp;*</span>
                          <input type="hidden" v-model="model.id"/>
                          <input
                              id="firstName"
                              v-model="model.firstName"
                              type="text"
                              class="form-control"
                              placeholder="Nhập tên"
                              :class="{
                                'is-invalid':
                                  submitted && $v.model.firstName.$error,
                              }"
                          />
                          <div
                              v-if="submitted && !$v.model.firstName.required"
                              class="invalid-feedback"
                          >
                            Tên không được trống.
                          </div>
                        </div>
                      </div>
                      <div class="col-6">
                        <div class="mb-3">
                          <label class="text-left">Số điện thoại</label>
                          <input type="hidden" v-model="model.id"/>
                          <input
                              id="phoneNumber"
                              v-model="model.phoneNumber"
                              type="tel"
                              maxlength="11"
                              minlength="9"
                              class="form-control"
                              placeholder="Nhập số điện thoại"
                          />
                        </div>
                      </div>
                      <div class="col-6">
                        <div class="mb-3">
                          <label class="text-left">Email</label>
                          <input type="hidden" v-model="model.id"/>
                          <input
                              id="email"
                              v-model="model.email"
                              type="email"
                              pattern="[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$"
                              class="form-control"
                              placeholder="Nhập email"
                          />
                        </div>
                      </div>
                      <div class="col-6">
                        <div class="mb-3">
                          <label class="text-left">Cơ quan</label>
                          <span style="color: red">&nbsp;*</span>
                          <treeselect
                              v-on:select="addCoQuanToModel"
                              :options="listCoQuan"
                              :value="model.donVi.id"
                              :searchable="true"
                              :show-count="true"
                              :default-expand-level="1"
                              :normalizer="normalizer"
                              :class="{
                                'is-invalid':
                                  submitted && $v.model.coQuan.$error,
                              }"
                              placeholder="Chọn cơ quan"
                          >
                            <label slot="option-label" slot-scope="{ node, shouldShowCount, count, labelClassName, countClassName }" :class="labelClassName">
                              {{ node.label }}
                              <span v-if="shouldShowCount" :class="countClassName">({{ count }})</span>
                            </label>
                          </treeselect>
                          <div
                              v-if="submitted && !$v.model.coQuan.required"
                              class="invalid-feedback"
                          >
                            Cơ quan không được trống.
                          </div>
                        </div>
                      </div>
                      <div class="col-6">
                        <div class="mb-3">
                          <label class="text-left">Vai trò</label>
                          <span style="color: red">&nbsp;*</span>
                          <Multiselect
                              v-model="model.roles"
                              mode="tags"
                              :close-on-select="false"
                              :searchable="true"
                              :create-option="true"
                              :options="listRole"
                          >

                          </Multiselect>
                        </div>
                      </div>
                      <div class="col-6">
                        <div class="mb-3">
                          <label class="text-left">Chức vụ</label>
                          <span style="color: red">&nbsp;*</span>
                          <Multiselect
                              v-model="model.chucVu"
                              :close-on-select="false"
                              :searchable="true"
                              :create-option="true"
                              :options="listChucVu"
                          >

                          </Multiselect>
                        </div>
                      </div>
                    </div>
                    <div class="text-end pt-2 mt-3">
                      <b-button variant="light" @click="showModal = false">
                        Đóng
                      </b-button>
                      <b-button  type="submit" variant="success" class="ms-1">Lưu
                      </b-button>
                    </div>
                  </form>
                </b-modal>
              </div>
            </div>

            <div class="row">
              <div class="col-12">
                <div class="row mt-4">
                  <div class="col-sm-12 col-md-6">
                    <div id="tickets-table_length" class="dataTables_length">
                      <label class="d-inline-flex align-items-center">
                        Show&nbsp;
                        <b-form-select
                            class="form-select form-select-sm"
                            v-model="perPage"
                            size="sm"
                            :options="pageOptions"
                        ></b-form-select
                        >&nbsp;entries
                      </label>
                    </div>
                  </div>
                </div>
                <div class="table-responsive mb-0">
                  <b-table
                      class="datatables"
                      :items="data"
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
                  >
                    <template v-slot:cell(STT)="data">
                      {{ data.index + ((currentPage-1)*perPage) + 1  }}
                    </template>
                    <template v-slot:cell(donVi)="data">
                      <span v-if="data.item.donVi!=null">
                        {{data.item.donVi.ten}}
                      </span>
                    </template>
                    <template v-slot:cell(process)="data">
                      <button
                          type="button"
                          size="sm"
                          class="btn btn-outline btn-sm"
                          data-toggle="tooltip" data-placement="bottom" title="Chi tiết"
                          v-on:click="handleDetail(data.item.id)">
                        <i class="ri-eye-fill  text-warning me-1"></i>
                      </button>
                      <button
                          type="button"
                          size="sm"
                          class="btn btn-outline btn-sm"
                          data-toggle="tooltip" data-placement="bottom" title="Cập nhật"
                          v-on:click="handleUpdate(data.item.id)">
                        <i class="ri-edit-2-fill text-success me-1"></i>
                      </button>
                      <button
                          type="button"
                          size="sm"
                          class="btn btn-outline btn-sm"
                          data-toggle="tooltip" data-placement="bottom" title="Xóa"
                          v-on:click="handleShowDeleteModal(data.item.id)">
                        <i class=" ri-delete-bin-fill text-danger me-1"></i>
                      </button>
                    </template>
                  </b-table>
                </div>
                <div class="row">
                  <b-col>
                    <div>Hiển thị {{numberOfElement}} trên tổng số {{totalRows}} dòng</div>
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
          </div>
        </div>
      </div>
    </div>
  </Layout>
</template>
<style>
.td-stttt {
  text-align: center;
  width: 80px;
}
.td-tenss {
  text-align: center;
  width: 250px;
}
.td-taikhoannn {
  text-align: center;
  width: 250px;
}
.td-xulyys {
  text-align: center;
  width: 80px;
}
.td-sodienthoaiii {
  text-align: center;
  width: 140px;
}
.table>tbody> tr >td{
  padding: 0px;
  line-height: 30px;
}
.td-donviiii{
  width: 350px;
}
.td-mail{
  text-align: center;
  width: 80px;
}
.hidden-sortable:after, .hidden-sortable:before {
  display: none !important;
}
</style>

