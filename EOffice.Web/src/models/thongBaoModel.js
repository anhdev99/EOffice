const toJson = (item) => {
    return {
        id: item.id,
        title: item.title,
        content: item.content,
        read: item.read,
        recipientId: item.recipientId,
        recipient: item.recipient,
        senderId : item.senderId,
        sender:item.sender,
        url:item.url,
        congVanId: item.congVanId,
        loaiCongVan: item.loaiCongVan
    }
}

const fromJson = (item) => {
    return {
        id: item.id,
        title: item.title,
        content: item.content,
        read: item.read,
        recipientId: item.recipientId,
        recipient: item.recipient,
        senderId : item.senderId,
        sender:item.sender,
        url:item.url,
        congVanId: item.congVanId,
        loaiCongVan: item.loaiCongVan
    }
}

const baseJson = () => {
    return {
        id: null,
        title: null,
        content: null,
        read: null,
        recipientId: null,
        recipient: null,
        senderId : null,
        sender:null,
        url:null,
        createdAt: null,
        congVanId: null,
        loaiCongVan: 0
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

export const thongBaoModel = {
    toJson, fromJson, baseJson, toListModel
}
