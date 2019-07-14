<template>
    <div tabindex="0" aria-label="Loading" class="vld-overlay is-active is-full-page">
        <div class="vld-background" style="background: rgb(255, 255, 255);"></div>
        <div class="vld-icon">
            <div class="spinner-border text-primary" style="width: 5rem; height: 5rem;" role="status">
                <span class="sr-only">Loading...</span>
            </div>
        </div>
    </div>

</template>
<style>
    /* moved styles to the loading page so that it starts with the styles immediately .*/
</style>

<script lang="ts">
    import Vue from "vue";
    import { Component } from "vue-property-decorator";

    @Component
    export default class ModalLoadingIndicator extends Vue {
    }
</script>
