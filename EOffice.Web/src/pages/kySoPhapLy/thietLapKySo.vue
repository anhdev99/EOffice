<template>

  <Layout>
    <PageHeader :title="title" :items="items"/>
    <div class="row">
      <div class="col-md-12">
        <PdfEditor pdf="https://localhost:5001/api/v1/files/view/62d592720117b32f573a3a8b" />
      </div>
    </div>
  </Layout>
</template>

<script>
import PdfEditor from "@/components/pdfEditor/PdfEditor";
import appConfig from "@/app.config.json";
import Layout from "../../layouts/main";
import PageHeader from "@/components/page-header";
export default {
  name: "thietLapKySo",
  page: {
    title: "Ký số",
    meta: [{name: "description", content: appConfig.description}],
  },
  components:{
    PdfEditor,
    Layout,
    PageHeader
  },
  data() {
    return {
      title: "Ký số",
      items: [
        {
          text: "Trang chủ",
          href: "/",
        },
        {
          text: "Thông tin ký số",
          active: true,
        }
      ]
    };
  },
}
</script>

<style scoped>

</style>