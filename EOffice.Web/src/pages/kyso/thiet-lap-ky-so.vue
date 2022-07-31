<script>
import {FormWizard, TabContent} from "vue-form-wizard";
import Layout from "../../layouts/main";
import PageHeader from "@/components/page-header";
import appConfig from "@/app.config";

import vue2Dropzone from "vue2-dropzone";

export default {
  name: "thiet-lap-ky-so",
  data() {
    return {
       //getKySiIframe: "https://signdigital.dthu.edu.vn/home/ThietLapKySo?",
      // getKySiIframe: "https://localhost:5003/ThietLapKySo?",
        getKySiIframe: "https://localhost:44323/home/ThietLapKySo?",
      files: null,
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
        acceptedFiles: ".jpeg,.jpg,.png,.gif,.doc,.docx,.xlsx,.pptx,.pdf",
      },
      showModal: false,
    };
  },
  created() {
    this.loadKySoLocalStoage();
  },
  methods: {
    loadKySoLocalStoage(){
      let dataJSON = localStorage.getItem("kysophaply");
      if(dataJSON){
        let convertData = JSON.parse(dataJSON);
        if(convertData){
          this.getKySiIframe += "fileName=" + convertData.fileName + "&path=" + convertData.path + "&vanBanDiId=" + convertData.vanBanDiId;
        }
      }
      console.log(this.getKySiIframe)
    },
    removeThisFile(file, error, xhr) {
      let fileCongViec = JSON.parse(file.xhr.response);
      if (fileCongViec.data && fileCongViec.data.id) {
        let idFile = fileCongViec.data.id;
        let resultData = this.files.filter(x => {
          return x.fileId != idFile;
        })
        this.files = resultData;
      }
    },
    addThisFile(file, response) {
      console.log("response", response);
      if (this.files == null || this.files.length <= 0) {
        this.files = [];
      }
      let fileSuccess = response.data;
      console.log("file", fileSuccess);
      this.files.push({
        fileId: fileSuccess.id,
        fileName: fileSuccess.fileName,
        ext: fileSuccess.ext,
        size: fileSuccess.size,
      })
    },
    onComplete() {
      this.showModal = true;
    },
  }
}
</script>
<template>
  <div class="row">
    <div class="col-md-12">
      <div class="card">
        <div class="card-body">
          <div class="row">
            <div class="col-12">
              <iframe style="height: 80vh; width: 100%" :src="getKySiIframe"></iframe>
            </div>
            <!-- end col -->
          </div>
        </div>
      </div>
    </div>
    <!--      <div class="col-md-5">-->
    <!--        <div class="card">-->
    <!--          <div class="card-body">-->
    <!--            <div class="title mb-4 text-primary">-->
    <!--              <h4>Thông tin tài liệu ký</h4>-->
    <!--            </div>-->
    <!--            <div class="info-doc">-->
    <!--              <div class="row">-->
    <!--                <div class="col-md-3">-->
    <!--                  <p class="fw-bold">Tên file: </p>-->
    <!--                </div>-->
    <!--                <div class="col-md-9">-->
    <!--                  <p v-if="files">{{ files[0].fileName }}</p>-->
    <!--                </div>-->
    <!--              </div>-->
    <!--              <div class="row">-->
    <!--                <div class="col-md-3">-->
    <!--                  <p class="fw-bold">Kích thước: </p>-->
    <!--                </div>-->
    <!--                <div class="col-md-9">-->
    <!--                  <p v-if="files">{{ files[0].size }}</p>-->
    <!--                </div>-->
    <!--              </div>-->
    <!--              <div class="row">-->
    <!--                <div class="col-md-3">-->
    <!--                  <p class="fw-bold">Định dạng: </p>-->
    <!--                </div>-->
    <!--                <div class="col-md-9">-->
    <!--                  <p v-if="files">{{ files[0].ext }}</p>-->
    <!--                </div>-->
    <!--              </div>-->

    <!--            </div>-->
    <!--            <hr>-->
    <!--            <div class="info-process">-->
    <!--              <div class="title mb-4 text-primary">-->
    <!--                <h4>Thông tin chứng thư</h4>-->
    <!--              </div>-->
    <!--              <div class="row mb-1 d-flex justify-content-center">-->
    <!--                <div class="col-md-3">-->
    <!--                  <p class="fw-bold">Cấp cho: </p>-->
    <!--                </div>-->
    <!--                <div class="col-md-9 bg-secondary" style="border-radius: 3px;">-->
    <!--                  <p class="p-1">noi dung</p>-->
    <!--                </div>-->
    <!--              </div>-->
    <!--              <div class="row mb-1">-->
    <!--                <div class="col-md-3">-->
    <!--                  <p class="fw-bold">Được cấp bởi: </p>-->
    <!--                </div>-->
    <!--                <div class="col-md-9 bg-secondary" style="border-radius: 3px;">-->
    <!--                  <p class="p-1">noi dung</p>-->
    <!--                </div>-->
    <!--              </div>-->
    <!--              <div class="row mb-1">-->
    <!--                <div class="col-md-3">-->
    <!--                  <p class="fw-bold">Ngày bất đầu: </p>-->
    <!--                </div>-->
    <!--                <div class="col-md-9 bg-secondary" style="border-radius: 3px;">-->
    <!--                  <p class="p-1">noi dung</p>-->
    <!--                </div>-->
    <!--              </div>-->
    <!--              <div class="row mb-1">-->
    <!--                <div class="col-md-3">-->
    <!--                  <p class="fw-bold">Ngày kết thúc: </p>-->
    <!--                </div>-->
    <!--                <div class="col-md-9 bg-secondary" style="border-radius: 3px;">-->
    <!--                  <p class="p-1">noi dung</p>-->
    <!--                </div>-->
    <!--              </div>-->
    <!--            </div>-->
    <!--          </div>-->
    <!--        </div>-->
    <!--      </div>-->
  </div>
</template>
<style scoped>
.td-stt {
  text-align: center;
  width: 50px;
}

.td-xuly {
  text-align: center;
  width: 130px;
}

.table > tbody > tr > td {
  padding: 0px;
  line-height: 30px;
}

.hidden-sortable:after, .hidden-sortable:before {
  display: none !important;
}

.divider:before {
  content: "";
  display: block;
  position: absolute;
  height: 2px;
  width: 150px;
  background-color: #00568c;
}
</style>
