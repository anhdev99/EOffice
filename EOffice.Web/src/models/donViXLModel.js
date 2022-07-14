import moment from "moment";
const toJson = (item) => {
    return {
        id: iten.id,
        NguoiPhuTrach: item.NguoiPhuTrach,
        NguoiChuTri: item.NguoiChuTri,
        NguoiPhoiHopXuLy: item.NguoiPhoiHopXuLy,
        NguoiXemDeBiet: item.NguoiXemDeBiet,
        DonViNhanXuLy: item.DonViNhanXuLy,
        DonViPhoiHop: item.DonViPhoiHop,
        TrangThaiDuyetVB: item.TrangThaiDuyetVB,
        NguoiDuyetVB: item.NguoiDuyetVB,
        NgayDuyetVB: item.NgayDuyetVB,
        ghiChu: item.ghiChu,
    }
}
const fromJson = (item) => {
    return {
        id: iten.id,
        NguoiPhuTrach: item.NguoiPhuTrach,
        NguoiChuTri: item.NguoiChuTri,
        NguoiPhoiHopXuLy: item.NguoiPhoiHopXuLy,
        NguoiXemDeBiet: item.NguoiXemDeBiet,
        DonViNhanXuLy: item.DonViNhanXuLy,
        DonViPhoiHop: item.DonViPhoiHop,
        TrangThaiDuyetVB: item.TrangThaiDuyetVB,
        NguoiDuyetVB: item.NguoiDuyetVB,
        NgayDuyetVB: item.NgayDuyetVB,
        ghiChu: item.ghiChu,
    }
}

const baseJson = () => {
    return {
        id: null,
        NguoiPhuTrach: null,
        NguoiChuTri: null,
        NguoiPhoiHopXuLy: null,
        NguoiXemDeBiet: null,
        DonViNhanXuLy: null,
        DonViPhoiHop: null,
        TrangThaiDuyetVB: null,
        NguoiDuyetVB: null,
        NgayDuyetVB: new Date(),
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

export const donViXLModel = {
    toJson, fromJson, baseJson, toListModel
}
