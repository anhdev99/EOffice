import { createApp } from 'vue'
import App from './App.vue'
import router from "./router";
import AOS from 'aos'
import 'aos/dist/aos.css'
import i18n from './i18n'
import store from "./state/store";
import VueLoading from 'vue-loading-overlay';
import 'vue-loading-overlay/dist/vue-loading.css';
import BootstrapVue3 from 'bootstrap-vue-3'
//  import BootstrapVue from 'bootstrap-vue'
import vClickOutside from "click-outside-vue3"
import VueApexCharts from "vue3-apexcharts";
import Maska from 'maska';
import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue-3/dist/bootstrap-vue-3.css'
import VueFeather from 'vue-feather';
import Particles from "particles.vue3";

import '@/assets/scss/config/default/app.scss';
import '@vueform/slider/themes/default.css';

import { initFirebaseBackend } from './authUtils'
import { configureFakeBackend } from './helpers/fake-backend';
import VueBlocksTree from 'vue3-blocks-tree';
import 'vue3-blocks-tree/dist/vue3-blocks-tree.css';

//Toast
import Toaster from '@meforma/vue-toaster';

const firebaseConfig = {
    apiKey: process.env.VUE_APP_APIKEY,
    authDomain: process.env.VUE_APP_AUTHDOMAIN,
    databaseURL: process.env.VUE_APP_VUE_APP_DATABASEURL,
    projectId: process.env.VUE_APP_PROJECTId,
    storageBucket: process.env.VUE_APP_STORAGEBUCKET,
    messagingSenderId: process.env.VUE_APP_MESSAGINGSENDERID,
    appId: process.env.VUE_APP_APPId,
    measurementId: process.env.VUE_APP_MEASUREMENTID
};

if (process.env.VUE_APP_DEFAULT_AUTH === "firebase") {
    initFirebaseBackend(firebaseConfig);
} else {
    configureFakeBackend();
}
let defaultoptions = {treeName:'blocks-tree'}

AOS.init({
    easing: 'ease-out-back',
    duration: 1000
})

createApp(App)
    .use(store)
    .use(router)
    .use(VueApexCharts)
     // .use(BootstrapVue)
    .use(BootstrapVue3)
    .component(VueFeather.type, VueFeather)
    .use(Maska)
    .use(Particles)
    .use(VueBlocksTree,defaultoptions)
    .use(i18n)
    .use(Toaster,{
        position: 'top-right',
        duration : 3000,
    })
    .use(VueLoading,{
            canCancel: false,
            color: '#000000',
            loader: 'spinner',
            width: 30,
            height: 30,
            backgroundColor: '#ffffff',
            opacity: 0.7,
            zIndex: 999,
        }, {})
    .use(vClickOutside).mount('#app')

