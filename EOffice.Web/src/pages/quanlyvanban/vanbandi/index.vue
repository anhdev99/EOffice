<script>
import Layout from "../../../layouts/main";
import PageHeader from "@/components/page-header";
import appConfig from "../../../../app.config.json";
import {data} from "./data";
import VanBanDi from "./modalvanbandi.vue";
import PhanCong from "./phancong.vue";

export default {
  page: {
    title: "Văn bản đến",
    meta: [
      {
        name: "van-ban-den",
        content: appConfig.description,
      },
    ],
  },
  data() {
    return {
      title: "Văn bản đi",
      items: [
        {
          text: "Quản lý văn bản đi",
          href: "/",
        },
        {
          text: "Văn bản đi",
          active: true,
        },
      ],
      data: data,
    };
  },
  components: {Layout, PageHeader, VanBanDi, PhanCong},
  methods: {
    HandleSubmit(e){
      e.preventDefault();
      console.log("handle submit");
    }
  },
};
</script>

<template>
  <Layout>
    <PageHeader :title="title" :items="items"/>

    <div class="row page-vanbanden">
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
                    data-bs-target="#them-moi"
                >
                  <i class="ri-add-line align-bottom me-1"></i> Thêm mới
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
                          href="javascript:void(0);"
                          class="link-info"
                          data-bs-toggle="modal"
                          data-bs-target="#phancong"
                      ><i class="ri-user-add-line"></i
                      ></a>
                      <a
                          class="link-primary edit-btn"
                          data-bs-toggle="modal"
                          data-bs-target="#chinh-sua"
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

    <!--  create modal form  -->
    <div
        class="modal fade zoomIn"
        id="them-moi"
        tabindex="-1"
        aria-labelledby="CreateModalLabel"
        aria-hidden="true"
    >
      <div class="modal-dialog modal-dialog-centered modal-fullscreen">
        <div class="modal-content border-0">
          <form class="" @submit="HandleSubmit" novalidate>
            <div class="modal-header p-3 bg-primary-dark">
              <h5 class="modal-title" id="CreateModalLabel">
                Thêm mới văn bản đi
              </h5>
              <div class="d-flex">
                <button
                    type="submit"
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
            <van-ban-di/>
            <!-- end content -->
          </form>
        </div>
      </div>
    </div>

    <!-- Edit modal form -->
    <div
        class="modal fade zoomIn"
        id="chinh-sua"
        tabindex="-1"
        aria-labelledby="EditModalLabel"
        aria-hidden="true"
    >
      <div class="modal-dialog modal-dialog-centered modal-fullscreen">
        <div class="modal-content border-0">
          <div class="modal-header p-3 bg-primary-dark">
            <h5 class="modal-title" id="EditModalLabel">
              Chỉnh sửa văn bản đi
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
          <van-ban-di/>
          <!-- end content -->
        </div>
      </div>
    </div>
    <!-- Modal phân công -->
    <div
        class="modal fade zoomIn"
        id="phancong"
        tabindex="-1"
        aria-labelledby="phancongLabel"
        aria-hidden="true"
    >
      <div class="modal-dialog modal-dialog-centered modal-lg">
        <div class="modal-content border-0">
          <div class="modal-header p-3 bg-primary-dark">
            <h5 class="modal-title" id="phancongLabel">Phân công</h5>
            <button
                type="button"
                class="btn-close"
                data-bs-dismiss="modal"
                aria-label="Close"
            ></button>
          </div>
          <div class="modal-body p-3">
            <phan-cong/>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-light" data-bs-dismiss="modal">
              Đóng
            </button>
            <button type="button" class="btn btn-primary">Lưu</button>
          </div>
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
