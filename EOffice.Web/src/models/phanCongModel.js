import moment from "moment";
const toJson = (item) => {
    return {
        id: item.id,
        yKienChiDao: item.yKienChiDao,
        nguoiButPhe: item.nguoiButPhe,
        NguoiNhanXuLy: item.NguoiNhanXuLy,
        file: item.file,

    }
}
const fromJson = (item) => {
    return {
        id: item.id,
        yKienChiDao: item.yKienChiDao,
        nguoiButPhe: item.nguoiButPhe,
        NguoiNhanXuLy: item.NguoiNhanXuLy,
        file: item.file,
    }
}

const baseJson = (items) => {
    return {
        id: items?.id ?? null,
        vanBanDenId: null,
        yKienChiDao: null,
        nguoiButPhe: null,
        NguoiNhanXuLy: null,
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

export const phanCongModel = {
    toJson, fromJson, baseJson, toListModel
}
