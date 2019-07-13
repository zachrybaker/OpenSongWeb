<template>
    <b-form inline class=" my-2 my-lg-0" id="nav-search" :validated="searched" v-on:submit.stop.prevent="searchServer" novalidate>
        <label class="sr-only" for="main-search">Search For:</label>

        <b-form-group id="fieldset-1" label-for="main-search">
            <b-form-input id="main-search" name="main-search" 
                          v-model="searchInput" 
                          v-bind:min="minSearchLen" 
                          :minlength="minSearchLen" 
                          :state="searchIsValid" trim 
                          placeholder="Search Songs..." 
                          aria-label="Search" class="mr-sm-2 form-control-sm" type="text" required></b-form-input>
            <b-form-invalid-feedback :state="searchIsValid" tooltip>
                Your search needs at least {{minSearchLen}} characters.
            </b-form-invalid-feedback>
            <b-button class="btn btn-outline-light btn-sm" type="submit" :click="searchServer" id="main-search-button">
                <span class="glyphicon glyphicon-search"></span> Search
            </b-button>

        </b-form-group>
    </b-form>
</template>

<style>
  
</style>

<script lang="ts">
    import Vue from 'vue';
    import BootstrapVue, { BvEvent }  from 'bootstrap-vue';
    import { Component } from 'vue-property-decorator';
    Vue.use(BootstrapVue);


    @Component
    export default class NavSearch extends Vue {
        searchInput: string = '';
        searched: boolean = false;
        readonly minSearchLen: number = 3;

        get searchIsValid(): string | null {
            if (!this.searched) {
                return null;
            }

            return this.searchInput.length >= this.minSearchLen ? '' : 'invalid'
        }

        searchServer(): void {
            this.searched = true;
            if ( this.searchInput.length >= this.minSearchLen) {
                this.$router.push({ name: 'songs-search', params: { text: this.searchInput } });
            }
        }
    }
</script>