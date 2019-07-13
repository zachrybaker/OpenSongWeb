import Vue from 'vue';
import VueRouter, { Route } from 'vue-router';

Vue.use(VueRouter);

const routes = [
    { path: '*', redirect: '/home/login' }, 
    { path: '/login', redirect: '/home/login' },

    { path: '/home/login', component: require("@/common/views/login.vue").default, name: "login" },
    { path: '/home', component: require('@/home/views/home.vue').default, name: 'home', meta: { requiresAuth: true } }
];

const router = new VueRouter({ mode: 'history', routes: routes });
router.beforeEach((to: Route, from: Route, next: any) => {
    Vue.prototype.$isNavigating = true;
    const requiresAuth = to.matched.some(record => record.meta.requiresAuth);
    if (requiresAuth    ) {
        next('/home/login');
    } else {
        next();
    } 
});

router.afterEach((any) => {
    Vue.prototype.$isNavigating = false;
});

export default router;