import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import appState, { AuthState } from '../../store/modules/appModule'

@Component
export default class NavMenu extends Vue {
    get authenticationState(): AuthState {
        return appState.authenticationState;
    }
}