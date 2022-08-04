<script>
import Layout from "../../layouts/main";
import appConfig from "@/app.config";

import Chart from "@/components/widgets/chart";
import Stat from "@/components/widgets/widget-stat";
import Transaction from "@/components/widgets/transaction";
import Chat from "@/components/widgets/chat";
import Activity from "@/components/widgets/activity";

import { sparklineChartData, salesDonutChart, radialBarChart } from "./data";
import {CONSTANTS} from "@/helpers/constants";
import moment from "moment";

export default {
  page: {
    title: "Bảng điều khiển",
    meta: [{ name: "description", content: appConfig.description }]
  },
  components: {
    Layout,
    Stat,
  },
  data() {
    return {
      sparklineChartData: sparklineChartData,
      salesDonutChart: salesDonutChart,
      radialBarChart: radialBarChart,
      statData: [
        {
          title: " Văn bản đến",
          image: require("@/assets/images/services-icon/01.png"),
          value: "12",
          subText: "VBD",
          color: "success"
        },
        {
          title: " Văn bản đi",
          image: require("@/assets/images/services-icon/02.png"),
          value: "40",
          subText: "VBD",
          color: "danger"
        },
        {
          title: "Văn bản đã xử lý",
          image: require("@/assets/images/services-icon/03.png"),
          value: "15",
          subText: "VBDXL",
          color: "info"
        },
        {
          title: "Thông báo",
          image: require("@/assets/images/services-icon/04.png"),
          value: "45",
          subText: "HT",
          color: "warning"
        }
      ],
      lichCongTacCaNhan: null,
      lichCongTacDonVi: null,
      lichCongTacTruong: null,
      isCollapse: false,
      isCollapseCaNhan: true
    };
  },
 async created() {
 this.myProviderLichCongTacTruong();
 this.myProviderLichCongCaNhan();
  },
  methods:{
    myProviderLichCongTacTruong() {
      try {
        this.$store.dispatch("lichCongTacStore/getPaging",{loaiLichCongTac: 'lctt', selectDay: moment().format("DD/MM/YYYY") } ).then(resp => {
          if (resp.resultCode == CONSTANTS.SUCCESS) {
            this.lichCongTacTruong = resp.data;
            return;
          } else {
            return null;
          }
        })
      }finally {
        console.log()
      }
    },
    myProviderLichCongCaNhan() {
      try {
        this.$store.dispatch("lichCongTacStore/getPaging",{loaiLichCongTac: null, selectDay: moment().format("DD/MM/YYYY") } ).then(resp => {
          if (resp.resultCode == CONSTANTS.SUCCESS) {
            this.lichCongTacCaNhan = resp.data;
            return;
          } else {
            return null;
          }
        })
      }finally {
        console.log()
      }
    },
  }
};
</script>

<template>
  <Layout>
    <!-- start page title -->
    <div class="row align-items-center">
      <div class="col-sm-6">
        <div class="page-title-box">
          <h4 class="font-size-18"> Bảng điều khiển</h4>
          <ol class="breadcrumb mb-0">
            <li class="breadcrumb-item active"> Chào mừng đến với hệ thống EOffice - Trường Đại học Đồng Tháp</li>
          </ol>
        </div>
      </div>

      <div class="col-sm-6">
        <div class="float-end d-none d-md-block">
          <b-dropdown right variant="primary" menu-class="dropdown-menu-end">
            <template v-slot:button-content>
              <i class="mdi mdi-cog me-2"></i> Truy cập nhanh
            </template>
            <b-dropdown-item href="http://dthu.edu.vn" target="_blank"> Trang chủ DThU</b-dropdown-item>
            <b-dropdown-item> Thông tin bản quyền </b-dropdown-item>
          </b-dropdown>
        </div>
      </div>
    </div>
    <!-- end page title -->
    <div class="row">
      <div class="col-xl-3 col-md-6" v-for="stat of statData" :key="stat.icon">
        <Stat
            :title="stat.title"
            :image="stat.image"
            :subText="stat.subText"
            :value="stat.value"
            :color="stat.color"
        />
      </div>
    </div>

    <div class="row">
      <div class="col-xl-12">
        <section v-if="lichCongTacCaNhan">
          <div v-for="(item, index) in lichCongTacCaNhan" :key="index">
            <b-card style="padding: 10px" class="card-lichcongtac">
              <h5 style="font-weight: bold;" class="title-capso mb-2">Lịch công tác cá nhân</h5>
              <b-card-header v-if="item"
                             class="fw-bold text-white bg-primary"
                             style="display: flex; justify-content: space-between"
              >
                <div >
                  <i class="fas fa-calendar-alt me-1"></i>
                  {{ item.ngayXepLich }}
                </div>

                <button
                    @click="isCollapseCaNhan = !isCollapseCaNhan"
                    type="button"
                    size="sm"
                    class="btn btn-outline btn-sm"
                    :class="isCollapseCaNhan ? null : 'collapsed'"
                    :aria-expanded="isCollapseCaNhan ? 'true' : 'false'"
                    aria-controls="collapse1"
                    data-toggle="tooltip" data-placement="bottom" title="Xem chi tiết lịch">

                  <i v-if="isCollapseCaNhan" class="fas fa-chevron-up text-white me-1"></i>
                  <i v-else class="fas fa-chevron-down text-white me-1"></i>
                </button>
              </b-card-header>
              <b-collapse  v-model="isCollapseCaNhan" id="collapse1">
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
        <section v-if="lichCongTacTruong">
          <div v-for="(item, index) in lichCongTacTruong" :key="index">
            <b-card style="padding: 10px" class="card-lichcongtac">
              <h5 style="font-weight: bold;" class="title-capso mb-2">Lịch công tác trường</h5>
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
      </div>
    </div>
  </Layout>
</template>

<style>
.title-capso{
  font-weight: bold; color: #00568C;

}
</style>