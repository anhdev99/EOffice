const toJson = (item) => {
    return {
        id: item.id,
        ngayXepLich: item.ngayXepLich,
        chuTri: item.chuTri,
        congViecs: item.congViecs,
        loaiLichCongTac: item.loaiLichCongTac
    }
}
const fromJson = (item) => {
    return {
        id: item.id,
        ngayXepLich: item.ngayXepLich,
        chuTri: item.chuTri,
        congViecs: item.congViecs,
        loaiLichCongTac: item.loaiLichCongTac
    }
}

const baseJson = () => {
    return {
        id: null,
        ngayXepLich: null,
        chuTri: null,
        congViecs: null,
        lichCongTac: null,
        loaiLichCongTac: null
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
