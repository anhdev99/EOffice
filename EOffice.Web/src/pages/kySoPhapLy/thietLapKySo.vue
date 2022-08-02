<template>

  <Layout>
    <PageHeader :title="title" :items="items"/>
    <div class="row">
      <div class="col-md-4">
        <label for="">Tải tệp tin cần ký số</label>
        <div style="color: red">Lưu ý: Tệp tin pdf </div>
        <vue-dropzone
            id="dropzone"
            ref="myVueDropzone"
            :use-custom-slot="true"
            :options="dropzoneOptions"
            v-on:vdropzone-removed-file="removeThisFile"
            v-on:vdropzone-success="addThisFile"
        >
          <div class="dropzone-custom-content">
            <i
                class="display-1 text-muted bx bxs-cloud-upload"
                style="font-size: 70px"
            ></i>
            <h4>
              Kéo thả tệp tin hoặc bấm vào để tải tệp tin
            </h4>
          </div>
        </vue-dropzone>
      </div>
      <div class="col-md-12" v-if="pathUrl">
        <PdfEditor :file="infoFile" :pdf="`${pathUrl}`" />
      </div>


    </div>
  </Layout>
</template>

<script>
import PdfEditor from "@/components/pdfEditor/PdfEditor";
import appConfig from "@/app.config.json";
import Layout from "../../layouts/main";
import PageHeader from "@/components/page-header";
import vue2Dropzone from "vue2-dropzone";
import {vanBanDenModel} from "@/models/vanBanDenModel";
import {notifyModel} from "@/models/notifyModel";
export default {
  name: "thietLapKySo",
  page: {
    title: "Ký số",
    meta: [{name: "description", content: appConfig.description}],
  },
  components:{
    PdfEditor,
    Layout,
    PageHeader,
    vueDropzone: vue2Dropzone,
  },
  computed:{
    pathUrl (){
      if(this.files && this.files.length > 0){
        if(this.files[0]){
          return this.apiUrl + 'files/view/' + this.files[0].fileId
        }

      }
      return null;
    },
    infoFile(){
      if(this.files){
        if(this.files[0])
          return this.files[0];
      }
      return null
    }
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
      ],
      apiUrl: process.env.VUE_APP_API_URL,
      url: `${process.env.VUE_APP_API_URL}files/upload`,
      dropzoneOptions: {
        url: `${process.env.VUE_APP_API_URL}files/upload`,
        thumbnailWidth: 300,
        thumbnailHeight: 160,
        maxFiles: 1,
        maxFilesize: 30,
        headers: {"My-Awesome-Header": "header value"},
        addRemoveLinks: true,
        acceptedFiles: ".pdf",
      },
      files: null,
    };
  },
  methods:{
    removeThisFile(file, error, xhr) {
      this.files = null;
    },
    addThisFile(file, response) {
      if (this.files == null || this.files.length <= 0) {
        this.files = [] ;
      }
      let fileSuccess = response.data;
      console.log("file", fileSuccess);

      this.files.push(
          {
            fileId: fileSuccess.id,
            fileName: fileSuccess.fileName,
            ext: fileSuccess.ext,
            size: fileSuccess.size,
          }
      );
      console.log("file", this.files);
    },

  }
}
</script>

<style scoped>

</style>