<!-- TODO: GET .vue files picked up by both webpack loader and ts compiler the IDE -->
<template>
    <div>
        Search for songs <input :bind="text"/>

        <button @click="performSearch">Search</button>
    </div>
</template>

<script lang="ts">
import Vue from 'vue';
import { Component } from 'vue-property-decorator';

@Component
export default class NavSearchComponent extends Vue {
  // ... options ...
}
</script>

<style>
    input {background-color:red;}
</style>