import moment from "moment";
const toJson = (item) => {
    return {
        id: item.id,
        ten: item.ten,
        thuTu: item.thuTu,
        code: item.code,
        nextTrangThai: item.nextTrangThai,
        actions: item.actions,
        color: item.color
    }
}
const fromJson = (item) => {
    return {
        id: item.id,
        ten: item.ten,
        thuTu: item.thuTu,
        code: item.code,
        color: item.color,
        modifiedAt: item.modifiedAt,
        createdBy: item.createdBy,
        modifiedBy: item.modifiedBy,
        lastModifiedShow: item.lastModifiedShow,
        createdAtShow : item.createdAtShow,
        nextTrangThai: item.nextTrangThai,
        actions: item.actions,
    }
}

const baseJson = () => {
    return {
        id: null,
        ten: null,
        thuTu: 0,
        code: null,
        createdAt: null,
        modifiedAt: null,
        createdBy: null,
        modifiedBy: null,
        nextTrangThai: null,
        actions: null,
        color: null
    }
}

const currentBaseJson = () =>{
    return {
        currentTrangThai: null,
        newTrangThai: null,
        vanBanDiId: null
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

export const trangThaiModel = {
    toJson, fromJson, baseJson, toListModel, currentBaseJson
}
