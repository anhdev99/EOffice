
const toJson = (item) => {
    return {
        id: item.id,
        ten: item.ten,
        code: item.code,
        thuTu: item.thuTu
    }
}

const fromJson = (item) => {
    return {
        id: item.id,
        ten: item.ten,
        code: item.code,
        thuTu: item.thuTu,
        createdAt: item.createdAt,
        modifiedAt: item.modifiedAt,
        createdBy: item.createdBy,
        modifiedBy: item.modifiedBy
    }
}

const baseJson = () => {
    return {
        id: null,
        ten: null,
        code: null,
        thuTu: 0,
        createdAt: null,
        modifiedAt: null,
        createdBy: null,
        modifiedBy: null,
        moTa: null
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

export const roleModel = {
    toJson, fromJson, baseJson, toListModel
}
