import Vue from "vue";
import VueRouter, { Route } from "vue-router";

Vue.use(VueRouter);

let songsView = require("./views/songs.vue").default;

const routes = [
    { path: "/",                        component: songsView, name: "songs-default" },
    { path: "/songs/all/",              component: songsView, name: "songs-all" },
    { path: "/songs/tagged-with/:tag/", component: songsView, name: "songs-tagged-with", meta: { type: "tags" } },
    { path: "/songs/search/:text/",     component: songsView, name: "songs-search",      meta: { type: "search" } },
    { path: "/songs/view/:id/:title",   component: require("./views/song.vue").default },

    { path: "/login", redirect: "/home/login" },

    { path: "/home/login", component: require("@/common/views/login.vue").default, name: "login" },
    { path: "/home", component: require("@/home/views/home.vue").default, name: "home", meta: { requiresAuth: true } }

];

const router = new VueRouter({ mode: "history", routes: routes });
router.beforeEach((to: Route, from: Route, next: any) => {
    Vue.prototype.$isNavigating = true;
    const requiresAuth = to.matched.some(record => record.meta.requiresAuth);
    if (requiresAuth) {
        next("/home/login");
    } else {
        next();
    }
});

router.afterEach((any) => {
    Vue.prototype.$isNavigating = false;
});

export default router;
