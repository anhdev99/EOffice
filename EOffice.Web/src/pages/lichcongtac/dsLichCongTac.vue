<script>
import Layout from "../../layouts/main";
import PageHeader from "@/components/page-header";

import appConfig from "@/app.config";
import {lichCongTacModel} from "@/models/lichCongTacModel";
import {CONSTANTS} from "@/helpers/constants";
import DatePicker from "vue2-datepicker";
import moment from "moment";

export default {
  page: {
    title: "Danh sách lịch công tác",
    meta: [{name: "description", content: appConfig.description}],
  },
  components: {Layout, PageHeader, DatePicker},
  data() {
    return {
      title: "Danh sách lịch công tác",
      items: [
        {
          text: "Lịch công tác",
          href: "/danh-sach-lich-cong-tac",
          // active: true,
        },
        {
          text: "Danh sách",
          active: true,
        }
      ],
      model: lichCongTacModel.baseJson(),
      selectDay: moment().format("DD/MM/YYYY"),
      fields: [
        {key: 'STT', label: 'STT', class: 'td-stt', sortable: false, thClass: 'hidden-sortable'},
        {
          key: "chuTri",
          label: "Người chủ trì",
          class: "px-2",
          sortable: true,
          thStyle: {width: '100px', minWidth: '100px'},
        },
        {
          key: "thoiGian",
          label: "Thời gian",
          thClass: 'hidden-sortable',
          thStyle: {width: '100px', minWidth: '100px'},
          sortable: false,
        },
        {
          key: 'noiDung',
          label: "Nội dung",
          thClass: 'hidden-sortable',
          sortable: false,
          class: "text-center"
        },
        {
          key: 'diaDiem',
          label: "Địa điểm",
          thClass: 'hidden-sortable',
          sortable: false,
          thStyle: {width: '100px', minWidth: '100px'},
          class: "text-center"
        },
        {
          key: 'thanhPhan',
          label: "Thành phần",
          thClass: 'hidden-sortable',
          sortable: false,
          class: "text-center"
        },
        {
          key: 'ghiChu',
          label: "Ghi chú",
          thClass: 'hidden-sortable',
          sortable: false,
          thStyle: {width: '100px', minWidth: '100px'},
          class: "text-center"
        },
      ],
      loaiLichCongTac: this.$route.params.loaiLichCongTac,
      lang: {
        formatLocale: {
          firstDayOfWeek: 1,
        },
        monthBeforeYear: false,
      },
      isCollapse: false
    };
  },
  created() {
    this.getUser();
    this.myProvider();
  },
  watch: {
    model: {
      deep: true,
      handler(val) {
        // addCoQuanToModel()
        // this.saveValueToLocalStorage()
      },
    },
    '$route.params.loaiLichCongTac':{
      deep: true,
      handler(val) {
        this.loaiLichCongTac = val;
        this.myProvider();
      }
    }
  },
  methods: {
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
    myProvider(ctx) {
      try {
        this.$store.dispatch("lichCongTacStore/getPaging",{loaiLichCongTac: this.loaiLichCongTac, selectDay: this.selectDay } ).then(resp => {
          if (resp.resultCode == CONSTANTS.SUCCESS) {
            let data = resp.data;
            this.model = resp.data;
            this.loading = false
          } else {
            return [];
          }
        })
      } finally {
        this.loading = false
      }
    },
    handleTimKiem(){
     this.myProvider();
    },
  }
}
</script>
<template>
  <Layout>
    <PageHeader :title="title" :items="items"/>
    <section>
      <div class="card">
        <div class="card-body">
          <div class="row" style="display: flex;align-items: center">
            <b-card-title>
              Chọn ngày công tác
            </b-card-title>

            <div class="col-md-4">
              <date-picker
                  v-model="selectDay"
                  :lang="lang"
                  format="DD/MM/YYYY"
                  placeholder="Chọn ngày"
                  :value="selectDay"
              ></date-picker>
            </div>
            <div class="col-md-4">
              <b-button
                  variant="primary"
                  type="button"
                  class="btn w-md btn-primary"
                  @click="handleTimKiem"
                  size="sm"
              >
                <i class="mdi mdi-plus me-1"></i> Tìm kiếm
              </b-button>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!--    Danh sach lic cong tac -->
    <section>
      <div v-for="(item, index) in model" :key="index">
        <b-card style="padding: 0" class="card-lichcongtac">

          <b-card-header v-if="item"
              class="fw-bold text-white bg-primary"
                         style="display: flex; justify-content: space-between"
          >
            <div >
              <i class="fas fa-calendar-alt me-1"></i>
              {{ item.ngayXepLich }}
            </div>

            <button
                v-b-toggle="'collapse-'+index"
                @click="isCollapse =!isCollapse"
                type="button"
                size="sm"
                class="btn btn-outline btn-sm"
                data-toggle="tooltip" data-placement="bottom" title="Xem chi tiết lịch">

              <i v-if="isCollapse" class="fas fa-chevron-up text-white me-1"></i>
              <i v-else class="fas fa-chevron-down text-white me-1"></i>
            </button>
          </b-card-header>
          <b-collapse :id="'collapse-' +index" accordion="my-accordion" role="tabpanel">
            <!--              Table -->
            <table style="margin: 0px" class="table table-bordered">
              <thead>
              <tr>
                <th width="15%" class="title-capso">Người chủ trì</th>
                <th class="text-center title-capso" width="100px">Thời gian</th>
                <th  class="title-capso">Nội dung</th>
                <th  class="title-capso" width="20%">Địa điểm</th>
                <th class="title-capso"  width="20%">Thành phần</th>
                <th  class="title-capso" width="10%">Ghi chú</th>
              </tr>
              </thead>
              <tbody v-if="item">
              <tr
                  v-for="(cv, index) in item.congViecs"
                  :key="index"
              >
                <td v-if="cv.rowSpan > 0" :rowspan="`${cv.rowSpan}`" style="vertical-align : middle;text-align:left;" class="px-1">
                  <div  v-for="(value, index) in cv.chuTri" :key="index">
                    {{value.fullName}}
                  </div>
                </td>
                <td class="text-center" style="vertical-align : middle;text-align:left;" >
                  <div  v-if="cv.tuNgay && cv.denNgay">
                    {{cv.tuNgay}} - {{cv.denNgay}}
                  </div>
                  <div v-else-if="cv.thoiGian">
                    {{cv.thoiGian}}
                  </div>
                  <div v-else>
                    {{cv.tuNgay}}
                  </div>
                </td>
                <td class="px-1" style="vertical-align : middle;text-align:left;" >
                  <div v-if="cv.noiDung" :inner-html.prop="cv.noiDung" >
                  </div>
                </td>
                <td class="px-2" style="vertical-align : middle;text-align:left;" >
                  <div v-if="cv.diaDiem" :inner-html.prop="cv.diaDiem" >
                  </div>
                </td>
                <td class="px-1" style="vertical-align : middle;text-align:left;" >
                  <div v-if="cv.thanhPhanThamDu" >
                    <div class="title-capso">Thành phần tham dự:</div>
                    <span  v-for="(value, index) in cv.thanhPhanThamDu" :key="index">
                    <span>
                        {{value.fullName}},
                    </span>
                  </span>
                  </div>
                  <div v-if="cv.thanhPhan">
                    <div class="title-capso">Thành phần khác:</div>
                    <div v-if="cv.thanhPhan" :inner-html.prop="cv.thanhPhan" >
                    </div>
                  </div>

                </td>
                <td style="vertical-align : middle;text-align:left;" >
                  <div v-if="cv.ghiChu" :inner-html.prop="cv.ghiChu">
                  </div>
                </td>
              </tr>
              </tbody>
            </table>

          </b-collapse>

        </b-card>
      </div>
    </section>

  </Layout>
</template>
<style>
.title-capso{
  font-weight: bold; color: #00568C;

}

.card-lichcongtac .card-body{
  padding: 0px;
}
</style>
