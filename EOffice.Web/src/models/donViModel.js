import moment from "moment";
const toJson = (item) => {
    return {
        id: item.id,
        maDonVi: item.maDonVi,
        ten: item.ten,
        donViCha: item.donViCha,
    }
}

const fromJson = (item) => {
    return {
        id: item.id,
        maDonVi: item.maDonVi,
        ten: item.ten,
        donViCha: item.donViCha,
    }
}

const baseJson = () => {
    return {
        id: null,
        maDonVi: null,
        ten: null,
        donViCha: null,
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

export const donViModel = {
    toJson, fromJson, baseJson, toListModel
}
