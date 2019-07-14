<template>

    <nav class="navbar navbar-expand-lg navbar-dark bg-dark main-nav">
        <div class="container">
            <a href="/" class="navbar-brand  mb-0 h1">OpenSongWeb</a>

            <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" 
                    aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>

            <div class="collapse navbar-collapse" id="navbarSupportedContent">
                <ul class="navbar-nav mr-auto">
                    <li class="nav-item">
                        <router-link to="/" :exact="true" class="nav-link" active-class="active">
                            <span class="glyphicon glyphicon-music"></span> Songs
                        </router-link>
                    </li>
                </ul>

                <nav-search></nav-search>

                <span v-if="authenticationState.isAuthenticated" class="navbar-text mr-2 ml-2">
                    Welcome {{ profile ? profile.displayName : "??"}}
                </span>
                <span v-else>
                    <router-link to="/login" :exact="true" class="nav-link" active-class="active">
                        <span class="glyphicon glyphicon-login"></span> Login
                    </router-link>
                </span>
                <b-form inline v-if="authenticationState.isAuthenticated" class="ml-2 my-2 my-lg-0" @submit.prevent="logout">
                    <button class="btn btn-secondary btn-sm">Logout</button>
                </b-form>
                <!-- make login a modal? <form v-else class="form-inline my-2 my-lg-0 d-none" id="login-controls">

                <button v-b-modal.prevent.loginModal class="btn btn-secondary btn-sm" type="submit">Login</button>
                </form>-->
            </div>
        </div>
    </nav>
</template>

<style>
.main-nav .form-inline {
    display: inline-block;  
    padding-top: 10px;
    padding-right: 10px;
}
</style>

<script lang="ts">
    import Vue from "vue";
    import { Component } from "vue-property-decorator";
    import { UserModels } from "@/common/models/userModels";

    import appState from "@/common/store/modules/appModule";
    import userModule from "@/common/store/modules/userModule";
    
    @Component({
        components: {
            NavSearch: require("./NavSearch.vue").default
        }
    })
    export default class NavMenu extends Vue {

        get authenticationState(): UserModels.AuthState {
            console.log("auth state is ", appState.authenticationState);
            return appState.authenticationState;
        }
        get profile(): (UserModels.IUserProfile | null) {
            return userModule.profile;
        }

        logout($event : any): void {
            $event.preventDefault();
            console.log("sigining out");
            userModule.logout();
        }
    }
</script>
