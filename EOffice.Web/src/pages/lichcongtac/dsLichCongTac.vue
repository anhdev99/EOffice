<script>
import Layout from "../../layouts/main";
import PageHeader from "@/components/page-header";

import appConfig from "@/app.config";
import {lichCongTacModel} from "@/models/lichCongTacModel";
import {CONSTANTS} from "@/helpers/constants";
import DatePicker from "vue2-datepicker";

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
      selectDay: null,
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
    myProvider() {
      try {
        this.$store.dispatch("lichCongTacStore/getAll").then(resp => {
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
    }
  }
}
</script>
<template>
  <Layout>
    <PageHeader :title="title" :items="items"/>
    <section>
      <b-card>
        <b-card-body>
          <div class="row">
            <div class="col-md-4">
              <b-card-title>
                Chọn ngày công tác
              </b-card-title>
              <date-picker
                  v-model="selectDay"
                  :first-day-of-week="1"
                  format="DD/MM/YYYY"
                  lang="vi-VN"
                  confirm
                  placeholder="Chọn ngày"
              ></date-picker>
            </div>
          </div>

        </b-card-body>
      </b-card>
    </section>

    <!--    Danh sach lic cong tac -->
    <section>
      <div v-for="(item, index) in model" :key="index">
        <b-card style="padding: 0" class="card-lichcongtac">

          <b-card-header v-if="item"
              class="fw-bold text-white bg-primary"
          >
            <i class="fas fa-calendar-alt me-1"></i>
            {{ item.ngayXepLich }}
          </b-card-header>

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
