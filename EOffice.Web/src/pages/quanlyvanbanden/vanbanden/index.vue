<script>

import Layout from "../../../layouts/main";
import PageHeader from "@/components/page-header";
import appConfig from "../../../../app.config.json";
import {data} from "./data";

import ThemVanVanDen from "./themvanbanden";

export default {
  page: {
    title: "Văn bản đến",
    meta: [{
      name: "van-ban-den",
      content: appConfig.description
    }],
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
        }
      ],
      data: data,
    };
  },
  components: {Layout, PageHeader, ThemVanVanDen},
  methods: {
    addnew() {
      document.getElementById("addform").reset();
      document.getElementById('exampleModalLabel').innerHTML = "Thêm mới văn bản đến";
      document.getElementById('add-btn').style.display = 'block';
      document.getElementById('edit-btn').style.display = 'none';
    },
  },
}
</script>

<template>
  <Layout>
    <PageHeader :title="title" :items="items"/>

    <div class="row">
      <div class="col-xl-12">
        <div class="card">
          <div class="card-header align-items-center d-flex">
            <h4 class="card-title mb-0 flex-grow-1">Danh sách văn bản đến</h4>

            <div class="flex-shrink-0">
              <div class="form-check form-switch form-switch-right form-switch-md">
                <button class="btn btn-primary add-btn btn-sm" data-bs-toggle="modal" href="#showmodal" @click="addnew">
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
                  <th scope="col">Số Lưu</th>
                  <th scope="col">Số VB đến</th>
                  <th scope="col">Trích yếu</th>
                  <th scope="col">Hạn xử lý</th>
                  <th scope="col">Trạng thái VB</th>
                  <th scope="col">Thao tác</th>
                </tr>
                </thead>
                <tbody>
                <tr v-if="data.length <= 0" class="text-center">
                  <td colspan="6">Không tìm thấy dữ liệu</td>
                </tr>
                <tr v-else
                    v-for="(item, index) in data"
                    :key="item.id"
                >
                  <td>{{++index}}</td>
                  <td>{{item.soluu}}</td>
                  <td>{{item.sovanbanden}}</td>
                  <td>{{item.trichyeu}}</td>
                  <td><span class="badge badge-soft-danger">{{item.hanxuxly}}</span></td>
                  <td v-if="item.trangthaivanban == 0">
                    <span class="badge badge-soft-secondary ">Vừa tiếp nhận</span>
                  </td>
                  <td v-if="item.trangthaivanban == 1">
                    <span class="badge badge-soft-primary ">Đã tiếp nhận</span>
                  </td>
                  <td v-if="item.trangthaivanban == 2">
                    <span class="badge badge-soft-success">Hoàn thành</span>
                  </td>
                  <td>
                    <div class="hstack gap-3 fs-15">
                      <a href="javascript:void(0);" class="link-info"><i class="ri-newspaper-line"></i></a>
                      <a href="javascript:void(0);" class="link-primary"><i class="ri-edit-2-line"></i></a>
                      <a href="javascript:void(0);" class="link-danger"><i class="ri-delete-bin-5-line"></i></a>
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

    <!--    modal form  -->
    <div class="modal fade zoomIn" id="showmodal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
      <div class="modal-dialog modal-dialog-centered modal-fullscreen">
        <div class="modal-content border-0">
          <div class="modal-header p-3 bg-soft-dark">
            <h5 class="modal-title" id="exampleModalLabel"></h5>
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
          <them-van-van-den/>
        </div>
      </div>
    </div>

  </Layout>
</template>