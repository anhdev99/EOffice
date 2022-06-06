<script>
import Layout from "../../../layouts/main";
import PageHeader from "@/components/page-header";
import BanCongTacCaNhan from "./bancongtaccanhan.vue";
import LichCongTacCaNhan from "./lichcongtaccanhan";

export default {
  components: {Layout, PageHeader, BanCongTacCaNhan, LichCongTacCaNhan},
  data() {
    return {
      title: " Lịch công tác cá nhân",
      items: [
        {
          text: "Trang chủ",
          herf: "/",
        },
        {
          text: "lịch công tác cá nhân",
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
              Lịch công tác
            </a>
          </li>
          <li class="nav-item waves-effect waves-light">
            <a class="nav-link" data-bs-toggle="tab" href="#bang-cong-tac" role="tab" aria-selected="false">
              Bảng công tác
            </a>
          </li>
        </ul>
        <!-- Tabs panels -->
        <div class="tab-content text-muted">
          <div class="tab-pane active" id="lich-cong-tac" role="tabpanel">
            <lich-cong-tac-ca-nhan/>
          </div>
          <div class="tab-pane" id="bang-cong-tac" role="tabpanel">
            <ban-cong-tac-ca-nhan/>
          </div>
        </div>
      </div>
    </div>
  </Layout>
</template>

<style></style>
