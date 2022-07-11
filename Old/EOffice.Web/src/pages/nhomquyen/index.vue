<script>
import Layout from "@/layouts/main";
import PageHeader from "@/components/page-header";
import appConfig from "../../../app.config.json";
import {data} from "./data";
import ThemMoi from "./themmoi.vue";
import ChinhSua from "./chinhsua.vue";
import Vue3TreeVue from 'vue3-tree-vue';
import {ref} from "@vue/reactivity";
import "vue3-tree-vue/dist/style.css";

export default {
  page: {
    title: "Nhóm quyền",
    meta: [
      {
        content: appConfig.description,
      },
    ],
  },
  data() {
    return {
      title: "Nhóm quyền",
      items: [
        {
          text: "Trang chủ",
          href: "/",
        },
        {
          text: "Nhóm quyền",
          active: true,
        },
      ],
      data: data,
      selectedItem: ref() ,
      selectedItems: ref(),
      isCheckable: ref(true),
      item: [
        {
          "name": "Rejection Emails",
          "id": 1,
          "type": "emails",
          "children": [
            {
              "name": "List of Rejection Emails",
              "id": 2,
              "type": ".excel"
            },
            {
              "name": "Handling Rejection Through Open-sourcing User Guide",
              "id": 3,
              "type": ".doc"
            },
            {
              "name": "Leet Code Tracking Sheet",
              "id": 4,
              "type": "folder",
              "children": [
                {
                  "name": "Solved Problems (30)",
                  "type": ".excel",
                  "id": 5
                },
                {
                  "name": "Unsolved Problems (5000)",
                  "type": "folder",
                  "id": 6,

                  "children": [
                    {
                      "name": "Inverting a linked list (lol)",
                      "id": 7,
                      "type": ".playlist"
                    },
                    {
                      "name": "Decoding hieroglyphs",
                      "id": 8,
                      "type": ".playlist"
                    }
                  ]
                }
              ]
            }
          ]
        },
        {
          "name": "System Design Playlist",
          "id": 9,
          "type": ".playlist",
          "children": [
            {
              "name": "Space Station Design",
              "id": 10,
              "type": ".playlist"
            },
            {
              "name": "Gabbage Collector Design",
              "id": 11,
              "type": ".playlist"
            },
            {
              "name": "Design Extra-Terrestrial Life Support",
              "type": ".playlist",
              "id": 12
            }
          ]
        }
      ],
    };
  },
  components: {Layout, PageHeader, ThemMoi, ChinhSua, Vue3TreeVue},
  methods: {
    onItemSelected(){
      console.log("selectedItem", this.selectedItem);
      console.log("selectedItem", this.isCheckable);
      console.log("item", this.item.name);
    },
    onItemChecked(){
      console.log("selectedItems", this.selectedItems);
      console.log("item", this.item.name);
    }
  },
};
</script>

<template>
  <Layout>
    <PageHeader :title="title" :items="items"/>

    <div class="row">
      <div class="col-md-6">
        <div id="app">
          <div style="display: block">
            <input type="checkbox" v-model="isCheckable"/>
            <hr>
            <div class="d-flex">
              <vue3-tree-vue :items="item"
                             :isCheckable="isCheckable"
                             :hideGuideLines="false"
                             :selectedItem="selectedItem"
                             :checkedItems="selectedItems"
                             @onSelect="onItemSelected"
                             @onCheck="onItemChecked"
                             :expandAll="true"
                             style="width: 500px; display: block; border-right: 1px solid gray">
                <template v-slot:item-prepend-icon="treeViewItem">
                  <img src="@/assets/images/folder.svg" alt="folder"
                       v-if="treeViewItem.type === 'folder'"
                       height="20" width="20">

                  <img src="@/assets/images/word.svg"
                       v-if="treeViewItem.type === '.doc'"
                       height="20" width="20">

                  <img src="@/assets/images/excel.svg"
                       v-if="treeViewItem.type === '.excel'"
                       height="20" width="20">


                  <img src="@/assets/images/playlist.svg"
                       v-if="treeViewItem.type === '.playlist'"
                       height="20" width="20">

                  <img src="@/assets/images/email.png"
                       v-if="treeViewItem.type === 'emails'"
                       height="20" width="20">
                </template>
                <template v-slot:item-prepend>
                  <div style="background: blue; height: 18px; width: 18px; margin-right: 0.2em" ></div>
                </template>
              </vue3-tree-vue>
              <div style="max-width: 600px">
                <div style="margin: 0.4em">
                  Selected Item {{ selectedItem?.name }}
                </div>
                <div style="margin: 0.4em">
                  {{ selectedItems?.map(node => node.name) }}
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div class="col-md-6">

      </div>
    </div>

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
              <!--  Table -->
              <table class="table align-middle table-nowrap mb-0">
                <thead class="table-light">
                <tr>
                  <th scope="col">#</th>
                  <th scope="col">Tên module</th>
                  <th scope="col">Thao tác</th>
                </tr>
                </thead>
                <tbody>
                <tr v-if="data.length <= 0" class="text-center">
                  <td colspan="6">Không tìm thấy dữ liệu</td>
                </tr>
                <tr v-else v-for="(item, index) in data" :key="item.id">
                  <td>{{ ++index }}</td>
                  <td>{{ item.name }}</td>
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
            <them-moi/>
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
<style scoped>
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

.selected-tree-item {
  background: rgba(235, 106, 59, 0.795);
  color: white;
}

ul {
  padding-left: 0;
  margin: 0;
  list-style-type: none;
  display: inline-block;
}

li {
  border-radius: 4px;
}

li.drag-over {
  background-color: rgba(22, 22, 22, 0.068);
}

.tree-view-item {
  margin: 0.1em;
  padding: 0.35em;
}

.chevron-right {
  color: gray;
}

.hide-chevron {
  visibility: collapse;
}

.icon-area {
  width: 22px;
  margin-right: 0.4em;
}

.horizontal-dashes {
  width: 1em;
  border-top: 1px dashed rgb(192, 192, 192);
}

.node-name {
  cursor: pointer;
  margin-left: 5px;
}

.d-flex {
  display: flex;
}

.align-items-center {
  align-items: center;
}

.node-child {
  margin-left: 37px !important;
  border-left: 1px dashed rgb(192, 192, 192);
  display: block;
}

.hide {
  display: none;
}

.hide-guidelines {
  border-left: none !important;
}

.chevron-right {
  box-sizing: border-box;
  position: relative;
  display: block;
  transform: scale(var(--ggs,1));
  width: 22px;
  height: 22px;
  border: 2px solid transparent;
  border-radius: 100px;
  transition: .2s;
}

.pointer {
  cursor: pointer;
}

.chevron-right.rotate-90::after {
  transform: rotateZ(45deg);
}

.chevron-right::after {
  content: "";
  display: block;
  box-sizing: border-box;
  position: absolute;
  width: 7px;
  height: 7px;
  border-bottom: 2px solid;
  border-right: 2px solid;
  transform: rotate(-45deg);
  right: 6px;
  top: 5px
}
</style>

