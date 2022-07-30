const toJson = (item) => {
    return {
        id: item.id,
        tuNgay: item.tuNgay,
        denNgay: item.denNgay,
        thoiGian: item.thoiGian,
        mauSac: item.mauSac,
        diaDiem: item.diaDiem,
        noiDung: item.noiDung,
        ghiChu: item.ghiChu,
        thanhPhanThamDu: item.thanhPhanThamDu,
        thanhPhan: item.thanhPhan,
        file: item.file,
        fileUpload: item.fileUpload,
        lichCongTacId: item.lichCongTacId
    }
}
const fromJson = (item) => {
    return {
        id: item.id,
        tuNgay: item.tuNgay,
        denNgay: item.denNgay,
        thoiGian: item.thoiGian,
        mauSac: item.mauSac,
        diaDiem: item.diaDiem,
        noiDung: item.noiDung,
        ghiChu: item.ghiChu,
        thanhPhanThamDu: item.thanhPhanThamDu,
        thanhPhan: item.thanhPhan,
        file: item.file,
        fileUpload: item.fileUpload,
        lichCongTacId: item.lichCongTacId
    }
}

const baseJson = () => {
    return {
        id: null,
        tuNgay: null,
        denNgay: null,
        thoiGian: null,
        mauSac: null,
        diaDiem: "",
        noiDung: "",
        ghiChu: "",
        thanhPhanThamDu: null,
        thanhPhan: "",
        file: null,
        fileUpload: null,
        lichCongTacId: null
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

export const congViecModel = {
    toJson, fromJson, baseJson, toListModel
}
