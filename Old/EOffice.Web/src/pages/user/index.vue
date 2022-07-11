<template>
  <Layout>
    <PageHeader :title="title" :items="items"/>
    <div class="row">
      <div class="col-12">
        văn bản đến
      </div>
    </div>
  </Layout>
</template>
<script>

import appConfig from "../../../app.config.json";
import Layout from "@/layouts/main";
import PageHeader from "@/components/page-header";

export default{
  page: {
    title: "Người dùng",
    meta: [{
      name: "nguoi-dung",
      content: appConfig.description
    }],
  },
  data() {
    return{
      title: "Người dùng",
      items: [
        {
          text: "Người dùng",
          href: "/"
        },
        {
          text:"Danh sách",
          active: true
        }

      ]
    }
  },
  components: { Layout, PageHeader, }
}
</script>
