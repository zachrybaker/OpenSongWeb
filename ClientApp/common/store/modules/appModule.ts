import { getModule, Module, VuexModule, Mutation } from 'vuex-module-decorators'
import store from '../store'

export interface AuthState {
    isAuthenticated: boolean
}

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
    authState: AuthState = {
        isAuthenticated: false
    }
    isLoading: boolean = true;


    // technically should be an action...that calls a mutation
    @Mutation setIsLoading(isLoading: boolean) {
        this.isLoading = isLoading;
        console.log('loading', isLoading);
    }
    
    @Mutation
    setAuthentication(authState: AuthState) {
        this.authState = authState;
    }

    get authenticationState(): AuthState {
        return this.authState;
    }
}
export default getModule(AppModule);