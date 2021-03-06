import store from '@/state/store'

export default [{
        path: '/',
        meta: {
            authRequired: true
        },
        name: 'home',
        component: () => import('../pages/dashboard/index'),
    },
    {
        path: '/dang-nhap',
        name: 'login',
        component: () => import('../pages/auth/auth'),
        meta: {
            beforeResolve(routeTo, routeFrom, next) {
                // If the user is already logged in
                if (store.getters['auth/loggedIn']) {
                    // Redirect to the home page instead
                    next({name: "default"});
                } else {
                    // Continue to the login page
                    next()
                }
            },
        },
    },
    {
        path: '/register',
        name: 'register',
        component: () => import('./views/account/register'),
        meta: {
            beforeResolve(routeTo, routeFrom, next) {
                // If the user is already logged in
                if (store.getters['auth/loggedIn']) {
                    // Redirect to the home page instead
                    next({
                        name: 'home'
                    })
                } else {
                    // Continue to the login page
                    next()
                }
            },
        },
    },
    {
        path: '/forgot-password',
        name: 'forgot-password',
        component: () => import('./views/account/forgot-password'),
        meta: {
            beforeResolve(routeTo, routeFrom, next) {
                // If the user is already logged in
                if (store.getters['auth/loggedIn']) {
                    // Redirect to the home page instead
                    next({
                        name: 'home'
                    })
                } else {
                    // Continue to the login page
                    next()
                }
            },
        },
    },
    {
        path: '/logout',
        name: 'logout',
        meta: {
            authRequired: true,
            beforeResolve(routeTo, routeFrom, next) {
                if (process.env.VUE_APP_DEFAULT_AUTH === "firebase") {
                    store.dispatch('auth/logOut')
                } else {
                    store.dispatch('authfack/logout')
                }
                const authRequiredOnPreviousRoute = routeFrom.matched.some(
                    (route) => route.push('/login')
                )
                // Navigate back to previous page, or home as a fallback
                next(authRequiredOnPreviousRoute ? {
                    name: 'default'
                } : {
                    ...routeFrom
                })
            },
        },
    },
    {
        path: '/calendar',
        name: 'Calendar',
        component: () => import('./views/calendar/index'),
        meta: {
            authRequired: true,
        },
    },
    {
        path: '/email/inbox',
        name: 'Email Inbox',
        component: () => import('./views/email/inbox'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/email/read-email',
        name: 'Read email',
        component: () => import('./views/email/reademail'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/email/compose',
        name: 'Compose',
        component: () => import('./views/email/compose'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/ui/alerts',
        name: 'Alerts',
        component: () => import('./views/ui/alerts'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/ui/rating',
        name: 'Rating',
        component: () => import('./views/ui/rating'),
        meta: {
            authRequired: true,
        }
    },

    {
        path: '/ui/buttons',
        name: 'Buttons',
        component: () => import('./views/ui/buttons'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/ui/cards',
        name: 'Cards',
        component: () => import('./views/ui/cards'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/ui/carousel',
        name: 'Carousel',
        component: () => import('./views/ui/carousel'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/ui/colors',
        name: 'Colors',
        component: () => import('./views/ui/colors'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/ui/dropdowns',
        name: 'Dropdowns',
        component: () => import('./views/ui/dropdowns'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/ui/general',
        name: 'General',
        component: () => import('./views/ui/general'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/ui/grid',
        name: 'Grid',
        component: () => import('./views/ui/grid'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/ui/images',
        name: 'Images',
        component: () => import('./views/ui/images'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/ui/modals',
        name: 'Modals',
        component: () => import('./views/ui/modals'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/ui/progressbar',
        name: 'Progressbar',
        component: () => import('./views/ui/progressbar'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/ui/rangeslider',
        name: 'Range-slider',
        component: () => import('./views/ui/rangeslider'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/ui/sweetalert',
        name: 'Sweet-alert',
        component: () => import('./views/ui/sweetalert'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/ui/tabs',
        name: 'Tabs & Accordions',
        component: () => import('./views/ui/tabs'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/ui/typography',
        name: 'Typography',
        component: () => import('./views/ui/typography'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/ui/video',
        name: 'Video',
        component: () => import('./views/ui/video'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/pages/timeline',
        name: 'Timeline',
        component: () => import('./views/pages/timeline'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/pages/invoice',
        name: 'Invoice',
        component: () => import('./views/pages/invoice'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/pages/pricing',
        name: 'Pricing',
        component: () => import('./views/pages/pricing'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/pages/blank-page',
        name: 'Blank page',
        component: () => import('./views/pages/blank'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/pages/faqs',
        name: 'FAQs',
        component: () => import('./views/pages/faqs'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/pages/directory',
        name: 'Directory',
        component: () => import('./views/pages/directory'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/pages/404',
        name: 'Page-404',
        component: () => import('./views/pages/error-404'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/pages/500',
        name: 'Page-500',
        component: () => import('./views/pages/error-500'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/pages/maintenance',
        name: 'Maintenance',
        component: () => import('./views/pages/maintenance'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/pages/login-1',
        name: 'Login-1',
        component: () => import('./views/pages/login-1'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/pages/login-2',
        name: 'Login-2',
        component: () => import('./views/pages/login-2'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/pages/register-1',
        name: 'Register-1',
        component: () => import('./views/pages/register-1'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/pages/register-2',
        name: 'Register-2',
        component: () => import('./views/pages/register-2'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/pages/recoverpwd-1',
        name: 'Recover Password 1',
        component: () => import('./views/pages/recoverpwd-1'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/pages/recoverpwd-2',
        name: 'Recover Password 2',
        component: () => import('./views/pages/recoverpwd-2'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/pages/lock-screen1',
        name: 'Lock-screen 1',
        component: () => import('./views/pages/lock-screen1'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/pages/lock-screen2',
        name: 'Lock-screen 2',
        component: () => import('./views/pages/lock-screen2'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/form/elements',
        name: 'Form Elements',
        component: () => import('./views/forms/elements'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/form/advanced',
        name: 'Form advanced',
        component: () => import('./views/forms/advanced'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/form/editor',
        name: 'Form editor',
        component: () => import('./views/forms/editor'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/form/mask',
        name: 'Form mask',
        component: () => import('./views/forms/mask'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/form/uploads',
        name: 'Form uploads',
        component: () => import('./views/forms/uploads'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/form/validation',
        name: 'Form validation',
        component: () => import('./views/forms/validation'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/form/repeater',
        name: 'Form Repeater',
        component: () => import('./views/forms/repeater'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/form/wizard',
        name: 'Form wizard',
        component: () => import('./views/forms/wizard'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/icons/dripicons',
        name: 'Dripicons icons',
        component: () => import('./views/icons/dripicons'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/icons/fontawesome',
        name: 'Font-awesome icons',
        component: () => import('./views/icons/fontawesome'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/icons/ion',
        name: 'Ion icons',
        component: () => import('./views/icons/ion'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/icons/material',
        name: 'Material icons',
        component: () => import('./views/icons/material'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: "/van-ban-di",
        name: "V??n b???n ??i",
        // meta: {},
        component: () => import("../pages/vanbandi"),
    },
    {
        path: "/xu-ly-van-ban-di",
        name: "V??n b???n ??i",
        // meta: {},
        component: () => import("../pages/vanbandi/xuLyVanBanDi"),
    },
    {
        path: "/cap-so-van-ban-di",
        name: " C???p s??? v??n b???n ??i",
        // meta: {},
        component: () => import("../pages/vanbandi/capSoVanBanDi"),
    },
    {
        path: '/icons/themify',
        name: 'Themify icons',
        component: () => import('./views/icons/themify'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/icons/typicons',
        name: 'Typicons icons',
        component: () => import('./views/icons/typicons'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/tables/basic',
        name: 'Basic table',
        component: () => import('./views/tables/basic'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/tables/advanced',
        name: 'Advanced table',
        component: () => import('./views/tables/advancedtable'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/charts/chartist',
        name: 'Chartist',
        component: () => import('./views/charts/chartist'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/charts/chartjs',
        name: 'Chartjs',
        component: () => import('./views/charts/chartjs/index'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/charts/apex',
        name: 'apex',
        component: () => import('./views/charts/apex'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/charts/echart',
        name: 'Echart chart',
        meta: { authRequired: true },
        component: () => import('./views/charts/echart/index')
    },
    {
        path: '/email-template/basic',
        name: 'Email-template Basic',
        component: () => import('./views/email-template/basic'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/email-template/alert',
        name: 'Alert Email',
        component: () => import('./views/email-template/alert'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/email-template/billing',
        name: 'Billing Email',
        component: () => import('./views/email-template/billing'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/maps/google',
        name: 'Google map',
        component: () => import('./views/maps/google'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/linh-vuc',
        name: 'LinhVuc',
        component: () => import('../pages/linhvuc'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: "/nhom-quyen",
        name: "NhomQuyen",
        meta: {
            authRequired: true,
        },
        component: () => import("../pages/module"),
    },
    {
        path: "/nhom-quyen/action/:id?",
        name: "H??nh ?????ng",
        // meta: {},
        component: () => import("../pages/module/action"),
    },
    {
        path: "/menu",
        name: "Menu",
         meta: {          authRequired: true, },
        component: () => import("../pages/menu"),
    },
    {
        path: "/vai-tro",
        name: "Quy???n",
        meta: {can: 'viewpage-roles'},
        component: () => import("../pages/role"),
    },
    {
        path: "/vai-tro/thiet-lap-quyen/:id?",
        name: "Vai tr??",
        meta: {can: 'viewpage-roles' },
        component: () => import("../pages/role/addPermissions01"),
    },
    {
        path: "/add-permissions",
        name: "Test ",
        component: () => import("../pages/role/addPermissions"),
    },
    {
        path: "/tai-khoan",
        name: "T??i kho???n",
        meta: {
            can: 'viewpage-user',
        },
        component: () => import("../pages/user"),
    },
    {
        path: "/tai-khoan/doi-mat-khau",
        name: "?????i m???t kh???u",
        meta: {can: 'viewpage-user', },
        component: () => import("../pages/user/ChangePass"),
    },
    {
        path: "/don-vi",
        name: "????n v???",
        meta: { },
        component: () => import("../pages/donVi"),
    },
    {
        path: "/loggings",
        name: "Loggings",
        meta: {},
        component: () => import("../pages/loggings"),
    },
    {
        path: "/thong-tin-ca-nhan",
        name: "Th??ng tin c?? nh??n",
        // meta: {},
        component: () => import("../pages/auth/profile"),
    },
    {
        path: "/chuc-vu",
        name: "Ch???c v???",
        // meta: {},
        component: () => import("../pages/chucVu"),
    },
    {
        path: "/loai-van-ban",
        name: "Lo???i v??n b???n",
        // meta: {},
        component: () => import("../pages/loaiVanBan"),
    },
    {
        path: "/trang-thai",
        name: "Tr???ng th??i",
        // meta: {},
        component: () => import("../pages/trangThai"),
    },
    {
        path: "/hinh-thuc-gui",
        name: "H??nh th???c g???i",
        // meta: {},
        component: () => import("../pages/hinhThucGui"),
    },
    {
        path: "/khoi-co-quan",
        name: "Kh???i c?? quan",
        // meta: {},
        component: () => import("../pages/khoiCoQuan"),
    },
    {
        path: "/co-quan",
        name: "C?? quan",
        // meta: {},
        component: () => import("../pages/coQuan"),
    },
    {
        path: "/van-ban-den",
        name: "V??n b???n ?????n",
        //meta: { },
        component: () => import("../pages/vanbandenXL"),
    },
    {
        path: "/xu-ly-van-ban-den",
        name: "V??n b???n ?????n",
        //meta: { },
        component: () => import("../pages/vanbandenXL/xuLyVanBanDen"),
    },
    {
        path: "/cap-so-van-ban-den",
        name: "C???p s??? v??n b???n ?????n",
        //meta: { },
        component: () => import("../pages/vanbandenXL/capSoVanBanDen"),
    },
    {
        path: "/thong-bao",
        name: " Th??ng b??o",
        // meta: {},
        component: () => import("../pages/thongBao"),
    }
    ,
    {
        path: "/ky-so",
        name: "K?? s???",
        // meta: {},
        component: () => import("../pages/kyso"),
    },
    {
        path: "/loai-trang-thai",
        name: " Lo???i tr???ng th??i",
        // meta: {},
        component: () => import("../pages/loaiTrangThai"),
    },
    {
        path: "/xac-nhan-ky-so-noi-bo",
        name: " X??c nh???n k?? s??? n???i b???",
        // meta: {},
        component: () => import("../pages/xacNhanKySo"),
    },
    {
        path: "/lich-cong-tac/:loaiLichCongTac",
        name: "L???ch c??ng t??c",
        // meta: {},
        component: () => import("../pages/lichcongtac/index"),
    },
    {
        path: "/lich-cong-tac-ca-nhan",
        name: "L???ch c??ng t??c c?? nh??n",
        // meta: {},
        component: () => import("../pages/lichcongtac/lichcongtaccanhan"),
    },
    {
        path: "/cong-viec/:id",
        name: "congviec",
        // meta: {},
        component: () => import("../pages/lichcongtac/danhSachCongViec"),
    },
    {
        path: "/sign-digital",
        name: "L???ch c??ng t??c",
        // meta: {},
        component: () => import("../pages/signDigital"),
    },
    {
        path: "/xem-lich-cong-tac/:loaiLichCongTac?",
        name: "Danh s??ch l???ch c??ng t??c",
        // meta: {},
        component: () => import("../pages/lichcongtac/dsLichCongTac"),
    },
    {
        path: "/xac-nhan-ky-so",
        name: "X??c nh???n k?? s??? n???i b???",
        // meta: {},
        component: () => import("../pages/xacNhanKySo/pages"),
    },
    {
        path: "/thu-den",
        name: "ThuDen",
        component: () => import("../pages/hopThu/index"),
    },
    {
        path: "/thu-da-gui",
        name: "ThuDaGui",
        component: () => import("../pages/hopThu/thuDaGui"),
    },
    {
        path: "/thu-rac",
        name: "ThuRac",
        component: () => import("../pages/hopThu/thuRac"),
    },
    {
        path: "/thiet-lap-ky-so-phap-ly",
        name: "ThietLapKySoPhapLy",
        component: () => import("../pages/kySoPhapLy/thietLapKySo"),
    }
]