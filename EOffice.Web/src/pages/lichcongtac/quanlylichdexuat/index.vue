<script>
import Layout from "../../../layouts/main";
import PageHeader from "@/components/page-header";
import BanQuanLyDeXuat from "./banquanlydexuat.vue";
import LichQuanLyDeXuat from "./lichquanlydexuat.vue";

export default {
  components: {Layout, PageHeader, BanQuanLyDeXuat, LichQuanLyDeXuat},
  data() {
    return {
      title: "Quản lý lịch đề xuất",
      items: [
        {
          text: "Trang chủ",
          herf: "/",
        },
        {
          text: "quản lý lịch đề xuất",
          active: true,
        },
      ],
    };
  },
};
</script>

<template>
  <Layout>
    <PageHeader :title="title" :items="items"/>
    <div class="card">
      <div class="card-body">
        <ul class="nav nav-tabs nav-pills mb-3" role="tablist">
          <li class="nav-item waves-effect waves-light">
            <a class="nav-link active" data-bs-toggle="tab" href="#lich-cong-tac" role="tab" aria-selected="false">
              Lịch quản lý đề xuất
            </a>
          </li>
          <li class="nav-item waves-effect waves-light">
            <a class="nav-link" data-bs-toggle="tab" href="#bang-cong-tac" role="tab" aria-selected="false">
              Bảng quản lý đề xuất
            </a>
          </li>
        </ul>
        <!-- Tabs panels -->
        <div class="tab-content text-muted">
          <div class="tab-pane active" id="lich-cong-tac" role="tabpanel">
            <lich-quan-ly-de-xuat/>
          </div>
          <div class="tab-pane" id="bang-cong-tac" role="tabpanel">
            <ban-quan-ly-de-xuat/>
          </div>
        </div>
      </div>
    </div>
  </Layout>
</template>

<style></style>
