import {apiClient} from "@/state/modules/apiClient";
const controller = "VanBanDi";
export const actions = {
    async get({commit}) {
        return apiClient.get(controller +"/get");
    },
    async getPagingParams({commit}, params) {
        return apiClient.post(controller + "/get-paging-params", params);
    },
    async getPagingParamsCapSo({commit}, params) {
        return apiClient.post(controller + "/get-paging-params-capso", params);
    },
    async getPagingParamsXuLy({commit}, params) {
        return apiClient.post(controller + "/get-paging-params-xuly", params);
    },
    async getPagingParamsVM({commit}, params) {
        return apiClient.post(controller + "/get-paging-params-vm", params);
    },
    async create({commit}, values) {
        return apiClient.post(controller + "/create", values);
    },
    async capSoVanBanDi({commit}, values) {
        return apiClient.post(controller + "/cap-so-van-ban-di", values);
    },
    async assignOrReject({commit}, values) {
        return apiClient.post(controller + "/assign-or-reject", values);
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
    },
    async getVaCapSo({commit}, id) {
        return apiClient.get(controller + "/cap-so-khi-tao-van-ban");
    }
};
