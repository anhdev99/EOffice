import store from "@/state/store";

export default [
  {
    path: "/dang-nhap",
    name: "dang-nhap",
    component: () => import("../pages/auth/login.vue"),
    meta: {
      title: "Đăng nhập",
      beforeResolve(routeTo, routeFrom, next) {
        // If the user is already logged in
        if (store.getters["auth/loggedIn"]) {
          // Redirect to the home page instead
          next({ name: "default" });
        } else {
          // Continue to the login page
          next();
        }
      },
    },
  },
  {
    path: "/register",
    name: "Register",
    component: () => import("../views/account/register.vue"),
    meta: {
      title: "Register",
      beforeResolve(routeTo, routeFrom, next) {
        // If the user is already logged in
        if (store.getters["auth/loggedIn"]) {
          // Redirect to the home page instead
          next({ name: "default" });
        } else {
          // Continue to the login page
          next();
        }
      },
    },
  },
  {
    path: "/forgot-password",
    name: "Forgot password",
    component: () => import("../views/account/forgot-password.vue"),
    meta: {
      title: "Forgot Password",
      beforeResolve(routeTo, routeFrom, next) {
        // If the user is already logged in
        if (store.getters["auth/loggedIn"]) {
          // Redirect to the home page instead
          next({ name: "default" });
        } else {
          // Continue to the login page
          next();
        }
      },
    },
  },
  {
    path: "/",
    name: "default",
    meta: {
      title: "Dashboard",
      authRequired: true,
    },
    component: () => import("../pages/thongbao/index"),
  },
  {
    path: "/nguoi-dung",
    name: "nguoi-dung",
    meta: {
      title: "Người dùng",
      authRequired: true,
    },
    component: () => import("../pages/user/index"),
  },
  {
    path: "/thong-bao",
    name: "thong-bao",
    meta: {
      title: "Thông báo",
      authRequired: true,
    },
    component: () => import("../pages/thongbao/index.vue"),
  },
  {
    path: "/lich-ca-nhan",
    name: "lich-ca-nhan",
    meta: {
      title: "Lịch cá nhân",
      authRequired: true,
    },
    component: () => import("../pages/lichcanhan/index.vue"),
  },
  {
    path: "/quan-ly-lich-de-xuat",
    name: "quan-ly-lich-de-xuat",
    meta: {
      title: "Quản lý lịch đề xuất",
      authRequired: true,
    },
    component: () => import("../pages/quanlylichdexuat/index.vue"),
  },
  {
    path: "/co-quan",
    name: "co-quan",
    meta: {
      title: "Cơ quan",
      authRequired: true,
    },
    component: () => import("../pages/dmcoquan/index"),
  },
  {
    path: "/linh-vuc",
    name: "linh-vuc",
    meta: {
      title: "Lĩnh vực",
      authRequired: true,
    },
    component: () => import("../pages/dmlinhvuc/index"),
  },
  {
    path: "/don-vi",
    name: "don-vi",
    meta: {
      title: "Đơn vị",
      authRequired: true,
    },
    component: () => import("../pages/dmdonvi/index"),
  },
  {
    path: "/chuc-vu",
    name: "chuc-vu",
    meta: {
      title: "Chức vụ",
      authRequired: true,
    },
    component: () => import("../pages/chucVu/index"),
  },
  {
    path: "/cap-co-quan",
    name: "cap-co-quan",
    meta: {
      title: "Cấp cơ quan",
      authRequired: true,
    },
    component: () => import("../pages/dmcapcoquan/index"),
  },
  {
    path: "/nhom-quyen",
    name: "nhom-quyen",
    meta: {
      title: "Nhóm quyền",
      authRequired: true,
    },
    component: () => import("../pages/nhomquyen/index"),
  },
  {
    path: "/tai-khoan",
    name: "tai-khoan",
    meta: {
      title: "Tài khoản",
      authRequired: true,
    },
    component: () => import("../pages/taikhoan/index"),
  },
  {
    path: "/menu",
    name: "menu",
    meta: {
      title: "Menu",
      authRequired: true,
    },
    component: () => import("../pages/menu/index"),
  },
  {
    path: "/vai-tro",
    name: "vai-tro",
    meta: {
      title: "Quản lý vai trò",
      authRequired: true,
    },
    component: () => import("../pages/role/index"),
  },
  {
    path: "/trang-thai",
    name: "trang-thai",
    meta: {
      title: "Quản lý trạng thái",
      authRequired: true,
    },
    component: () => import("../pages/trangThai/index"),
  },
  {
    path: "/loai-van-ban",
    name: "loai-van-ban",
    meta: {
      title: "Quản lý loại văn bản",
      authRequired: true,
    },
    component: () => import("../pages/loaiVanBan/index"),
  },
  {
    path: "/van-ban-den",
    name: "van-ban-den",
    meta: {
      title: "Văn bản đến",
      authRequired: true,
    },
    component: () => import("../pages/vanbanden/index"),
  },
  {
    path: "/xu-ly-van-ban-den",
    name: "/xu-ly-van-ban-den",
    meta: {
      title: "Xử lý văn bản đến",
      authRequired: true,
    },
    component: () => import("../pages/vanbandenXL/index"),
  },
  {
    path: "/van-ban-den/nhan-xu-ly",
    name: "vanbanden-nhanxuly",
    meta: {
      title: "Văn bản đến",
      authRequired: true,
    },
    component: () => import("../pages/vanbandenXL"),
  },
  {
    path: "/van-ban-den/phan-quyen-xu-ly",
    name: "vanbanden-phancongxuly",
    meta: {
      title: "Văn bản đến",
      authRequired: true,
    },
    component: () =>
      import("../pages/vanbanden/phanquyenxuly/index"),
  },
  {
    path: "/van-ban-di",
    name: "vanbandi",
    meta: {
      title: "Văn bản đi",
      authRequired: true,
    },
    component: () => import("../pages/vanbandi/index"),
  },
  {
    path: "/chu-ky-so",
    name: "chu-ky-so",
    meta: {
      title: "Chữ ký số",
      authRequired: true,
    },
    component: () => import("../pages/vanbandi/chukyso/index"),
  },

  {
    path: "/logout",
    name: "logout",
    meta: {
      title: "Logout",
      authRequired: true,
      beforeResolve(routeTo, routeFrom, next) {
        if (process.env.VUE_APP_DEFAULT_AUTH === "firebase") {
          store.dispatch("auth/logOut");
        } else {
          store.dispatch("authfack/logout");
        }
        const authRequiredOnPreviousRoute = routeFrom.matched.some((route) =>
          route.push("/login")
        );
        // Navigate back to previous page, or home as a fallback
        next(
          authRequiredOnPreviousRoute ? { name: "default" } : { ...routeFrom }
        );
      },
    },
    component: () => import("../views/auth/logout/basic"),
  },
];
