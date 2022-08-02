import {apiClient} from "@/state/modules/apiClient";
const controller = "SignDigital";
export const actions = {
    async kySoPhapLy({commit}, values) {
        return apiClient.post(controller + "/KySoPhapLy", values);
    }
};
