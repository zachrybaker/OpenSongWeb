import Vue from 'vue';
import {  Component, Prop, Model } from 'vue-property-decorator';

import songModule from '../../store/modules/songModule'
import { SongModel } from '../../models/songModels';
import Filter  from "../../scripts/Filters";

@Component({
    components: {},
    filters: {
        pluralize: Filter.pluralize
    }
})
export default class SongResultsHeader extends Vue {
    @Prop({ /* the crazy way to define an array prop - the @Component.props can't  handle arrays. */
        default: () => {
            return []
        }
    }) private songs!: SongModel.SongBrief[];
    @Prop({ default: null })
    private searchParameters!: SongModel.SongSearchParameters;

    get isAll() {
        return this.isType(SongModel.SongSearchType.All);
    }
    get isTags() {
        return this.isType(SongModel.SongSearchType.Tags);
    }
    isType(type: SongModel.SongSearchType) : boolean {
        return this.searchParameters && this.searchParameters.type == type;
    }
    get tag() {
        return this.$route.params.tag;
    }
}