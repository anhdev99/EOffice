<script>
import {FormWizard, TabContent} from "vue-form-wizard";
import Layout from "../../layouts/main";
import PageHeader from "@/components/page-header";
import appConfig from "@/app.config";

import vue2Dropzone from "vue2-dropzone";

export default {
  page: {
    title: "Ký số",
    meta: [{name: "description", content: appConfig.description}],
  },
  components: {Layout, PageHeader, FormWizard, TabContent, vueDropzone: vue2Dropzone,},
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
  methods: {
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
  <Layout>
    <PageHeader :title="title" :items="items"/>
    <div class="row">
      <div class="col-md-7">
        <div class="card">
          <div class="card-body">
            <div class="row">
              <div class="col-md-12">
                <form-wizard
                    color="#00568C"
                    finish-button-text="Ký số"
                    nextButtonText="Chuyển tiếp"
                    prevButtonText="Trở lại"
                    @on-complete="onComplete"
                >
                  <tab-content icon="fas fa-file-upload">
                    <div class="row">
                      <div class="col-12">
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
                                class="display-1 text-muted fas fa-cloud-upload-alt"
                                style="font-size: 70px"
                            ></i>
                            <h4>
                              Kéo thả tệp tin hoặc bấm vào để tải tệp tin
                            </h4>
                          </div>
                        </vue-dropzone>
                      </div>
                      <!-- end col -->
                    </div>
                    <!-- end row -->
                  </tab-content>
                  <tab-content icon="fas fa-pen-alt">
                    <div class="row">
                      <div class="col-12">
                        file thông tin ký số
                      </div>
                      <!-- end col -->
                    </div>
                    <!-- end row -->
                  </tab-content>
                </form-wizard>
                <!--                Modal -->
                <b-modal
                    v-model="showModal"
                    id="modal-lg"
                    size="lg"
                    title="Large modal"
                    title-class="font-18"
                    hide-header
                    hide-footer
                >
                  <template>
                    <div class="text-end">
                      <a @click="showModal = false">
                        <i class="fas fa-times"></i>
                      </a>
                    </div>
                    <div class="title mb-3">
                        <h4>Xác nhận ký tài liệu</h4>
                    </div>
                    <div class="row">
                      <div class="col-md-6">

                      </div>
                      <div class="col-md-6">

                      </div>
                    </div>
                  </template>
                </b-modal>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div class="col-md-5">
        <div class="card">
          <div class="card-body">
            <div class="title mb-4 text-primary">
              <h4>Thông tin tài liệu ký</h4>
            </div>
            <div class="info-doc">
              <div class="row">
                <div class="col-md-3">
                  <p class="fw-bold">Tên file: </p>
                </div>
                <div class="col-md-9">
                  <p v-if="files">{{ files[0].fileName }}</p>
                </div>
              </div>
              <div class="row">
                <div class="col-md-3">
                  <p class="fw-bold">Kích thước: </p>
                </div>
                <div class="col-md-9">
                  <p v-if="files">{{ files[0].size }}</p>
                </div>
              </div>
              <div class="row">
                <div class="col-md-3">
                  <p class="fw-bold">Định dạng: </p>
                </div>
                <div class="col-md-9">
                  <p v-if="files">{{ files[0].ext }}</p>
                </div>
              </div>

            </div>
            <hr>
            <div class="info-process">
              <div class="title mb-4 text-primary">
                <h4>Thông tin chứng thư</h4>
              </div>
              <div class="row mb-1 d-flex justify-content-center">
                <div class="col-md-3">
                  <p class="fw-bold">Cấp cho: </p>
                </div>
                <div class="col-md-9 bg-secondary" style="border-radius: 3px;">
                  <p class="p-1">noi dung</p>
                </div>
              </div>
              <div class="row mb-1">
                <div class="col-md-3">
                  <p class="fw-bold">Được cấp bởi: </p>
                </div>
                <div class="col-md-9 bg-secondary" style="border-radius: 3px;">
                  <p class="p-1">noi dung</p>
                </div>
              </div>
              <div class="row mb-1">
                <div class="col-md-3">
                  <p class="fw-bold">Ngày bất đầu: </p>
                </div>
                <div class="col-md-9 bg-secondary" style="border-radius: 3px;">
                  <p class="p-1">noi dung</p>
                </div>
              </div>
              <div class="row mb-1">
                <div class="col-md-3">
                  <p class="fw-bold">Ngày kết thúc: </p>
                </div>
                <div class="col-md-9 bg-secondary" style="border-radius: 3px;">
                  <p class="p-1">noi dung</p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </Layout>
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
