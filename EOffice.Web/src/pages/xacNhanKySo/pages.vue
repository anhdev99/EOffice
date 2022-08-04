<script>
import appConfig from "@/app.config";
import Multiselect from "vue-multiselect";
import vue2Dropzone from "vue2-dropzone";
import {vanBanDenModel} from "@/models/vanBanDenModel";
import {notifyModel} from "@/models/notifyModel";
import log from "echarts/src/scale/Log";

/**
 * Maintenance page
 */
export default {
  page: {
    title: "Xác nhận ký số",
    meta: [{name: "description", content: appConfig.description}],
  },
  components: {Multiselect, vueDropzone: vue2Dropzone},
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
            this.$swal({
              position: 'center',
              icon: 'success',
              title: 'Xác thực thành công',
              showConfirmButton: false,
              timer: 2000
            })
          }else{
            this.$swal({
              position: 'center',
              icon: 'error',
              title: 'Xác thực không thành công',
              showConfirmButton: false,
              timer: 2000
            })
          }

        })
      } else {
        //Create modelhandleSubmit
        // await this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage({
        //   resultString: "Dữ liệu không đúng",
        //   resultCode: "ERROR"
        // }));
        this.$swal({
          position: 'center',
          icon: 'error',
          title: 'Dữ liệu không đúng',
          showConfirmButton: false,
          timer: 2000
        })
      }
    },
  }
}
</script>
<template>
  <div>
    <section>
      <div class="row ">
        <div class="col-12 text-center bg-xac-nhan">
          <div class="mt-5 mb-3">
            <img
                src="@/assets/images/logo-eoffice.png"
                alt="logo"
                height="90"
                class="text-white logo-eoffice"
            />
          </div>
<!--          <div class="maintenance-img">-->
<!--            <img-->
<!--                src="@/assets/images/DTHU.png"-->
<!--                alt="logo"-->
<!--                height="100"-->
<!--            />-->
<!--          </div>-->
        </div>
      </div>
      <div class="row d-flex justify-content-center">
        <div class="col-md-6 col-xs-12 col-sm-12 p-0" style="margin-top: -400px;">
          <b-card style="border-radius: 10px;">
            <b-card-body>
              <div class="home-wrapper mt-1 text-center">
                <h3 class="mt-4 text-primary">Xác nhận chữ ký số nội bộ.</h3>

                <div class="row d-flex justify-content-center">
                  <div class="text-center col-md-12">
                    <div class="my-3">
                      <div class="d-flex justify-content-start align-items-baseline">
                        <i class="fas fa-user-edit me-1"></i>
                        <h6 class="fw-bold">Cán bộ soạn</h6>
                      </div>
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
                  <div class="text-center col-md-12">
                    <div class="my-3">
                      <div class="d-flex justify-content-start align-items-baseline">
                        <i class="fas fa-cloud-upload-alt me-1"></i>
                        <h6 class="fw-bold">File đính kèm</h6>
                      </div>
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
                  <div class="text-center col-md-8">
                    <div class=" mt-4">
                      <b-button
                          :disabled="checkData"
                          @click="handleSubmit"
                          variant="primary"
                          class="mb-2 w-100"
                          style="border-radius: 99px;"
                      >
                        <i class="fas fa-marker"></i>
                        Xác thực chữ ký
                      </b-button>
                    </div>
                  </div>
                </div>
                <!-- end row -->
              </div>
            </b-card-body>
          </b-card>
        </div>
      </div>
    </section>
  </div>
</template>
<style>
.bg-xac-nhan{
  background: url("../../assets/images/bg_blue.png");
  height: 600px;
  background-repeat: no-repeat;
  background-position: center;
  background-size: cover;
}

@media only screen and (max-width: 320px) {
  .logo-eoffice{
    height: 60px;
    margin-bottom: -80px;
  }
}
</style>
