<script>
import Layout from "@/layouts/main";
import PageHeader from "@/components/page-header";

import Treeselect from "vue3-treeselect";
import "vue3-treeselect/dist/vue3-treeselect.css";

import appConfig from "../../../../app.config.json";
import {donViModel} from "@/models/donViModel";
import {pagingModel} from "@/models/pagingModel";

export default {
  page: {
    title: "Đơn vị",
    meta: [
      {
        content: appConfig.description,
      },
    ],
  },
  data() {
    return {
      title: "Đơn vị",
      items: [
        {
          text: "Trang chủ",
          href: "/",
        },
        {
          text: "đơn vị",
          active: true,
        },
      ],
      showModal: false,
      showDetail: false,
      showDeleteModal: false,
      data: [],
      model: donViModel.baseJson(),
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
          key: "maDonVi",
          label: "Mã đơn vị",
          class: 'ps-4',
          sortable: true,
          thClass: 'hidden-sortable py-2'
        },
        {
          key: "ten",
          label: "Tên đơn vị",
          class: 'ps-4',
          sortable: true,
          thClass: 'hidden-sortable py-2'
        },
        {
          key: "tenDonViCha",
          label: "Đơn vị cha",
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
      donViCha: [
        {
          id: '',
          label: '',
          children: [{
            id: '',
            label: '',
          }],
        }
      ]
    };
  },
  components: { Layout, PageHeader, Treeselect },
  created() {
    this.myProvider()
    this.getDonViCha()
  },
  watch:{
    currentPage: {
      deep: true,
      handler(val) {
        this.myProvider();
      }
    },
  },
  methods: {
    handleShowModal(){
      this.showModal = true;
      this.model = donViModel.baseJson()
    },
    async handleUpdate(id) {
      console.log("handleUpdate");
      await this.$store.dispatch("donViStore/getById", id).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.model = donViModel.fromJson(res.data);
          this.showModal = true;
          this.myProvider()
        } else {
          // this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
          this.myProvider()
        }
      });
    },
    async handleDetail(id) {
      await this.$store.dispatch("donViStore/getById", id).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.model = donViModel.fromJson(res.data);
          console.log("LOG DETAIL  : " , this.model)
          this.showDetail = true;
        }
      });
    },
    async handleDelete() {
      if (this.model.id != 0 && this.model.id != null && this.model.id) {
        await this.$store.dispatch("donViStore/delete", this.model.id).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            this.showDeleteModal = false;
            this.model = donViModel.baseJson()
            this.myProvider();
          }
          // });
        });
      }
    },
    handleShowDeleteModal(id) {
      this.model.id = id;
      this.showDeleteModal = true;
    },
    async HandleSubmit(e){
      e.preventDefault();
      console.log("handle submit", this.model);
      if(
          this.model.id != 0 &&
          this.model.id != null &&
          this.model.id
      ){
        //Update model
        await this.$store.dispatch("donViStore/update", this.model).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            this.showModal = false;
            this.model = donViModel.baseJson()
            this.myProvider()
          }
        })
      }else{
        //Create model
        await this.$store.dispatch("donViStore/create",this.model).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            this.showModal = false;
            this.model = donViModel.baseJson()
            this.myProvider()
          }
        });
      }
    },
    myProvider (ctx) {
      const params = {
        start: this.currentPage,
        limit: this.perPage,
        content: this.filter,
        sortBy: "",
        sortDesc: false,
      }
      this.loading = true

      try {
        let promise =  this.$store.dispatch("donViStore/getPagingParams", params)
        return promise.then(resp => {
          if(resp.resultCode == "SUCCESS"){
            console.log(resp.data)
            let items = resp.data.data
            this.totalRows = resp.data.totalRows
            this.numberOfElement = resp.data.data.length
            this.loading = false
            this.data = items;
            return items || []
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },
    getDonViCha() {
      try {
        let promise =  this.$store.dispatch("donViStore/getDonViCha")
        return promise.then(resp => {
          if(resp.resultCode == "SUCCESS"){
            let items = resp.data
            this.loading = false
            console.log("Đơn vị cha",items);
            this.donViCha = items;
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
    <PageHeader :title="title" :items="items" />

    <div class="row page-donvi">
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
                    @click="handleShowModal"
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
                <template v-slot:cell(maDonVi)="data">
                  <div class="ps-2">
                    {{data.item.maDonVi}}
                  </div>
                </template>
                <template v-slot:cell(ten)="data">
                  <div class="ps-2">
                    {{data.item.ten}}
                  </div>
                </template>
                <template v-slot:cell(tenDonViCha)="data">
                  <div class="ps-2">
                    {{data.item.tenDonViCha}}
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
                        :model-value="currentPage"
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
        title="Thông tin đơn vị"
        header-class="bg-primary-dark modal-title p-3"
        hide-footer
        v-model="showModal"
    >
      <form class="" @submit="HandleSubmit">
        <div class="mb-3">
          <label for="name" class="form-label">
            Mã đơn vị
            <span class="text-danger">*</span>
          </label>
          <input
              type="text"
              class="form-control"
              id="name"
              name="name"
              v-model="model.maDonVi"
          />
        </div>
        <div class="mb-3">
          <label for="name" class="form-label">
            Tên đơn vị
            <span class="text-danger">*</span>
          </label>
          <input
              type="search"
              class="form-control"
              id="so-thu-tu"
              name="so-thu-tu"
              v-model="model.ten"
          />
        </div>
        <div class="mb-3 h-100">
          <label for="donvicha" class="form-label">Đơn vị cha</label>
          <div class="">
            <treeselect
                placeholder="Chọn đơn vị cha"
                v-model="model.donViCha"
                :options="donViCha"
                class="w-100"
            >
            </treeselect>
            <treeselect-value :value="model.donViCha" />
            <div class="invalid-feedback">Chọn đơn vị cha.</div>
          </div>
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
