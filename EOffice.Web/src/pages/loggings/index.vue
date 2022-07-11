<script>
import Layout from "../../layouts/main";
import PageHeader from "@/components/page-header";
import {required} from "vuelidate/lib/validators";
import appConfig from "@/app.config";
import {notifyModel} from "@/models/notifyModel";
import {pagingModel} from "@/models/pagingModel";
import {chucVuModel} from "@/models/chucVuModel";
export default {
  page: {
    title: "Quản Lý Logging",
    meta: [{name: "description", content: appConfig.description}],
  },
  components: {Layout, PageHeader},
  data() {
    return {
      title: "Logging",
      items: [
        {
          text: "Logging",
          href: "/logging",
        },
        {
          text: "Danh sách",
          active: true,
        }
      ],
      pagination: pagingModel.baseJson(),
      totalRows: 1,
      currentPage: 1,
      numberOfElement: 1,
      perPage: 10,
      pageOptions: [5,10, 25, 50, 100],
      filter: null,
      fields: [
        {
          key: 'STT',
          label: 'STT',
          class : "text-center",
          thStyle: {width: '50px', minWidth: '50px' },
          tdClass: 'align-middle',
          thClass: 'hidden-sortable'
        },
        {
          key: "lastModifiedShow",
          label: "Ngày",
          class : "text-center",
          sortable: true,
          thStyle: {width: '110px', minWidth: '110px'},
        },
        {
          key: "modifiedBy",
          label: "UserName",
          sortable: true,
        },
        {
          key: "collectionName",
          class : "text-center",
          label: "Collection Name",
          sortable: true,
        },
        {
          key: "action",
          label: "Action",
          class : "text-center",
          thStyle: {width: '100px', minWidth: '100px'},
          thClass: 'hidden-sortable'
        },
        {
          key: "actionResult",
          label: "ActionResult",
          class : "text-center",
          thStyle: {width: '130px', minWidth: '130px'},
          thClass: 'hidden-sortable'
        },
        {
          key: "contentLog",
          label: "ConnentLog",
          thClass: 'hidden-sortable',
          class : "text-center",
          sortable: true,
        }
      ],
    };
  },
  validations: {
    model: {
      ten: {required},
      thuTu: {required}
    },
  },
  watch: {
    model: {
      deep: true,
      handler(val) {
        // addCoQuanToModel()
        // this.saveValueToLocalStorage()
      }
    },
    showModal(status) {
      if (status == false) this.model = chucVuModel.baseJson();
    },
    showDeleteModal(val) {
      if (val == false) {
        this.model.id = null;
      }
    },
  },
  methods: {
    myProvider (ctx) {
      const params = {
        start: ctx.currentPage,
        limit: ctx.perPage,
        content: this.filter,
        sortBy: ctx.sortBy,
        sortDesc: ctx.sortDesc,
      }
      this.loading = true
      try {
        let promise =  this.$store.dispatch("loggingStore/getPagingParams", params)
        return promise.then(resp => {
          let items = resp.data.data
          this.totalRows = resp.data.totalRows
          this.numberOfElement = resp.data.data.length
          this.loading = false
          return items || []
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
      <div class="col-12">
        <div class="card">
          <div class="card-body">
            <div class="row mb-2">
              <div class="col-sm-4">
                <div class="search-box me-2 mb-2 d-inline-block">
                  <div class="position-relative">
                    <input
                        v-model = "filter"
                        type="text"
                        class="form-control"
                        placeholder="Tìm kiếm ..."
                    />
                    <i class="bx bx-search-alt search-icon"></i>
                  </div>
                </div>
              </div>
            </div>
            <div class="row" style="margin-top: -20px">
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
                          :filter="filter"
                          ref="tblList"
                          primary-key="id"
                      >
                        <template v-slot:cell(STT)="data">
                          {{ data.index + ((currentPage-1)*perPage) + 1  }}
                        </template>
                        <template v-slot:cell(ten)="data">&nbsp;&nbsp;
                          {{data.item.ten}}
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
                <button v-b-modal.modal-close_visit
                        class="btn btn-outline-info m-1"
                        v-on:click="showDeleteModal = false">
                  Đóng
                </button>
                <button v-b-modal.modal-close_visit
                        class="btn btn-danger btn m-1"
                        v-on:click="handleDelete">
                  Xóa
                </button>
              </template>
            </b-modal>
          </div>
        </div>
  </Layout>
</template>
<style>
.table>tbody> tr >td{
  padding: 0px;
  line-height: 30px;
}
.hidden-sortable:after, .hidden-sortable:before {
  display: none !important;
}

</style>
