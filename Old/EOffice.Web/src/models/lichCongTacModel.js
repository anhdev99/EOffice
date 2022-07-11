import moment from "moment";
const toJson = (item) => {
    return {
        ngayCongTac: item.ngayCongTac,
        thoiGian: item.thoiGian,
        tieuDe: item.tieuDe,
        chuTri: item.chuTri,
        diaDiem: item.diaDiem,
        thanhPhanThamDu: item.thanhPhanThamDu,
        ghiChu: item.ghiChu,
    }
}

const fromJson = (item) => {
    return {
        ngayCongTac: item.ngayCongTac,
        thoiGian: item.thoiGian,
        tieuDe: item.tieuDe,
        chuTri: item.chuTri,
        diaDiem: item.diaDiem,
        thanhPhanThamDu: item.thanhPhanThamDu,
        ghiChu: item.ghiChu,
    }
}

const baseJson = () => {
    return {
        ngayCongTac: new Date(),
        thoiGian: null,
        tieuDe: null,
        chuTri: null,
        diaDiem: null,
        thanhPhanThamDu: null,
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

export const lichCongTacModel = {
    toJson, fromJson, baseJson, toListModel
}

