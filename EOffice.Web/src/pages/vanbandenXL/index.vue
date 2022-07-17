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
// import the component
import Treeselect from '@riophae/vue-treeselect'
// import the styles
import '@riophae/vue-treeselect/dist/vue-treeselect.css'

/**
 * Form editor
 */
import CKEditor from "@ckeditor/ckeditor5-vue";
import ClassicEditor from "@ckeditor/ckeditor5-build-classic";
import vue2Dropzone from "vue2-dropzone";
import {butPheModel} from "@/models/butPheModel";
import {phanCongModel} from "@/models/phanCongModel";
import {notifyModel} from "@/models/notifyModel";

/**
 * Advanced table component
 */
export default {
  page: {
    title: "Văn bản đến",
    meta: [{name: "description", content: appConfig.description}]
  },
  components: {
    Layout,
    PageHeader,
    Multiselect,
    ckeditor: CKEditor.component,
    Switches,
    DatePicker,
    vueDropzone: vue2Dropzone,
    Treeselect,
  },
  data() {
    return {
      title: "Văn bản đến",
      items: [
        {
          text: "E-Office",
          href: "/"
        },
        {
          text: "Văn bản đến",
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
      model: vanBanDenModel.baseJson(),
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
          thStyle: {width: '50px', minWidth: '50px'},
          class: "text-center"
        },
        {
          key: "soLuuCV",
          label: "Số lưu CV",
          thStyle: {width: '10px', minWidth: '100px'},
          class: "px-1",
          sortable: true,
        },
        {
          key: "soVBDen",
          label: "Số văn bản đến",
          thStyle: {width: '160px', minWidth: '100px'},
          class: "px-1",
        },
        {
          key: "trichYeu",
          label: "Trích yếu",
          thStyle: {width: '100px', minWidth: '100px', maxHeight : '200px',},
          class: "px-1 w-25",
        },
        {
          key: "hanXuLy",
          label: "Hạn xử lý",
          thStyle: {width: '100px', minWidth: '100px'},
          class: "px-1",
        },
        {
          key: "trangThai",
          label: "Trạng thái",
          thStyle: {width: '100px', minWidth: '100px'},
          class: "px-1",
        },
        {
          key: 'process',
          label: 'Xử lý',
          thStyle: {width: '110px', minWidth: '110px'},
          class: "px-1"
        }
      ],
      optionsLoaiVanBan: null,
      optionsDonVi: null,
      optionsLinhVuc: null,
      optionsUser: null,
      optionsHinhThucNhan: null,
      optionsMucDo: null,
      optionsTrangThai: null,
      editor: ClassicEditor,
      editorConfig: {
        height: '200px'
      },
      apiUrl: process.env.VUE_APP_API_URL,
      url: `${process.env.VUE_APP_API_URL}files/upload`,
      dropzoneOptions: {
        url: `${process.env.VUE_APP_API_URL}files/upload`,
        thumbnailWidth: 300,
        thumbnailHeight: 160,
        maxFiles: 4,
        maxFilesize: 30,
        headers: {"My-Awesome-Header": "header value"},
        addRemoveLinks: true,
        acceptedFiles: ".jpeg,.jpg,.png,.gif,.doc,.docx,.xlsx,.pptx,.pdf",
      },
      showModalButPhe: false,
      showModalPhanCong: false,
      phanCong: [{id: 1}],
    };
  },
  validations: {
    model: {
      soLuuCV: {required},
      soVBDen: {required},
      loaiVanBan: {required},
      trichYeu: {required},
      ngayBanHanh: {required},
      ngayNhan: {required},
    }
  },
  computed: {
    /**
     * Total no. of records
     */
    rows() {
      return this.data.length;
    },
  },
  async created() {
    this.getLoaiVanBan();
    this.getTrangThai();
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
      console.log("LOG ON PAGE CHAGNE : ")
      this.pagination.currentPage = page;
      const params = {
        pageNumber: this.pagination.currentPage,
        pageSize: this.pagination.pageSize,
      }
      this.$refs.tblList?.refresh()
    },
    onFiltered(filteredItems) {
      // Trigger pagination to update the number of buttons/pages due to filtering
      this.totalRows = filteredItems.length;
      this.currentPage = 1;
    },

    async handleSubmit(e) {
      e.preventDefault();
      console.log("handle submit", this.model);
      if (
          this.model.id != 0 &&
          this.model.id != null &&
          this.model.id
      ) {
        //Update model
        await this.$store.dispatch("vanBanDenStore/update", this.model).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            this.showModal = false;
            this.model = vanBanDenModel.baseJson()
            this.myProvider()
          }
        })
      } else {
        console.log("this.model-create", this.model);
        //Create modelhandleSubmit
        this.model.version = 1;
        await this.$store.dispatch("vanBanDenStore/create", this.model).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            this.showModal = false;
            this.model = vanBanDenModel.baseJson()
            this.myProvider()
          }
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
          this.$refs.tblList.refresh()
        });
      }
    },
    async handleUpdate(id) {
      await this.$store.dispatch("vanBanDenStore/getById", id).then((res) => {
        if (res.resultCode == "SUCCESS") {
          console.log("res", res.data)
          this.model = res.data;
          this.showModal = true;
        } else {
          // this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        }
      });
    },
    async handleButPhe(e){
      e.preventDefault();
      this.model.butPhe = this.modelButPhe;
      console.log("ModelButPhe", this.model);
      await this.$store.dispatch("vanBanDenStore/update", this.model).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.showModal = false;
          this.model = vanBanDenModel.baseJson()
          this.myProvider()
        }
      });
    },
    async handlePhanCong(e){
      e.preventDefault();
      this.model.phanCong = this.modelPhanCong;
      console.log("ModelPhanCong", this.model);
      console.log("ModelPhanCong 2", this.phanCong);

      // await this.$store.dispatch("vanBanDenStore/update", this.model).then((res) => {
      //   if (res.resultCode === 'SUCCESS') {
      //     this.showModal = false;
      //     this.model = vanBanDenModel.baseJson()
      //     this.myProvider()
      //   }
      // });
    },
    handleShowDeleteModal(id) {
      this.model.id = id;
      this.showDeleteModal = true;
    },
    async handleDelete() {
      if (this.model.id != 0 && this.model.id != null && this.model.id) {
        await this.$store.dispatch("vanBanDenStore/delete", this.model.id).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            this.showDeleteModal = false;
            this.myProvider()
          }
          this.$refs.tblList.refresh()
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
    async getTrangThai() {
      try {
        await this.$store.dispatch("trangThaiStore/getTrangThai").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data;
            this.loading = false
            this.optionsTrangThai = items;
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
        await this.$store.dispatch("donViStore/getDonViCha").then(resp => {
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
            console.log("this.optionUser", this.optionsUser);
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
        console.log("optionsMucDo", this.optionsMucDo);
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
        console.log("LOG ADD THIS FILE ", response)
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
        let promise = this.$store.dispatch("vanBanDenStore/getPagingParams", params)
        return promise.then(resp => {
          let items = resp.data.data
          this.totalRows = resp.data.totalRows
          this.numberOfElement = resp.data.data.length
          this.loading = false
          console.log("data", items);
          return items || []
        })
      } finally {
        this.loading = false
      }
    }
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
                      @click="showModal = true"
                      size="sm"
                  >
                    <i class="mdi mdi-plus me-1"></i> Thêm mới
                  </b-button>
                  <!-- Model create -->
                  <b-modal
                      v-model="showModal"
                      title="Tiếp nhận văn bản đến"
                      title-class="text-black font-18"
                      body-class="p-3"
                      hide-footer
                      centered
                      no-close-on-backdrop
                      size="xl"
                  >
                    <form @submit.prevent="handleSubmit"
                          ref="formContainer">
                      <div class="row">
                        <div class="col-md-7">
                          <div class="row">
                            <!--                              Số lưu -->
                            <div class="col-md-3">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01">Số lưu CV</label> <span
                                  class="text-danger">*</span>
                                <input
                                    id="validationCustom01"
                                    v-model="model.soLuuCV"
                                    type="text"
                                    class="form-control"
                                    placeholder="eg: 123-dthu"
                                    :class="{
                                      'is-invalid': submitted && $v.model.soLuuCV.$error,
                                    }"
                                />
                                <div
                                    v-if="submitted && $v.model.soLuuCV.$error"
                                    class="invalid-feedback"
                                >
                                <span v-if="!$v.model.soLuuCV.required"
                                >Vui lòng thêm số lưu CV.</span
                                >
                                </div>
                              </div>
                            </div>
                            <!--                            Số VB đến -->
                            <div class="col-md-5">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01">Số văn bản đến</label> <span
                                  class="text-danger">*</span>
                                <input
                                    id="validationSoVBDen"
                                    v-model="model.soVBDen"
                                    type="text"
                                    class="form-control"
                                    placeholder="eg: 123-dthu"
                                    :class="{
                                      'is-invalid': submitted && $v.model.soVBDen.$error,
                                    }"
                                />
                                <div
                                    v-if="submitted && $v.model.soVBDen.$error"
                                    class="invalid-feedback"
                                >
                                <span v-if="!$v.model.soVBDen.required"
                                >Vui lòng thêm số văn bản đến.</span
                                >
                                </div>
                              </div>
                            </div>
                            <!--                            Loại văn bản-->
                            <div class="col-md-4">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01">Loại văn bản</label> <span
                                  class="text-danger">*</span>
                                <multiselect
                                    v-model="model.loaiVanBan"
                                    :options="optionsLoaiVanBan"
                                    track-by="id"
                                    label="ten"
                                    placeholder="Chọn trạng thái"

                                ></multiselect>
                                <div
                                    v-if="submitted && $v.model.soVBDen.$error"
                                    class="invalid-feedback"
                                >
                                <span v-if="!$v.model.soVBDen.required"
                                >Vui lòng thêm số văn bản đến.</span
                                >
                                </div>
                              </div>
                            </div>
                            <!--                            Trích yếu -->
                            <div class="col-md-12">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01">Trích yếu</label> <span
                                  class="text-danger">*</span>
                                <ckeditor
                                    v-model="model.trichYeu"
                                    :editor="editor"
                                    :config="editorConfig"
                                ></ckeditor>
                              </div>
                            </div>
                            <!--                            Ngày ban hành -->
                            <div class="col-md-6">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01">Ngày ban hành</label>
                                <date-picker
                                    v-model="model.ngayBanHanh"
                                    format="DD/MM/YYYY"
                                    :first-day-of-week="1"
                                    lang="en"
                                    placeholder="Chọn ngày ban hành"
                                ></date-picker>
                              </div>
                            </div>
                            <!--                            Ngày nhận -->
                            <div class="col-md-6">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01">Ngày nhận</label>
                                <date-picker
                                    v-model="model.ngayNhan"
                                    format="DD/MM/YYYY"
                                    :first-day-of-week="1"
                                    lang="en"
                                    placeholder="Chọn ngày nhận"
                                ></date-picker>
                              </div>
                            </div>

                            <!--                            file đính kèm-->
                            <div class="col-md-12">
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
                        <div class="col-md-5">
                          <div class="row">
                            <!--                            Ngày Ký-->
                            <div class="col-md-6">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01">Ngày ký</label>
                                <date-picker
                                    v-model="model.ngayKy"
                                    format="DD/MM/YYYY"
                                    :first-day-of-week="1"
                                    lang="en"
                                    placeholder="Chọn ngày ban hành"
                                ></date-picker>
                              </div>
                            </div>
                            <!--                            Người ký-->
                            <div class="col-md-6">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01">Người ký</label>
                                <multiselect
                                    v-model="model.nguoiKy"
                                    :options="optionsUser"
                                    track-by="id"
                                    label="fullName"
                                    placeholder="Chọn người ký"
                                    deselect-label="Nhấn để xoá"
                                    selectLabel="Nhấn enter để chọn"
                                    selectedLabel="Đã chọn"
                                ></multiselect>
                              </div>
                            </div>
                            <!--                            Thời hạn xử lý-->
                            <div class="col-md-6">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01">Thời hạn xử lý</label>
                                <date-picker
                                    v-model="model.hanXuLy"
                                    format="DD/MM/YYYY"
                                    :first-day-of-week="1"
                                    lang="en"
                                    placeholder="Chọn ngày ban hành"
                                ></date-picker>
                              </div>
                            </div>
                            <!--                            Trạng thái-->
                            <div class="col-md-6">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01">Trạng thái</label>
                                <multiselect
                                    v-model="model.trangThai"
                                    :options="optionsTrangThai"
                                    track-by="id"
                                    label="ten"
                                    placeholder="Chọn trạng thái"
                                    deselect-label="Nhấn để xoá"
                                    selectLabel="Nhấn enter để chọn"
                                    selectedLabel="Đã chọn"
                                ></multiselect>
                              </div>
                            </div>
                            <!--                            Khối cơ quan gửi-->
                            <div class="col-md-12">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01">Khối cơ quan gửi</label>
                                <treeselect
                                    v-model="model.khoiCoQuanGui"
                                    :options="optionsDonVi"
                                    placeholder="Chọn khối cơ quan gửi"
                                    value-format="object"
                                />
                                <treeselect-value :value="model.khoiCoQuanGui" />
                              </div>
                            </div>
                            <!--                            Cơ quan gửi-->
                            <div class="col-md-12">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01">Cơ quan gửi</label>
                                <treeselect
                                    v-model="model.coQuanGui"
                                    :options="optionsDonVi"
                                    placeholder="Chọn cơ quan gửi"
                                    value-format="object"
                                />
                                <treeselect-value :value="model.coQuanGui" />
                              </div>
                            </div>
                            <!--                            Hình thức nhận -->
                            <div class="col-md-6">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01">Hình thức nhận</label>
                                <multiselect
                                    v-model="model.hinhThucNhan"
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
                            <div class="col-md-12">
                              <div class="mb-2 d-flex align-items-center">
                                <switches v-model="model.congVanChiDoc" color="primary" class="ml-1 mx-2"></switches>
                                <label for="">Là công văn chỉ đọc</label>
                              </div>
                              <div class="mb-2 d-flex align-items-center">
                                <switches v-model="model.banChinh" color="primary" class="ml-1 mx-2"></switches>
                                <label for=""> Là bản chính</label>
                              </div>
                              <div class="mb-2 d-flex align-items-center">
                                <switches v-model="model.hienThiThongBao" color="primary" class="ml-1 mx-2"></switches>
                                <label for="">Hiển thị mục thông báo</label>
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>
                      <div class="text-end pt-2 mt-3">
                        <b-button variant="light" class="w-md" size="sm" @click="showModal = false">
                          Đóng
                        </b-button>
                        <b-button type="submit" variant="primary" size="sm" class="ms-1 w-md">
                          Lưu
                        </b-button>
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
                    <template v-slot:cell(hanXuLy)="data">
                      <span class="badge bg-danger"> {{ data.item.hanXuLy }}</span>
                    </template>
                    <template v-slot:cell(trichYeu)="data">
                      <div :inner-html.prop="data.item.trichYeu | truncate(100)"
                            class="text-left text-black w-100 block-ellipsis mx-2"
                      ></div>
                    </template>
                    <template v-slot:cell(process)="data">
                      <div class="d-flex justify-content-around">
                        <button
                            type="button"
                            size="sm"
                            class="btn btn-outline btn-sm p-0"
                            data-toggle="tooltip" data-placement="bottom" title="Chi tiết"
                            v-on:click="handleDetail(data.item.id)">
                          <i class="fas fa-eye  text-warning me-1"></i>
                        </button>
                        <button
                            type="button"
                            size="sm"
                            class="btn btn-outline btn-sm p-0"
                            data-toggle="tooltip" data-placement="bottom" title="Cập nhật"
                            v-on:click="handleUpdate(data.item.id)">
                          <i class="fas fa-pencil-alt text-success me-1"></i>
                        </button>
                        <button
                            type="button"
                            size="sm"
                            class="btn btn-outline btn-sm p-0"
                            data-toggle="tooltip" data-placement="bottom" title="Cập nhật"
                            v-on:click="HandleShowPhanCong(data.item.id)">
                          <i class="fas fa-user-plus text-info me-1"></i>
                        </button>
                        <button
                            type="button"
                            size="sm"
                            class="btn btn-outline btn-sm p-0"
                            data-toggle="tooltip" data-placement="bottom" title="Cập nhật"
                            v-on:click="handleShowButPhe(data.item.id)">
                          <i class="fas fa-feather-alt text-primary me-1"></i>
                        </button>
                        <button
                            type="button"
                            size="sm"
                            class="btn btn-outline btn-sm p-0"
                            data-toggle="tooltip" data-placement="bottom" title="Xóa"
                            v-on:click="handleShowDeleteModal(data.item.id)">
                          <i class="fas fa-trash-alt text-danger me-1"></i>
                        </button>
                      </div>
                    </template>
                    <template v-slot:cell(ten)="data">&nbsp;&nbsp;
                      {{ data.item.ten }}
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
          <form @submit.prevent="handleButPhe" ref="formContainer">
            <div class="row">
              <div class="col-md-6">
                <!--                Số lưu and số văn bản đến -->
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
                    <label class="form-label" for="validationCustom01">Số văn bản đến</label> <span
                      class="text-danger">*</span>
                    <input
                        id="validationSoVBDen"
                        v-model="model.soVBDen"
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
                      :options="optionsUser"
                      track-by="id"
                      label="fullName"
                      placeholder="Chọn người phối hợp xử lý"
                      deselect-label="Nhấn để xoá"
                      selectLabel="Nhấn enter để chọn"
                      selectedLabel="Đã chọn"
                  ></multiselect>
                </div>
                <!--                Đơn vị xử lý-->
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Đơn vị xử Lý</label>
                  <treeselect
                      :multiple="true"
                      v-model="modelButPhe.donViXuLy"
                      :options="optionsDonVi"
                      placeholder="Chọn đơn vị xử lý"
                      value-format="object"
                  />
                  <treeselect-value :value="model.donViXuLy" />
                </div>
                <!--                Đơn vị phối hợp-->
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01"> Đơn vị phối hợp</label>
                  <treeselect
                      :multiple="true"
                      v-model="modelButPhe.donViPhoiHop"
                      :options="optionsDonVi"
                      placeholder="Chọn đơn vị phối hợp"
                      value-format="object"
                  />
                  <treeselect-value :value="model.donViXuLy" />
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
                      v-on:vdropzone-removed-file="removeThisFile"
                      v-on:vdropzone-success="addThisFile"
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
          <form @submit.prevent="handlePhanCong" ref="formContainer">
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
                          <p class="text-light p-1">Nhóm {{index}}</p>
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
                                v-on:vdropzone-removed-file="removeThisFile"
                                v-on:vdropzone-success="addThisFile"
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
      </div>
    </div>
  </Layout>
</template>
<style>
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

.ck-editor__editable {
  min-height: 100px !important;
}

.ck-content {
  height: 100px !important;
}

.customAddElement {
  position: absolute;
  top: 8px;
  left: -20px;
}
.custom-ribon {
  position: absolute;
  width: 130px;
  top: 15px;
  left: -15px;
}

.custom-ribon>div {
  border-radius: 3px;
}
</style>