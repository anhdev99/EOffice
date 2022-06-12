<script>
import Layout from "@/layouts/main";
import PageHeader from "@/components/page-header";
import appConfig from "../../../app.config.json";
import {pagingModel} from "@/models/pagingModel";
import {roleModel} from "@/models/roleModel";

export default {
  page: {
    title: "Quyền",
    meta: [
      {
        content: appConfig.description,
      },
    ],
  },
  data() {
    return {
      title: "Quyền",
      items: [
        {
          text: "Trang chủ",
          href: "/",
        },
        {
          text: "Quyền",
          active: true,
        },
      ],
      showModal: false,
      showDetail: false,
      showDeleteModal: false,
      data: [],
      model: roleModel.baseJson(),
      pagination: pagingModel.baseJson(),
      totalRows: 1,
      currentPage: 1,
      numberOfElement: 1,
      perPage: 10,
      pageOptions: [5,10, 25, 50, 100],
      filter: null,
      fields: [
        { key: 'STT',
          label: 'STT',
          class: 'ps-4',
          thStyle: {width: '80px', minWidth: '80px'},
          thClass: 'hidden-sortable py-2'
        },
        {
          key: "code",
          label: "Code",
          class: 'ps-4',
          sortable: true,
          thClass: 'hidden-sortable py-2'
        },
        {
          key: "ten",
          label: "Tên",
          class: 'ps-4',
          sortable: true,
          thClass: 'hidden-sortable py-2'
        },
        {
          key: "thuTu",
          label: "Thứ tự",
          class: 'ps-4',
          thStyle: {width: '100px', minWidth: '100px'},
          thClass: 'hidden-sortable py-2'
        },
        {
          key: 'process',
          label: 'Xử lý',
          class: 'ps-4',
          thStyle: {width: '110px', minWidth: '110px'},
          thClass: 'hidden-sortable py-2'
        }
      ],
    };
  },
  components: { Layout, PageHeader },
  created() {
    this.myProvider()
  },
  watch:{
    currentPage: {
      deep: true,
      handler(val) {
        console.log(val)
        this.myProvider();
      }
    },
  },
  methods: {
    async handleUpdate(id) {
      await this.$store.dispatch("roleStore/getById", id).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.model = res.data;
          this.showModal = true;
          this.myProvider()
        } else {
          // this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
          this.myProvider()
        }
      });
    },
    async handleDetail(id) {
      await this.$store.dispatch("roleStore/getById", id).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.model = res.data;
          this.showDetail = true;
        }
      });
    },
    async handleDelete() {
      if (this.model.id != 0 && this.model.id != null && this.model.id) {
        await this.$store.dispatch("roleStore/delete", this.model.id).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            this.showDeleteModal = false;
            this.myProvider()
          }
        });
      }
    },
    handleShowDeleteModal(id) {
      this.model.id = id;
      this.showDeleteModal = true;
    },
    async HandleSubmit(e){
      e.preventDefault();
      if(
          this.model.id != 0 &&
          this.model.id != null &&
          this.model.id
      ){
        //Update model
        await this.$store.dispatch("roleStore/update", this.model).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            this.showModal = false;
            this.model = roleModel.baseJson()
            this.myProvider()
          }
        })
      }else{
        //Create model
        await this.$store.dispatch("roleStore/create",this.model).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            this.showModal = false;
            this.model = roleModel.baseJson()
            this.myProvider()
          }
        });
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
        let promise =  this.$store.dispatch("roleStore/getPagingParams", params)
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
    }
  },
};
</script>

<template>
  <Layout>
    <PageHeader :title="title" :items="items" />

    <div class="row page-linhvuc">
      <div class="col-xl-12">
        <div class="card">
          <div class="card-header align-items-center d-flex">
            <h4 class="card-title mb-0 flex-grow-1"></h4>
            <!-- Button -->
            <div class="flex-shrink-0">
              <div
                  class="form-check form-switch form-switch-right form-switch-md"
              >
                <b-button
                    class="btn btn-primary add-btn btn-sm"
                    data-bs-toggle="modal"
                    @click="showModal = true"
                >
                  <i class="ri-add-line align-bottom me-1"></i> Thêm mới
                </b-button>
              </div>
            </div>
          </div>
          <div class="row">
            <div class="col-12">
              <!--  Table -->
              <b-table
                  class="table-light align-middle table-nowrap mb-0"
                  :items="data"
                  small
                  :fields="fields"
                  responsive="sm"
                  :per-page="perPage"
                  :current-page="currentPage"
                  :filter="filter"
                  :head-variant="light"
                  ref="tblList"
                  primary-key="id"
              >
                <template v-slot:cell(STT)="data">
                  {{ data.index + ((currentPage-1)*perPage) + 1  }}
                </template>
                <template v-slot:cell(ten)="data">
                  <div class="ps-2">
                    {{data.item.ten}}
                  </div>
                </template>
                                <template v-slot:cell(code)="data">
                                  <div class="ps-2">
                                    {{data.item.code}}
                                  </div>
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
          </div>
          <div
              class="align-items-center mt-2 row g-3 text-center text-sm-start px-3 mb-3"
          >
            <div class="col-sm">
              <div class="text-muted">
                Hiển thị<span class="fw-semibold">{{data.length}}</span> of
                <span class="fw-semibold">{{totalRows}}</span> kết quả
              </div>
            </div>
            <div class="col-sm-auto">
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

    <!-- Modal add and edit -->
    <b-modal
        ref="modal"
        title="Thông tin vai trò"
        header-class="bg-primary-dark modal-title p-3"
        hide-footer
        v-model="showModal"
    >
      <form class="" @submit="HandleSubmit">
        <div class="mb-3">
          <label for="name" class="form-label">
            Code
            <span class="text-danger">*</span>
          </label>
          <input
              type="text"
              class="form-control"
              v-model="model.code"
          />
        </div>
        <div class="mb-3">
          <label for="name" class="form-label">
           Tên vai trò
            <span class="text-danger">*</span>
          </label>
          <input
              type="text"
              class="form-control"
              v-model="model.ten"
          />
        </div>
        <div class="mb-3">
          <label for="name" class="form-label">
            Số thứ tự
            <span class="text-danger">*</span>
          </label>
          <input
              type="number"
              class="form-control"
              v-model="model.thuTu"
          />
        </div>
        <div class="d-flex justify-content-end">
          <b-button
              type="button"
              class="btn btn-danger waves-effect waves-light me-2 d-flex align-items-center"
              data-bs-dismiss="modal"
              aria-label="Close"
              id="close-modal"
          >
            Hủy
          </b-button>
          <b-button
              type="submit"
              class="btn btn-primary waves-effect waves-light me-2 d-flex align-items-center"
          >
            Lưu
          </b-button>
        </div>
      </form>
    </b-modal>
    <!-- Modal edit -->
    <!-- Modal delete -->
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
</style>
