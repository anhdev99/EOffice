<script>
import Layout from "../../layouts/main";
import PageHeader from "@/components/page-header";
import appConfig from "@/app.config";
import Multiselect from "vue-multiselect";
import vue2Dropzone from "vue2-dropzone";
import {vanBanDenModel} from "@/models/vanBanDenModel";
import {notifyModel} from "@/models/notifyModel";
import log from "echarts/src/scale/Log";

export default {
  page: {
    title: "Xác nhận ký số",
    meta: [{name: "description", content: appConfig.description}],
  },
  components: {Layout, PageHeader, Multiselect, vueDropzone: vue2Dropzone},
  data(){
    return {
      title: "Xác nhận ký số",
      items: [
        {
          text: "Ký số",
          //href: "/nhom-quan-ly-du-an",
          active: true,
        },
        {
          text: "Xác nhận ký số",
          active: true,
        }
      ],
      data: [],
      model:{user: null,uploadFiles: null },
      optionsUser: [],
      dropzoneOptions: {
        url: `${process.env.VUE_APP_API_URL}files/upload`,
        thumbnailWidth: 300,
        thumbnailHeight: 160,
        maxFiles: 4,
        maxFilesize: 10,
        headers: {"My-Awesome-Header": "header value"},
        addRemoveLinks: true,
        acceptedFiles: ".pdf",
      },
      checkData: true
    }
  },
  validations: {

  },
    watch:{
      model: {
        deep: true,
        handler(val) {
          if(val && val.user != null && val.uploadFiles != null){
            this.checkData = false;
          }else{
            this.checkData = true;
          }
        }
      },
  },
  created() {
    this.getUser();
  },
  methods: {
    removeThisFile(file, error, xhr) {
      let fileCongViec = JSON.parse(file.xhr.response);
      if (fileCongViec.data && fileCongViec.data.id) {
        let idFile = fileCongViec.data.id;
        let resultData = this.model.uploadFiles.filter(x => {
          return x.fileId != idFile;
        })
        this.model.uploadFiles = resultData;
      }
    },
    addThisFile(file, response) {
      if (this.model) {
        if (this.model.uploadFiles == null || this.model.uploadFiles.length <= 0) {
          this.model.uploadFiles = [];
        }
        let fileSuccess = response.data;
        this.model.uploadFiles.push({fileId: fileSuccess.id, fileName: fileSuccess.fileName, ext: fileSuccess.ext})
      }
    },
    async getUser() {
        await this.$store.dispatch("userStore/getAll").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.optionsUser = items;
          }
          return [];
        });
    },
    async handleSubmit(e) {
      if (
          this.model != null
      ) {
        //Update model
        await this.$store.dispatch("vanBanDiStore/xacThuc", this.model).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            console.log("");
          }
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        })
      } else {
        //Create modelhandleSubmit
        await this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage({
          resultString: "Dữ liệu không đúng",
          resultCode: "ERROR"
        }));
      }
    },
  }
}
</script>
<template>
  <Layout>
    <PageHeader :title="title" :items="items" />
    <div class="row">
      <div class="col-xl-12">
        <div class="card">
          <div class="card-body">
            <b-button :disabled="checkData" @click="handleSubmit" variant="primary" class="mb-2">Xác thực chữ ký</b-button>
            <div class="col-md-6">
              <div class="mb-2">
                <label class="form-label" for="validationCustom01"> Cán bộ soạn</label>
                <multiselect
                    v-model="model.user"
                    :options="optionsUser"
                    track-by="id"
                    label="fullName"
                    placeholder="Chọn cán bộ soạn"
                    deselect-label="Nhấn để xoá"
                    selectLabel="Nhấn enter để chọn"
                    selectedLabel="Đã chọn"
                >
                  <template slot="singleLabel" slot-scope="{ option }">
                    <strong>{{ option.fullName }}</strong>

                    <span v-if="option.donVi" style="color:red">&nbsp;{{ option.donVi.ten }}</span>
                  </template>
                  <template slot="option" slot-scope="{ option }">
                    <div class="option__desc">
          <span class="option__title">
            <strong>{{ option.fullName }}&nbsp;</strong>
          </span>
                      <span v-if="option.donVi" class="option__small" style="color:green">{{ option.donVi.ten }}</span>
                    </div>
                  </template>
                </multiselect>
              </div>
            </div>
            <div class="col-md-6">
              <label for="">File đính kèm</label>
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
          </div>
        </div>
      </div>
    </div>
  </Layout>
</template>
