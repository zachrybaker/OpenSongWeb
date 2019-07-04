import Vue from 'vue';
import { Component, Prop, Model } from 'vue-property-decorator';

import { SongModel } from '../../../models/songModels';
import songModule from '../../../store/modules/songModule'


@Component ({
    components: {
        SongContent: require('../../SongContent/SongContent.vue.html')
    },
    props: {}
})
export default class SongComponent extends Vue {

    get song() : (SongModel.Song | null) {
        return songModule.song;
    }

    /* leads to runtime error in the child: 
     * [Vue warn]: Invalid prop: type check failed for prop "song". Expected , got Object.
     * */
    // song: (SongModel.Song | null) = null; 
    // song?: SongModel.Song = new SongModel.Song;(followed by next error...)

    /* leads to run time error on the child
     * [Vue warn]: Property or method "song" is not defined on the instance but referenced during render. Make sure to declare reactive data properties in the data option.
     * */
    // song!: SongModel.Song;
    // song: SongModel.Song | undefined;
    
    /* leads to compile error on this:
     *     TS2564: Property 'song' has no initializer and is not definitely assigned in the constructor.
     * */
    // song: SongModel.Song;

    

    get transpose(): number {
        return parseInt(this.$route.params.transpose, 0) || 0;
    }
    get transposedKey(): string {
        return songModule.transposedKey(this.transpose);
    }
    
    get id(): string {
        return this.$route.params.id;
    }

    get title(): string {
        return this.$route.params.title;
    }

    async mounted() {
        songModule.getSong(this.$route.params.id);
    }

}
