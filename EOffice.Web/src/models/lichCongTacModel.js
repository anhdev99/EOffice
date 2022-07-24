import moment from "moment";
const toJson = (item) => {
    return {
        id: item.id,
        tuNgay: item.tuNgay,
        denNgay: item.denNgay,
        thoiGian: item.thoiGian,
        chuTri: item.chuTri,
        mauSac: item.mauSac,
        diaDiem: item.diaDiem,
        tieuDe: item.tieuDe,
        ghiChu: item.ghiChu,
        thanhPhanThamDu: item.thanhPhanThamDu,
    }
}
const fromJson = (item) => {
    return {
        id: item.id,
        tuNgay: item.tuNgay,
        denNgay: item.denNgay,
        thoiGian: item.thoiGian,
        chuTri: item.chuTri,
        mauSac: item.mauSac,
        diaDiem: item.diaDiem,
        tieuDe: item.tieuDe,
        ghiChu: item.ghiChu,
        thanhPhanThamDu: item.thanhPhanThamDu,
    }
}

const baseJson = () => {
    return {
        id: null,
        tuNgay: null,
        denNgay: null,
        thoiGian: null,
        chuTri: null,
        mauSac: null,
        diaDiem: null,
        tieuDe: null,
        ghiChu: null,
        thanhPhanThamDu: null,
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
