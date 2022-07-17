import moment from "moment";
const toJson = (item) => {
    return {
        id: item.id,
        version: item.version,
        number: item.number,
        loaiVanBan: item.loaiVanBan,
        loaiVanBanTen: item.loaiVanBanTen,
        trangThai: item.trangThai,
        trangThaiTen: item.trangThaiTen,
        soLuuCV: item.soLuuCV,
        soVBDen: item.soVBDen,
        ngayNhap: item.ngayNhap,
        ngayBanHanh: item.ngayBanHanh,
        ngayTraLoi: item.ngayTraLoi,
        traLoiCVso: item.traLoiCVso,
        soBan: item.soBan,
        trichYeu: item.trichYeu,
        donViSoan: item.donViSoan,
        donViSoanTen: item.donViSoanTen,
        canBoSoan: item.canBoSoan,
        canBoSoanTen: item.canBoSoanTen,
        hinhThucGui: item.hinhThucGui,
        hinhThucGuiTen: item.hinhThucGuiTen,
        hanXuLy: item.hanXuLy,
        linhVuc: item.linhVuc,
        linhVucTen: item.linhVucTen,
        mucDoBaoMat: item.mucDoBaoMat,
        mucDoBaoMatTen: item.mucDoBaoMatTen,
        mucDoTinhChat: item.mucDoTinhChat,
        mucDoTinhChatTen: item.mucDoTinhChatTen,
        hoSoDonVi: item.hoSoDonVi,
        hoSoDonViTen: item.hoSoDonViTen,
        noiLuuTru: item.noiLuuTru,
        coQuanNhan: item.coQuanNhan,
        coQuanNhanTen: item.coQuanNhanTen,
        congVanChiDoc: item.congVanChiDoc,
        banChinh: item.banChinh,
        hienThiThongBao: item.hienThiThongBao,
        ngayNhan: item.ngayNhan,
        coQuanGui: item.coQuanGui,
        khoiCoQuanGui: item.khoiCoQuanGui,
        hinhThucNhan: item.hinhThucNhan,
        nguoiKy: item.nguoiKy,

        butphe: item.butphe,
        donViNhanXuLy: item.donViNhanXuLy,
        phanCong: item.phanCong,
    }
}
const fromJson = (item) => {
    return {
        id: item.id,
        version: item.version,
        number: item.number,
        loaiVanBan: item.loaiVanBan,
        loaiVanBanTen: item.loaiVanBanTen,
        trangThai: item.trangThai,
        trangThaiTen: item.trangThaiTen,
        soLuuCV: item.soLuuCV,
        soVBDen: item.soVBDen,
        ngayNhap: item.ngayNhap,
        ngayTraLoi: item.ngayTraLoi,
        ngayBanHanh: item.ngayBanHanh,
        traLoiCVso: item.traLoiCVso,
        soBan: item.soBan,
        trichYeu: item.trichYeu,
        donViSoan: item.donViSoan,
        donViSoanTen: item.donViSoanTen,
        canBoSoan: item.canBoSoan,
        canBoSoanTen: item.canBoSoanTen,
        hinhThucGui: item.hinhThucGui,
        hinhThucGuiTen: item.hinhThucGuiTen,
        hanXuLy: item.hanXuLy,
        linhVuc: item.linhVuc,
        linhVucTen: item.linhVucTen,
        mucDoBaoMat: item.mucDoBaoMat,
        mucDoBaoMatTen: item.mucDoBaoMatTen,
        mucDoTinhChat: item.mucDoTinhChat,
        mucDoTinhChatTen: item.mucDoTinhChatTen,
        hoSoDonVi: item.hoSoDonVi,
        hoSoDonViTen: item.hoSoDonViTen,
        noiLuuTru: item.noiLuuTru,
        coQuanNhan: item.coQuanNhan,
        coQuanNhanTen: item.coQuanNhanTen,
        congVanChiDoc: item.congVanChiDoc,
        banChinh: item.banChinh,
        hienThiThongBao: item.hienThiThongBao,
        ngayNhan: item.ngayNhan,
        coQuanGui: item.coQuanGui,
        khoiCoQuanGui: item.khoiCoQuanGui,
        hinhThucNhan: item.hinhThucNhan,
        nguoiKy: item.nguoiKy,

        butphe: item.butphe,
        donViNhanXuLy: item.donViNhanXuLy,
        phanCong: item.phanCong,
    }
}

const baseJson = (items) => {
    return {
        version: 1,
        id: null,
        number: 0,
        loaiVanBan: null,
        loaiVanBanTen: null,
        trangThai: null,
        trangThaiTen: null,
        soLuuCV: null,
        soVBDen: null,
        ngayNhap: null,
        ngayTraLoi: null,
        ngayBanHanh: null,
        traLoiCVso: null,
        soBan: null,
        trichYeu: null,
        donViSoan: null,
        donViSoanTen: null,
        canBoSoan: null,
        canBoSoanTen: null,
        hinhThucNhan: null,
        hinhThucNhantTen: null,
        hinhThucGui: null,
        hinhThucGuiTen: null,
        hanXuLy: null,
        linhVuc: null,
        linhVucTen: null,
        mucDoBaoMat: null,
        mucDoBaoMatTen: null,
        mucDoTinhChat: null,
        mucDoTinhChatTen: null,
        hoSoDonVi: null,
        hoSoDonViTen: null,
        noiLuuTru: null,
        coQuanNhan: null,
        coQuanNhanTen: null,
        congVanChiDoc: false,
        banChinh: false,
        hienThiThongBao: false,
        ngayNhan: null,
        coQuanGui: null,
        khoiCoQuanGui: null,
        nguoiKy: null,
        ngayKy: new Date(),
        file: null,
        uploadFiles: null,
        nguoiPhanCong: null,
        butPhe: null,
        phanCong: null,
        donViXuLy: null,
    }
}

const toListModel = (items) =>{
    if(items.length > 0){
        let data = [];
        items.map((value, index) =>{
            data.push(fromJson(value));
        })
        return data??[];
    }
    return [];
}

export const vanBanDenModel = {
    toJson, fromJson, baseJson, toListModel
}
