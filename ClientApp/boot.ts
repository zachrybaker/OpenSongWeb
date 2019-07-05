import './css/site.css';
import 'bootstrap';
import Vue from 'vue';
import VueRouter from 'vue-router';
import Vuex from 'vuex';
import store from './store/store';

Vue.use(VueRouter);

import 'bootstrap/dist/css/bootstrap.css'
let songsView = require('./components/views/songs/songs.vue.html');

const routes = [
    { path: '/',                        component: songsView, name: "songs-default" },
    { path: '/songs/all/',              component: songsView, name: "songs-all" },
    { path: '/songs/tagged-with/:tag/', component: songsView, name: "songs-tagged-with", meta: { type: 'tags' } },
    { path: '/songs/search/:text/',     component: songsView, name: "songs-search",      meta: { type: 'search' } },
    { path: '/songs/view/:id/:title',   component: require('./components/views/song/song.vue.html') }
];

const app = new Vue({
    el: '#app-root',
    router: new VueRouter({ mode: 'history', routes: routes }),
    render: h => h(require('./components/app/app.vue.html')),
    store
});

export default app