<template>
    <div id="app-root" class="<!--container-fluid-->">
        <nav-menu-component />

        <div class="container body-content mt-4">
            <main role="main" class="container mt-4">
                <transition>
                    <modal-loading-indicator v-show="isLoading || isNavigating" 
                        :class="{'isLoading' : isLoading, 'isNavigating': isNavigating}"></modal-loading-indicator>
                </transition>
                    <transition>
                        <router-view></router-view>
                    </transition>
            </main>
            <hr />
            <footer>
                <p>&copy; {{ copyright.year }} {{ copyright.by }} </p>
            </footer>

        </div>
    </div>
</template>

<script lang="ts">
    import Vue from "vue";
    import { Component, Watch } from "vue-property-decorator";
    import appState, { ICopyright } from "../store/modules/appModule";
    
    @Component({
        components: {
            NavMenuComponent: require("./NavMenu.vue").default,
            ModalLoadingIndicator: require("./ModalLoadingIndicator.vue").default
        }
    })
    export default class AppComponent extends Vue {
        get isNavigating(): boolean {
            return Vue.prototype.$isNavigating || false;
        }
        get copyright(): ICopyright {
            return appState.copyright;
        }
        get isLoading(): boolean {
            return appState.isLoading;
        }  
    }
</script>

