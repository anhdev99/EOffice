<script>
import Layout from "../../../layouts/main";
import PageHeader from "@/components/page-header";
import FullCalendar from "@fullcalendar/vue3";
import dayGridPlugin from "@fullcalendar/daygrid";
import timeGridPlugin from "@fullcalendar/timegrid";
import interactionPlugin from "@fullcalendar/interaction";
import bootstrapPlugin from "@fullcalendar/bootstrap";
import listPlugin from "@fullcalendar/list";
import {categories, INITIAL_EVENTS} from "@/pages/lichcongtac/lichcanhan/utils";
import Swal from "sweetalert2";
import {lichCongTacModel} from "@/models/lichCongTacModel";

import {required, helpers} from "@vuelidate/validators";
import useVuelidate from "@vuelidate/core";

import flatPickr from "vue-flatpickr-component";
import "flatpickr/dist/flatpickr.css";


export default {
  components: {Layout, PageHeader, FullCalendar, flatPickr},
  setup() {
    return {v$: useVuelidate()};
  },
  validations: {
    event: {
      tieuDe: {
        required: helpers.withMessage("Vui lòng thêm tiêu đề.", required),
      },
      ngayCongTac: {
        required: helpers.withMessage("Vui lòng chọn ngày.", required),
      },
      thoiGian: {
        required: helpers.withMessage("Vui lòng chọn ngày.", required),
      },
      diaDiem: {
        required: helpers.withMessage("Vui lòng thêm địa điểm.", required),
      },
    },
  },
  data() {
    return {
      title: "Quản lý lịch đề xuất",
      items: [
        {
          text: "Trang chủ",
          herf: "/",
        },
        {
          text: "lịch đề xuất",
          active: true,
        },
      ],
      dataTable: INITIAL_EVENTS,
      calendarOptions: {
        timeZone: "local",
        droppable: true,
        navLinks: true,
        plugins: [
          dayGridPlugin,
          timeGridPlugin,
          interactionPlugin,
          bootstrapPlugin,
          listPlugin,
        ],
        locale: "vi-VN",
        slotLabelFormat: {
          hour: "numeric",
          minute: "numeric",
          hour12: false,
        },
        themeSystem: "bootstrap",
        headerToolbar: {
          left: "prev,next today",
          center: "title",
          right: "dayGridMonth,timeGridWeek,timeGridDay,listMonth",
        },
        windowResize: () => {
          this.getInitialView();
        },
        initialView: this.getInitialView(),
        initialEvents: INITIAL_EVENTS,
        editable: true,
        selectable: true,
        selectMirror: true,
        dayMaxEvents: true,
        weekends: true,
        dateClick: this.dateClicked,
        eventClick: this.editEvent,
        eventsSet: this.handleEvents,
      },
      currentEvents: [],
      showModal: false,
      eventModal: false,
      categories: categories,
      submitted: false,
      submit: false,
      newEventData: {},
      edit: {},
      deleteId: {},
      event: lichCongTacModel.baseJson(),
      editevent: lichCongTacModel.baseJson(),
      fields: [
        { key: 'STT',
          label: 'STT',
          class: 'ps-4',
          thStyle: {width: '80px', minWidth: '80px'},
          thClass: 'hidden-sortable py-2'
        },
        {
          key: "ngayCongTac",
          label: "Ngày công tác",
          class: 'ps-4',
          sortable: true,
          thClass: 'hidden-sortable py-2'
        },
        {
          key: "thoiGian",
          label: "Thời gian",
          class: 'ps-4',
          sortable: true,
          thClass: 'hidden-sortable py-2'
        },
        {
          key: "tieuDe",
          label: "Tiêu đề",
          class: 'ps-4',
          thStyle: {width: '100px', minWidth: '100px'},
          thClass: 'hidden-sortable py-2'
        },
        {
          key: "chuTri",
          label: "Chủ trì",
          class: 'ps-4',
          thStyle: {width: '100px', minWidth: '100px'},
          thClass: 'hidden-sortable py-2'
        },
        {
          key: "diaDiem",
          label: "Địa điểm",
          class: 'ps-4',
          thStyle: {width: '100px', minWidth: '100px'},
          thClass: 'hidden-sortable py-2'
        },
        {
          key: "thanhPhanThanDu",
          label: "Thàn phần tham dự",
          class: 'ps-4',
          thStyle: {width: '100px', minWidth: '100px'},
          thClass: 'hidden-sortable py-2'
        },
        {
          key: "ghiChu",
          label: "Ghi chú",
          class: 'ps-4',
          thStyle: {width: '100px', minWidth: '100px'},
          thClass: 'hidden-sortable py-2'
        },
        {
          key: 'process',
          label: 'Xử lý',
          class: 'ps-4',
          thStyle: {width: '110px', minWidth: '110px'},
          thClass: 'hidden-sortable py-2'
        }
      ],
      config: {
        wrap: true, // set wrap to true only when using 'input-group'
        altFormat: "M j, Y",
        dateFormat: "d/m/Y",
      },
    };
  },
  methods: {
    getInitialView() {
      if (window.innerWidth >= 768 && window.innerWidth < 1200) {
        return "timeGridWeek";
      } else if (window.innerWidth <= 768) {
        return "listMonth";
      } else {
        return "dayGridMonth";
      }
    },

    //Model open for add event

    dateClicked(info) {
      this.newEventData = info;
      this.showModal = true;
    },

    hideModal(e) {
      this.submitted = false;
      this.showModal = false;
      this.event = {};
    },

    editEvent(info, id) {
      this.edit = info.event;
      this.eventModal = true;
      try {
        let promise = this.$store.dispatch("lichCongTacStore/getById" + id)
        return promise.then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.editevent = items;
            return items || []
          }
          return [];
        })
      } finally {
        this.loading = false
      }
    },

    editSubmit(e) {
      this.submit = true;
      const editTitle = this.editevent.tieuDe;
      const editcategory = this.editevent.editcategory;

      this.edit.setProp("title", editTitle);
      this.edit.setProp("classNames", editcategory);
      this.successmsg();
      this.eventModal = false;
    },
    closeModal() {
      this.eventModal = false;
    },

    handleEvents(events) {
      this.currentEvents = events;
    },

    handleSubmit(e) {
      this.submitted = true;
      console.log("HandleSubmit", this.event);

      // stop here if form is invalid
      this.v$.$touch();
      if (this.v$.$invalid) {
        return;
      } else {
        const title = this.event.tieuDe;
        const category = this.event.category;
        let calendarApi = this.newEventData.view.calendar;

        this.currentEvents = calendarApi.addEvent({
          id: this.newEventData.length + 1,
          title,
          start: this.newEventData.date,
          end: this.newEventData.date,
          classNames: [category],
        });
        this.successmsg();
        this.showModal = false;
        this.newEventData = {};
      }
      this.submitted = false;
      this.event = {};
    },

    confirm() {
      Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to delete this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#34c38f",
        cancelButtonColor: "#f46a6a",
        confirmButtonText: "Yes, delete it!",
      }).then((result) => {
        if (result.value) {
          this.deleteEvent();
          Swal.fire("Deleted!", "Event has been deleted.", "success");
        }
      });
    },

    successmsg() {
      Swal.fire({
        position: "center",
        icon: "success",
        title: "Event has been saved",
        showConfirmButton: false,
        timer: 1000,
      });
    },
  }
};
</script>

<template>
  <Layout>
    <PageHeader :title="title" :items="items"/>
    <div class="card">
      <div class="card-body">
        <ul class="nav nav-tabs nav-pills mb-3" role="tablist">
          <li class="nav-item waves-effect waves-light">
            <a class="nav-link active" data-bs-toggle="tab" href="#lich-cong-tac" role="tab" aria-selected="false">
              Lịch cá nhân
            </a>
          </li>
          <li class="nav-item waves-effect waves-light">
            <a class="nav-link" data-bs-toggle="tab" href="#bang-cong-tac" role="tab" aria-selected="false">
              Bảng cá nhân
            </a>
          </li>
        </ul>
        <!-- Tabs panels -->
        <div class="tab-content text-muted">
          <div class="tab-pane active" id="lich-cong-tac" role="tabpanel">
            <div>
              <div class="row page-lctld">
                <div class="col-xl-12">
                  <div class="card card-h-100">
                    <FullCalendar :options="calendarOptions"/>
                  </div>
                </div>
              </div>
              <!-- Modal add -->
              <b-modal
                  v-model="showModal"
                  title="Thêm lịch công tác"
                  title-class="text-black font-18"
                  size="lg"
                  body-class="p-3"
                  hide-footer
              >
                <form @submit.prevent="handleSubmit">
                  <div class="row">
                    <div class="col-6">
                      <div class="row">
                        <!-- Tiêu đề -->
                        <div class="col-12">
                          <div class="mb-2">
                            <label for="tieu-de" class="col-form-label col-form-label-sm">Tiêu đề</label>
                            <input
                                id="tieu-de"
                                v-model="event.tieuDe"
                                type="text"
                                class="form-control form-control-sm"
                                placeholder="Nhập tiêu đề công tác"
                                :class="{ 'is-invalid': submitted && v$.event.tieuDe.$error }"
                            />
                            <div
                                v-if="submitted && v$.event.tieuDe.$error"
                                class="invalid-feedback"
                            >
                        <span v-if="v$.event.tieuDe.required.$message">{{
                            v$.event.tieuDe.required.$message
                          }}</span>
                            </div>
                          </div>
                        </div>
                        <!-- Ngày công tác-->
                        <div class="col-12">
                          <div class="mb-2">
                            <label for="ngay-cong-tac" class="col-form-label col-form-label-sm">Ngày công tác</label>
                            <flat-pickr
                                v-model="event.ngayCongTac"
                                :config="config"
                                class="form-control form-control-sm"
                            ></flat-pickr>
                          </div>
                        </div>
                        <!-- Thời gian -->
                        <div class="col-12">
                          <div class="mb-2">
                            <label for="thoi-gian" class="col-form-label col-form-label-sm">Thời gian</label>
                            <input
                                id="thoi-gian"
                                v-model="event.thoiGian"
                                type="text"
                                class="form-control form-control-sm"
                                placeholder="Nhập thời gian công tác"
                                :class="{ 'is-invalid': submitted && v$.event.thoiGian.$error }"
                            />
                            <div
                                v-if="submitted && v$.event.thoiGian.$error"
                                class="invalid-feedback"
                            >
                        <span v-if="v$.event.thoiGian.required.$message">{{
                            v$.event.thoiGian.required.$message
                          }}</span>
                            </div>
                          </div>
                        </div>
                        <!-- Chủ trì -->
                        <div class="col-12">
                          <div class="mb-2">
                            <label for="chutri" class="col-form-label col-form-label-sm">Chủ trì</label>
                            <input
                                id="chutri"
                                v-model="event.chuTri"
                                type="text"
                                class="form-control form-control-sm"
                                placeholder="Nhập chủ trì"
                            />
                          </div>
                        </div>
                        <!--Màu sắc hiển thị-->
                        <div class="col-12">
                          <div class="mb-2">
                            <label class="control-label">Màu sắc</label>
                            <select
                                v-model="event.category"
                                class="form-control form-control-sm"
                                name="category"
                            >
                              <option
                                  v-for="option in categories"
                                  :key="option.backgroundColor"
                                  :value="`${option.value}`"
                              >
                                {{ option.name }}
                              </option>
                            </select>
                          </div>
                        </div>
                      </div>
                    </div>
                    <div class="col-6">
                      <div class="row">
                        <!-- Địa điểm -->
                        <div class="col-12">
                          <div class="mb-2">
                            <label for="diadiem" class="col-form-label col-form-label-sm">Địa điểm</label>
                            <textarea
                                id="diadiem"
                                v-model="event.diaDiem"
                                type="text"
                                class="form-control form-control-sm"
                                placeholder="Nhập địa điểm"
                                rows="3"
                                :class="{ 'is-invalid': submitted && v$.event.diaDiem.$error }"
                            ></textarea>
                            <div
                                v-if="submitted && v$.event.diaDiem.$error"
                                class="invalid-feedback"
                            >
                        <span v-if="v$.event.diaDiem.required.$message">{{
                            v$.event.diaDiem.required.$message
                          }}</span>
                            </div>
                          </div>
                        </div>
                        <!--Thành phần tham dự-->
                        <div class="col-12">
                          <div class="mb-2">
                            <label for="thanhphanthamdu" class="col-form-label col-form-label-sm">Tiêu đề</label>
                            <textarea
                                id="thanhphanthamdu"
                                v-model="event.thanhPhanThamDu"
                                type="text"
                                rows="3"
                                class="form-control form-control-sm"
                                placeholder="Nhập thành phần tham dự"
                            ></textarea>
                          </div>
                        </div>
                        <!-- Ghi chú-->
                        <div class="col-12">
                          <div class="mb-2">
                            <label for="ghichu" class="col-form-label col-form-label-sm">Ghi chú</label>
                            <textarea
                                id="ghichu"
                                v-model="event.ghiChu"
                                type="text"
                                class="form-control form-control-sm"
                                placeholder="Nhập ghi chú..."
                                rows="3"
                            ></textarea>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                  <div class="text-end pt-2 mt-2">
                    <b-button variant="light" @click="hideModal">Đóng</b-button>
                    <b-button type="submit" variant="primary" class="ms-1"
                    >Lưu
                    </b-button
                    >
                  </div>
                </form>
              </b-modal>

              <!-- Modal detail -->
              <b-modal
                  v-model="eventModal"
                  title="Thông tin chi tiết"
                  title-class="text-black font-18"
                  size="lg"
                  hide-footer
                  body-class="p-3"
              >
                <form @submit.prevent="editSubmit">
                  <div class="row">
                    <div class="col-6">
                      <div class="row">
                        <!-- Tiêu đề -->
                        <div class="col-12">
                          <div class="mb-2">
                            <label for="tieu-de" class="col-form-label col-form-label-sm">Tiêu đề</label>
                            <input
                                id="tieu-de"
                                v-model="editevent.tieuDe"
                                type="text"
                                class="form-control form-control-sm"
                                placeholder="Nhập tiêu đề công tác"
                                :class="{ 'is-invalid': submitted && v$.event.tieuDe.$error }"
                            />
                            <div
                                v-if="submitted && v$.event.tieuDe.$error"
                                class="invalid-feedback"
                            >
                        <span v-if="v$.event.tieuDe.required.$message">{{
                            v$.event.tieuDe.required.$message
                          }}</span>
                            </div>
                          </div>
                        </div>
                        <!-- Ngày công tác-->
                        <div class="col-12">
                          <div class="mb-2">
                            <label for="ngay-cong-tac" class="col-form-label col-form-label-sm">Ngày công tác</label>
                            <flat-pickr
                                v-model="editevent.ngayCongTac"
                                :config="config"
                                class="form-control form-control-sm"
                            ></flat-pickr>
                          </div>
                        </div>
                        <!-- Thời gian -->
                        <div class="col-12">
                          <div class="mb-2">
                            <label for="thoi-gian" class="col-form-label col-form-label-sm">Thời gian</label>
                            <input
                                id="thoi-gian"
                                v-model="editevent.thoiGian"
                                type="text"
                                class="form-control form-control-sm"
                                placeholder="Nhập thời gian công tác"
                                :class="{ 'is-invalid': submitted && v$.event.thoiGian.$error }"
                            />
                            <div
                                v-if="submitted && v$.event.thoiGian.$error"
                                class="invalid-feedback"
                            >
                        <span v-if="v$.event.thoiGian.required.$message">{{
                            v$.event.thoiGian.required.$message
                          }}</span>
                            </div>
                          </div>
                        </div>
                        <!-- Chủ trì -->
                        <div class="col-12">
                          <div class="mb-2">
                            <label for="chutri" class="col-form-label col-form-label-sm">Chủ trì</label>
                            <input
                                id="chutri"
                                v-model="editevent.chuTri"
                                type="text"
                                class="form-control form-control-sm"
                                placeholder="Nhập chủ trì"
                            />
                          </div>
                        </div>
                        <!--Màu sắc hiển thị-->
                        <div class="col-12">
                          <div class="mb-2">
                            <label class="control-label">Màu sắc</label>
                            <select
                                v-model="editevent.category"
                                class="form-control form-control-sm"
                                name="category"
                            >
                              <option
                                  v-for="option in categories"
                                  :key="option.backgroundColor"
                                  :value="`${option.value}`"
                              >
                                {{ option.name }}
                              </option>
                            </select>
                          </div>
                        </div>
                      </div>
                    </div>
                    <div class="col-6">
                      <div class="row">
                        <!-- Địa điểm -->
                        <div class="col-12">
                          <div class="mb-2">
                            <label for="diadiem" class="col-form-label col-form-label-sm">Địa điểm</label>
                            <textarea
                                id="diadiem"
                                v-model="editevent.diaDiem"
                                type="text"
                                class="form-control form-control-sm"
                                placeholder="Nhập địa điểm"
                                rows="3"
                                :class="{ 'is-invalid': submitted && v$.event.diaDiem.$error }"
                            ></textarea>
                            <div
                                v-if="submitted && v$.event.diaDiem.$error"
                                class="invalid-feedback"
                            >
                        <span v-if="v$.event.diaDiem.required.$message">{{
                            v$.event.diaDiem.required.$message
                          }}</span>
                            </div>
                          </div>
                        </div>
                        <!--Thành phần tham dự-->
                        <div class="col-12">
                          <div class="mb-2">
                            <label for="thanhphanthamdu" class="col-form-label col-form-label-sm">Tiêu đề</label>
                            <textarea
                                id="thanhphanthamdu"
                                v-model="editevent.thanhPhanThamDu"
                                type="text"
                                rows="3"
                                class="form-control form-control-sm"
                                placeholder="Nhập thành phần tham dự"
                            ></textarea>
                          </div>
                        </div>
                        <!-- Ghi chú-->
                        <div class="col-12">
                          <div class="mb-2">
                            <label for="ghichu" class="col-form-label col-form-label-sm">Ghi chú</label>
                            <textarea
                                id="ghichu"
                                v-model="editevent.ghiChu"
                                type="text"
                                class="form-control form-control-sm"
                                placeholder="Nhập ghi chú..."
                                rows="3"
                            ></textarea>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                  <div class="text-end p-3">
                    <b-button variant="light" @click="closeModal">Đóng</b-button>
                    <b-button class="ms-1" variant="danger" @click="confirm"
                    >Xóa
                    </b-button
                    >
                    <b-button class="ms-1" variant="success" @click="editSubmit"
                    >Lưu
                    </b-button
                    >
                  </div>
                </form>
              </b-modal>
            </div>
          </div>
          <div class="tab-pane" id="bang-cong-tac" role="tabpanel">
            <div class="row page-lctld">
              <div class="col-xl-12">
                <div class="card car-h-100">
                  <div class="row">
                    <div class="col-12">
                      <!--  Table -->
                      <b-table
                          class="table-light align-middle table-nowrap mb-0"
                          :items="data"
                          small
                          :fields="fields"
                          responsive="sm"
                          :per-page="perPage"
                          :current-page="currentPage"
                          :filter="filter"
                          headerBgVariant="bg-primary-dark"
                          ref="tblList"
                          primary-key="id"
                      >
                        <template v-slot:cell(STT)="data">
                          {{ data.index + ((currentPage-1)*perPage) + 1  }}
                        </template>
                        <template v-slot:cell(ngayCongTac)="data">
                          <div class="ps-2">
                            {{data.item.ngayCongTac}}
                          </div>
                        </template>
                        <template v-slot:cell(thoiGian)="data">
                          <div class="ps-2">
                            {{data.item.thoiGian}}
                          </div>
                        </template>
                        <template v-slot:cell(tieuDe)="data">
                          <div class="ps-2">
                            {{data.item.tieuDe}}
                          </div>
                        </template>
                        <template v-slot:cell(chuTri)="data">
                          <div class="ps-2">
                            {{data.item.chuTri}}
                          </div>
                        </template>
                        <template v-slot:cell(diaDiem)="data">
                          <div class="ps-2">
                            {{data.item.diaDiem}}
                          </div>
                        </template>
                        <template v-slot:cell(thanhPhanThamDu)="data">
                          <div class="ps-2">
                            {{data.item.thanhPhanThamDu}}
                          </div>
                        </template>
                        <template v-slot:cell(ghiChu)="data">
                          <div class="ps-2">
                            {{data.item.ghiChu}}
                          </div>
                        </template>
                        <template v-slot:cell(process)="data">
                          <button
                              type="button"
                              size="sm"
                              class="btn btn-outline btn-sm"
                              data-toggle="tooltip" data-placement="bottom" title="Chi tiết"
                              v-on:click="handleDetail(data.item.id)">
                            <i class="ri-eye-fill  text-warning me-1"></i>
                          </button>
                          <button
                              type="button"
                              size="sm"
                              class="btn btn-outline btn-sm"
                              data-toggle="tooltip" data-placement="bottom" title="Cập nhật"
                              v-on:click="handleUpdate(data.item.id)">
                            <i class="ri-edit-2-fill text-success me-1"></i>
                          </button>
                          <button
                              type="button"
                              size="sm"
                              class="btn btn-outline btn-sm"
                              data-toggle="tooltip" data-placement="bottom" title="Xóa"
                              v-on:click="handleShowDeleteModal(data.item.id)">
                            <i class=" ri-delete-bin-fill text-danger me-1"></i>
                          </button>
                        </template>
                      </b-table>
                    </div>
                  </div>
                  <div
                      class="align-items-center mt-2 row g-3 text-center text-sm-start px-3 mb-3"
                  >
                    <div class="col-sm">
                      <div class="text-muted">
                        Hiển thị<span class="fw-semibold">{{dataTable.length}}</span> of
                        <span class="fw-semibold">{{totalRows}}</span> kết quả
                      </div>
                    </div>
                    <div class="col-sm-auto">
                      <div class="col">
                        <div
                            class="dataTables_paginate paging_simple_numbers float-end">
                          <ul class="pagination pagination-rounded mb-0">
                            <!-- pagination -->
                            <b-pagination
                                v-model="currentPage"
                                :model-value.sync="currentPage"
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
          </div>
        </div>
      </div>
    </div>
  </Layout>
</template>

<style></style>
