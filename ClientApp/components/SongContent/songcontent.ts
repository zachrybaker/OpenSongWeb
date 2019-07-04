import Vue from 'vue';
import { Component } from 'vue-property-decorator';

import songModule from '../../store/modules/songModule'
import { SongModel } from '../../models/songModels';

@Component({
    components: {
        SongLyricsAndChords: require('../SongLyricsAndChords/SongLyricsAndChords.vue.html')
    },
    props: {
        song: {
            type: SongModel.Song,
            default: null
        },
        transpose: Number
    }
})
export default class SongContent extends Vue {
    
}