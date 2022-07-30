<script>
import Layout from "../../layouts/main";
import PageHeader from "@/components/page-header";
import appConfig from "@/app.config";
import {pagingModel} from "@/models/pagingModel";
import {required} from "vuelidate/lib/validators";
import {vanBanDenModel} from "@/models/vanBanDenModel";
import Multiselect from "vue-multiselect";
import DatePicker from "vue2-datepicker";
import Switches from "vue-switches";

/**
 * Form editor
 */
import CKEditor from "@ckeditor/ckeditor5-vue";
import ClassicEditor from "@ckeditor/ckeditor5-build-classic";
import vue2Dropzone from "vue2-dropzone";
import {butPheModel} from "@/models/butPheModel";
import {phanCongModel} from "@/models/phanCongModel";
import {notifyModel} from "@/models/notifyModel";
import {vanBanDiModel} from "@/models/vanBanDiModel";
import {trangThaiModel} from "@/models/trangThaiModel";
import {CURRENT_USER} from "@/helpers/currentUser";

/**
 * Advanced table component
 */
export default {
  page: {
    title: "Văn bản đi",
    meta: [{name: "description", content: appConfig.description}]
  },
  components: {
    Layout,
    PageHeader,
    Multiselect,
    ckeditor: CKEditor.component,
    Switches,
    DatePicker,
    vueDropzone: vue2Dropzone
  },
  data() {
    return {
      title: "Văn bản đi",
      items: [
        {
          text: "E-Office",
          href: "/"
        },
        {
          text: "Văn bản đi",
          href: "/van-ban-den"
        },
        {
          text: "Danh sách",
          active: true
        }
      ],
      data: [],
      showModal: false,
      showDetail: false,
      showDeleteModal: false,
      submitted: false,
      listCoQuan: [],
      listRole: [],
      showModalMembers: false,
      showTrangThaiModal: false,
      showCheckVanBanModal: false,
      model: vanBanDiModel.baseJson(),
      modelKySo: {
        choPhepKy: true,
        nguoiKy: null,
        vanBanDiId: null,
        thuTu: 0
      },
      modelButPhe: butPheModel.baseJson(),
      modelPhanCong: phanCongModel.baseJson(),
      pagination: pagingModel.baseJson(),
      totalRows: 1,
      todoTotalRows: 1,
      currentPage: 1,
      numberOfElement: 1,
      perPage: 10,
      pageOptions: [5, 10, 25, 50, 100],
      filter: null,
      todoFilter: null,
      filterOn: [],
      todofilterOn: [],
      isBusy: false,
      sortBy: "age",
      sortDesc: false,
      fields: [
        {
          key: 'STT',
          label: 'STT',
          thStyle: {width: '30px', minWidth: '30px'},
          class: "text-center"
        },
        {
          key: "soLuuCV",
          label: "Số lưu CV",
          thStyle: {width: '150px', minWidth: '100px'},
          class: "px-1",
          sortable: false ,
        },
        {
          key: "soVBDi",
          label: "Số CV đi",
          thStyle: {width: '150px', minWidth: '100px'},
          class: "px-1",
          sortable: false ,
        },
        {
          key: "trichYeu",
          label: "Trích yếu",
          thStyle: {width: 'auto', minWidth: 'auto'},
          class: "px-1",
          sortable: false ,
        },
        {
          key: "loaiVanBan",
          label: "Loại văn bản",
          thStyle: {width: '150px', minWidth: '100px'},
          class: "text-center",
          sortable: false ,

        },
        {
          key: "trangThai",
          label: "Trạng thái",
          thStyle: {width: '150px', minWidth: '100px'},
          class: "text-center",
          sortable: false ,
        },
        {
          key: "ngayNhap",
          label: "Ngày nhập",
          thStyle: {width: '100px', minWidth: '100px'},
          class: "text-center"
        },
        {
          key: 'chuyenTrangThai',
          label: '',
          thStyle: {width: '110px', minWidth: '110px'},
          class: "text-center",
          sortable: false ,
        },
        {
          key: 'process',
          label: '',
          thStyle: {width: '140px', minWidth: '100px'},
          class: "text-center",
          sortable: false ,
        }
      ],
      optionsLoaiVanBan: [],
      optionsDonVi: [],
      optionsLinhVuc: [],
      optionsUser: [],
      optionsHinhThucNhan: [],
      optionsMucDo: [],
      optionsTrangThai: [],
      listPhanCongKySo: [],
      editor: ClassicEditor,
      editorConfig: {
        height: '200px'
      },
      apiUrl: process.env.VUE_APP_API_URL,
      url: `${process.env.VUE_APP_API_URL}files/upload`,
      urlView: `${process.env.VUE_APP_API_URL}files/view/`,
      dropzoneOptions: {
        url: `${process.env.VUE_APP_API_URL}files/upload`,
        thumbnailWidth: 300,
        thumbnailHeight: 160,
        maxFiles: 4,
        maxFilesize: 30,
        headers: {"My-Awesome-Header": "header value"},
        addRemoveLinks: true,
        acceptedFiles: ".doc,.docx,.pdf",
      },
      showModalButPhe: false,
      showModalPhanCong: false,
      phanCong: [{id: 1}],
      currentStatus: null,
      modelTrangThai: trangThaiModel.currentBaseJson(),
      currentUserName: CURRENT_USER.USERNAME,
      showHistoryModal: false,
      listHistoryQuestion: [],
      noiDungTuChoi: null,
      showModalNoiDungTuChoi : false
    };
  },
  validations: {
    model: {
      loaiVanBan: {required},
      trangThai: {required},
    },
    modelTrangThai: {
      currentTrangThai: {required},
      newTrangThai: {required},
      userName: {required},
    }
  },

  async created() {
    this.getLoaiVanBan();

    this.getDonVi();
    this.getUser();
    this.getLinhVuc();
    this.getHinhThuc();
    this.getMucDo();
  },
  mounted() {
    // Set the initial number of items
    this.totalRows = this.items.length;
  },
  methods: {
    /**
     * Search the table data with search input
     */
    async fnGetList() {
      await this.onPageChange();
    },
    async onPageChange(page = 1) {
      // this.pagination.currentPage = page;
      // const params = {
      //   pageNumber: this.pagination.currentPage,
      //   pageSize: this.pagination.pageSize,
      // }
      this.$refs.tblList?.refresh()
    },
    onFiltered(filteredItems) {
      // Trigger pagination to update the number of buttons/pages due to filtering
      this.totalRows = filteredItems.length;
      this.currentPage = 1;
    },

    async handleSubmit(e) {
      e.preventDefault();
      this.submitted = true;
      this.$v.$touch();
      if (this.$v.model.$invalid) {
        return;
      } else {
        let loader = this.$loading.show({
          container: this.$refs.formContainer,
        });
        if (
            this.model.id != 0 &&
            this.model.id != null &&
            this.model.id
        ) {
          //Update model
          await this.$store.dispatch("vanBanDiStore/update", this.model).then((res) => {
            if (res.resultCode === 'SUCCESS') {
              this.showModal = false;
              this.model = vanBanDenModel.baseJson()
              this.$refs.tblList.refresh();
            }
            this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
          })
        } else {
          //Create modelhandleSubmit
          this.model.version = 1;
          await this.$store.dispatch("vanBanDiStore/create", this.model).then((res) => {
            if (res.resultCode === 'SUCCESS') {
              this.showModal = false;
              this.model = vanBanDenModel.baseJson()
              this.$refs.tblList.refresh();
            }
            this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
          });
        }
        loader.hide();
      }
      this.submitted = false;

    },
    async handleHistory(id) {
      let loader = this.$loading.show({
        container: this.$refs.formContainer,
      });
      await this.$store.dispatch("historyVanBanStore/getHistoryVanBanDi", id).then((res) => {
        if (res.resultCode == "SUCCESS") {
          this.listHistoryQuestion = res.data;
          this.showHistoryModal = true;

          loader.hide();
        }
      });
    },
    async handleUpdate(id) {
      let loader = this.$loading.show({
        container: this.$refs.formContainer,
      });
      await this.$store.dispatch("vanBanDiStore/getById", id).then((res) => {
        if (res.resultCode == "SUCCESS") {
          this.model = res.data;
          this.showModal = true;
          this.getTrangThai(this.model.trangThai)
          loader.hide();
        } else {
          // this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        }
      });
    },
    async handleShowNoiDungTuChoi(value) {
        this.noiDungTuChoi = value;
        this.showModalNoiDungTuChoi = true;

    },
    async handleCreate() {
      await this.$store.dispatch("vanBanDiStore/getVaCapSo").then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.model = res.data;
          this.showModal = true;
        }
      });
      this.getTrangThai(null);
    },
    handleShowDeleteModal(id) {
      this.model.id = id;
      this.showDeleteModal = true;
    },
    handleDeleteFile(index) {
      if (this.model) {
        if (this.model.file) {
          this.model.file.splice(index, 1);
        }

      }
    },
    async handleDelete() {
      if (this.model.id != 0 && this.model.id != null && this.model.id) {
        await this.$store.dispatch("vanBanDiStore/delete", this.model.id).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            this.showDeleteModal = false;
            this.$refs.tblList.refresh();
          }
        });
      }
    },
    handleDetail(id) {

    },
    HandleShowPhanCong(id) {
      this.model.id = id;
      this.showModalPhanCong = true;

    },
    handleShowButPhe(id) {
      this.model.id = id;
      this.showModalButPhe = true;
    },
    async getLoaiVanBan() {
      await this.$store.dispatch("loaiVanBanStore/getAll").then((res) => {
        if (res.resultCode === "SUCCESS") {
          this.optionsLoaiVanBan = res.data;
        } else {
          this.optionsLoaiVanBan = [];
        }
      });
    },
    async getTrangThai(currentTrangThai) {
      try {
        await this.$store.dispatch("trangThaiStore/getNextTrangThai", {
          loaiTrangThai: "VBDi",
          currentTrangThai: currentTrangThai
        }).then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data;
            this.loading = false
            this.optionsTrangThai = items;
            console.log(   this.optionsTrangThai)
            return items || []
          }
          return [];
        })
      } finally {
        this.loading = false
      }
    },
    async getPhanCongKySoByVanBanId(id) {
      // this.modelKySo = null;
      try {
        await this.$store.dispatch("vanBanDiStore/getPhanCongKySoByVanBanId", id).then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data;
            this.loading = false
            this.listPhanCongKySo = items || [];
            return items || []
          }
          return [];
        })
      } finally {
        this.loading = false
      }
    },
    async getDonVi() {
      try {
        await this.$store.dispatch("donViStore/get").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionsDonVi = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },
    async getUser() {
      try {
        await this.$store.dispatch("userStore/getAll").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionsUser = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },
    async getLinhVuc() {
      try {
        await this.$store.dispatch("dmLinhVucStore/get").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionsLinhVuc = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },
    async getHinhThuc() {
      try {
        let promise = this.$store.dispatch("hinhThucGuiStore/get")
        return promise.then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            console.log("hinhthucnhan", items);
            this.loading = false
            this.optionsHinhThucNhan = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },
    async getMucDo() {
      await this.$store.dispatch("enumStore/getMucDo").then((res) => {
        if (res.resultCode === "SUCCESS") {
          console.log("resMucDo", res.data);
          this.optionsMucDo = res.data;
        } else {
          this.optionsMucDo = [];
        }
      });
    },
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
    removeCommentFile(file, error, xhr) {
      let fileCongViec = JSON.parse(file.xhr.response);
      if (fileCongViec.data && fileCongViec.data.id) {
        let idFile = fileCongViec.data.id;
        let resultData = this.model.uploadFiles = null;
        this.model.uploadFiles = resultData;
      }
    },
    addCommentFile(file, response) {
      if (this.model) {
        if (this.model.uploadFiles == null || this.model.length <= 0) {
          this.model.uploadFiles = [];
        }
        let fileSuccess = response.data;
        this.model.uploadFiles = {fileId: fileSuccess.id, fileName: fileSuccess.fileName, ext: fileSuccess.ext}
      }
    },
    normalizer(node) {
      if (node.children == null || node.children == 'null') {
        delete node.children;
      }
    },
    AddformData() {
      this.phanCong.push({yKienChiDao: null, nguoiButPhe: null, nguoiNhanXuLy: null, file: null});
    },
    deleteRow(index) {
      if (confirm("Bạn có chắc muốn xoá không?"))
        this.phanCong.splice(index, 1);
    },
    myProvider(ctx) {
      const params = {
        start: ctx.currentPage,
        limit: ctx.perPage,
        content: this.filter,
        sortBy: ctx.sortBy,
        sortDesc: ctx.sortDesc,
      }
      this.loading = true

      try {
        let promise = this.$store.dispatch("vanBanDiStore/getPagingParams", params)
        return promise.then(resp => {
          let items = resp.data.data
          this.totalRows = resp.data.totalRows
          this.numberOfElement = resp.data.data.length
          this.loading = false
          return items || []
        })
      } finally {
        this.loading = false
      }
    },
    async handleAssignSign(e) {
      e.preventDefault();
      if (
          this.modelKySo.vanBanDiId != 0 &&
          this.modelKySo.vanBanDiId != null &&
          this.modelKySo.vanBanDiId
      ) {
        //Update model
        await this.$store.dispatch("vanBanDiStore/assignSign", this.modelKySo).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            this.showModal = false;
            this.getPhanCongKySoByVanBanId(this.modelKySo.vanBanDiId);
            this.modelKySo.nguoiKy = null
          }
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        })
      }
    },
    async handleRemoveAssignSign(userName) {
      if (
          this.modelKySo.vanBanDiId != 0 &&
          this.modelKySo.vanBanDiId != null &&
          this.modelKySo.vanBanDiId
      ) {
        this.modelKySo.nguoiKy = {userName: userName};
        //Update model
        await this.$store.dispatch("vanBanDiStore/removeAssignSign", this.modelKySo).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            this.showModal = false;
            this.getPhanCongKySoByVanBanId(this.modelKySo.vanBanDiId);
            this.modelKySo.nguoiKy = null
          }
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        })
      }
    },
    handleChuyenTrangThai: function(currentStatus, vanBanDiId){
      this.getTrangThai(currentStatus)
      this.modelTrangThai.currentTrangThai = currentStatus;
      this.modelTrangThai.newTrangThai = null;
      this.modelTrangThai.vanBanDiId = vanBanDiId;
      this.modelTrangThai.userName = this.currentUserName;
      this.showTrangThaiModal = true;
    },
    async handleChuyenTrangThaiVanBan(value) {
      console.log("value",value)
      if(value){
        this.modelTrangThai.newTrangThai = value;
      }
      this.submitted = true;
      this.$v.$touch();
      if (this.$v.modelTrangThai.$invalid) {
        return;
      } else {
        let loader = this.$loading.show({
          container: this.$refs.formContainer,
        });
        if (
            this.modelTrangThai
            && this.modelTrangThai.vanBanDiId != null
        ) {
          //Update model
          await this.$store.dispatch("vanBanDiStore/chuyenTrangThaiVanBan", this.modelTrangThai).then((res) => {
            if (res.resultCode === 'SUCCESS') {
              this.showTrangThaiModal = false;
              this.showCheckVanBanModal = false;
              this.model = trangThaiModel.currentBaseJson()
              this.$refs.tblList.refresh();
            }
            this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
          })
        }
        loader.hide();
      }
      this.submitted = false;

    },
    async handleCheckVanBanModal(id) {
      await this.$store.dispatch("vanBanDiStore/getById", id).then((res) => {
        if (res.resultCode == "SUCCESS") {
          this.model = res.data;
          this.getTrangThai(this.model.trangThai)
          this.modelTrangThai.currentTrangThai = this.model.trangThai;
          this.modelTrangThai.newTrangThai = null;
          this.modelTrangThai.vanBanDiId =  this.model.id;
          this.modelTrangThai.userName = this.currentUserName;
          this.showCheckVanBanModal = true;
        } else {
          // this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        }
      });
    },
  }
};
</script>

<template>
  <Layout>
    <PageHeader :title="title" :items="items"/>
    <div class="row">
      <div class="col-12">
        <div class="card">
          <div class="card-body">
            <div class="row mb-2">
              <div class="col-sm-4">
                <div class="search-box me-2 mb-2 d-inline-block">
                  <div class="position-relative">
                    <input
                        v-model="filter"
                        type="text"
                        class="form-control"
                        placeholder="Tìm kiếm ..."
                    />
                    <i class="bx bx-search-alt search-icon"></i>
                  </div>
                </div>
              </div>
              <div class="col-sm-8">
                <div class="text-sm-end">
                  <b-button
                      variant="primary"
                      type="button"
                      class="btn w-md btn-primary"
                      @click="handleCreate"
                      size="sm"
                  >
                    <i class="mdi mdi-plus me-1"></i> Thêm mới
                  </b-button>
                  <!-- Model create -->
                  <b-modal
                      v-model="showModal"
                      title="Tiếp nhận văn bản đi"
                      title-class="text-black font-18"
                      body-class="p-3"
                      hide-footer
                      centered
                      no-close-on-backdrop
                      size="xl"
                  >
                    <template #modal-header="{ close }">
                      <!-- Emulate built in modal header close button action -->
                      <h5> Văn bản đi</h5>
                      <div class="text-end">
                        <b-button variant="light" size="sm" style="width: 80px" @click="showModal = false">
                          Đóng
                        </b-button>
                        <b-button type="submit" variant="primary" class="ms-1" style="width: 80px" size="sm"
                                  @click="handleSubmit"
                        >
                          Lưu
                        </b-button>
                      </div>
                    </template>
                    <form @submit.prevent="handleSubmit"
                          ref="formContainer">
                      <div class="row">
                        <div class="col-md-7">
                          <div class="row">
                            <!--                            Loại văn bản-->
                            <div class="col-md-6">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01">Loại văn bản</label> <span
                                  class="text-danger">*</span>
                                <multiselect
                                    v-model="model.loaiVanBan"
                                    :options="optionsLoaiVanBan"
                                    track-by="id"
                                    label="ten"
                                    placeholder="Chọn loại văn bản"
                                    :class="{
                                'is-invalid':
                                  submitted && $v.model.loaiVanBan.$error,
                                }"
                                ></multiselect>
                                <div
                                    v-if="submitted && !$v.model.loaiVanBan.required"
                                    class="invalid-feedback"
                                >
                                  Loại văn bản không được để trống.
                                </div>
                              </div>
                            </div>
                            <!--                            Trạng thái-->
                            <div class="col-md-6">

                              <div v-if="model.trangThai && model.id" class="mb-2">
                                <label class="form-label" for="validationCustom01">Trạng thái</label> <span
                                  class="text-danger">*</span>
                                <input
                                    id="validationCustom01"
                                    :value="model.trangThai.ten"
                                    type="text"
                                    class="form-control"
                                    placeholder=""
                                    disabled
                                />
                              </div>

                              <div v-else class="mb-2">
                                <label class="form-label" for="validationCustom01">Trạng thái</label> <span
                                  class="text-danger">*</span>
                                <multiselect
                                    v-model="model.trangThai"
                                    :options="optionsTrangThai"
                                    track-by="id"
                                    label="ten"
                                    placeholder="Chọn trạng thái"
                                    :class="{
                                'is-invalid':
                                  submitted && $v.model.trangThai.$error,
                                }"
                                ></multiselect>
                                <div
                                    v-if="submitted && !$v.model.trangThai.required"
                                    class="invalid-feedback"
                                >
                                  Trạng thái không được để trống.
                                </div>
                              </div>
                            </div>
                            <!--                              Số lưu -->
                            <div class="col-md-4">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01">Số lưu CV</label>
                                <!--                                <span-->
                                <!--                                  class="text-danger">*</span>-->
                                <input
                                    id="validationCustom01"
                                    v-model="model.soLuuCV"
                                    type="text"
                                    class="form-control"
                                    placeholder=""
                                />
                              </div>
                            </div>
                            <!--                            Số VB đến -->
                            <div class="col-md-4">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01">Số CV đi</label>
                                <!--                                <span-->
                                <!--                                  class="text-danger">*</span>-->
                                <input
                                    id="validationSoVBDen"
                                    v-model="model.soVBDi"
                                    type="text"
                                    class="form-control"
                                    placeholder=""

                                />
                              </div>
                            </div>
                            <!--                            Ngày nhận -->
                            <div class="col-md-4">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01">Ngày nhập</label>
                                <!--                                <date-picker-->
                                <!--                                    v-model="model.ngayNhap"-->
                                <!--                                    format="DD/MM/YYYY"-->
                                <!--                                    :first-day-of-week="1"-->
                                <!--                                    lang="en"-->
                                <!--                                    placeholder="Chọn ngày nhập"-->
                                <!--                                ></date-picker>-->
                                <date-picker v-model="model.ngayNhap"
                                             format="DD/MM/YYYY"
                                             value-type="format"
                                >
                                  <div slot="input">
                                    <input v-model="model.ngayNhap"
                                           v-mask="'##/##/####'" type="text" class="form-control"
                                           placeholder="Nhập ngày nhập"/>
                                  </div>
                                </date-picker>
                              </div>
                            </div>
                            <!--                              Số lưu -->
                            <div class="col-md-4">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01"> Trả lời CV số</label>
                                <!--                                <span-->
                                <!--                                  class="text-danger">*</span>-->
                                <input
                                    id="validationCustom01"
                                    v-model="model.traLoiCVSo"
                                    type="text"
                                    class="form-control"
                                    placeholder=""
                                />
                              </div>
                            </div>
                            <!--                            Ngày nhận -->
                            <div class="col-md-4">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01"> Ngày trả lời</label>
                                <date-picker v-model="model.ngayTraLoi"
                                             format="DD/MM/YYYY"
                                             value-type="format"
                                >
                                  <div slot="input">
                                    <input v-model="model.ngayTraLoi"
                                           v-mask="'##/##/####'" type="text" class="form-control"
                                           placeholder="Nhập ngày trả lời"/>
                                  </div>
                                </date-picker>
                              </div>
                            </div>
                            <!--                            Số VB đến -->
                            <div class="col-md-4">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01"> Số bản</label>
                                <!--                                <span-->
                                <!--                                  class="text-danger">*</span>-->
                                <input
                                    id="validationSoVBDen"
                                    v-model="model.soBan"
                                    type="number"
                                    class="form-control"
                                    placeholder=""

                                />
                              </div>
                            </div>
                            <div class="col-md-6">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01"> Người ký</label>
                                <multiselect
                                    v-model="model.nguoiKy"
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
                                      <span v-if="option.donVi" class="option__small"
                                            style="color:green">{{ option.donVi.ten }}</span>
                                    </div>
                                  </template>
                                </multiselect>
                              </div>
                            </div>
                            <!--                            Ngày Ký-->
                            <div class="col-md-6">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01">Ngày ký</label>
                                <date-picker v-model="model.ngayKy"
                                             format="DD/MM/YYYY"
                                             value-type="format"
                                >
                                  <div slot="input">
                                    <input v-model="model.ngayKy"
                                           v-mask="'##/##/####'" type="text" class="form-control"
                                           placeholder="Nhập ngày ký"/>
                                  </div>
                                </date-picker>
                              </div>
                            </div>
                            <!--                            Trích yếu -->
                            <div class="col-md-12">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01">Trích yếu</label>
                                <!--                                <span-->
                                <!--                                  class="text-danger">*</span>-->
                                <ckeditor
                                    v-model="model.trichYeu"
                                    :editor="editor"
                                    :config="editorConfig"
                                ></ckeditor>
                              </div>
                            </div>
                            <div v-if="model.filePDF != null  && model.filePDF.length > 0" class="col-md-12">
                              <label for="">Danh sách đã ký (Nhấn vào để tải xuống)</label>
                              <template>
                                <div v-for="(file, index) in model.filePDF" :key="index">
                                  <a
                                      :href="`${apiUrl}files/view/${file.fileId}`"
                                      class=" fw-medium"
                                  ><i
                                      :class="`mdi font-size-16 align-middle me-2`"
                                  ></i>
                                    {{ index + 1 }}: {{ file.fileName }}</a
                                  >
                                </div>
                              </template>

                            </div>
                            <div class="col-md-12">
                              <label for="">Danh sách tệp tin (Nhấn vào để tải xuống)</label>
                              <template v-if="model.file == null || (model.file != null &&model.file.length <= 0)">
                                <div>Không có tệp tin</div>
                              </template>
                              <template v-else>
                                <div v-for="(file, index) in model.file" :key="index">
                                  <a
                                      :href="`${apiUrl}files/view/${file.fileId}`"
                                      class=" fw-medium"
                                  ><i
                                      :class="`mdi font-size-16 align-middle me-2`"
                                  ></i>
                                    {{ index + 1 }}: {{ file.fileName }}</a
                                  >
                                  <button
                                      type="button"
                                      size="sm"
                                      class="btn btn-outline btn-sm"
                                      data-toggle="tooltip" data-placement="bottom" title="Xóa"
                                      v-on:click="handleDeleteFile(index)">
                                    <i class="fas fa-trash-alt text-danger me-1"></i>
                                  </button>
                                </div>
                              </template>

                            </div>

                            <!--                            file đính kèm-->
                            <div class="col-md-12">
                              <label for="">Tệp tin đính kèm</label>
                              <div style="color: red">Lưu ý: Tệp tin pdf hoặc word</div>
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

                        <div class="col-md-5">
                          <div class="row">
                            <div class="col-md-12">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01"> Đơn vị soạn</label>
                                <multiselect
                                    v-model="model.donViSoan"
                                    :options="optionsDonVi"
                                    track-by="id"
                                    label="ten"
                                    placeholder="Chọn đơn vị soạn"
                                    deselect-label="Nhấn để xoá"
                                    selectLabel="Nhấn enter để chọn"
                                    selectedLabel="Đã chọn"
                                ></multiselect>

                              </div>
                            </div>
                            <!--                            Cơ quan gửi-->
                            <div class="col-md-12">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01"> Cán bộ soạn</label>
                                <multiselect
                                    v-model="model.canBoSoan"
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
                                      <span v-if="option.donVi" class="option__small"
                                            style="color:green">{{ option.donVi.ten }}</span>
                                    </div>
                                  </template>
                                </multiselect>
                              </div>
                            </div>
                            <!--                            Hình thức nhận -->
                            <div class="col-md-6">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01">Hình thức nhận</label>
                                <multiselect
                                    v-model="model.hinhThucGui"
                                    :options="optionsHinhThucNhan"
                                    track-by="id"
                                    label="ten"
                                    placeholder="Chọn hình thức nhận"
                                    deselect-label="Nhấn để xoá"
                                    selectLabel="Nhấn enter để chọn"
                                    selectedLabel="Đã chọn"
                                ></multiselect>
                              </div>
                            </div>
                            <!--                            Lĩnh vực-->
                            <div class="col-md-6">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01">Lĩnh vực</label>
                                <multiselect
                                    v-model="model.linhVuc"
                                    :options="optionsLinhVuc"
                                    track-by="id"
                                    label="ten"
                                    placeholder="Chọn lĩnh vực"
                                    deselect-label="Nhấn để xoá"
                                    selectLabel="Nhấn enter để chọn"
                                    selectedLabel="Đã chọn"
                                ></multiselect>
                              </div>
                            </div>
                            <!--                            Mức độ tính chất-->
                            <div class="col-md-6">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01">Mức độ tính chất</label>
                                <multiselect
                                    v-model="model.mucDoTinhChat"
                                    :options="optionsMucDo"
                                    track-by="id"
                                    label="name"
                                    placeholder="Chọn mức độ tính chất"
                                    deselect-label="Nhấn để xoá"
                                    selectLabel="Nhấn enter để chọn"
                                    selectedLabel="Đã chọn"
                                ></multiselect>
                              </div>
                            </div>
                            <!--                            Mức độ bảo mật-->
                            <div class="col-md-6">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01">Mức độ bảo mật</label>
                                <multiselect
                                    v-model="model.mucDoBaoMat"
                                    :options="optionsMucDo"
                                    track-by="id"
                                    label="name"
                                    placeholder="Chọn mức độ bảo mật"
                                    deselect-label="Nhấn để xoá"
                                    selectLabel="Nhấn enter để chọn"
                                    selectedLabel="Đã chọn"
                                ></multiselect>
                              </div>
                            </div>
                            <!--                            Hồ sơ đơn vị-->
                            <div class="col-md-6">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01">Hồ sơ đơn vị</label>
                                <input
                                    v-model="model.hoSoDonVi"
                                    type="text"
                                    class="form-control"
                                    placeholder=""
                                />
                              </div>
                            </div>
                            <!--                            Nơi lưu trữ-->
                            <div class="col-md-6">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01">Nơi lưu trữ</label>
                                <input
                                    v-model="model.noiLuuTru"
                                    type="text"
                                    class="form-control"
                                    placeholder="Nhập nơi lưu trữ"
                                />
                              </div>
                            </div>
                            <!--                            Điều kiện-->
<!--                            <div class="col-md-12">-->
<!--                              <div class="mb-2 d-flex align-items-center">-->
<!--                                <switches v-model="model.trinhLanhDaoTruong" color="primary"-->
<!--                                          class="ml-1 mx-2"></switches>-->
<!--                                <label for=""> Trình lãnh đạo trường</label>-->
<!--                              </div>-->
<!--                              <div class="mb-2 d-flex align-items-center">-->
<!--                                <switches v-model="model.congVanChiDoc" color="primary" class="ml-1 mx-2"></switches>-->
<!--                                <label for="">Là công văn chỉ đọc</label>-->
<!--                              </div>-->
<!--                              <div class="mb-2 d-flex align-items-center">-->
<!--                                <switches v-model="model.banChinh" color="primary" class="ml-1 mx-2"></switches>-->
<!--                                <label for=""> Là bản chính</label>-->
<!--                              </div>-->
<!--                              <div class="mb-2 d-flex align-items-center">-->
<!--                                <switches v-model="model.hienThiThongBao" color="primary" class="ml-1 mx-2"></switches>-->
<!--                                <label for="">Hiển thị mục thông báo</label>-->
<!--                              </div>-->

<!--                            </div>-->
                          </div>
                        </div>
                      </div>
                    </form>
                  </b-modal>
                </div>
              </div>
            </div>
            <div class="row">
              <div class="col-12">
                <div class="row mb-2">
                  <div class="col-sm-12 col-md-6">
                    <div id="tickets-table_length" class="dataTables_length">
                      <label class="d-inline-flex align-items-center">
                        Hiện
                        <b-form-select
                            class="form-select form-select-sm"
                            v-model="perPage"
                            size="sm"
                            :options="pageOptions"
                        ></b-form-select
                        >&nbsp;dòng
                      </label>
                    </div>
                  </div>
                </div>
                <!-- Table -->
                <div class="table-responsive-sm">
                  <b-table
                      class="datatables"
                      :items="myProvider"
                      :fields="fields"
                      striped
                      bordered
                      responsive="sm"
                      :per-page="perPage"
                      :current-page="currentPage"
                      :sort-by.sync="sortBy"
                      :sort-desc.sync="sortDesc"
                      :filter="filter"
                      :filter-included-fields="filterOn"
                      ref="tblList"
                      primary-key="id"
                      :busy.sync="isBusy"
                  >
                    <template v-slot:cell(STT)="data">
                      {{ data.index + ((currentPage - 1) * perPage) + 1 }}
                    </template>
                    <template v-slot:cell(loaiVanBan)="data">
                      <span v-if="data.item.loaiVanBan"> {{ data.item.loaiVanBan.ten }}</span>
                    </template>
                    <template v-slot:cell(trangThai)="data">
                      <div style="display: flex; justify-content: center; align-items: center; flex-direction: column">
                        <div>
                          <span v-if="data.item.trangThai" class="badge"  :class="'bg-' + data.item.trangThai.bgColor"> {{ data.item.trangThai.ten }}</span>
                        </div>

<!--                        <button-->
<!--                            v-if="data.item.noiDungTuChoi != null && data.item.noiDungTuChoi != ''"-->
<!--                            type="button"-->
<!--                            size="sm"-->
<!--                            class="btn btn-outline btn-sm"-->
<!--                            data-toggle="tooltip" data-placement="bottom" title="Nội dung từ chối"-->
<!--                            v-on:click="handleShowNoiDungTuChoi(data.item.noiDungTuChoi)">-->
<!--                          <i class="fas fa-question-circle text-danger me-1"></i>-->

<!--                        </button>-->
                        <i v-if="data.item.noiDungTuChoi != null && data.item.noiDungTuChoi != ''"
                           v-on:click="handleShowNoiDungTuChoi(data.item.noiDungTuChoi)"
                           class="fas fa-question-circle text-danger me-1"
                           style="margin-left: 6px"
                        ></i>
                      </div>


                    </template>
                    <template v-slot:cell(trichYeu)="data">
                      <div v-if="data.item.trichYeu" :inner-html.prop="data.item.trichYeu | truncate(150)">
                      </div>
                    </template>
                    <template v-slot:cell(process)="data">
                      <button
                          v-if="currentUserName == data.item.createdBy && (data.item.trangThai.code == 'ktvb' || data.item.trangThai.code == 'VTTTC')"
                          type="button"
                          size="sm"
                          class="btn btn-outline btn-sm"
                          data-toggle="tooltip" data-placement="bottom" title="Cập nhật"
                          v-on:click="handleUpdate(data.item.id)">
                        <i class="fas fa-pencil-alt text-success me-1"></i>

                      </button>
                      <button
                          type="button"
                          size="sm"
                          class="btn btn-outline btn-sm"
                          data-toggle="tooltip" data-placement="bottom" title="Lịch sử"
                          v-on:click="handleHistory(data.item.id)">
                        <i class="fas fa-history text-info me-1"></i>
                      </button>
                      <template v-if="data.item.trangThai">
                        <button
                            v-if="data.item.trangThai.ten == 'Ký số'"
                            type="button"
                            size="sm"
                            class="btn btn-outline btn-sm"
                            data-toggle="tooltip" data-placement="bottom" title=" Ký số"
                            v-on:click="showModalMembers = true, modelKySo.vanBanDiId=data.item.id, getPhanCongKySoByVanBanId(data.item.id)">
                          <i class="fas fa-user-plus text-info me-1"></i>
                        </button>
                      </template>

<!--                      <button-->
<!--                          type="button"-->
<!--                          size="sm"-->
<!--                          class="btn btn-outline btn-sm"-->
<!--                          data-toggle="tooltip" data-placement="bottom" title="Cập nhật"-->
<!--                          v-on:click="handleShowButPhe(data.item.id)">-->
<!--                        <i class="fas fa-feather-alt text-primary me-1"></i>-->
<!--                      </button>-->
                      <button
                          v-if="currentUserName == data.item.createdBy && data.item.trangThai.code == 'ktvb'"
                          type="button"
                          size="sm"
                          class="btn btn-outline btn-sm"
                          data-toggle="tooltip" data-placement="bottom" title="Xóa"
                          v-on:click="handleShowDeleteModal(data.item.id)">
                        <i class="fas fa-trash-alt text-danger me-1"></i>
                      </button>

                    </template>
                    <template v-slot:cell(ten)="data">&nbsp;&nbsp;
                      {{ data.item.ten }}
                    </template>

                    <template v-slot:cell(chuyenTrangThai)="data">
                      <b-button
                          v-if="(data.item.trangThai.code == 'tldt' || data.item.trangThai.code == 'tkhtxn'
                          || data.item.trangThai.code == 'kpl' || data.item.trangThai.code == 'DVBPL' || data.item.trangThai.code == 'TKHTTC' || data.item.trangThai.code == 'HTK') && data.item.ower && data.item.ower.userName == currentUserName"
                          type="button"
                          size="sm"
                          class="btn btn-light "
                          data-toggle="tooltip" data-placement="bottom" title="Xóa"
                          v-on:click="handleCheckVanBanModal(data.item.id)">
                        <i class="fas fa-exchange-alt  me-1"></i>
                        Xử lý VB
                      </b-button>

                      <b-button
                          v-else-if="data.item.ower && data.item.ower.userName == currentUserName && (data.item.trangThai.code == 'ktvb' || data.item.trangThai.code == 'VTTTC' )"
                          type="button"
                          size="sm"
                          class="btn btn-light btn-danger"
                          data-toggle="tooltip" data-placement="bottom" title="Xóa"
                          v-on:click="handleChuyenTrangThai(data.item.trangThai,data.item.id)">
                        <i class="fas fa-exchange-alt  me-1"></i>
                        Xử lý VB

                      </b-button>


                    </template>

                  </b-table>
                  <template v-if="isBusy">
                    <div align="center">Đang tải dữ liệu</div>
                  </template>
                  <template v-if="totalRows <= 0 && !isBusy">
                    <div align="center">Không có dữ liệu</div>
                  </template>
                </div>
                <!-- Paginnation -->
                <div class="row">
                  <b-col>
                    <div>Hiển thị {{ numberOfElement }} trên tổng số {{ totalRows }} dòng</div>
                  </b-col>
                  <div class="col">
                    <div
                        class="dataTables_paginate paging_simple_numbers float-end">
                      <ul class="pagination pagination-rounded mb-0">
                        <!-- pagination -->
                        <b-pagination
                            v-model="currentPage"
                            :total-rows="totalRows"
                            :per-page="perPage"
                        ></b-pagination>
                      </ul>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
        <b-modal
            v-model="showModalMembers"
            title="Thành viên ký số"
            title-class="text-black font-18"
            body-class="p-3"
            hide-footer
            centered
            no-close-on-backdrop
            size="lg"
        >
          <div class="row" style="display: flex; justify-content: center; align-items: center;">
            <div class="col-md-5">
              <!--                Lãnh đạo bút phê -->
              <div class="mb-2">
                <label class="form-label" for="validationCustom01"> Thành viên</label>
                <multiselect
                    v-model="modelKySo.nguoiKy"
                    :options="optionsUser"
                    track-by="id"
                    label="fullName"
                    placeholder="Chọn người ký số"
                    deselect-label="Nhấn để xoá"
                    selectLabel="Nhấn enter để chọn"
                    selectedLabel="Đã chọn"
                ></multiselect>
              </div>
            </div>
            <div class="col-md-3">
              <div class=" d-flex align-items-center">
                <switches v-model="modelKySo.choPhepKy" color="primary" class="ml-1 mx-2"></switches>
                <label for=""> Cho phép ký </label>
              </div>

            </div>
            <div class="col-md-2">
              <div class="mb-2">
                <label class="form-label" for="validationCustom01"> Thứ tự</label>
                <input
                    v-model="modelKySo.thuTu"
                    type="text"
                    class="form-control"
                    placeholder="Nhập thứ tự"
                />
              </div>
            </div>
            <div class="col-md-2">
              <b-button @click="handleAssignSign" variant="primary"> Thêm thành viên</b-button>
            </div>
            <div class="col-md-12">
              <div class="table-responsive-sm">
                <table class="datatables table b-table table-striped table-bordered" style="width:100%">
                  <thead>
                  <tr>
                    <th>#</th>
                    <th>Tài khoản</th>
                    <th>Họ và tên</th>
                    <th>Cho phép ký</th>
                    <th></th>
                  </tr>
                  </thead>
                  <tbody>
                  <template
                      v-if="listPhanCongKySo == null || (listPhanCongKySo != null && listPhanCongKySo.length <= 0)">
                    <tr>
                      <td colspan="5">Không có dữ liệu</td>
                    </tr>
                  </template>
                  <template v-else>
                    <tr v-for="(item, index) in listPhanCongKySo" :key="index">
                      <td>{{ ++index }}</td>
                      <td>{{ item.userName }}</td>
                      <td>{{ item.fullName }}</td>
                      <td>
                        <span v-if="item.choPhepKy">Ký số</span>
                        <span v-else>Xem duyệt</span>
                      </td>
                      <td>
                        <b-button @click="handleRemoveAssignSign(item.userName)" variant="danger">Xóa</b-button>
                      </td>
                    </tr>
                  </template>
                  </tbody>
                </table>
              </div>
            </div>


          </div>
        </b-modal>
        <!--        Modal bút phê-->
        <b-modal
            v-model="showModalButPhe"
            title="Bút phê đơn vị lãnh đạo, đơn vị nhận/ xử lý văn bản"
            title-class="text-black font-18"
            body-class="p-3"
            hide-footer
            centered
            no-close-on-backdrop
            size="xl"
        >
          <form @submit.prevent="handleButPhe" ref="">
            <div class="row">
              <div class="col-md-6">
                <!--                Số lưu and số văn bản đi -->
                <div class="d-flex justify-content-between mb-2">
                  <!--                              Số lưu -->
                  <div class="me-4">
                    <label class="form-label" for="validationCustom01">Số lưu CV</label> <span
                      class="text-danger">*</span>
                    <input
                        id="validationCustom01"
                        v-model="model.soLuuCV"
                        type="text"
                        class="form-control"
                        disabled
                    />
                  </div>
                  <!--                            Số VB đến -->
                  <div class="">
                    <label class="form-label" for="validationCustom01">Số CV đi</label> <span
                      class="text-danger">*</span>
                    <input
                        id="validationSoVBDen"
                        v-model="model.soVBDi"
                        type="text"
                        class="form-control"
                        disabled
                    />
                  </div>

                </div>
                <!--                Trích yếu -->
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Trích yếu</label> <span
                    class="text-danger">*</span>
                  <textarea
                      v-model="model.trichYeu"
                      name=""
                      id="trichyeu"
                      rows="2"
                      class="form-control"
                      disabled
                  ></textarea>
                </div>
                <!--                Bút phê -->
                <label class="form-label" for="validationCustom01">Bút phê</label> <span
                  class="text-danger">*</span>
                <ckeditor
                    v-model="modelButPhe.noiDungButPhe"
                    :editor="editor"
                    :config="editorConfig"
                ></ckeditor>
                <!--                Lãnh đạo bút phê -->
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Lãnh đạo bút phê</label>
                  <multiselect
                      v-model="modelButPhe.nguoiButPhe"
                      :options="optionsUser"
                      track-by="id"
                      label="fullName"
                      placeholder="Chọn lãnh đạp bút phê"
                      deselect-label="Nhấn để xoá"
                      selectLabel="Nhấn enter để chọn"
                      selectedLabel="Đã chọn"
                  ></multiselect>
                </div>
                <!--                Ngày bút phê -->
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Ngày bút phê</label>
                  <date-picker
                      v-model="modelButPhe.ngayButPhe"
                      format="DD/MM/YYYY"
                      :first-day-of-week="1"
                      lang="en"
                      placeholder="Chọn ngày bút phê"
                  ></date-picker>
                </div>
              </div>
              <div class="col-md-6">
                <!--                Mức độ quan trong -->
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Mức độ quan trọng</label>
                  <multiselect
                      v-model="modelButPhe.mucDoQuanTrong"
                      :options="optionsMucDo"
                      track-by="id"
                      label="name"
                      placeholder="Chọn mức độ quan trọng"
                      deselect-label="Nhấn để xoá"
                      selectLabel="Nhấn enter để chọn"
                      selectedLabel="Đã chọn"
                  ></multiselect>
                </div>
                <!--                Người phụ trách -->
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Người phụ trách</label>
                  <multiselect
                      :multiple="true"
                      v-model="modelButPhe.nguoiPhuTrach"
                      :options="optionsUser"
                      track-by="id"
                      label="fullName"
                      placeholder="Chọn người phụ trách"
                      deselect-label="Nhấn để xoá"
                      selectLabel="Nhấn enter để chọn"
                      selectedLabel="Đã chọn"
                  ></multiselect>
                </div>
                <!--                Ngừoi chủ trì -->
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Người chủ trì</label>
                  <multiselect
                      :multiple="true"
                      v-model="modelButPhe.nguoiChuTri"
                      :options="optionsUser"
                      track-by="id"
                      label="fullName"
                      placeholder="Chọn người chủ trì"
                      deselect-label="Nhấn để xoá"
                      selectLabel="Nhấn enter để chọn"
                      selectedLabel="Đã chọn"
                  ></multiselect>
                </div>
                <!--                Người phối hợp xử lý-->
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Người phối hợp xử lý</label>
                  <multiselect
                      :multiple="true"
                      v-model="modelButPhe.nguoiPhoihopXuLy"
                      :options="optionsMucDo"
                      track-by="id"
                      label="name"
                      placeholder="Chọn người phối hợp xử lý"
                      deselect-label="Nhấn để xoá"
                      selectLabel="Nhấn enter để chọn"
                      selectedLabel="Đã chọn"
                  ></multiselect>
                </div>
                <!--                Đơn vị xử lý-->
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Đơn vị xử Lý</label>
                  <multiselect
                      :multiple="true"
                      v-model="modelButPhe.donViXuLy"
                      :options="optionsDonVi"
                      track-by="id"
                      label="label"
                      placeholder="Chọn mức độ bảo mật"
                      deselect-label="Nhấn để xoá"
                      selectLabel="Nhấn enter để chọn"
                      selectedLabel="Đã chọn"
                  ></multiselect>
                </div>
                <!--                Đơn vị phối hợp-->
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01"> Đơn vị phối hợp</label>
                  <multiselect
                      :multiple="true"
                      v-model="modelButPhe.donViPhoiHop"
                      :options="optionsDonVi"
                      track-by="id"
                      label="label"
                      placeholder="Chọn đơn vị phối hợp"
                      deselect-label="Nhấn để xoá"
                      selectLabel="Nhấn enter để chọn"
                      selectedLabel="Đã chọn"
                  ></multiselect>
                </div>
                <!--                Ngừoi xem để biết-->
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Người xem để biết</label>
                  <multiselect
                      :multiple="true"
                      v-model="modelButPhe.nguoiXemDeBiet"
                      :options="optionsUser"
                      track-by="id"
                      label="fullName"
                      placeholder="Chọn người xem để biết"
                      deselect-label="Nhấn để xoá"
                      selectLabel="Nhấn enter để chọn"
                      selectedLabel="Đã chọn"
                  ></multiselect>
                </div>
              </div>
              <div class="col-md-12">
                <!--                File đính kèm-->
                <div class="mb-2">
                  <label for="">File đính kèm</label>
                  <vue-dropzone
                      id="dropzone"
                      ref="myVueDropzone"
                      :options="dropzoneOptions"
                  ></vue-dropzone>
                </div>
              </div>
            </div>
            <div class="text-end pt-2 mt-3">
              <b-button variant="light" class="w-md" size="sm" @click="showModalButPhe = false">
                Đóng
              </b-button>
              <b-button type="submit" variant="primary" size="sm" class="ms-1 w-md">
                Lưu
              </b-button>
            </div>
          </form>
        </b-modal>

        <!--        Modal phân công -->

        <b-modal
            v-model="showModalPhanCong"
            title="Phân công xử lý"
            title-class="text-black font-18"
            body-class="p-3"
            hide-footer
            centered
            no-close-on-backdrop
            size="xl"
        >
          <form @submit.prevent="handlePhanCong" ref="">
            <div class="row">
              <div class="col-md-12">
                <div class="customAddElement">
                  <b-button
                      pill
                      variant="success"
                      @click="AddformData"
                  >
                    <i class="fas fa-plus text-light fs-3"></i>
                  </b-button>
                </div>
                <div class="mb-4"></div>
                <div data-repeater-list="group-a">
                  <div
                      v-for="(items, index) in phanCong"
                      :key="items.id"
                      class="card mx-3"
                  >
                    <div class="card-body" style="position: relative">
                      <div class="custom-ribon">
                        <div class="bg-primary text-center">
                          <p class="text-light p-1">Nhóm {{ index }}</p>
                        </div>
                      </div>
                      <div class="row align-items-center">
                        <div class="col-lg-12 d-flex justify-content-end">
                          <b-button
                              pill
                              @click="deleteRow(index)"
                          >
                            <i class="fas fa-trash text-danger"></i>
                          </b-button>
                        </div>
                        <div class="col-md-12 mb-2">
                          <label class="form-label" for="validationCustom01">Ý kiến chỉ đạo</label> <span
                            class="text-danger">*</span>
                          <ckeditor
                              v-model="items.yKienChiDao"
                              :editor="editor"
                              :config="editorConfig"
                          ></ckeditor>
                        </div>
                        <div class="col-md-5 mb-3">
                          <div class="mb-2">
                            <label class="form-label" for="validationCustom01">Người bút phê</label>
                            <multiselect
                                v-model="items.nguoiButPhe"
                                :options="optionsUser"
                                track-by="id"
                                label="fullName"
                                placeholder="Chọn người bút phê"
                                deselect-label="Nhấn để xoá"
                                selectLabel="Nhấn enter để chọn"
                                selectedLabel="Đã chọn"
                            ></multiselect>
                          </div>
                        </div>
                        <div class="col-md-2 mb-3 text-center">
                          <div class="d-flex align-items-center justify-content-center ">
                            <i class="fas fa-folder-open text-lime fs-2 me-1"></i>
                            <i class="fas fa-chevron-right text-secondary "></i>
                            <i class="fas fa-chevron-right text-lime"></i>
                          </div>

                        </div>
                        <div class="col-md-5 mb-3">
                          <div class="mb-2">
                            <label class="form-label" for="validationCustom01">Người nhận</label>
                            <multiselect
                                :multiple="true"
                                v-model="items.nguoiNhanXuLy"
                                :options="optionsUser"
                                track-by="id"
                                label="fullName"
                                placeholder="Chọn người nhận xử lý"
                                deselect-label="Nhấn để xoá"
                                selectLabel="Nhấn enter để chọn"
                                selectedLabel="Đã chọn"
                            ></multiselect>
                          </div>
                        </div>
                        <div class="col-md-12">
                          <div class="mb-2">
                            <label for="">File đính kèm</label>
                            <vue-dropzone
                                id="dropzone"
                                ref="myVueDropzone"
                                :options="dropzoneOptions"
                            ></vue-dropzone>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>

              </div>
            </div>
            <div class="text-end pt-2 mt-3">
              <b-button variant="light" class="w-md" size="sm" @click="showModalPhanCong = false">
                Đóng
              </b-button>
              <b-button type="submit" variant="primary" size="sm" class="ms-1 w-md">
                Lưu
              </b-button>
            </div>
          </form>
        </b-modal>

        <!--        Modal delete -->
        <b-modal
            v-model="showDeleteModal"
            centered
            title="Xóa dữ liệu"
            title-class="font-18"
            no-close-on-backdrop
        >
          <p>
            Dữ liệu xóa sẽ không được phục hồi!
          </p>
          <template #modal-footer>
            <b-button v-b-modal.modal-close_visit
                      size="sm"
                      class="btn btn-outline-info w-md"
                      v-on:click="showDeleteModal = false">
              Đóng
            </b-button>
            <b-button v-b-modal.modal-close_visit
                      size="sm"
                      variant="danger"
                      type="button"
                      class="w-md"
                      v-on:click="handleDelete">
              Xóa
            </b-button>
          </template>
        </b-modal>

<!--        Trạng thái-->
        <b-modal
            v-model="showTrangThaiModal"
            centered
            title="Xử lý văn bản"
            title-class="font-18"
            no-close-on-backdrop
            size="lg"
        >
          <div class="row">
            <div class="col-md-6">

              <div v-if="modelTrangThai.currentTrangThai" class="mb-2">
                <label class="form-label" for="validationCustom01">TT Hiện tại</label> <span
                  class="text-danger">*</span>
                <input
                    id="validationCustom01"
                    :value="modelTrangThai.currentTrangThai.ten"
                    type="text"
                    class="form-control"
                    placeholder=""
                    disabled
                />
              </div>

            </div>
            <div class="col-md-6">

              <div class="mb-2">
                <label class="form-label" for="validationCustom01">TT Tiếp theo</label> <span
                  class="text-danger">*</span>
                <multiselect
                    v-model="modelTrangThai.newTrangThai"
                    :options="optionsTrangThai"
                    track-by="code"
                    label="ten"
                    placeholder="Chọn trạng thái"
                    :class="{
                                'is-invalid':
                                  submitted && $v.modelTrangThai.newTrangThai.$error,
                                }"
                ></multiselect>
                <div
                    v-if="submitted && !$v.modelTrangThai.newTrangThai.required"
                    class="invalid-feedback"
                >
                  Trạng thái không được để trống.
                </div>
              </div>
            </div>
          </div>
          <template #modal-footer>
            <b-button v-b-modal.modal-close_visit
                      size="sm"
                      class="btn btn-outline-info w-md"
                      v-on:click="showTrangThaiModal = false">
              Đóng
            </b-button>
            <b-button v-b-modal.modal-close_visit
                      size="sm"
                      variant="primary"
                      type="button"
                      class="w-md"
                      v-on:click="handleChuyenTrangThaiVanBan(null)">
              Chuyển trạng thái
            </b-button>
          </template>
        </b-modal>

<!--        history-->
        <b-modal
            v-model="showHistoryModal"
            centered
            title="Lịch sử hoạt động"
            title-class="font-18"
            no-close-on-backdrop
            size="lg"
        >
          <div class="row">
            <div class="col-lg-12">
              <div id="cd-timeline" style="margin: 0">
                <ul v-if="listHistoryQuestion && listHistoryQuestion.length > 0" class="timeline list-unstyled" style="padding: 0">
                  <li  v-for="(item, index) in listHistoryQuestion" class="timeline-list" :key="index" style="padding: 10px 0 10px 90px;">
                    <div class="cd-timeline-content p-2" style="width: 100%;" >
                      <h5 class="mt-0 mb-3">
                            <span style="font-weight: bold" class="text-success">
                                                                #{{listHistoryQuestion.length - index}}  {{item.title}}
                                                     </span></h5>
                      <div style="font-weight: bold">Trạng thái:  <span class="badge font-size-small min-width-30 p-2 btn-xs bg-danger  font-weight-semibold" v-if="item.trangThai">{{item.trangThai.ten}}</span></div>


                      <div
                          v-if="item.userName"
                          class="mt-1"
                      >
                                  <span style="font-weight: bold">
                                    Người thao tác:
                                  </span>
                        <span>
                                    {{item.userName}} - {{item.fullName}}
                                  </span>
                      </div>
                      <div v-else >
                                  <span style="font-weight: bold">
                                       Người thao tác:
                                  </span>
                        <span>
                                    Không có dữ liệu
                                  </span>
                      </div>
                   <div v-if="item.content" style=" margin-top: 8px; display: flex">
                   <span style="font-weight: bold;">
                      Nội dung:
                   </span>
                     <p
                         class="mb-2 " style="margin-left: 8px"
                     >{{item.content}}</p>
                   </div>
                      <div class="date bg-primary" style="top: 10px;">
                        <h6 class="mt-0 text-center">{{ item.createdAtShow }}</h6>
                        <h6 class="mt-0 text-center">{{ item.createdAtTimeShow }}</h6>
                        <!--                       -->
                      </div>
                    </div>
                  </li>
                </ul>
                <div v-else>Không có dữ liệu</div>
              </div>
            </div>
          </div>
          <template #modal-footer>
            <b-button v-b-modal.modal-close_visit
                      size="sm"
                      class="btn btn-outline-info w-md"
                      v-on:click="showHistoryModal = false">
              Đóng
            </b-button>
          </template>
        </b-modal>

        <b-modal
            v-model="showCheckVanBanModal"
            centered
            title="Chi tiết VB"
            title-class="font-18"
            no-close-on-backdrop
            size="lg"
            ref="refshowCheckVanBanModal"
            hide-footer
        >
          <div class="row" style="width: 100%; margin: 0" >
            <div class="col-md-6">
              <div class="row">
                <div class="col-md-12 capso-container">
                  <div class="title-capso">Số lưu CV</div>
                  <div class="content-capso">{{model.soLuuCV}}</div>
                </div>
                <div class="col-md-12 capso-container">
                  <div class="title-capso"> Ngày nhập công văn</div>
                  <div class="content-capso">{{model.ngayNhap}}</div>
                </div>

                <div class="col-md-12 capso-container">
                  <div class="title-capso"> Ngày ký</div>
                  <div class="content-capso">{{model.ngayKy}}</div>
                </div>
                <div class="col-md-12 capso-container">
                  <div class="title-capso"> Trích yếu</div>
                  <div class="content-capso">
                    <div v-if="model.trichYeu" :inner-html.prop="model.trichYeu | truncate(150)">
                  </div>
                </div>
                <!--                <div class="col-md-12 capso-container">-->
                <!--                  <div class="title-capso"> Ngày ký</div>-->
                <!--                  <div class="content-capso">{{model.ngayKy}}</div>-->
                <!--                </div>-->
              </div>
            </div>
            </div>
            <div class="col-md-6">
              <div class="row">
                <div class="col-md-12 capso-container">
                  <div class="title-capso"> Loại văn bản</div>
                  <template v-if="model.loaiVanBan">
                    <div class="content-capso">{{model.loaiVanBan.ten}}</div>
                  </template>
                </div>
                <div class="col-md-12 capso-container">
                  <div class="title-capso"> Trạng thái</div>
                  <template v-if="model.trangThai">
                    <div class="content-capso">{{model.trangThai.ten}}</div>
                  </template>
                </div>

              </div>

            </div>
            <div class="col-md-12" style="padding: 0px">
              <h5 style="font-weight: bold">Duyệt thể thức văn bản</h5>
              <div class="col-md-12 capso-container" style="display: flex; flex-direction: column">
                <div class="title-capso">Tện tin dính kèm của người soạn</div>
                <ul v-if="model.file && model.file.length > 0" style="padding-left: 30px">
                  <li class="title-capso" style="font-weight: normal" v-for="(value, index) in model.file" :key="index">

                    <a
                        :href="`${apiUrl}files/view/${value.fileId}`"
                        class=" fw-medium"
                    ><i
                        :class="`mdi font-size-16 align-middle me-2`"
                    ></i>
                      [Tải về]</a
                    >
                    <span  style="padding-left: 20px">{{value.fileName}}</span>
                  </li>
                </ul>

              </div>
              <div class="mb-3">
                <label class="form-label title-capso">Ghi chú</label>
                <div>
                  <textarea
                      v-model="modelTrangThai.noiDung"
                      class="form-control"
                      name="textarea"

                  ></textarea>
                </div>
              </div>

            </div>

          </div>
          <template #modal-header="{ close }">
            <!-- Emulate built in modal header close button action -->
            <h5 style="min-width: 200px"> Văn bản đi</h5>
            <div style="width: 100%; display: flex; justify-content: flex-end" class="text-end">
              <div v-if="optionsTrangThai && optionsTrangThai.length">
                <b-button v-for="(value, index) in optionsTrangThai" :key="index" type="button" variant="primary" class="ms-1" style="min-width: 80px;" size="sm"
                          @click="handleChuyenTrangThaiVanBan(value)"
                >
                  {{ value.ten }}
                </b-button>
              </div>

              <b-button variant="light"  class="ms-1" size="sm" style="width: 80px" @click="showCheckVanBanModal = false">
                Đóng
              </b-button>
            </div>
          </template>
<!--          <template #modal-footer>-->
<!--            <b-button v-b-modal.modal-close_visit-->
<!--                      size="sm"-->
<!--                      class="btn btn-outline-info w-md"-->
<!--                      v-on:click="showCheckVanBanModal = false">-->
<!--              Đóng-->
<!--            </b-button>-->
<!--            <b-button v-b-modal.modal-close_visit-->
<!--                      size="sm"-->
<!--                      variant="primary"-->
<!--                      type="button"-->
<!--                      class="w-md"-->
<!--                      v-on:click="handleChuyenTrangThaiVanBan">-->
<!--              Chuyển trạng thái-->
<!--            </b-button>-->
<!--          </template>-->
        </b-modal>

        <!--       Nội dung từ chối-->
        <b-modal
            v-model="showModalNoiDungTuChoi"
            centered
            title="Lý do không duyệt văn bản"
            title-class="font-18"
            no-close-on-backdrop
        >
          <p>
            {{noiDungTuChoi}}
          </p>
          <template #modal-footer>
            <b-button v-b-modal.modal-close_visit
                      size="sm"
                      class="btn btn-outline-info w-md"
                      v-on:click="showModalNoiDungTuChoi = false">
              Đóng
            </b-button>
          </template>
        </b-modal>
      </div>
    </div>
  </Layout>
</template>
<style>
.title-capso{
  font-weight: bold; color: #00568C; margin-right: 10px;

}
.content-capso{
  color: #00568C;
}
.capso-container{
  margin-top: 10px;
  display: flex;
  padding: 0px;
}
</style>