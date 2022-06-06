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
    component: () => import("../pages/lichcongtac/dexuatlichcongtac/index.vue"),
  },
  {
    path: "/nguoi-dung",
    name: "nguoi-dung",
    meta: {
      title: "Người dùng",
      authRequired: true,
    },
    component: () => import("../pages/user/index.vue"),
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
    path: "/lich-cong-tac-lanh-dao",
    name: "lich-cong-tac-lanh-dao",
    meta: {
      title: "Lịch công tác lãnh đạo",
      authRequired: true,
    },
    component: () => import("../pages/lichcongtac/dexuatlichcongtac/index"),
  },
  {
    path: "/lich-cong-tac-ca-nhan",
    name: "lich-cong-tac-ca-nhan",
    meta: {
      title: "Lịch công tác cá nhân",
      authRequired: true,
    },
    component: () => import("../pages/lichcongtac/lichcongtaccanhan/index.vue"),
  },
  {
    path: "/lich-cong-tac-phong-ban",
    name: "lich-cong-tac-phong-ban",
    meta: {
      title: "Lịch công tác phòng ban",
      authRequired: true,
    },
    component: () => import("../pages/lichcongtac/lichcongtacphongban/index.vue"),
  },
  {
    path: "/lich-ca-nhan",
    name: "lich-ca-nhan",
    meta: {
      title: "Lịch cá nhân",
      authRequired: true,
    },
    component: () => import("../pages/lichcongtac/lichcanhan/index.vue"),
  },
  {
    path: "/de-xuat-lich-cong-tac",
    name: "de-xuat-lich-cong-tac",
    meta: {
      title: "Đề xuất lịch công tác",
      authRequired: true,
    },
    component: () => import("../pages/lichcongtac/dexuatlichcongtac/index.vue"),
  },
  {
    path: "/quan-ly-lich-de-xuat",
    name: "quan-ly-lich-de-xuat",
    meta: {
      title: "Quản lý lịch đề xuất",
      authRequired: true,
    },
    component: () => import("../pages/lichcongtac/quanlylichdexuat/index.vue"),
  },
  {
    path: "/co-quan",
    name: "co-quan",
    meta: {
      title: "Cơ quan",
      authRequired: true,
    },
    component: () => import("../pages/quanlydanhmuc/coquan/index"),
  },
  {
    path: "/linh-vuc",
    name: "linh-vuc",
    meta: {
      title: "Lĩnh vực",
      authRequired: true,
    },
    component: () => import("../pages/quanlydanhmuc/linhvuc/index"),
  },
  {
    path: "/loai-nguon-von",
    name: "loai-nguon-von",
    meta: {
      title: "Loại nguồn vốn",
      authRequired: true,
    },
    component: () => import("../pages/quanlydanhmuc/loainguonvon/index"),
  },
  {
    path: "/loai-co-quan",
    name: "loai-co-quan",
    meta: {
      title: "Loại cơ quan",
      authRequired: true,
    },
    component: () => import("../pages/quanlydanhmuc/loaicoquan/index"),
  },
  {
    path: "/cap-co-quan",
    name: "cap-co-quan",
    meta: {
      title: "Cấp cơ quan",
      authRequired: true,
    },
    component: () => import("../pages/quanlydanhmuc/capcoquan/index"),
  },
  {
    path: "/nhom-du-an",
    name: "nhom-du-an",
    meta: {
      title: "Nhóm dự án",
      authRequired: true,
    },
    component: () => import("../pages/quanlydanhmuc/nhomduan/index"),
  },
  {
    path: "/loai-du-an",
    name: "loai-du-an",
    meta: {
      title: "Loại dự án",
      authRequired: true,
    },
    component: () => import("../pages/quanlydanhmuc/loaiduan/index"),
  },
  {
    path: "/nhom-quan-ly-du-an",
    name: "nhom-quan-ly-du-an",
    meta: {
      title: "Nhóm quản lý dự án",
      authRequired: true,
    },
    component: () => import("../pages/quanlydanhmuc/nhomquanlyduan/index"),
  },
  {
    path: "/nhom-quyen",
    name: "nhom-quyen",
    meta: {
      title: "Nhóm quyền",
      authRequired: true,
    },
    component: () => import("../pages/quanlytaikhoan/nhomquyen/index"),
  },
  {
    path: "/tai-khoan",
    name: "tai-khoan",
    meta: {
      title: "Tài khoản",
      authRequired: true,
    },
    component: () => import("../pages/quanlytaikhoan/taikhoan/index"),
  },
  {
    path: "/vai-tro",
    name: "vai-tro",
    meta: {
      title: "Quản lý vai trò",
      authRequired: true,
    },
    component: () => import("../pages/quanlytaikhoan/quyen/index"),
  },
  {
    path: "/van-ban-den",
    name: "van-ban-den",
    meta: {
      title: "Văn bản đến",
      authRequired: true,
    },
    component: () => import("../pages/quanlyvanban/vanbanden/index"),
  },
  {
    path: "/van-ban-den/nhan-xu-ly",
    name: "vanbanden-nhanxuly",
    meta: {
      title: "Văn bản đến",
      authRequired: true,
    },
    component: () => import("../pages/quanlyvanban/vanbanden/nhanxuly/index"),
  },
  {
    path: "/van-ban-den/phan-quyen-xu-ly",
    name: "vanbanden-phancongxuly",
    meta: {
      title: "Văn bản đến",
      authRequired: true,
    },
    component: () =>
      import("../pages/quanlyvanban/vanbanden/phanquyenxuly/index"),
  },
  {
    path: "/van-ban-di",
    name: "vanbandi",
    meta: {
      title: "Văn bản đi",
      authRequired: true,
    },
    component: () => import("../pages/quanlyvanban/vanbandi/index"),
  },
  {
    path: "/chu-ky-so",
    name: "chu-ky-so",
    meta: {
      title: "Chữ ký số",
      authRequired: true,
    },
    component: () => import("../pages/quanlyvanban/vanbandi/chukyso/index"),
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
