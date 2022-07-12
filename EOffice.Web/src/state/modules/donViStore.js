import {apiClient} from "@/state/modules/apiClient";
const  controller = "DonVi";
export const actions = {
    async getAll({commit}) {
        return apiClient.get(controller +"/get-all");
    },
    async getTree({commit}) {
        return apiClient.get(controller +"/getTree");
    },
    async getPagingParams({commit}, params) {
        if (params.linhVuc != null)
        {
            params.linhVuc = params.linhVuc.id
        }
        return apiClient.post(controller +"/get-paging-params", params);
    },
    async create({commit}, values) {
        return apiClient.post(controller +"/create", values);
    },
    async update({commit, dispatch}, values) {
        return apiClient.put(controller +"/update", values);
    },
    async delete({commit}, id) {
        return await apiClient.delete(controller +"/delete/" + id);
    },
    async getById({commit}, id) {
        return apiClient.get(controller +"/get-by-id/" + id);
    }
};
