<script>
import Treeselect from "vue3-treeselect";
// import the styles
import "vue3-treeselect/dist/vue3-treeselect.css";

export default {
  components: { Treeselect },
  data() {
    return {
      form: {
        coquan: "",
        vaitro: "",
      },
      optionCoQuan: [
        {
          id: "1",
          label: "Đại học Đồng tháp",
          children: [
            {
              id: "2",
              label: "khoa CNTT",
            },
          ],
        },
      ],
      optionVaiTro: [
        {
          id: "1",
          label: "Quản lý",
          children: [
            {
              id: "2",
              label: "Quản lý nhân sự",
            },
          ],
        },
      ],
    };
  },
};
</script>
<template>
  <form class="">
    <div class="row">
      <div class="col-6">
        <!-- Họ -->
        <div class="mb-3">
          <label for="validationLastName" class="form-label">Họ</label>
          <span class="text-danger">*</span>
          <input
            type="text"
            class="form-control"
            id="validationLastName"
            name="lastName"
          />
        </div>
        <!-- Họ -->
        <div class="mb-3">
          <label for="validationFirstName" class="form-label">Tên</label>
          <span class="text-danger">*</span>
          <input
            type="text"
            class="form-control"
            id="validationFirstName"
            name="firstName"
          />
        </div>
        <!-- Cơ quan -->
        <div class="mb-3">
          <label for="validationHoSoDonVi">Cơ quan</label>
          <treeselect
            placeholder="Chọn cơ quan"
            v-model="form.coquan"
            :options="optionCoQuan"
          >
          </treeselect>
          <treeselect-value :value="form.hoSoDonVi" />
        </div>
        <!-- Số điện thoại -->
        <div class="mb-3">
          <label for="validationPhoneNumber" class="form-label"
            >Số điện thoại</label
          >
          <input
            type="text"
            v-maska="['####-###-###', '####-###-###']"
            class="form-control"
            id="validationPhoneNumber"
          />
        </div>
      </div>
      <div class="col-6">
        <!-- Tên tài khoản -->
        <div class="mb-3">
          <label for="validationUsername" class="form-label"
            >Tên tài khoản
          </label>
          <span class="text-danger">*</span>
          <input
            type="text"
            class="form-control"
            id="validationUsername"
            name="username"
          />
        </div>
        <!-- Mật khẩu -->
        <div class="mb-3">
          <label for="validationPassword" class="form-label">Mật khẩu</label>
          <span class="text-danger">*</span>
          <input
            type="password"
            class="form-control"
            id="validationPassword"
            name="password"
          />
        </div>
        <!-- Vai trò -->
        <div class="mb-3">
          <label for="validationPassword" class="form-label">Email</label>
          <span class="text-danger">*</span>
          <div class="input-group">
            <input
              type="text"
              class="form-control"
              placeholder="email"
              aria-label="email"
            />
            <span class="input-group-text">@</span>
            <input
              type="text"
              class="form-control"
              placeholder="domain.com"
              aria-label="domain"
            />
          </div>
        </div>
        <!-- Vai trò -->
        <div class="mb-3">
          <label for="validationVaiTro">Vai trò</label>
          <treeselect
            placeholder="Vai trò"
            v-model="form.vaitro"
            :options="optionVaiTro"
          >
          </treeselect>
          <treeselect-value :value="form.optionVaiTro" />
        </div>
      </div>
    </div>
  </form>
</template>

<style></style>
