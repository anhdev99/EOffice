export const menuItems = [
  {
    id: "2",
    label: "E-Office",
    isTitle: true
  },
  {
    id: "2",
    label: "Quản lý Danh mục",
    icon: "ri-dashboard-2-line",
    subItems: [
      {
        id: "3",
        label: "Cơ quan",
        link: "/",
        parentId: "2"
      },
      {
        id: "3",
        label: "Lĩnh vực",
        link: "/linh-vuc",
        parentId: "2"
      },
      {
        id: "3",
        label: "Loại nguồn vốn",
        link: "/loai-nguon-von",
        parentId: "2"
      },
      {
        id: "3",
        label: "Loại cơ quan",
        link: "/loai-co-quan",
        parentId: "2"
      },
      {
        id: "3",
        label: "Cấp cơ quan",
        link: "/cap-co-quan",
        parentId: "2"
      },
      {
        id: "3",
        label: "Nhóm dự án",
        link: "/nhom-du-an",
        parentId: "2"
      },
      {
        id: "3",
        label: "Loại dự án",
        link: "/",
        parentId: "2"
      },
      {
        id: "3",
        label: "Nhóm quản lý dự án",
        link: "/nhom-quan-ly-du-an",
        parentId: "2"
      }
    ]
  },
  {
    id: "3",
    label: "Quản lý tài khoản",
    icon: "ri-account-circle-line",
    subItems: [
      {
        id: "3",
        label: "Tài khoản",
        link: "/tai-khoan",
        parentId: "2"
      },
      {
        id: "3",
        label: "Quyền",
        link: "/vai-tro",
        parentId: "2"
      },
      {
        id: "3",
        label: "Menu",
        link: "/menu",
        parentId: "2"
      }
    ]
  },
  {
    id: "3",
    label: "Quản lý tài khoản",
    icon: "ri-account-circle-line",
    link: "/vai-tro",
  }
];

