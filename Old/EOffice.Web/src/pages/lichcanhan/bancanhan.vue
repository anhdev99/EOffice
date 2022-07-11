<script>
import {INITIAL_EVENTS} from "./utils";
import ChinhSua from "./chinhsua.vue";

export default {

  data() {
    return {
      data: INITIAL_EVENTS,
    };
  },
  components: {ChinhSua},
  methods: {},
};
</script>

<template>
  <div class="row page-lctld">
    <div class="col-xl-12">
      <div class="card car-h-100">
        <div class="row">
          <div class="col-12">
            <!--  Table -->
            <table class="table align-middle mb-0">
              <thead class="table-light">
              <tr>
                <th scope="col">#</th>
                <th scope="col">Tên</th>
                <th scope="col">Nội dung</th>
                <th scope="col">Thao tác</th>
              </tr>
              </thead>
              <tbody>
              <tr v-if="data.length <= 0" class="text-center">
                <td colspan="6">Không tìm thấy dữ liệu</td>
              </tr>
              <tr v-else v-for="(item, index) in data" :key="item.id" class="bct-content-box">
                <td>{{ ++index }}</td>
                <td>{{ item.title }}</td>
                <td class="bct-description">{{ item.description }}</td>
                <td>
                  <div class="hstack gap-3 fs-15">
                    <a href="javascript:void(0);" class="link-info"
                    ><i class="ri-newspaper-line"></i
                    ></a>
                    <a
                        href="javascript:void(0);"
                        class="link-primary edit-btn"
                        data-bs-toggle="modal"
                        data-bs-target="#chinh-sua"
                    ><i class="ri-edit-2-line"></i
                    ></a>
                    <a
                        href="javascript:void(0);"
                        class="link-danger"
                        data-bs-toggle="modal"
                        data-bs-target="#delete"
                    ><i class="ri-delete-bin-5-line"></i
                    ></a>
                  </div>
                </td>
              </tr>
              </tbody>
            </table>
          </div>
        </div>
        <div
            class="align-items-center mt-2 row g-3 text-center text-sm-start px-3 mb-3"
        >
          <div class="col-sm">
            <div class="text-muted">
              Hiển thị<span class="fw-semibold">4</span> of
              <span class="fw-semibold">125</span> kết quả
            </div>
          </div>
          <div class="col-sm-auto">
            <ul
                class="pagination pagination-separated pagination-sm justify-content-center justify-content-sm-start mb-0"
            >
              <li class="page-item disabled">
                <a href="#" class="page-link">←</a>
              </li>
              <li class="page-item">
                <a href="#" class="page-link">1</a>
              </li>
              <li class="page-item active">
                <a href="#" class="page-link">2</a>
              </li>
              <li class="page-item">
                <a href="#" class="page-link">3</a>
              </li>
              <li class="page-item">
                <a href="#" class="page-link">→</a>
              </li>
            </ul>
          </div>
        </div>
      </div>
    </div>
    <!-- Modal add - edit -->
    <div
        class="modal fade zoomIn"
        id="chinh-sua"
        tabindex="-1"
        aria-hidden="true"
    >
      <div class="modal-dialog modal-dialog-centered modal-lg">
        <div class="modal-content border-0">
          <div class="modal-header p-3 bg-primary-dark">
            <h5 class="modal-title">Chỉnh sửa lịch công tác</h5>
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
          <div class="p-3">
            <chinh-sua/>
          </div>
        </div>
      </div>
    </div>
    <!-- Modal delete -->
    <div
        class="modal fade"
        id="delete"
        data-bs-backdrop="static"
        data-bs-keyboard="false"
        tabindex="-1"
        role="dialog"
        aria-labelledby="staticBackdropLabel"
        aria-hidden="true"
    >
      <div class="modal-dialog modal-dialog-centered" role="document">
        <div class="modal-content">
          <div class="modal-body text-center p-5">
            <lord-icon
                src="https://cdn.lordicon.com/lupuorrc.json"
                trigger="loop"
                colors="primary:#121331,secondary:#08a88a"
                style="width: 120px; height: 120px"
            >
            </lord-icon>

            <div class="mt-4">
              <div class="mb-3 cl-warning">
                <i class="ri-alert-line fs-1"></i>
              </div>
              <p class="text-muted mb-4">Bạn thật sự muốn xóa dữ liệu.</p>
              <div class="hstack gap-2 justify-content-center">
                <a
                    href="javascript:void(0);"
                    class="btn btn-link link-danger fw-medium"
                    data-bs-dismiss="modal"
                ><i class="ri-close-line me-1 align-middle"></i> Đóng</a
                >
                <a
                    href="javascript:void(0);"
                    data-bs-dismiss="modal"
                    class="btn btn-success"
                >Đồng ý</a
                >
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<style>
.modal-title {
  color: #fff;
}

.bg-primary-dark {
  background: linear-gradient(135deg, #06548e, #ffffff);
  box-shadow: 0px 3px 0px #06548e;
}

.cl-warning {
  color: rgb(253, 216, 123);
}
</style>
