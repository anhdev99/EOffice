<script>
import Layout from "@/layouts/main";
import PageHeader from "@/components/page-header";
import appConfig from "../../../app.config.json";
import { data } from "./data";

export default {
  page: {
    title: "Thông báo",
    meta: [
      {
        content: appConfig.description,
      },
    ],
  },
  data() {
    return {
      title: "Thông báo",
      items: [
        {
          text: "Trang chủ",
          href: "/",
        },
        {
          text: "Thông báo",
          active: true,
        },
      ],
      data: data,
    };
  },
  components: { Layout, PageHeader },
  methods: {
    ThemMoi() {
      document.getElementById("ThemMoi").reset();
      document.getElementById("CreateModalLabel").innerHTML =
        "Thêm mới văn bản đi";
      document.getElementById("ThemMoi").style.display = "block";
    },
    KySo() {
      document.getElementById("KySo").reset();
      document.getElementById("EditModalLabel").innerHTML =
        "Chỉnh sửa văn bản đi";
      document.getElementById("KySo").style.display = "block";
    },
  },
};
</script>

<template>
  <Layout>
    <PageHeader :title="title" :items="items" />

    <div class="row page-vanbanden">
      <div class="col-xl-12">
        <div class="card">
          <div class="card-header align-items-center d-flex">
            <h4 class="card-title mb-0 flex-grow-1">Mới nhất</h4>
          </div>
          <div class="row">
            <div class="col-12">
              <!--  Table -->
              <table class="table align-middle table-nowrap mb-0">
                <thead class="table-light">
                  <tr>
                    <th scope="col">#</th>
                    <th scope="col">Nội dung</th>
                    <th scope="col">Ngày tạo</th>
                    <th scope="col"></th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-if="data.length <= 0" class="text-center">
                    <td colspan="6">Không tìm thấy dữ liệu</td>
                  </tr>
                  <tr v-else v-for="(item, index) in data" :key="item.id">
                    <td>{{ ++index }}</td>
                    <td>{{ item.content }}</td>
                    <td>{{ item.createDate }}</td>
                    <td>
                      <div class="hstack gap-3 fs-15">
                        <a href="javascript:void(0);" class="link-info"
                          ><i class="ri-checkbox-blank-circle-fill"></i
                        ></a>
                      </div>
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
          <div
            class="align-items-center mt-2 row g-3 text-center text-sm-start px-3 mb-3"
          >
            <div class="col-sm">
              <div class="text-muted">
                Hiển thị<span class="fw-semibold">4</span> of
                <span class="fw-semibold">125</span> kết quả
              </div>
            </div>
            <div class="col-sm-auto">
              <ul
                class="pagination pagination-separated pagination-sm justify-content-center justify-content-sm-start mb-0"
              >
                <li class="page-item disabled">
                  <a href="#" class="page-link">←</a>
                </li>
                <li class="page-item">
                  <a href="#" class="page-link">1</a>
                </li>
                <li class="page-item active">
                  <a href="#" class="page-link">2</a>
                </li>
                <li class="page-item">
                  <a href="#" class="page-link">3</a>
                </li>
                <li class="page-item">
                  <a href="#" class="page-link">→</a>
                </li>
              </ul>
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
