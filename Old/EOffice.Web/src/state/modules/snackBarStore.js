import { createToaster } from "@meforma/vue-toaster";

const toaster = createToaster({ /* options */ });


const options = {
    timeout: 10000,
    position: "top-right"
}

const initialNotify =  {resultString: null, resultCode: null};

export const state = {
    isMenuAction: false
}
export const mutations = {
    SET_MENU(state, newValue) {
        state.isMenuAction = newValue
    }
}
export const actions = {
    async addNotify({commit}, data = initialNotify)
    {
        console.log("data", data);
        if (data.resultCode === 'SUCCESS') {
            console.log("data.resultString", data.resultString );
            toaster.success(data.resultString, options);
        } else if (data.resultCode === 'NAME_EXIST') {
            toaster.warning(data.resultString, options);
        }
        else {
            toaster.error(data.resultString, options);
        }
    },
    async addNotification({commit}, data = {resultString: null, resultCode: null, isShow: false, code: null}){
        commit('SET_NOTIFY', data);
    },
    async clickMenu({commit}, data){
        console.log("SET_MENU",data)
        commit("SET_MENU", data)
    }
}