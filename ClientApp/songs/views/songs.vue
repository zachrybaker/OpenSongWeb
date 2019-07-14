<template>
    <div>
        <song-results-header v-bind="{songs:songs, searchParameters:searchParameters}"></song-results-header>
        
        <form v-if="songs && songs.length" class="table-filter float-lg-right" v-on:submit.prevent>
            <label for="table-filter"><span v-if="clientFilter.length">
                        Filtered to {{ filteredSongs.length }} matching {{ filteredSongs.length | pluralize('song','songs') }}
                    </span>
                    <span v-else>Filter these for:</span></label>
            <input type="search" placeholder="Filter..." v-on:keyup="filterAtClient" v-on:input="filterAtClient" v-model="filterInput" /> 
            
        </form>
        <song-results-table v-bind="{songs:filteredSongs}"></song-results-table>
    </div>
</template>

<script lang="ts">
    import Vue from "vue";
    import { Component, Watch } from 'vue-property-decorator';

    import songModule from '@/common/store/modules/songModule';
    import { SongModel } from "@/common/models/songModels";
    import Filter from "@/common/helpers/ui/Filters";

    import { Action } from 'vuex-module-decorators';
    import VueRouter, { NavigationGuard, Route } from 'vue-router';

    @Component ({
        components: {
            SongResultsTable: require("../components/SongResultsTable.vue").default,
            SongResultsHeader: require("../components/SongResultsHeader.vue").default
        },
        filters: {
            pluralize: Filter.pluralize
        },
        watch: {
            // Allows us to reuse the view for several purposes.
            '$route': 'go' 
        }
    })
    export default class SongsView extends Vue {
        // To be set based on route params.
        searchParameters?: (SongModel.SongSearchParameters | null) = null;

        // the value bound to the input.  Driving this local rather than by store...
        filterInput: string = '';

        get songs(): (SongModel.SongBrief[] | null) {
            return songModule.songResults;
        }
        get filteredSongs(): (SongModel.SongBrief[] | null) {
            return songModule.filteredSongResults;
        }
        get clientFilter(): string {
            return songModule.clientFilter;
        }
    
        mounted() {
            this.go();
        }

        private go() {
            this.filterInput = '';
            this.searchParameters = new SongModel.SongSearchParameters();

            if (this.$route.name == 'songs-tagged-with' && this.$route.params.tag) {
                this.searchParameters.type = SongModel.SongSearchType.Tags;
                this.searchParameters.text = this.$route.params.tag;
            }
            else if (this.$route.name == 'songs-search' && this.$route.params.text) {
                this.searchParameters.text = this.$route.params.text;
            }

            console.log('bootstrapping songs view', this.searchParameters);
            // TODO: paged input?
            // TODO: search results from the nav bar.

            songModule.setClientFilter(this.filterInput);
            songModule.searchSongs(this.searchParameters);

        }

        filterAtClient(): void{
            songModule.clientFilterChanged(this.filterInput);
        }
    }
</script>

