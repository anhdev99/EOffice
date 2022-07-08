const addMessage = (item, show, code) => {
    return {
        resultString: item.resultString,
        resultCode: item.resultCode,
        isShow: show,
        code: code
    }
}

export const notifyModel = {addMessage};