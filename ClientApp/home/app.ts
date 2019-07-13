import Vue from 'vue';

// TODO: Optimize. See https://getbootstrap.com/docs/4.0/getting-started/webpack/ for discussion of just importing what is needed.  But also look at bootstrap-vue's optimization options
import BootstrapVue from 'bootstrap-vue'

import store from '@/common/store/store';
import router from './router';

Vue.use(BootstrapVue);

const app = new Vue({
    el: '#app-root',
    router: router,
    render: h => h(require('../common/components/app.vue').default),
    store
});

export default app