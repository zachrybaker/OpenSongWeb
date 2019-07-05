<template>
    <form class="form-inline my-2 my-lg-0 needs-validation" v-on:submit.stop.prevent="searchServer" novalidate v-bind:class="{'was-validated': (searched && searchInput.length < minSearchLen)}">

        <span class="invalid-feedback" for="main-search" v-bind:class="{'d-inline': (searched && searchInput.length < minSearchLen)}">
            Please search with at least {{ minSearchLen }} characters.
        </span>
        <input name="main-search" class="col-5 form-control mr-sm-2 form-control-sm" type="text" placeholder="Search Songs..." aria-label="Search" v-model="searchInput" v-on:blur="searched = false" v-on:input="checkReset"
               v-bind:min="minSearchLen" v-bind:minlength="minSearchLen" required v-bind:class="{':invalid': (searchInput.length < minSearchLen)}" />

        <button class="btn btn-outline-light btn-sm" type="submit" :click="searchServer">
            <span class="glyphicon glyphicon-search"></span> Search
        </button>
    </form>
</template>

<script lang="ts">
import Vue from 'vue';
import { Component } from 'vue-property-decorator';

@Component
export default class NavSearch extends Vue {
  
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
</script>

<style>
    input {background-color:red;}
</style>