<script>
import Layout from "../../../layouts/main";
import PageHeader from "@/components/page-header";
import BanCaNhan from "./bancanhan.vue";
import LichCaNhan from "./lichcanhan";

export default {
  components: {Layout, PageHeader, LichCaNhan, BanCaNhan},
  data() {
    return {
      title: "Lịch cá nhân ",
      items: [
        {
          text: "Trang chủ",
          herf: "/",
        },
        {
          text: "lịch cá nhân",
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
              Lịch cá nhân
            </a>
          </li>
          <li class="nav-item waves-effect waves-light">
            <a class="nav-link" data-bs-toggle="tab" href="#bang-cong-tac" role="tab" aria-selected="false">
              Bảng cá nhân
            </a>
          </li>
        </ul>
        <!-- Tabs panels -->
        <div class="tab-content text-muted">
          <div class="tab-pane active" id="lich-cong-tac" role="tabpanel">
            <lich-ca-nhan/>
          </div>
          <div class="tab-pane" id="bang-cong-tac" role="tabpanel">
            <ban-ca-nhan/>
          </div>
        </div>
      </div>
    </div>
  </Layout>
</template>

<style></style>
