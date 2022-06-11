<script>
import Layout from "@/layouts/main";
import PageHeader from "@/components/page-header";
import appConfig from "../../../../app.config.json";


export default {
  page: {
    title: "Menu",
    meta: [
      {
        content: appConfig.description,
      },
    ],
  },
  data() {
    return {
      title: "Menu",
      items: [
        {
          text: "Trang chủ",
          href: "/",
        },
        {
          text: "menu",
          active: true,
        },
      ],
    };
  },
  components: {Layout, PageHeader},
  methods: {},
};
</script>

<template>
  <Layout>
    <PageHeader :title="title" :items="items"/>
    <!--  ./ start menu  -->

    <!--  ./ end menu  -->
  </Layout>
</template>
<style>

</style>
