<script>
export default {
  components: {},
  data() {
    return {};
  },
};
</script>

<template>
  <div class="row p-4 content-ky-so">
    <div class="col-md-7">
      <span>file PDF</span>
    </div>
    <div class="col-md-5">
      <div class="card ribbon-box border shadow-none mb-lg-0">
        <div class="card-body p-4">
          <div>
            <div class="">
              <h5>Thông tin tài liệu ký</h5>
            </div>

            <div class="ribbon-content mt-4 text-muted">
              <div class="m-0">
                <li class="ks-item mb-2">
                  Tên file : <span>DH_GDCT.pdf</span>
                </li>
                <li class="ks-item mb-2">Kích thước : <span>100KB</span></li>
              </div>
            </div>
          </div>
          <hr />
          <div>
            <div class="mt-3">
              <h5>Thông tin chứng thư</h5>
            </div>

            <div class="ribbon-content mt-4 text-muted">
              <div class="m-0">
                <li class="ks-item mb-2">
                  Số lưu CV: <span>DHDT-HC-TH-0060/2020</span>
                </li>
                <li class="ks-item mb-2">
                  Loại văn bản : <span>Văn bản hành chính</span>
                </li>
                <li class="ks-item mb-2">Ngày cấp: <span>28/03/2022</span></li>
                <li class="ks-item mb-2">Trạng thái: <span>Trình ký</span></li>
                <li class="ks-item mb-2">
                  <p>Trích yếu:</p>
                  <p class="p-2" style="border: 1px dotted; border-radius: 3px">
                    Lorem ipsum dolor sit amet consectetur adipisicing elit.
                    Voluptatibus, eum. Eius harum neque,
                  </p>
                </li>
                <li class="ks-item mb-2">
                  <p>Ghi chú:</p>
                  <textarea
                    class="w-100"
                    rows="1"
                    style="border: none; border-bottom: 1px dotted"
                  >
                  </textarea>
                </li>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style>
.content-ky-so {
  background: #f3f3f9;
  color: #2a2a2a;
}
.ks-item {
  list-style: none;
}
.ks-item span {
  font-weight: 600;
}
textarea:focus {
  border: none;
}
</style>
