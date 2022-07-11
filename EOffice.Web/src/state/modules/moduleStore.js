import {apiClient} from "@/state/modules/apiClient";

const controller = "module";
export const actions = {
    async getAll({commit}) {
        return apiClient.get(controller + "/get-all");
    },
    async create({commit}, values) {
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
    async createPermission({commit}, values) {
        return apiClient.post(controller + "/createPermission", values);
    },
    async deletePermission({commit}, values) {
        console.log("deletePermission", values)
        return apiClient.post(controller + "/deletePermission", values);
    }
};
