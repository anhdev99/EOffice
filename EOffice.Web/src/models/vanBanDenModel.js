import moment from "moment";
const toJson = (item) => {
    return {
        id: item.id,
        version: item.version,
        number: item.number,
        loaiVanBan: item.loaiVanBan,
        loaiVanBanTen: item.loaiVanBanTen,
        trangThaiTen: item.trangThaiTen,
        soLuuCV: item.soLuuCV,
        soVBDen: item.soVBDen,
        ngayNhap: item.ngayNhap,
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
        butPhe: item.butPhe,
        donViXuLy: item.donViXuLy,
        phanCong: item.phanCong,
        congVanChiDoc: item.congVanChiDoc,
        banChinh: item.banChinh,
        hienThiThongBao: item.hienThiThongBao,
    }
}
const fromJson = (item) => {
    return {
        id: item.id,
        version: item.version,
        number: item.number,
        loaiVanBan: item.loaiVanBan,
        loaiVanBanTen: item.loaiVanBanTen,
        trangThaiTen: item.trangThaiTen,
        soLuuCV: item.soLuuCV,
        soVBDen: item.soVBDen,
        ngayNhap: item.ngayNhap,
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
        butPhe: item.butPhe,
        donViXuLy: item.donViXuLy,
        phanCong: item.phanCong,
        congVanChiDoc: item.congVanChiDoc,
        banChinh: item.banChinh,
        hienThiThongBao: item.hienThiThongBao,
    }
}

const baseJson = () => {
    return {
        id: null,
        version: null,
        number: 0,
        loaiVanBan: null,
        loaiVanBanTen: null,
        trangThaiTen: null,
        soLuuCV: null,
        soVBDen: null,
        ngayNhap: new Date(),
        ngayTraLoi: new Date(),
        traLoiCVso: null,
        soBan: null,
        trichYeu: null,
        donViSoan: null,
        donViSoanTen: null,
        canBoSoan: null,
        canBoSoanTen: null,
        hinhThucGui: null,
        hinhThucGuiTen: null,
        hanXuLy: new Date(),
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
        butPhe: null,
        donViXuLy: null,
        phanCong: null,
        congVanChiDoc: false,
        banChinh: false,
        hienThiThongBao: false,
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
