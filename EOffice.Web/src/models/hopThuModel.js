const toJson = (item) => {
    return {
        id: item.id,
        tieuDe: item.tieuDe,
        nguoiNhans: item.nguoiNhans,
        cc: item.cc,
        noiDung: item.noiDung,
        files: item.files,
        uploadFiles: item.uploadFiles,
        nguoiGui: item.nguoiGui,
        nguoiNhan: item.nguoiNhan,
        ngayGui: item.ngayGui,
        ngayNhan: item.ngayNhan,
        daXem: item.daXem,
        ngayNhanFull: item.ngayNhanFull
    }
}

const fromJson = (item) => {
    return {
        id: item.id,
        tieuDe: item.tieuDe,
        nguoiNhans: item.nguoiNhans,
        cc: item.cc,
        noiDung: item.noiDung,
        files: item.files,
        uploadFiles: item.uploadFiles,
        nguoiGui: item.nguoiGui,
        nguoiNhan: item.nguoiNhan,
        ngayGui: item.ngayGui,
        ngayNhan: item.ngayNhan,
        daXem: item.daXem,
        ngayNhanFull: item.ngayNhanFull
    }
}

const baseJson = () => {
    return {
        id: null,
        tieuDe: null,
        nguoiNhans: null,
        cc: null,
        noiDung: null,
        files:null,
        uploadFiles: null,
        nguoiGui: null,
        nguoiNhan: null,
        ngayGui: null,
        ngayNhan: null,
        daXem: false,
        ngayNhanFull: null
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
export const hopThuModel = {
    toJson, fromJson, baseJson, toListModel
}
