<script>
import Layout from "@/layouts/main";
import PageHeader from "@/components/page-header";
import ButPhe from "./butphe.vue";
import PhanCong from "./phancong.vue";
import { data } from "./data";

export default {
  page: {
    title: "Văn bản đến",
    meta: [
      {
        name: "van-ban-den",
      },
    ],
  },
  data() {
    return {
      title: "Văn bản đến",
      items: [
        {
          text: "Quản lý văn bản đến",
          href: "/",
        },
        {
          text: "Văn bản đến",
          active: true,
        },
      ],
      data: data,
    };
  },
  components: { Layout, PageHeader, ButPhe, PhanCong },
  methods: {
    ThemMoi() {
      document.getElementById("ThemMoi").reset();
      document.getElementById("CreateModalLabel").innerHTML =
        "Thêm mới văn bản đến";
      document.getElementById("ThemMoi").style.display = "block";
    },
    ChinhSua() {
      document.getElementById("ChinhSua").reset();
      document.getElementById("EditModalLabel").innerHTML =
        "Chỉnh sửa văn bản đến";
      document.getElementById("ChinhSua").style.display = "block";
    },
    PhanCongXuLy() {
      document.getElementById("PhanCongXuLy").reset();
      document.getElementById("PhanCongXuLyModalLabel").innerHTML =
        "Phân công xử lý";
      document.getElementById("PhanCongXuLy").style.display = "block";
    },
  },
};
</script>

<template>
  <Layout>
    <PageHeader :title="title" :items="items" />

    <div class="row">
      <div class="col-xl-12">
        <div class="card">
          <div class="card-header align-items-center d-flex">
            <h4 class="card-title mb-0 flex-grow-1">Danh sách văn bản đến</h4>

            <div class="flex-shrink-0">
              <div
                class="form-check form-switch form-switch-right form-switch-md"
              >
                <button
                  class="btn btn-primary add-btn btn-sm"
                  data-bs-toggle="modal"
                >
                  <i class="ri-add-line align-bottom me-1"></i> PDF
                </button>
              </div>
              <div
                class="form-check form-switch form-switch-right form-switch-md"
              >
                <button
                  class="btn btn-primary add-btn btn-sm"
                  data-bs-toggle="modal"
                >
                  <i class="ri-add-line align-bottom me-1"></i> EXCEL
                </button>
              </div>
            </div>
          </div>
          <div class="row">
            <div class="col-12">
              <!--  Table -->
              <table class="table align-middle table-nowrap mb-0">
                <thead class="table-light">
                  <tr>
                    <th scope="col">#</th>
                    <th scope="col">Số Lưu</th>
                    <th scope="col">Số VB đến</th>
                    <th scope="col">Trích yếu</th>
                    <th scope="col">Hạn xử lý</th>
                    <th scope="col">Trạng thái VB</th>
                    <th scope="col">Chuyển xử lý</th>
                    <th scope="col">Thao tác</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-if="data.length <= 0" class="text-center">
                    <td colspan="6">Không tìm thấy dữ liệu</td>
                  </tr>
                  <tr v-else v-for="(item, index) in data" :key="item.id">
                    <td>{{ ++index }}</td>
                    <td>{{ item.soluu }}</td>
                    <td>{{ item.sovanbanden }}</td>
                    <td>{{ item.trichyeu }}</td>
                    <td>
                      <span class="badge badge-soft-danger">{{
                        item.hanxuxly
                      }}</span>
                    </td>
                    <td v-if="item.trangthaivanban == 0">
                      <span class="badge badge-soft-secondary"
                        >Vừa tiếp nhận</span
                      >
                    </td>
                    <td v-if="item.trangthaivanban == 1">
                      <span class="badge badge-soft-primary">Đã tiếp nhận</span>
                    </td>
                    <td v-if="item.trangthaivanban == 2">
                      <span class="badge badge-soft-success">Hoàn thành</span>
                    </td>
                    <td v-if="item.chuyenXuLy == 0">
                      <button
                        class="badge badge-soft-secondary border-0"
                        data-bs-toggle="modal"
                        href="#PhanCongXuLy"
                        @click="PhanCongXuLy"
                      >
                        <i class="ri-send-plane-fill"></i> Chưa chuyển
                      </button>
                    </td>
                    <td v-if="item.chuyenXuLy == 1">
                      <span class="badge badge-soft-success"
                        ><i class="ri-send-plane-fill"></i> Đã chuyển</span
                      >
                    </td>
                    <td>
                      <div class="hstack gap-3 fs-15">
                        <a href="javascript:void(0);" class="link-info"
                          ><i class="ri-newspaper-line"></i
                        ></a>
                        <a
                          href="#ChinhSua"
                          class="link-primary edit-btn"
                          data-bs-toggle="modal"
                          @click="ChinhSua"
                          ><i class="ri-edit-2-line"></i
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

    <!--  Modal chuyển xử lý -->
    <div
      class="modal fade zoomIn"
      id="PhanCongXuLy"
      tabindex="-1"
      aria-labelledby="PhanCongXuLyModalLabel"
      aria-hidden="true"
    >
      <div class="modal-dialog modal-dialog-centered modal-lg">
        <div class="modal-content border-0">
          <div class="modal-header p-3 bg-primary-dark">
            <h5 class="modal-title" id="PhanCongXuLyModalLabel">
              Phân công xử lý
            </h5>
            <div class="d-flex">
              <button
                type="button"
                class="btn btn-sm btn-primary waves-effect waves-light me-2 d-flex align-items-center"
              >
                <i class="ri-save-3-fill me-1"></i>
                Lưu
              </button>

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
          <div class="p-3">
            <phan-cong />
          </div>

          <!-- end content -->
        </div>
      </div>
    </div>

    <!-- Modal chỉnh sửa -->
    <div
      class="modal fade zoomIn"
      id="ChinhSua"
      tabindex="-1"
      aria-labelledby="EditModalLabel"
      aria-hidden="true"
    >
      <div class="modal-dialog modal-dialog-centered modal-fullscreen">
        <div class="modal-content border-0">
          <div class="modal-header p-3 bg-primary-dark">
            <h5 class="modal-title" id="EditModalLabel">
              Bút phê đơn vị lãnh đạo, đơn vị nhận / Xử lý văn bản
            </h5>
            <div class="d-flex">
              <button
                type="button"
                class="btn btn-sm btn-primary waves-effect waves-light me-2 d-flex align-items-center"
              >
                <i class="ri-save-3-fill me-1"></i>
                Lưu
              </button>

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
          <!-- start content  -->
          <but-phe />
          <!-- end content  -->
        </div>
      </div>
    </div>
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
</style>
