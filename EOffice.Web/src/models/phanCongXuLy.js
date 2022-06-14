import moment from "moment";
const toJson = (item) => {
    return {
        soLuuCV: item.soLuu,
        soVanBanDen: item.soVanBanDen,
        trichYeu: item.trichYeu,
        butPhe: item.butPhe,
        lanhDaoButPhe: item.lanhDaoButPhe,
        ngayButPhe: item.ngayButPhe,
        file: item.file,
        mucDoQuanTrong: item.mucDoQuanTrong,
        nguoiPhuTrach: item.nguoiPhuTrach,
        nguoiChuTri: item.nguoiChuTri,
        nguoiPhoiHopXuLy: item.nguoiPhoiHopXuLy,
        donViXuLy: item.donViXuLy,
        donViPhoiHop: item.donViPhoiHop,
        nguoiXemDeBiet: item.nguoiXemDeBiet,
    }
}

const fromJson = (item) => {
    return {
        soLuuCV: item.soLuu,
        soVanBanDen: item.soVanBanDen,
        trichYeu: item.trichYeu,
        butPhe: item.butPhe,
        lanhDaoButPhe: item.lanhDaoButPhe,
        ngayButPhe: item.ngayButPhe,
        file: item.file,
        mucDoQuanTrong: item.mucDoQuanTrong,
        nguoiPhuTrach: item.nguoiPhuTrach,
        nguoiChuTri: item.nguoiChuTri,
        nguoiPhoiHopXuLy: item.nguoiPhoiHopXuLy,
        donViXuLy: item.donViXuLy,
        donViPhoiHop: item.donViPhoiHop,
        nguoiXemDeBiet: item.nguoiXemDeBiet,
    }
}

const baseJson = () => {
    return {
        soLuuCV: null,
        soVanBanDen: null,
        trichYeu: null,
        butPhe: null,
        lanhDaoButPhe: null,
        ngayButPhe: new Date(),
        file: null,
        mucDoQuanTrong: null,
        nguoiPhuTrach: null,
        nguoiChuTri: null,
        nguoiPhoiHopXuLy: null,
        donViXuLy: null,
        donViPhoiHop: null,
        nguoiXemDeBiet: null,
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

export const phanCongXyLyModel = {
    toJson, fromJson, baseJson, toListModel
}

