<script>
import Layout from "../../layouts/main";
import PageHeader from "@/components/page-header";
import appConfig from "@/app.config";

export default {
  page: {
    title: "Danh mục Menu",
    meta: [{name: "description", content: appConfig.description}],
  },
  components: {},
  data() {
    return {
    };
  },
};
</script>

<template>
  <Layout>
    <PageHeader :title="title" :items="items"/>
    <div class="row">
   abc
    </div>
  </Layout>
</template>
