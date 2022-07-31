<script>
import Layout from "../../layouts/main";
import PageHeader from "@/components/page-header";
import appConfig from "@/app.config";
import {pagingModel} from "@/models/pagingModel";
import Sidepanel from "./sidepanel";
import {userModel} from "@/models/userModel";
import {thongBaoModel} from "@/models/thongBaoModel";
import {CONSTANTS} from "@/helpers/constants";

export default {
  page: {
    title: "Thông báo",
    meta: [{name: "description", content: appConfig.description}],
  },
  components: { Layout, PageHeader },
  data() {
    return {
      url: process.env.VUE_APP_API_URL + "files/view/",
      title: "Thông báo",
      items: [
        {
          text: "Thông báo",
          href: '/thong-bao'
        },
        {
          text: "Danh sách thông báo",
          active: true,
        }
      ],
      data: [],
      currentPage: 1,
      perPage: 30,
      pageOptions: [5, 10, 30, 50, 100],
      showModal: false,
      showDeleteModal: false,
      submitted: false,
      sortBy: "age",
      isBusy: false,
      filter: null,
      sortDesc: false,
      filterOn: [],
      todoFilter: null,
      todofilterOn: [],
      numberOfElement: 1,
      totalRows: 1,
      model: thongBaoModel.baseJson(),
      modelUser:userModel.baseJson(),
      pagination: pagingModel.baseJson(),
      fields: [
        {
          key: 'STT',
          label: 'STT',
          thStyle: {width: '50px', minWidth: '50px'},
          class: "text-center title-capso"
        },
        {
          key: "title",
          label: "Tiêu đề",
          class: "px-2 title-capso",
          sortable: true,
        },
        {
          key: "sender",
          label: "Người tạo",
          thStyle: {width: '160px', minWidth: '160px'},
          class: "text-center px-1 title-capso",
        },
        {
          key: "createdAtShow",
          label: "Ngày tạo",
          thStyle: {width: '100px', minWidth: '100px'},
          class: "text-center px-1 title-capso",
        },
        {
          key: "read",
          label: "Trạng thái",
          thStyle: {width: '100px', minWidth: '100px'},
          class: "text-center px-1 title-capso",
        },
        {
          key: 'process',
          label: 'Xử lý',
          class: "text-center title-capso",
          thStyle: {width: '110px', minWidth: '110px'},
        }
      ],
    };
  },
  validations: {},
  created() {
    this.$refs.tblList?.refresh()
  },
  watch: {
    currentPage: {
      deep: true,
      handler(val) {
        this.currentPage = val;
      }
    }
  },
  methods: {
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
        let promise = this.$store.dispatch("notificationStore/getPagingParams", params)
        return promise.then(resp => {
          if (resp.resultCode == CONSTANTS.SUCCESS) {
            let data = resp.data;
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
    async handleDetail(id) {
      await this.$store.dispatch("notificationStore/getById", id).then((res) => {
        this.model = res;
        this.showModal = true;
      });
    },
    async handleDetailUser(id) {
      await this.$store.dispatch("userStore/getById", id).then((res) => {
        this.modelUser = userModel.fromJson(res.data);
        this.showModal = true;
      });
    }
  }
};
</script>

<template>
  <Layout>
    <PageHeader :title="title" :items="items"/>
    <div class="row">
      <!-- Right Sidebar -->
      <div class="col-12">
<!--        <Sidepanel />-->
        <div class=" mb-3">
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
              </div>
              <div class="row">
                <div class="col-12">
                  <div class="row mb-2">
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
                      <template v-slot:cell(read)="data">
                        <div v-if="data.item.read">Đã đọc</div>
                        <div v-else>Chưa đọc</div>
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
        </div>
      </div>
    </div>
    <b-modal
        v-model="showModal"
        id="modal-lg"
        size="lg"
        body-class="p-0"
        hide-footer
        no-close-on-backdrop
    >
      <template #modal-header="{ close }">
        <!-- Emulate built in modal header close button action -->
        <h5> Thông báo</h5>
        <div class="text-end">
          <b-button variant="light" size="sm" style="width: 80px" @click="showModal = false">
            Đóng
          </b-button>
        </div>
      </template>
      <div class="card-body pb-0">
        <h4 class="mt-0 font-size-16 pb-3" style="font-weight: bold">{{model.title}}</h4>
        <div class="d-flex mb-4">
          <span>
              <span v-if="modelUser.avatar">
                <img
                    :src="url + `${modelUser.avatar.fileId}`"
                    alt="Avatar"
                    class="d-flex me-3 rounded-circle avatar-sm"
                />

              </span>
              <span v-else>
                <img
                    class="d-flex me-3 rounded-circle avatar-sm"
                    src="@/assets/images/4.png"
                    alt="Avatar"
                />
              </span></span>&nbsp;
          <div class="flex-grow-1">
            <h5 class="font-size-14 mt-1">{{model.sender}}</h5>
            <small class="text-muted">tới {{model.recipient}}</small>

          </div>
          <div class="flex-grow-2">
            <small class="text-muted" style="float: right"><i>Ngày gửi: {{ model.createdAtShow }} {{model.createdAtTimeShow}}</i></small>
          </div>
        </div>
        <p v-if="model.content" :inner-html.prop="model.content">
        </p>
      </div>
    </b-modal>
  </Layout>

</template>
<style>
.td-stttt {
  text-align: center;
  width: 10px;
}

.td-tb {
  text-align: left;
  width: 250px;
}

.td-content {
  text-align: center;
  width: 250px;
}

.td-nguoitao {
  text-align: center;
  width: 80px;
}

.td-trangthai {
  text-align: center;
  width: 50px;
}

.td-xuly {
  text-align: center;
  width: 50px;
}

.table > tbody > tr > td {
  padding: 0px;
  line-height: 30px;
}

.hidden-sortable:after, .hidden-sortable:before {
  display: none !important;
}

.title-capso {
  font-weight: 500;
  color: #00568C;
  margin-right: 10px;

}
</style>
