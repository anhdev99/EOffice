import {apiClient} from "@/state/modules/apiClient";
const controller = "HopThu";
export const actions = {
    async getPagingParams({commit}, params) {
        return apiClient.post(controller + "/get-paging-params", params);
    },
    async createSend({commit}, values) {
        return apiClient.post(controller + "/create-send", values);
    },
    async delete({commit}, id) {
        return await apiClient.delete(controller + "/delete/" + id);
    },
    async getById({commit}, id) {
        return apiClient.get(controller + "/get-by-id/" + id);
    },
};
