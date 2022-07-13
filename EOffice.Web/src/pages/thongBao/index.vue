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
      todofilterOn: [],
      numberOfElement: 1,
      totalRows: 1,
      model: thongBaoModel.baseJson(),
      modelUser:userModel.baseJson(),
      pagination: pagingModel.baseJson()
    };
  },
  validations: {},
  created() {
    //this.showModal=true;
    this.myProvider();
    this.fnGetList();
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
      console.log('Log param', params)
      this.loading = true
      try {
        this.$store.dispatch("notificationStore/getPagingParams", params)
        .then(resp => {
          if(resp.resultCode == CONSTANTS.SUCCESS){
            let data = resp.data;
            this.totalRows = data.totalRows
            let items = data.data
            this.numberOfElement = items.length
            this.loading = false
            return items || []
          }else{
            return [];
          }
        })
      } finally {
        this.loading = false
      }
    },
    async onPageChange(page = 1) {
      this.pagination.currentPage = page;
      const params = {
        pageNumber: this.pagination.currentPage,
        pageSize: this.pagination.pageSize,
      }
    },
    async fnGetList() {
      await this.onPageChange();
    },
    async handleDetail(id) {
      console.log(id)
      await this.$store.dispatch("notificationStore/getById", id).then((res) => {
        this.model = thongBaoModel.fromJson(res);
        console.log('model',this.model)
        this.showModal = true;
      });
    },
    async handleDetailUser(id) {
      console.log(id)
      await this.$store.dispatch("userStore/getById", id).then((res) => {
        this.modelUser = userModel.fromJson(res.data);
        console.log(userModel)
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
            <div class="btn-toolbar p-3">
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
            <div class="mt-3">
              <ul class="message-list" >
                <div v-for="item in data" :key="item.id" class="pe-3">
                  <div v-if="item.read==false" >
                    <li style="background-color: #fff">
                      <div class="col-mail col-mail-1" >
                        <div class="checkbox-wrapper-mail">
                          <input :id="`chk-${item.id}`" type="checkbox"/>
                          <label :for="`chk-${item.id}`"></label>
                        </div>
                        <span :class="`star-toggle far fa-star text-${item.id}`"></span>
                        <a href="#" class="title" style="font-weight: bold; width: 280px" v-on:click="handleDetail(item.id),handleDetailUser(item.senderId)">{{ item.sender }}</a>
                      </div>
                      <div class="col-mail col-mail-2" style="position: absolute;
                                top: 0;
                                left: 240px;
                                right: 0;
                                bottom: 0;font-weight: bold"
                      >
                        <div class="col-mail col-mail-1">
                          <span class="title" style="min-width: 300px">{{ item.content }}</span>
                        </div>
                        <div class="date" style="float: right">01/06/2022</div>
                        <div class="icon" style="float: right">
                          <i class="bx bxs-circle text-primary"></i>
                        </div>
                      </div>
                    </li>
                  </div>
                  <div v-else>
                    <li style="background-color: #fff">
                      <div class="col-mail col-mail-1" >
                        <div class="checkbox-wrapper-mail">
                          <input :id="`chk-${item.id}`" type="checkbox"/>
                          <label :for="`chk-${item.id}`"></label>
                        </div>
                        <span :class="`star-toggle far fa-star text-${item.id}`"></span>
                        <a href="#" class="title" style="width: 280px" v-on:click="handleDetail(item.id),handleDetailUser(item.senderId)">{{ item.sender }}</a>
                      </div>
                      <div class="col-mail col-mail-2" style="position: absolute;
                                top: 0;
                                left: 240px;
                                right: 0;
                                bottom: 0;"
                      >
                        <div class="col-mail col-mail-1"><span class="title" style="min-width: 300px">{{ item.content }}</span></div>
                        <div class="date" style="float: right">01/06/2022</div>
                      </div>
                    </li>
                  </div>
                </div>
              </ul>
            </div>
            <div class="row justify-content-md-between align-items-md-center">
              <div class="col-xl-7">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Hiển thị {{ numberOfElement }} trên tổng số {{ totalRows }} dòng</div>
              <!-- end col-->
              <div class="col-xl-5">
                <div class="text-md-right float-end mt-2 pagination-rounded">
                  <b-pagination
                      v-model="currentPage"
                      :total-rows="totalRows"
                      :per-page="perPage"
                  ></b-pagination>
                </div>
              </div>
              <!-- end col-->
            </div>
          </div>

        </div>
        <div class="table-responsive p-0">
          <b-table
              class="datatables"
              :items="myProvider"
              bordered
              outlined
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
            <!--                      <template v-slot:cell(STT)="data">-->
            <!--                        {{ data.index + ((currentPage - 1) * perPage) + 1 }}-->
            <!--                      </template>-->
            <!--                      <template v-slot:cell(process)="data">-->
            <!--                        <button-->
            <!--                            type="button"-->
            <!--                            size="sm"-->
            <!--                            class="btn btn-outline btn-sm"-->
            <!--                            data-toggle="tooltip" data-placement="bottom" title="Xem thông báo"-->
            <!--                            v-on:click="handleDetail(data.item.id)">-->
            <!--                          <i class="fas fa-eye  text-warning me-1"></i>-->
            <!--                        </button>-->
            <!--                      </template>-->
            <!--                      <template v-slot:cell(read)="data">-->
            <!--                      <span-->
            <!--                          class="badge font-size-small min-width-30 p-2 btn-xs"-->
            <!--                          :class="{-->
            <!--                        'badge-soft-success font-weight-semibold':`${data.item.read}` === true,-->

            <!--                        'badge-soft-warning  font-weight-semibold': `${data.item.read}` === false,-->

            <!--                      }"-->
            <!--                      >-->
            <!--                        <span v-if="data.item.read" class="badge-soft-success font-weight-semibold">Đã xem</span>-->
            <!--                        <span v-else class="badge-soft-warning  font-weight-semibold">Chưa xem</span>-->
            <!--                      </span>-->
            <!--                      </template>-->
          </b-table>
        </div>
      </div>
    </div>
    <b-modal
        v-model="showModal"
        id="modal-lg"
        size="lg"
        body-class="p-0"
    >
      <template #modal-title>
        Thông tin chi tiết
      </template>

      <!--      <div class="row">-->
      <!--        <div class="col-12">-->
      <!--          <div class="col-12 pb-0">-->
      <!--            <h5 style="color: red">{{ model.title }}</h5>-->
      <!--          </div>-->
      <!--          <div class="col-12 text-end pt-0">-->
      <!--            <i>Người gửi: {{ model.sender }}</i>-->
      <!--          </div>-->
      <!--        </div>-->
      <!--      </div>-->
      <!--      <p class="mb-0">-->
      <!--        {{ model.content }}-->
      <!--      </p>-->
      <div class="card-body pb-0">
        <h4 class="mt-0 font-size-16 pb-3" style="font-weight: bold">{{model.title}}</h4>
        <div class="d-flex mb-4">
          <!--              <div>-->
          <!--                <img-->
          <!--                    class="d-flex me-3 rounded-circle avatar-sm"-->
          <!--                    src="@/assets/images/4.png"-->
          <!--                    alt="Generic placeholder image"-->
          <!--                />-->
          <!--              </div>-->
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
            <h5 class="font-size-14 mt-1">{{model.sender}} <small class="text-muted">&lt;{{modelUser.email}}&gt;</small></h5>
            <small class="text-muted">tới {{model.recipient}}</small>

          </div>
          <div class="flex-grow-2">
            <small class="text-muted" style="float: right"><i>Ngày gửi: 01/06/2022</i></small>
          </div>
        </div>

        <p>{{model.content}}</p>
        <!--            <hr />-->
        <!--            <div class="row pb-0">-->
        <!--              <div class="title pb-3 pt-0">-->
        <!--                <h4 class="mt-0 font-size-13 font-weight-semibold">Tệp đính kèm</h4>-->
        <!--              </div>-->
        <!--              <div class="col-xl-2 col-6 pb-0">-->
        <!--                <div class="card">-->
        <!--                  <img-->
        <!--                      class="card-img-top img-fluid"-->
        <!--                      src="@/assets/images/small/img-3.jpg"-->
        <!--                      alt="Card image cap"-->
        <!--                  />-->
        <!--                  <div class="py-2 text-center">-->
        <!--                    <a href class="fw-medium">Tải xuống</a>-->
        <!--                  </div>-->
        <!--                </div>-->
        <!--              </div>-->
        <!--              <div class="col-xl-2 col-6 pb-0">-->
        <!--                <div class="card">-->
        <!--                  <img-->
        <!--                      class="card-img-top img-fluid"-->
        <!--                      src="@/assets/images/small/img-4.jpg"-->
        <!--                      alt="Card image cap"-->
        <!--                  />-->
        <!--                  <div class="py-2 text-center">-->
        <!--                    <a href class="fw-medium">Tải xuống</a>-->
        <!--                  </div>-->
        <!--                </div>-->
        <!--              </div>-->
        <!--            </div>-->
        <!--            <a href="javascript: void(0);" class="btn btn-secondary waves-effect mt-4">-->
        <!--              <i class="mdi mdi-reply"></i> Reply-->
        <!--            </a>-->
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
</style>
