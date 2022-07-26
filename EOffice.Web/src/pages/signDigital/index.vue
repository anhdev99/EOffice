<script>
import Layout from "../../layouts/main";
import PageHeader from "@/components/page-header";
import appConfig from "@/app.config";

export default {
  page: {
    title: "Ký số pháp lý",
    meta: [{name: "description", content: appConfig.description}],
  },
  components: {Layout, PageHeader},
  data() {
    return {
      title: "Lĩnh vực",
    };
  },
}
</script>
<template>
  <Layout>
    <PageHeader :title="title" :items="items"/>
    <div class="row">

      <iframe style="height: 100vh" width="800" src="https://signdigital.dthu.edu.vn/"></iframe>
        </div>
  </Layout>
</template>

