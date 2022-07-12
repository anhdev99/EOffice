<script>
import { required, email } from "vuelidate/lib/validators";
import { mapState } from "vuex";

import {
  authMethods,
  authFackMethods,
  notificationMethods,
} from "@/state/helpers";
import appConfig from "@/app.config";
import Vue from "vue";

/**
 * Login component
 */
export default {
  page: {
    title: "Đăng nhập",
    meta: [{ name: "description", content: appConfig.description }],
  },
  components: {},
  validations: {
    model:{
      userName: {
        required,
      },
      password: {
        required
      }
    }
  },
  data() {
    return {
      email: "admin@themesbrand.com",
      password: "123456",
      submitted: false,
      authError: null,
      tryingToLogIn: false,
      isAuthError: false,
      modelAuth:{
        isAuthError: false,
        message: null
      },
      model:{
        userName: "admin",
        password: "DThU@123"
      },
    };
  },
  methods: {
    async Login(e) {
      e.preventDefault();
      this.submitted = true;
      this.$v.$touch();
      if (this.$v.$invalid) {
        return;
      } else {
        let loader = this.$loading.show({
          container: this.$refs.formContainer,
        });
        // await setTimeout(function () {
        //  }, 5000);
        await this.$store.dispatch("authStore/login", this.model).then(async (res) => {
          if (res.resultCode === 'SUCCESS') {
            await new Promise(resolve => setTimeout(resolve, 1000));
            localStorage.setItem('auth-user', JSON.stringify(res.data));
            localStorage.setItem("user-token", JSON.stringify(res.data.token));

            if (res.data.user) {
              if (res.data.user.menuItems) {
                localStorage.setItem("menuItems", JSON.stringify(res.data.user.menuItems));
              }
            }
            Vue.prototype.$auth_token = res.data.token;
            this.showModal = false;
            this.model = {};
            this.modelAuth.isAuthError = false;
            window.location.href = '/'
          } else {
            if (res.code == 400) {
              this.modelAuth.isAuthError = true;
              this.modelAuth.message = "Lỗi! Hãy kiểm tra kết nối mạng!";
            } else {
              this.modelAuth.isAuthError = true;
              this.modelAuth.message = res.resultString;
            }
            loader.hide();
          }

        })
            .finally(() => {
              loader.hide();
            });
      }
      this.submitted = false;
    },
  }
};
</script>

<template>
  <div class="account-pages ">
    <div class="container my-5 pt-5">
      <div class="row justify-content-center">
        <div class="col-md-8 col-lg-6 col-xl-4">
          <div class="card overflow-hidden">
            <div class="" style="background-color: #00568C">
              <div class="text-primary text-center p-4">
                <h5 class="text-white font-size-20"> Hệ thống EOffice - DThU</h5>
                <p class="text-white-50">Quản lý hành chính điện tử</p>
                <a href="/" class="logo logo-admin">
                  <img
                      src="@/assets/images/DTHU.png"
                      height="75"
                      alt="logo"
                  />
                </a>
              </div>
            </div>
            <div class="card-body p-4">
              <div class="p-3">
                <b-alert
                    v-model="modelAuth.isAuthError"
                    variant="danger"
                    class="mt-4"
                    dismissible
                >{{ modelAuth.message }}</b-alert
                >
                <b-form
                    @submit.prevent="Login"  ref="formContainer"
                    class="form-horizontal mt-4"
                >
                  <b-form-group
                      id="input-group-1"
                      label="Tài khoản"
                      label-for="input-1"
                      class="mb-3"
                      label-class="form-label"
                  >
                    <b-form-input
                        id="input-1"
                        v-model="model.userName"
                        type="text"
                        placeholder="Nhập tài khoản"
                        :class="{ 'is-invalid': submitted && $v.model.userName.$error }"
                    ></b-form-input>
                    <div
                        v-if="submitted && $v.model.userName.$error"
                        class="invalid-feedback"
                    >
                      <span v-if="!$v.model.userName.required">Tài khoản không được trống.</span>
                    </div>
                  </b-form-group>

                  <b-form-group
                      id="input-group-2"
                      label="Mật khẩu"
                      label-for="input-2"
                      class="mb-3"
                      label-class="form-label"
                  >
                    <b-form-input
                        id="input-2"
                        v-model="model.password"
                        type="password"
                        placeholder="Nhập mật khẩu"
                        :class="{ 'is-invalid': submitted && $v.model.password.$error }"
                    ></b-form-input>
                    <div
                        v-if="submitted && !$v.model.password.required"
                        class="invalid-feedback"
                    >
                      Mật khẩu không được trống.
                    </div>
                  </b-form-group>

                  <div class="form-group row">
                    <div class="col-sm-6">
                      <div class="form-check">
                        <input
                            type="checkbox"
                            class="form-check-input"
                            id="customControlInline"
                        />
                        <label
                            class="form-check-label"
                            for="customControlInline"
                        >Ghi nhớ tài khoản</label
                        >
                      </div>
                    </div>
                    <div class="col-sm-6 text-end">
                      <b-button type="submit" variant="primary" class="w-100" style="background-color: #00568C"
                      > Đăng nhập</b-button
                      >
                    </div>
                  </div>

<!--                  <div class="mt-2 mb-0 row">-->
<!--                    <div class="col-12 mt-4">-->
<!--                      <router-link to="/forgot-password">-->
<!--                        <i class="mdi mdi-lock"></i> Forgot your password?-->
<!--                      </router-link>-->
<!--                    </div>-->
<!--                  </div>-->
                </b-form>
              </div>
            </div>
            <!-- end card-body -->
          </div>
          <!-- end card -->
          <div class="mt-5 text-center">
<!--            <p>-->
<!--              Don't have an account ?-->
<!--              <router-link to="/register" class="fw-medium text-primary"-->
<!--              >Signup now</router-link-->
<!--              >-->
<!--            </p>-->
            <p class="mb-0">
              ©
              {{ new Date().getFullYear() }}
             -  Trường Đại học Đồng Tháp
            </p>
          </div>
        </div>
        <!-- end col -->
      </div>
      <!-- end row -->
    </div>
  </div>
</template>

<style lang="scss" >
//.account-pages{
//  background-image: url("~@/assets/images/banner-gioi-thiu-dthu.png");
//  background-repeat: no-repeat;
//  background-size: cover;
//  height: 100vh;
//}
</style>
