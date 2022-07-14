import moment from "moment";
const toJson = (item) => {
    return {
        id: item.id,
        noiDungButPhe: item.noiDungButPhe,
        nguoiButPhe: item.nguoiButPhe,
        ngayButPhe: item.ngayButPhe,
        file: item.file,
        mucDoQuanTrong: item.mucDoQuanTrong,
        nguoiPhuTrach: item.nguoiPhuTrach,
        nguoiChuTri: item.nguoiChuTri,
        nguoiPhoihopXuLy: item.nguoiPhoihopXuLy,
        donViXuLy: item.donViXuLy,
        donViPhoiHop: item.donViPhoiHop,
        nguoiXemDeBiet: item.nguoiXemDeBiet,

    }
}
const fromJson = (item) => {
    return {
        id: item.id,
        noiDungButPhe: item.noiDungButPhe,
        nguoiButPhe: item.nguoiButPhe,
        ngayButPhe: item.ngayButPhe,
        file: item.file,
        mucDoQuanTrong: item.mucDoQuanTrong,
        nguoiPhuTrach: item.nguoiPhuTrach,
        nguoiChuTri: item.nguoiChuTri,
        nguoiPhoihopXuLy: item.nguoiPhoihopXuLy,
        donViXuLy: item.donViXuLy,
        donViPhoiHop: item.donViPhoiHop,
        nguoiXemDeBiet: item.nguoiXemDeBiet,
    }
}

const baseJson = () => {
    return {
        id: null,
        noiDungButPhe: null,
        nguoiButPhe: null,
        ngayButPhe: new Date(),
        file: null,
        mucDoQuanTrong: null,
        nguoiPhuTrach: null,
        nguoiChuTri: null,
        nguoiPhoihopXuLy: null,
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

export const butPheModel = {
    toJson, fromJson, baseJson, toListModel
}
