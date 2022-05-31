<script>
import Layout from "@/layouts/main";
import PageHeader from "@/components/page-header";
import appConfig from "../../../../../app.config.json";
import { data } from "./data";
import KySo from "./vbtrinhky.vue";
import XacThuc from "./xacthuc.vue";

export default {
  page: {
    title: "Chữ ký số",
    meta: [
      {
        content: appConfig.description,
      },
    ],
  },
  data() {
    return {
      title: "Chữ ký số",
      items: [
        {
          text: "Quản lý văn bản đi",
          href: "/",
        },
        {
          text: "Chử ký số",
          active: true,
        },
      ],
      data: data,
      modalShow: false,
    };
  },
  components: { Layout, PageHeader, KySo, XacThuc },
  methods: {
    ThemMoi() {
      document.getElementById("ThemMoi").reset();
      document.getElementById("CreateModalLabel").innerHTML =
        "Thêm mới văn bản đi";
      document.getElementById("ThemMoi").style.display = "block";
    },
    KySo() {
      document.getElementById("KySo").reset();
      document.getElementById("EditModalLabel").innerHTML =
        "Chỉnh sửa văn bản đi";
      document.getElementById("KySo").style.display = "block";
    },
  },
};
</script>

<template>
  <Layout>
    <PageHeader :title="title" :items="items" />

    <div class="row page-vanbanden">
      <div class="col-xl-12">
        <div class="card">
          <div class="card-header align-items-center d-flex">
            <h4 class="card-title mb-0 flex-grow-1">Danh sách văn bản đến</h4>
          </div>
          <div class="row">
            <div class="col-12">
              <!--  Table -->
              <table class="table align-middle table-nowrap mb-0">
                <thead class="table-light">
                  <tr>
                    <th scope="col">#</th>
                    <th scope="col">Số Lưu CV</th>
                    <th scope="col">Số CB đi</th>
                    <th scope="col">Trích yếu</th>
                    <th scope="col">Loại văn bản</th>
                    <th scope="col">Trạng thái</th>
                    <th scope="col">Cơ quan nhận</th>
                    <th scope="col">Ngày nhập</th>
                    <th scope="col">Thao tác</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-if="data.length <= 0" class="text-center">
                    <td colspan="6">Không tìm thấy dữ liệu</td>
                  </tr>
                  <tr v-else v-for="(item, index) in data" :key="item.id">
                    <td>{{ ++index }}</td>
                    <td>{{ item.soLuuCV }}</td>
                    <td>{{ item.soCVDi }}</td>
                    <td>{{ item.trichYeu }}</td>
                    <td>{{ item.loaiVanBan }}</td>
                    <td v-if="item.trangThai == 0">
                      <span class="badge badge-soft-secondary"
                        >Vừa tiếp nhận</span
                      >
                    </td>
                    <td v-if="item.trangThai == 1">
                      <span class="badge badge-soft-primary">Đã tiếp nhận</span>
                    </td>
                    <td v-if="item.trangThai == 2">
                      <span class="badge badge-soft-success">Hoàn thành</span>
                    </td>
                    <td>{{ item.coQuanNhan }}</td>
                    <td>{{ item.ngayNhap }}</td>
                    <td>
                      <div class="hstack gap-3 fs-15">
                        <a href="javascript:void(0);" class="link-info"
                          ><i class="ri-newspaper-line"></i
                        ></a>
                        <a href="javascript:void(0);" class="link-info"
                          ><i class="ri-download-2-line"></i
                        ></a>
                        <a
                          href="#KySo"
                          class="link-primary edit-btn"
                          data-bs-toggle="modal"
                          @click="KySo"
                          ><i class="las la-feather-alt"></i
                        ></a>
                        <a href="javascript:void(0);" class="link-danger"
                          ><i class="ri-delete-bin-5-line"></i
                        ></a>
                      </div>
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Edit modal form -->
    <div
      class="modal fade zoomIn"
      id="KySo"
      tabindex="-1"
      aria-labelledby="EditModalLabel"
      aria-hidden="true"
    >
      <div class="modal-dialog modal-dialog-centered modal-fullscreen">
        <div class="modal-content border-0">
          <div class="modal-header p-3 bg-primary-dark">
            <h5 class="modal-title" id="EditModalLabel">Văn bản trình ký</h5>
            <div class="d-flex">
              <b-button
                type="button"
                class="btn btn-sm btn-danger waves-effect waves-light me-2 d-flex align-items-center"
              >
                <i class="ri-save-3-fill me-1"></i>
                Hủy ký số
              </b-button>
              <b-button
                type="button"
                class="btn btn-sm btn-primary waves-effect waves-light me-2 d-flex align-items-center"
                @click="modalShow = !modalShow"
              >
                <i class="ri-save-3-fill me-1"></i>
                Ký số
              </b-button>

              <button
                type="button"
                class="btn btn-sm btn-danger waves-effect waves-light me-2 d-flex align-items-center"
                data-bs-dismiss="modal"
                aria-label="Close"
                id="close-modal"
              >
                <i class="ri-close-line me-1"></i>
                Đóng
              </button>
            </div>
          </div>
          <!-- start content -->
          <ky-so />
          <!-- end content -->
        </div>
      </div>
    </div>

    <!-- modal ký số -->
    <b-modal
      v-model="modalShow"
      header-class="modal-header p-3 bg-primary-dark "
      content-class="modal-content border-0 "
      dialog-class="modal-dialog modal-dialog-centered modal-lg"
      title="Ký số"
      no-close-on-backdrop
      modal-class="bg-modal modal-shadown"
    >
      <xac-thuc />
    </b-modal>
  </Layout>
</template>
<style>
.modal-title {
  color: #fff;
}
.bg-primary-dark {
  background: linear-gradient(135deg, #06548e, #ffffff);
  box-shadow: 0px 3px 0px #06548e;
}

.bg-modal {
  background: rgba(0, 0, 0, 0.5);
}
</style>
