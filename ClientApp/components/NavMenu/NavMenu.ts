import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import appState, { AuthState } from '../../store/modules/appModule'

@Component
export default class NavMenu extends Vue {
    searchInput: string = '';
    searched: boolean = false;
    readonly minSearchLen: number = 3;
    checkReset(): void {
        if (this.searchInput.length == 0) {
            this.resetSearch();
        }
    }
    resetSearch() : void
    {
        this.searchInput = '';
        this.searched = false;
    }

    get authenticationState(): AuthState {
        return appState.authenticationState;
    }

    searchServer($event : any) {
        console.log('searched',$event);
        this.searched = true;
        if (this.searchInput.length >= this.minSearchLen) {
            this.$router.push('/songs/search/' + encodeURIComponent(this.searchInput) + '/', undefined, undefined);
        } else {
            return false;
        }
    }
}