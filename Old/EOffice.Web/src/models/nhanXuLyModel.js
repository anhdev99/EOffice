import moment from "moment";
const toJson = (item) => {
    return {
        vanBanDen: item.vanBanDen,
        trangThai: item.trangThai,
        content: item.content,
        file: item.file,
        yKienChiDao: item.yKienChiDao,
        nguoiNhanXuLy: item.nguoiNhanXuLy
    }
}

const fromJson = (item) => {
    return {
        vanBanDen: item.vanBanDen,
        trangThai: item.trangThai,
        content: item.content,
        file: item.file,
        yKienChiDao: item.yKienChiDao,
        nguoiNhanXuLy: item.nguoiNhanXuLy
    }
}

const baseJson = () => {
    return {
        vanBanDen: null,
        trangThai: null,
        content: null,
        file: null,
        yKienChiDao: null,
        nguoiNhanXuLy: null,
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

export const nhanXuLyModel = {
    toJson, fromJson, baseJson, toListModel
}

