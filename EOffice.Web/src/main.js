import Vue from 'vue'
import App from './App.vue'
import BootstrapVue from 'bootstrap-vue'
import Vuelidate from 'vuelidate'
import VueMask from 'v-mask'
import * as VueGoogleMaps from 'vue2-google-maps'
import VueSweetalert2 from 'vue-sweetalert2'
import VueApexCharts from 'vue-apexcharts'
import router from './router/index'
import store from '@/state/store'
import vco from "v-click-outside"
// Toast
import Toast from "vue-toastification";
import "vue-toastification/dist/index.css";
import VueLoading from 'vue-loading-overlay';
import 'vue-loading-overlay/dist/vue-loading.css';
import '@riophae/vue-treeselect/dist/vue-treeselect.css'
import VJstree from 'vue-jstree'
import "@/design/index.scss";

import Sparkline from 'vue-sparklines'

import { initFirebaseBackend } from './authUtils'
import { configureFakeBackend } from './helpers/fake-backend';

const firebaseConfig = {
  apiKey: process.env.VUE_APP_apiKey,
  authDomain: process.env.VUE_APP_authDomain,
  databaseURL: process.env.VUE_APP_databaseURL,
  projectId: process.env.VUE_APP_projectId,
  storageBucket: process.env.VUE_APP_storageBucket,
  messagingSenderId: process.env.VUE_APP_messagingSenderId,
  appId: process.env.VUE_APP_appId,
  measurementId: process.env.VUE_APP_measurementId
};

if (process.env.VUE_APP_DEFAULT_AUTH === "firebase") {
  initFirebaseBackend(firebaseConfig);
} else {
  configureFakeBackend();
}


Vue.config.productionTip = false
const options = {
  // You can set your default options here
};

Vue.use(Toast, options);

Vue.use(BootstrapVue);
Vue.use(VJstree)
Vue.use(Vuelidate);
Vue.use(VueMask)
Vue.use(VueSweetalert2)
Vue.use(require('vue-chartist'))
Vue.use(vco)
Vue.use(Sparkline)
Vue.use(VueLoading, {
  canCancel: false,
  color: '#000000',
  loader: 'spinner',
  width: 30,
  height: 30,
  backgroundColor: '#ffffff',
  opacity: 0.7,
  zIndex: 999,
}, {});
Vue.use(VueGoogleMaps, {
  load: {
    key: 'AIzaSyAbvyBxmMbFhrzP9Z8moyYr6dCr-pzjhBE',
    libraries: 'places',
  },
  installComponents: true
})
Vue.component('apexchart', VueApexCharts)

new Vue({
  router,
  store,
  render: h => h(App),
}).$mount('#app')
