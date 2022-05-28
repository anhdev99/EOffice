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
    label: "Quản lý Danh mục",
    icon: "ri-dashboard-2-line",
    subItems: [
      {
        id: "4",
        label: "Cơ quan",
        link: "/co-quan",
        parentId: "3",
      },
      {
        id: "5",
        label: "Lĩnh vực",
        link: "/linh-vuc",
        parentId: "3",
      },
      {
        id: "6",
        label: "Loại nguồn vốn",
        link: "/loai-nguon-von",
        parentId: "3",
      },
      {
        id: "7",
        label: "Loại cơ quan",
        link: "/loai-co-quan",
        parentId: "3",
      },
      {
        id: "8",
        label: "Cấp cơ quan",
        link: "/cap-co-quan",
        parentId: "3",
      },
      {
        id: "9",
        label: "Nhóm dự án",
        link: "/nhom-du-an",
        parentId: "3",
      },
      {
        id: "10",
        label: "Loại dự án",
        link: "/loai-du-an",
        parentId: "3",
      },
      {
        id: "11",
        label: "Nhóm quản lý dự án",
        link: "/nhom-quan-ly-du-an",
        parentId: "3",
      },
    ],
  },
  {
    id: "12",
    label: "Quản lý tài khoản",
    icon: "ri-account-circle-line",
    subItems: [
      {
        id: "13",
        label: "Tài khoản",
        link: "/tai-khoan",
        parentId: "12",
      },
      {
        id: "14",
        label: "Quyền",
        link: "/vai-tro",
        parentId: "12",
      },
      {
        id: "15",
        label: "Menu",
        link: "/menu",
        parentId: "12",
      },
    ],
  },
  {
    id: "16",
    label: "Quản lý tài khoản",
    icon: "ri-account-circle-line",
    link: "/vai-tro",
  },
  {
    id: "17",
    label: "QUẢN LÝ VĂN BẢN",
    isTitle: true,
  },
  {
    id: "18",
    label: "Văn bản đến",
    icon: "ri-account-circle-line",
    subItems: [
      {
        id: "19",
        label: "Văn bản đến",
        link: "/van-ban-den",
        parentId: "18",
      },
      {
        id: "20",
        label: "Xử lý văn bản",
        link: "/xu-ly-van-ban-den",
        parentId: "18",
      },
      {
        id: "21",
        label: "Văn bản đến - phân công",
        link: "/van-ban-den/phan-quyen-xu-ly",
        parentId: "18",
      },
      {
        id: "22",
        label: "Văn bản đến - nhận xử lý",
        link: "/van-ban-den/nhan-xu-ly",
        parentId: "18",
      },
    ],
  },
  {
    id: "23",
    label: "Văn bản đi",
    icon: "ri-account-circle-line",
    subItems: [
      {
        id: "24",
        label: "Văn bản đi",
        link: "/van-ban-di",
        parentId: "23",
      },
      {
        id: "25",
        label: "Xử lý văn bản",
        link: "/xu-ly-van-ban-di",
        parentId: "23",
      },
      {
        id: "26",
        label: "Chữ ký số",
        link: "/chu-ky-so",
        parentId: "23",
      },
    ],
  },
];
