<script>
import FormThongTinChinh from "./formthongtinchinh";
import FormButPheLanhDao from "./formbutphelanhdao";
import ForXuLyVanBan from "./formxulyvanban";

export default {
  data() {
    return {};
  },
  components: { FormThongTinChinh, FormButPheLanhDao, ForXuLyVanBan },
};
</script>

<template>
  <form id="addform">
    <div class="modal-body">
      <ul class="nav nav-tabs mb-3" role="tablist">
        <li class="nav-item">
          <a
            class="nav-link active"
            data-bs-toggle="tab"
            href="#thongtinchinh"
            role="tab"
            aria-selected="false"
          >
            Thông tin chính
          </a>
        </li>
        <li class="nav-item">
          <a
            class="nav-link"
            data-bs-toggle="tab"
            href="#butphelanhdao"
            role="tab"
            aria-selected="false"
          >
            Bút phê lãnh đạo
          </a>
        </li>
        <li class="nav-item">
          <a
            class="nav-link"
            data-bs-toggle="tab"
            href="#xulyvanban"
            role="tab"
            aria-selected="false"
          >
            Đơn vị nhận/ Xử lý văn bản
          </a>
        </li>
      </ul>

      <!--  tabs panes -->

      <div class="tab-content text-muted">
        <div class="tab-pane active" id="thongtinchinh" role="tabpanel">
          <form-thong-tin-chinh />
        </div>
        <div class="tab-pane" id="butphelanhdao" role="tabpanel">
          <form-but-phe-lanh-dao />
        </div>
        <div class="tab-pane" id="xulyvanban" role="tabpanel">
          <for-xu-ly-van-ban />
        </div>
      </div>
    </div>
  </form>
</template>

<style></style>
