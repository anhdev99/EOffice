import {apiClient} from "@/state/modules/apiClient";
const controller = "HistoryVanBan";
export const actions = {
    async getHistoryVanBanDi({commit}, id) {
        return apiClient.get(controller + "/get-van-ban-di-id/" + id);
    }
};
