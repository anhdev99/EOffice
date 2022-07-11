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
    title: "Vai",
    meta: [{
      name: "van-ban-den",
      content: appConfig.description
    }],
  },
  data() {
    return{
      title: "Văn bản đến",
      items: [
        {
          text: "Quản lý văn bản đến",
          href: "/",
        },
        {
          text:"Văn bản đến",
          active: true,
        }

      ]
    }
  },
  components: { Layout, PageHeader, }
}
</script>
