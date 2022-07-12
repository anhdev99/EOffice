<script>
import Layout from "../../layouts/main";
import appConfig from "@/app.config";

import Chart from "@/components/widgets/chart";
import Stat from "@/components/widgets/widget-stat";
import Transaction from "@/components/widgets/transaction";
import Chat from "@/components/widgets/chat";
import Activity from "@/components/widgets/activity";

import { sparklineChartData, salesDonutChart, radialBarChart } from "./data";

export default {
  page: {
    title: "Bảng điều khiển",
    meta: [{ name: "description", content: appConfig.description }]
  },
  components: {
    Layout,
    Stat,
    Transaction,
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
          value: "185",
          subText: "VBD",
          color: "success"
        },
        {
          title: " Văn bản đi",
          image: require("@/assets/images/services-icon/02.png"),
          value: "68",
          subText: "VBD",
          color: "danger"
        },
        {
          title: " Văn bản đã xử lý",
          image: require("@/assets/images/services-icon/03.png"),
          value: "15",
          subText: "VBDXL",
          color: "info"
        },
        {
          title: " Hộp thư",
          image: require("@/assets/images/services-icon/04.png"),
          value: "243",
          subText: "HT",
          color: "warning"
        }
      ],
      transactions: [
        {
          id: "#14256",
          name: " Văn bản test ",
          date: "15/1/2018",
          status: "Xử lý xong"
        },
        {
          id: "#14257",
          name: " Văn bản test ",
          date: "16/1/2019",
          status: "Đang xử lý"
        },
        {
          id: "#14258",
          name: " Văn bản test ",
          date: "17/1/2019",
          status: "Xử lý xong"
        },
        {
          id: "#14259",
          name: " Văn bản test ",
          date: "18/1/2019",
          status: "Xử lý xong"
        },
        {
          id: "#14260",
          name: " Văn bản test ",
          date: "15/1/2018",
          status: "Từ chối"
        },
        {
          id: "#14261",
          name: " Văn bản test ",
          date: "15/1/2018",
          status: "Đang xử lý"
        }
      ],
      chatMessages: [
        {
          id: 1,
          image: require("@/assets/images/users/user-2.jpg"),
          name: "John Deo",
          message: "Hello!",
          time: "10:00"
        },
        {
          id: 2,
          image: require("@/assets/images/users/user-3.jpg"),
          name: "Smith",
          message: "Hi, How are you? What about our next meeting?",
          time: "10:01",
          odd: true
        },
        {
          id: 3,
          image: require("@/assets/images/users/user-2.jpg"),
          name: "John Deo",
          message: "Yeah everything is fine",
          time: "10:01"
        },
        {
          id: 4,
          image: require("@/assets/images/users/user-3.jpg"),
          name: "Smith",
          message: "Wow that's great",
          time: "10:02",
          odd: true
        },
        {
          id: 5,
          image: require("@/assets/images/users/user-2.jpg"),
          name: "John Deo",
          message: "Yup!",
          time: "10:03"
        }
      ],
      activityData: [
        {
          date: "Jan 22",
          text: "Responded to need “Volunteer Activities”"
        },
        {
          date: "Jan 20",
          text:
              "At vero eos et accusamus et iusto odio dignissimos ducimus qui deleniti atque... "
        },
        {
          date: "Jan 19",
          text: "Joined the group “Boardsmanship Forum”"
        },
        {
          date: "Jan 17",
          text: "Responded to need “In-Kind Opportunity”"
        },
        {
          date: "Jan 16",
          text: "Sed ut perspiciatis unde omnis iste natus error sit rem."
        }
      ]
    };
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
            <b-dropdown-item> Văn bản đến</b-dropdown-item>
            <b-dropdown-item>Văn bản đi </b-dropdown-item>
            <b-dropdown-item> Thông báo</b-dropdown-item>
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
      <div class="col-xl-3">
        <div class="card">
          <div class="card-body">
            <h4 class="card-title mb-4"> Tình trạng xử lý</h4>

            <chartist
                ratio="ct-chart wid"
                :data="salesDonutChart.data"
                :options="salesDonutChart.options"
                type="Pie"
            ></chartist>
            <div class="mt-4">
              <table class="table mb-0">
                <tbody>
                <tr>
                  <td>
                    <span class="badge bg-primary"> Văn bản đến</span>
                  </td>
                  <td></td>
                  <td class="text-end">50</td>
                </tr>
                <tr>
                  <td>
                    <span class="badge bg-success"> Văn bản đi</span>
                  </td>
                  <td></td>
                  <td class="text-end">60</td>
                </tr>
                <tr>
                  <td>
                    <span class="badge bg-warning"> Văn bản trễ hạn</span>
                  </td>
                  <td></td>
                  <td class="text-end">17</td>
                </tr>
                </tbody>
              </table>
            </div>
          </div>
        </div>
      </div>
      <div class="col-xl-9">
        <div class="card">
          <div class="card-body">
            <h4 class="card-title mb-4"> Văn bản cần xử lý</h4>
            <Transaction :transactions="transactions" />
          </div>
        </div>
      </div>
    </div>
  </Layout>
</template>