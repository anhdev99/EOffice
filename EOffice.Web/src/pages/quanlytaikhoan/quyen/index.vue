<script>
import {defineComponent, ref, reactive} from 'vue';
import Layout from "@/layouts/main";
import PageHeader from "@/components/page-header";
import appConfig from "../../../../app.config.json";
import {data} from "./data";
import ThemMoi from "./themmoi.vue";
import ChinhSua from "./chinhsua.vue";

export default {
  page: {
    title: "Quản lý vai trò",
    meta: [
      {
        content: appConfig.description,
      },
    ],
  },
  data() {
    return {
      title: "Quản lý vai trò",
      items: [
        {
          text: "Trang chủ",
          href: "/",
        },
        {
          text: "quản lý vai trò khoản",
          active: true,
        },
      ],
      data: data,
      item: null,
      dataModel: [
        {
          id: "numberOrString", label: "Root Node", children: [
            {id: 1, label: "Child Node"},
            {id: "node2", label: "Second Child"}]
        }],
      config: {
        roots: ["id1", "id2"],
      },
      nodes: {
        id1: {
          text: "text1",
          children: ["id11", "id12"],
        },
        id11: {
          text: "text11",
        },
        id12: {
          text: "text12",
        },
        id2: {
          text: "text2",
        },
      },
      selected: ref([]),
      treeOrientation: ref("0"),
      treeData: reactive({
        label: 'root',
        expand: true,
        some_id: 1,
        children: [
          {label: 'child 1', some_id: 2,},
          {label: 'child 2', some_id: 3,},
          {
            label: 'subparent 1',
            some_id: 4,
            expand: false,
            children: [
              {label: 'subchild 1', some_id: 5},
              {
                label: 'subchild 2',
                some_id: 6,
                expand: false,
                children: [
                  {label: 'subchild 11', some_id: 7},
                  {label: 'subchild 22', some_id: 8},
                ]
              },
            ]
          },
        ]
      }),
      items1: [
        { age: 40, first_name: 'Dickerson', last_name: 'Macdonald' },
        { age: 21, first_name: 'Larsen', last_name: 'Shaw' },
        { age: 89, first_name: 'Geneva', last_name: 'Wilson' },
        { age: 38, first_name: 'Jami', last_name: 'Carney' }
      ]

    };
  },
  components: {Layout, PageHeader, ChinhSua},
  methods: {
    toggleSelect(node, isSelected) {
      isSelected ? this.selected.value.push(node.some_id) : this.selected.value.splice(this.selected.value.indexOf(node.some_id), 1);
      if (node.children && node.children.length) {
        node.children.forEach(ch => {
          this.toggleSelect(ch, isSelected)
        })
      }
    },
    myProvider(ctx){
      console.log("ctx")
      return this.items1
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
            <h4 class="card-title mb-0 flex-grow-1"></h4>
            <!-- Button -->
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
              <SimpleTable striped hover :items="myProvider"></SimpleTable>

              <!--  Table -->
<!--              <table class="table align-middle table-nowrap mb-0">-->
<!--                <thead class="table-light">-->
<!--                <tr>-->
<!--                  <th scope="col">#</th>-->
<!--                  <th scope="col">Code</th>-->
<!--                  <th scope="col">Tên quyền</th>-->
<!--                  <th scope="col">Thời gian tạo</th>-->
<!--                  <th scope="col">Cập nhật</th>-->
<!--                  <th scope="col">Thao tác</th>-->
<!--                </tr>-->
<!--                </thead>-->
<!--                <tbody>-->
<!--                <tr v-if="data.length <= 0" class="text-center">-->
<!--                  <td colspan="6">Không tìm thấy dữ liệu</td>-->
<!--                </tr>-->
<!--                <tr v-else v-for="(item, index) in data" :key="item.id">-->
<!--                  <td>{{ ++index }}</td>-->
<!--                  <td>{{ item.code }}</td>-->
<!--                  <td>{{ item.fullName }}</td>-->
<!--                  <td>{{ item.createDate }}</td>-->
<!--                  <td>{{ item.updateDate }}</td>-->
<!--                  <td>-->
<!--                    <div class="hstack gap-3 fs-15">-->
<!--                      <a href="javascript:void(0);" class="link-info"-->
<!--                      ><i class="ri-newspaper-line"></i-->
<!--                      ></a>-->
<!--                      <a-->
<!--                          href="javascript:void(0);"-->
<!--                          class="link-primary edit-btn"-->
<!--                          data-bs-toggle="modal"-->
<!--                          data-bs-target="#chinh-sua"-->
<!--                      ><i class="ri-edit-2-line"></i-->
<!--                      ></a>-->
<!--                      <a-->
<!--                          href="javascript:void(0);"-->
<!--                          class="link-danger"-->
<!--                          data-bs-toggle="modal"-->
<!--                          data-bs-target="#delete"-->
<!--                      ><i class="ri-delete-bin-5-line"></i-->
<!--                      ></a>-->
<!--                    </div>-->
<!--                  </td>-->
<!--                </tr>-->
<!--                </tbody>-->
<!--              </table>-->
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
    </div>

    <!-- Modal add -->
    <div
        class="modal fade zoomIn"
        id="them-moi"
        tabindex="-1"
        aria-hidden="true"
    >
      <div class="modal-dialog modal-dialog-centered modal-lg">
        <div class="modal-content border-0">
          <div class="modal-header p-3 bg-primary-dark">
            <h5 class="modal-title">Thêm mới nhóm quyền</h5>
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
            <div>
              <blocks-tree :data="treeData" :horizontal="treeOrientation=='0'" :collapsable="true">
                <template #node="{data,context}">
            <span>
                <input type="checkbox" :checked="selected.indexOf(data.some_id)> -1"
                       @change="(e)=>toggleSelect(data,e.target.checked)"/> {{ data.label }}
            </span>
                  <br/>
                  <span v-if="data.children && data.children.length">
                <a href="#" @click="context.toggleExpand">toggle expand</a>
            </span>
                </template>
              </blocks-tree>
              <div>
                Selected: {{ selected }}
              </div>

            </div>
          </div>


        </div>
      </div>
    </div>

    <!-- Modal edit -->
    <div
        class="modal fade zoomIn"
        id="chinh-sua"
        tabindex="-1"
        aria-hidden="true"
    >
      <div class="modal-dialog modal-dialog-centered modal-lg">
        <div class="modal-content border-0">
          <div class="modal-header p-3 bg-primary-dark">
            <h5 class="modal-title">Chỉnh sửa nhóm quyền</h5>
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

.cl-warning {
  color: rgb(253, 216, 123);
}
</style>
