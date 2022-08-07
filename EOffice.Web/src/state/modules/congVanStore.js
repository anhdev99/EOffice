import {apiClient} from "@/state/modules/apiClient";
const controller = "CongVan";
export const actions = {
    async get({commit}) {
        return apiClient.get(controller +"/get");
    },
    async getPagingParams({commit}, params) {
        return apiClient.post(controller + "/get-paging-params", params);
    },
    async getPagingParamsLuuCVDen({commit}, params) {
        return apiClient.post(controller + "/get-paging-params-luucvden", params);
    },
    async getPagingParamsLuuCVDi({commit}, params) {
        return apiClient.post(controller + "/get-paging-params-luucvdi", params);
    },
    async create({commit}, values) {
        console.log("values van ban den", values);
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
    },
    async getByIdLuuCVDen({commit}, id) {
        return apiClient.get(controller + "/get-by-id-luucvden/" + id);
    },
    async getByIdLuuCVDi({commit}, id) {
        return apiClient.get(controller + "/get-by-id-luucvdi/" + id);
    },
};
