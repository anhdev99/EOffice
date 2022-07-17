import {apiClient} from "@/state/modules/apiClient";
const controller = "VanBanDi";
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
        return apiClient.post(controller + "/create", values);
    },
    async assignSign({commit}, values) {
        return apiClient.post(controller + "/them-nguoi-ky-so", values);
    },
    async removeAssignSign({commit}, values) {
        return apiClient.post(controller + "/xoa-nguoi-ky-so", values);
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
    async getPhanCongKySoByVanBanId({commit}, id) {
        return apiClient.get(controller + "/get-phancongkyso-by-vanbanid/" + id);
    }
};
