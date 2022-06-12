export const menuItems = [
  {
    id: "1",
    label: "E-Office",
    isTitle: true,
  },
  {
    id: "2",
    label: "Thông báo",
    icon: "ri-account-circle-line",
    link: "/thong-bao",
  },
  {
    id: "3",
    label: "Lịch công tác",
    icon: "ri-dashboard-2-line",
    subItems: [
      {
        id: "4",
        label: "Lịch công tác lãnh đạo",
        link: "/lich-cong-tac-lanh-dao",
        parentId: "3",
      },
      {
        id: "5",
        label: "Lịch công tác cá nhân",
        link: "/lich-cong-tac-ca-nhan",
        parentId: "3",
      },
      {
        id: "6",
        label: "Lịch công tác phòng ban",
        link: "/lich-cong-tac-phong-ban",
        parentId: "3",
      },
      {
        id: "7",
        label: "Lịch cá nhân",
        link: "/lich-ca-nhan",
        parentId: "3",
      },
      {
        id: "8",
        label: "Đề xuất lịch công tác",
        link: "/de-xuat-lich-cong-tac",
        parentId: "3",
      },
      {
        id: "9",
        label: "Quản lý lịch đề xuất",
        link: "/quan-ly-lich-de-xuat",
        parentId: "3",
      },
    ],
  },
  {
    id: "11",
    label: "Quản lý Danh mục",
    icon: "ri-dashboard-2-line",
    subItems: [
      {
        id: "12",
        label: "Cơ quan",
        link: "/co-quan",
        parentId: "11",
      },
      {
        id: "13",
        label: "Lĩnh vực",
        link: "/linh-vuc",
        parentId: "11",
      },
      {
        id: "14",
        label: "Đơn vị",
        link: "/don-vi",

      },
      {
        id: "15",
        label: "Chức vụ",
        link: "/chuc-vu",
        parentId: "11",
      },
      {
        id: "16",
        label: "Cấp cơ quan",
        link: "/cap-co-quan",
        parentId: "11",
      }
    ],
  },
  {
    id: "20",
    label: "Quản lý tài khoản",
    icon: "ri-account-circle-line",
    subItems: [
      {
        id: "21",
        label: "Tài khoản",
        link: "/tai-khoan",
        parentId: "20",
      },
      {
        id: "22",
        label: "Quyền",
        link: "/vai-tro",
        parentId: "20",
      },
      {
        id: "23",
        label: "Nhóm quyền",
        link: "/nhom-quyen",
        parentId: "20",
      },
      {
        id: "24",
        label: "Menu",
        link: "/menu",
        parentId: "20",
      },
    ],
  },
  {
    id: "25",
    label: "QUẢN LÝ VĂN BẢN",
    isTitle: true,
  },
  {
    id: "26",
    label: "Văn bản đến",
    icon: "ri-account-circle-line",
    subItems: [
      {
        id: "27",
        label: "Văn bản đến",
        link: "/van-ban-den",
        parentId: "26",
      },
      {
        id: "28",
        label: "Xử lý văn bản",
        link: "/xu-ly-van-ban-den",
        parentId: "26",
      },
      {
        id: "29",
        label: "Văn bản đến - phân công",
        link: "/van-ban-den/phan-quyen-xu-ly",
        parentId: "26",
      },
      {
        id: "30",
        label: "Văn bản đến - nhận xử lý",
        link: "/van-ban-den/nhan-xu-ly",
        parentId: "26",
      },
    ],
  },
  {
    id: "31",
    label: "Văn bản đi",
    icon: "ri-account-circle-line",
    subItems: [
      {
        id: "32",
        label: "Văn bản đi",
        link: "/van-ban-di",
        parentId: "31",
      },
      {
        id: "33",
        label: "Xử lý văn bản",
        link: "/xu-ly-van-ban-di",
        parentId: "31",
      },
      {
        id: "34",
        label: "Chữ ký số",
        link: "/chu-ky-so",
        parentId: "31",
      },
    ],
  },
];
