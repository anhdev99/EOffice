import {apiClient} from "@/state/modules/apiClient";
const controller = "VanBanDen";
export const actions = {
    async get({commit}) {
        return apiClient.get(controller +"/get");
    },
    async getPagingParams({commit}, params) {
        return apiClient.post(controller + "/get-paging-params", params);
    },
    async getPagingParamsVM({commit}, params) {
        return apiClient.post(controller + "/get-paging-params-vm", params);
    },
    async create({commit}, values) {
        console.log("createVanBanDen", values);
        return apiClient.post(controller + "/create", values);
    },
    async update({commit, dispatch}, values) {
        return apiClient.put(controller + "/update", values);
    },
    async delete({commit}, id) {
        return await apiClient.delete(controller + "/delete/" + id);
    },
    async getById({commit}, id) {
        return apiClient.get(controller + "/get-by-id/" + id);
    }
};
