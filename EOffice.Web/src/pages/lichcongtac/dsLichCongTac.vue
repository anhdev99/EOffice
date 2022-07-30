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
    myProvider() {
      try {
        this.$store.dispatch("lichCongTacStore/getAll").then(resp => {
          if (resp.resultCode == CONSTANTS.SUCCESS) {
            let data = resp.data;
            console.log("data", data);
            this.model = resp.data;
            console.log("model", this.model);
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
        <b-card>
          <b-card-header
              class="fw-bold mb-3 text-white bg-primary"
          >
            <i class="fas fa-calendar-alt me-2"></i>
            {{ item.ngayXepLich }}
          </b-card-header>

          <!--              Table -->
          <table class="table table-striped table-bordered">
            <thead>
            <tr>
              <th>Người chủ trì</th>
              <th>Thời gian</th>
              <th>Nội dung</th>
              <th>Địa điểm</th>
              <th>Thành phần</th>
              <th>Thành phần tham dự</th>
              <th>Ghi chú</th>
            </tr>
            </thead>
            <tbody>
            <tr
                v-for="(lct, index) in item.lichCongTac"
                :key="index"
            >
              <td :rowspan="`${lct.congViecs.lenght}`">{{ lct.chuTri.fullName }}</td>
            </tr>
            <tr v-for="(cv, index) in item.lichCongTac" :key="index">
              <td>{{cv.thoiGian}}</td>
              <td>{{cv.noiDung}}</td>
              <td>{{cv.diaDiem}}</td>
              <td>{{cv.thanhPhan}}</td>
              <td v-for="(tptd, index) in cv.thanhPhanThamDu" :key="index">
                <li>{{tptd.fullName}}</li>
              </td>
              <td>{{cv.ghiChu}}</td>
            </tr>
            </tbody>
          </table>
        </b-card>
      </div>
    </section>

  </Layout>
</template>
<style lang="scss">

</style>
