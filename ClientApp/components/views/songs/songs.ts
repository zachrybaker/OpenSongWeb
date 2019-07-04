import Vue from 'vue';
import { Component } from 'vue-property-decorator';

import songModule from '../../../store/modules/songModule'
import { SongModel } from "../../../models/songModels"
import Filter from "../../../scripts/Filters";
import { Action } from 'vuex-module-decorators';

@Component ({
    components: {
        SongResultsTable: require("../../SongResultsTable/SongResultsTable.vue.html"),
        SongResultsHeader: require("../../SongResultsHeader/SongResultsHeader.vue.html")
    },
    filters: {
        pluralize: Filter.pluralize
    }
})
export default class SongsComponent extends Vue {
    // To be set based on route params.
    searchParameters?: (SongModel.SongSearchParameters | null) = null;

    // the value bound to the input.  Driving this local rather than by store...
    filterInput: string = '';//songModule.clientFilter;


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
        this.searchParameters = new SongModel.SongSearchParameters();

        if (this.$route.name == 'SongsTaggedWith' && this.$route.query.tag) {
            this.searchParameters.type = SongModel.SongSearchType.Tags;
            this.searchParameters.text = 'borked';//this.$route.query.tag.length ? 
              //  this.$route.query   .tag[0] : '';
        }

        // TODO: paged input
        // TODO: search results from the nav bar.

        songModule.setClientFilter(this.filterInput);
        songModule.searchSongs(this.searchParameters);
    }

    filterAtClient(): void{
        songModule.clientFilterChanged(this.filterInput);
    }
}
