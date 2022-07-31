import {apiClient} from "@/state/modules/apiClient";
const controller = "HopThu";
export const actions = {
    async getPagingParams({commit}, params) {
        return apiClient.post(controller + "/get-paging-params", params);
    },
    async getPagingParamsDaGui({commit}, params) {
        return apiClient.post(controller + "/get-paging-params-dagui", params);
    },
    async getPagingParamsRac({commit}, params) {
        return apiClient.post(controller + "/get-paging-params-rac", params);
    },
    async createSend({commit}, values) {
        return apiClient.post(controller + "/create-send", values);
    },
    async delete({commit}, id) {
        return await apiClient.delete(controller + "/delete/" + id);
    },
    async deleteR({commit}, id) {
        return await apiClient.delete(controller + "/deleter/" + id);
    },
    async getById({commit}, id) {
        return apiClient.get(controller + "/get-by-id/" + id);
    },
};
