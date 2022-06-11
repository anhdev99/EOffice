<script>
import Layout from "@/layouts/main";
import PageHeader from "@/components/page-header";
import appConfig from "../../../../app.config.json";
import {linhVucModel} from "@/models/linhVucModel";
import {pagingModel} from "@/models/pagingModel";

export default {
  page: {
    title: "Lĩnh vực",
    meta: [
      {
        content: appConfig.description,
      },
    ],
  },
  data() {
    return {
      title: "Lĩnh vực",
      items: [
        {
          text: "Trang chủ",
          href: "/",
        },
        {
          text: "Lĩnh vực",
          active: true,
        },
      ],
      showModal: false,
      showDetail: false,
      showDeleteModal: false,
      data: [],
      model: linhVucModel.baseJson(),
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
          class: 'text-center',
          thStyle: {width: '80px', minWidth: '80px'},
          thClass: 'hidden-sortable'
        },
        {
          key: "ten",
          label: "Tên",
          class: 'text-center',
          sortable: true,
        },
        {
          key: "donVis",
          label: "Số lượng đơn vị",
          class: 'text-center',
          thStyle: {width: '160px', minWidth: '160px'},
          thClass: 'hidden-sortable'
        },
        {
          key: "thuTu",
          label: "Thứ tự",
          class: 'text-center',
          thStyle: {width: '100px', minWidth: '100px'},
          thClass: 'hidden-sortable'
        },
        {
          key: 'process',
          label: 'Xử lý',
          class: 'text-center',
          thStyle: {width: '110px', minWidth: '110px'},
          thClass: 'hidden-sortable'
        }
      ],
    };
  },
  components: { Layout, PageHeader },
  created() {
    this.myProvider()
  },
  methods: {
    async handleUpdate(id) {
      await this.$store.dispatch("LinhVucStore/getById", id).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.model = linhVucModel.fromJson(res.data);
          this.showModal = true;
          this.myProvider()
        } else {
          // this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
          this.myProvider()
        }
      });
    },
    async handleDetail(id) {
      await this.$store.dispatch("LinhVucStore/getById", id).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.model = linhVucModel.fromJson(res.data);
          console.log("LOG DETAIL  : " , this.model)
          this.showDetail = true;
        }
      });
    },
    async handleDelete() {
      if (this.model.id != 0 && this.model.id != null && this.model.id) {
        await this.$store.dispatch("LinhVucStore/delete", this.model.id).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            this.showDeleteModal = false;
            this.$refs.tblList.refresh()
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
        await this.$store.dispatch("linhVucStore/update", this.model).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            this.showModal = false;
            this.model = linhVucModel.baseJson()
            this.myProvider()
          }
        })
      }else{
        //Create model
        await this.$store.dispatch("linhVucStore/create", linhVucModel.toJson(this.model)).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            this.showModal = false;
            this.model = linhVucModel.baseJson()
            this.myProvider()
          }
        });
      }
    },
    myProvider (ctx) {
      const params = {
        start: 0,
        limit: this.perPage,
        content: this.filter,
        sortBy: "",
        sortDesc: false,
      }
      this.loading = true

      try {
        let promise =  this.$store.dispatch("linhVucStore/getPagingParams", params)
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
                  data-bs-target="#create-and-update"
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
                  class="table align-middle table-nowrap mb-0"
                  :items="data"
                  :fields="fields"
                  striped
                  bordered
                  responsive="sm"
                  :per-page="perPage"
                  :current-page="currentPage"
                  :filter="filter"
                  ref="tblList"
                  primary-key="id"
              >
                <template v-slot:cell(STT)="data">
                  {{ data.index + ((currentPage-1)*perPage) + 1  }}
                </template>
                <template v-slot:cell(ten)="data">&nbsp;&nbsp;
                  <div style="text-align: left ; margin-top: -30px ; margin-left: 20px">
                    {{data.item.ten}}
                  </div>
                </template>
<!--                <template v-slot:cell(donVis)="data">-->
<!--                  <router-link :to='`/linh-vuc/${data.item.id}`'>-->
<!--                    <b-button-->
<!--                        v-if="data.item.donVis.length > 0 " :class="countClassName"-->
<!--                        variant="outline-success btn-sm"  >{{ data.item.donVis.length}}</b-button>-->
<!--                    <b-button v-else :class="countClassName" variant="outline-success btn-sm"  >-->
<!--                      {{0}}-->
<!--                    </b-button>-->
<!--                  </router-link>-->
<!--                </template>-->
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
                      data-toggle="tooltip" data-placement="bottom" title="Xóa"
                      v-on:click="handleShowDeleteModal(data.item.id)">
                    <i class="fas fa-trash-alt text-danger me-1"></i>
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

    <!-- Modal add -->
    <b-modal
        id="create-and-update"
        ref="modal"
        title="Thông tin lĩnh vực"
        header-class="bg-primary-dark modal-title p-3"
        hide-footer
        v-model="showModal"
    >
      <form class="" @submit="HandleSubmit">
        <div class="mb-3">
          <label for="name" class="form-label">
            Tên lĩnh vực
            <span class="text-danger">*</span>
          </label>
          <input
              type="text"
              class="form-control"
              id="name"
              name="name"
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
              id="so-thu-tu"
              name="so-thu-tu"
              v-model="model.thuTu"
          />
        </div>
        <div class="mb-3">
          <label for="description" class="form-label">Mô tả</label>
          <textarea
              type="text"
              class="form-control"
              id="mo-ta"
              name="mo-ta"
              rows="2"
              v-model="model.moTa"
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
    <div
      class="modal fade zoomIn"
      id="chinh-sua"
      tabindex="-1"
      aria-labelledby="EditModalLabel"
      aria-hidden="true"
    >
      <div class="modal-dialog modal-dialog-centered modal-lg">
        <div class="modal-content border-0">
          <div class="modal-header p-3 bg-primary-dark">
            <h5 class="modal-title" id="EditModalLabel">Chỉnh sửa lĩnh vực</h5>
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
          <div class="p-3">
            <chinh-sua />
          </div>
        </div>
      </div>
    </div>
    <!-- Modal delete -->
    <div
      class="modal fade"
      id="delete"
      data-bs-backdrop="static"
      data-bs-keyboard="false"
      tabindex="-1"
      role="dialog"
      aria-labelledby="staticBackdropLabel"
      aria-hidden="true"
    >
      <div class="modal-dialog modal-dialog-centered" role="document">
        <div class="modal-content">
          <div class="modal-body text-center p-5">
            <lord-icon
              src="https://cdn.lordicon.com/lupuorrc.json"
              trigger="loop"
              colors="primary:#121331,secondary:#08a88a"
              style="width: 120px; height: 120px"
            >
            </lord-icon>

            <div class="mt-4">
              <div class="mb-3 cl-warning">
                <i class="ri-alert-line fs-1"></i>
              </div>
              <p class="text-muted mb-4">Bạn thật sự muốn xóa dữ liệu.</p>
              <div class="hstack gap-2 justify-content-center">
                <a
                  href="javascript:void(0);"
                  class="btn btn-link link-danger fw-medium"
                  data-bs-dismiss="modal"
                  ><i class="ri-close-line me-1 align-middle"></i> Đóng</a
                >
                <a
                  href="javascript:void(0);"
                  data-bs-dismiss="modal"
                  class="btn btn-success"
                  >Đồng ý</a
                >
              </div>
            </div>
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
