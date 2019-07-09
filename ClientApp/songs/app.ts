import Vue from 'vue';

// TODO: Optimize. See https://getbootstrap.com/docs/4.0/getting-started/webpack/ for discussion of just importing what is needed.  But also look at bootstrap-vue's optimization options
import BootstrapVue from 'bootstrap-vue'
import VueRouter from 'vue-router';
import Vuex from 'vuex';
import store from '@/common/store/store';
import SongModule from '@/common/store/modules/songModule'
Vue.use(VueRouter);
Vue.use(BootstrapVue);

let songsView = require('./components/views/songs.vue').default

const routes = [
    { path: '/',                        component: songsView, name: "songs-default" },
    { path: '/songs/all/',              component: songsView, name: "songs-all" },
    { path: '/songs/tagged-with/:tag/', component: songsView, name: "songs-tagged-with", meta: { type: 'tags' } },
    { path: '/songs/search/:text/',     component: songsView, name: "songs-search",      meta: { type: 'search' } },
    { path: '/songs/view/:id/:title', component: require('./components/views/song.vue').default }
];

const app = new Vue({
    el: '#app-root',
    router: new VueRouter({ mode: 'history', routes: routes }),
    render: h => h(require('../common/components/app.vue').default),
    store
});

export default app