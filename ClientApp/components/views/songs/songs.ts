import Vue from 'vue';
import { Component, Watch } from 'vue-property-decorator';

import songModule from '../../../store/modules/songModule'
import { SongModel } from "../../../models/songModels"
import Filter from "../../../scripts/Filters";
import { Action } from 'vuex-module-decorators';
import VueRouter, { NavigationGuard, Route } from 'vue-router';

@Component ({
    components: {
        SongResultsTable: require("../../SongResultsTable/SongResultsTable.vue.html"),
        SongResultsHeader: require("../../SongResultsHeader/SongResultsHeader.vue.html")
    },
    filters: {
        pluralize: Filter.pluralize
    },
    watch: {
        // Allows us to reuse the view for several purposes.
        '$route': 'go' 
    }
})
export default class SongsComponent extends Vue {
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
        // TODO: paged input
        // TODO: search results from the nav bar.

        songModule.setClientFilter(this.filterInput);
        songModule.searchSongs(this.searchParameters);

    }

    filterAtClient(): void{
        songModule.clientFilterChanged(this.filterInput);
    }
}
