import moment from "moment";
const toJson = (item) => {
    return {
        soLuuCV: item.soLuuCV,
        soVBDen: item.soVBDen,
        loaiVanBan: item.loaiVanBan,
        trichYeu: item.trichYeu,
        ngayBanHanh: item.ngayBanHanh,
        ngayNhan: item.ngayNhan,
        trangThaiVanBan: item.trangThaiVanBan,
        ngayKy: item.ngayKy,
        nguoiKy: item.nguoiKy,
        thoiHanXuLy: item.thoiHanXuLy,
        khoiCoQuanGui: item.khoiCoQuanGui,
        coQuanGui: item.coQuanGui,
        hinhThucNhan: item.hinhThucNhan,
        linhVuc: item.linhVuc,
        mucDoTinhChat: item.mucDoTinhChat,
        mucDoBaoMat: item.mucDoBaoMat,
        hoSoDonVi: item.hoSoDonVi,
        noiLuuTru: item.noiLuuTru,
        congVanChiDoc: item.congVanChiDoc,
        banChinh: item.banChinh,
        thongBao: item.thongBao,
    }
}

const fromJson = (item) => {
    return {
        soLuuCV: item.soLuuCV,
        soVBDen: item.soVBDen,
        loaiVanBan: item.loaiVanBan,
        trichYeu: item.trichYeu,
        ngayBanHanh: item.ngayBanHanh,
        ngayNhan: item.ngayNhan,
        trangThaiVanBan: item.trangThaiVanBan,
        ngayKy: item.ngayKy,
        nguoiKy: item.nguoiKy,
        thoiHanXuLy: item.thoiHanXuLy,
        khoiCoQuanGui: item.khoiCoQuanGui,
        coQuanGui: item.coQuanGui,
        hinhThucNhan: item.hinhThucNhan,
        linhVuc: item.linhVuc,
        mucDoTinhChat: item.mucDoTinhChat,
        mucDoBaoMat: item.mucDoBaoMat,
        hoSoDonVi: item.hoSoDonVi,
        noiLuuTru: item.noiLuuTru,
        congVanChiDoc: item.congVanChiDoc,
        banChinh: item.banChinh,
        thongBao: item.thongBao,
        butphe: item.butphe,
        ngayButPhe: item.ngayButPhe,
        nguoiButPhe: item.nguoiButPhe,
    }
}

const baseJson = () => {
    return {
        soLuuCV: null,
        soVBDen: null,
        loaiVanBan: null,
        trichYeu: null,
        ngayBanHanh: new Date(),
        ngayNhan: new Date(),
        trangThaiVanBan: null,
        ngayKy: new Date(),
        nguoiKy: null,
        thoiHanXuLy: new Date(),
        khoiCoQuanGui: null,
        coQuanGui: null,
        hinhThucNhan: null,
        linhVuc: null,
        mucDoTinhChat: null,
        mucDoBaoMat: null,
        hoSoDonVi: null,
        noiLuuTru: null,
        congVanChiDoc: null,
        banChinh: null,
        thongBao: null,
    //  bút phê lãnh đạo
        butphe: null,
        ngayButPhe: new Date(),
        nguoiButPhe: null,
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

