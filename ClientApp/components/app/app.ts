import Vue from 'vue';
import { Component, Watch } from 'vue-property-decorator';
import appState, { Copyright } from '../../store/modules/appModule';
import VueRouter, { NavigationGuard, Route } from 'vue-router';

@Component({
    components: {
        NavMenuComponent: require('../NavMenu/NavMenu.vue.html'),
        ModalLoadingIndicator: require('../ModalLoadingIndicator/ModalLoadingIndicator.vue.html')
    }
})
export default class AppComponent extends Vue {
    isNavigating: boolean = false;
    get copyright(): Copyright {
        return appState.copyright;
    }
    get isLoading(): boolean {
        return appState.isLoading;
    }  
    mounted() {

        // set up the router, globally, to modify the isNavigating property
        this.$router.afterEach((any) => {
            this.isNavigating = false;
        });
        this.$router.beforeEach((to: Route, from: Route, next: any) => {
            this.isNavigating = true;
            next();
           
        });
    }

}
