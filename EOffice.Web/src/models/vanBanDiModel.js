import moment from "moment";
const toJson = (item) => {
    return {
        soLuuCV: item.soLuuCV,
        soVBDen: item.soVBDen,
        loaiVanBan: item.loaiVanBan,
        ngayNhap:item.ngayNhap,
        trangThai: item.trangThai,
        traLoiCVSo: item.traLoiCVSo,
        ngayTraLoi: item.ngayTraLoi,
        soBan: item.soBan,
        trichYeu: item.trichYeu,
        khoiCoQuanNhan: item.khoiCoQuanNhan,
        coQuanNhan: item.coQuanNhan,
        file: item.file,
        donViSoan: item.donViSoan,
        canBoSoan: item.canBoSoan,
        hinhThucGui: item.hinhThucGui,
        linhVuc: item.linhVuc,
        mucDoTinhChat: item.mucDoTinhChat,
        mucDoBaoMat: item.mucDoBaoMat,
        hoSoDonVi: item.hoSoDonVi,
        noiLuuTru: item.noiLuuTru,
        ghiChu: item.ghiChu,
    }
}

const fromJson = (item) => {
    return {
        soLuuCV: item.soLuuCV,
        soVBDen: item.soVBDen,
        loaiVanBan: item.loaiVanBan,
        ngayNhap:item.ngayNhap,
        trangThai: item.trangThai,
        traLoiCVSo: item.traLoiCVSo,
        ngayTraLoi: item.ngayTraLoi,
        soBan: item.soBan,
        trichYeu: item.trichYeu,
        khoiCoQuanNhan: item.khoiCoQuanNhan,
        coQuanNhan: item.coQuanNhan,
        file: item.file,
        donViSoan: item.donViSoan,
        canBoSoan: item.canBoSoan,
        hinhThucGui: item.hinhThucGui,
        linhVuc: item.linhVuc,
        mucDoTinhChat: item.mucDoTinhChat,
        mucDoBaoMat: item.mucDoBaoMat,
        hoSoDonVi: item.hoSoDonVi,
        noiLuuTru: item.noiLuuTru,
        ghiChu: item.ghiChu,
    }
}

const baseJson = () => {
    return {
        soLuuCV: null,
        soVBDen: null,
        loaiVanBan: null,
        ngayNhap: new Date(),
        trangThai: null,
        traLoiCVSo: null,
        ngayTraLoi: new Date(),
        soBan: null,
        trichYeu: null,
        khoiCoQuanNhan: null,
        coQuanNhan: null,
        file: null,
        donViSoan: null,
        canBoSoan: null,
        hinhThucGui: null,
        linhVuc: null,
        mucDoTinhChat: null,
        mucDoBaoMat: null,
        hoSoDonVi: null,
        noiLuuTru: null,
        ghiChu: null,
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

export const vanBanDiModel = {
    toJson, fromJson, baseJson, toListModel
}

