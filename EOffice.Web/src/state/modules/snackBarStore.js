
const options = {
    timeout: 10000
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
            this.$toast.success(data.resultString, options);
        } else if (data.resultCode === 'NAME_EXIST') {
            this.$toast.warning(data.resultString, options);
        }
        else {
            this.$toast.error(data.resultString, options);
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