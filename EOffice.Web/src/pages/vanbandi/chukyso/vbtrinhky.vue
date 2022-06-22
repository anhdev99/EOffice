<script>
import PSPDFKitContainer from "./PSPDFKitContainer";
export default {
  components: { PSPDFKitContainer },
  data() {
    return {
      pdfFile: this.pdfFile || "/DH_GDCT.pdf",
    };
  },
  methods: {
    handleLoaded(instance) {
      console.log("PSPDFKit has loaded: ", instance);
      // Do something.
    },

    openDocument() {
      // To access the Vue instance data properties, use `this` keyword.
      // console.log("this.pdfFile", imgLink);
      if (this.pdfFile) {
        window.URL.revokeObjectURL(this.pdfFile);
      }
       this.pdfFile = window.URL.createObjectURL('http://localhost:8080/DH_GDCT.pdf');
    },
  },
};
</script>

<template>
  <div class="row p-4 content-ky-so">
    <div class="col-md-7">
      <div id="app">
        <label for="file-upload" class="custom-file-upload"> Open PDF </label>
        <input
          id="file-upload"
          type="file"
          @change="openDocument"
          class="btn"
        />
        <PSPDFKitContainer :pdfFile="pdfFile" @loaded="handleLoaded" />
      </div>
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
                  <p>Trích yếu: ...</p>
<!--                  <p class="p-2" style="border: 1px dotted; border-radius: 3px">-->
<!--                    Lorem ipsum dolor sit amet consectetur adipisicing elit.-->
<!--                    Voluptatibus, eum. Eius harum neque,-->
<!--                  </p>-->
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

#app {
  color: #2c3e50;
}

body {
  margin: 0;
}

input[type="file"] {
  display: none;
}

.custom-file-upload {
  border: 1px solid #ccc;
  border-radius: 4px;
  display: inline-block;
  padding: 6px 12px;
  cursor: pointer;
  background: #4a8fed;
  padding: 10px;
  color: #fff;
  font: inherit;
  font-size: 16px;
  font-weight: bold;
}
</style>
