import { getModule, Module, VuexModule, Mutation, Action } from 'vuex-module-decorators'
import store from '../store';
import { UserModels } from '@/common/models/userModels';

export interface Copyright {
    year: number
    by: string
}

@Module({
    namespaced: true,
    dynamic: true, // not sure it needs to be...
    name: 'appState', // naming this module a particular thing
    store: store
})
class AppModule extends VuexModule {
    copyright: Copyright = {
        by: 'Zach Baker',
        year: 2019
    }
    authState: UserModels.AuthState = {
        isAuthenticated: false,
        isEmailVerified: null,
        isAppUser: null
    }
    isLoading: boolean = false;


    // technically should be an action...that calls a mutation
    @Mutation setIsLoading(isLoading: boolean) {
        this.isLoading = isLoading;
        console.log('loading', isLoading);
    }
    
    @Mutation
    _setAuthenticationState(authState: UserModels.AuthState) {
        this.authState = authState;
        console.log('setting authentication state', authState);
    }

    get authenticationState(): UserModels.AuthState {
        return this.authState;
    }

    @Action
    updateAuthenticationState(authState: UserModels.AuthState) {
        this.context.commit('_setAuthenticationState', authState);
    }
}
export default getModule(AppModule);