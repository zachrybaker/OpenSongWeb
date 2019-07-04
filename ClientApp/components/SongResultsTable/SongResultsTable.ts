import Vue from 'vue';
import {  Component, Prop, Model } from 'vue-property-decorator';

import songModule from '../../store/modules/songModule'
import { SongModel } from '../../models/songModels';
import Filter  from "../../scripts/Filters";

@Component({
    filters: {
        shortDate(value: any): string {
            return Filter.shortDate(value);
        }
    },
    components: {}
})
export default class SongResultsTable extends Vue {
    @Prop({ /* the crazy way to define an array prop - the @Component.props can't  handle arrays. */
        default: () => {
            return []
        }
    }) private songs!: SongModel.SongBrief[];
    
}