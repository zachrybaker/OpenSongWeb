<template>
    <div>
        <template v-if="song">
            <h2>
                {{ title }}
                <span class="transposed" v-if="transpose != 0">
                    transposed {{ song.key }} -> {{ song.transposedKey }}
                </span>
            </h2>

            <p class="author text-muted">Author: {{ song.author }}</p>
            <ul class="song-customizations list-unstyled">
                <li class="key list-inline-item text-muted">Key: <strong>{{ song.key }} <!--<< >> mutations coming--></strong></li>
                <li class="capo list-inline-item text-muted">Capo: <strong>{{ song.capo }} <!-- (sung in x coming ) --></strong></li>
                <li class="sequence list-inline-item text-muted">Sequence: <strong>{{ song.sequence }} </strong></li>
            </ul>

            <song-content v-bind="{song: song, transpose: transpose}"></song-content>

        </template>
        <template v-else>
            <h1>Loading Song "{{ title }}"</h1>
        </template>
    </div>
</template>

<script lang="ts">
import Vue from 'vue';
import { Component, Prop, Model } from 'vue-property-decorator';

import { SongModel } from '@/common/models/songModels';
import songModule from '@/common/store/modules/songModule'


@Component ({
    components: {
        SongContent: require('../SongContent.vue').default
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
</script>
<style>
    .transposed {

}
.song-customizations {
    padding-left: 0;
    margin-bottom: 5px 0;
}
.song-customizations > * {
    display: inline-block;
    position: relative;
    list-style: none;
    border-left: 1px solid;
    padding: 0px 10px;
}
.song-customizations > *:first-child {
    border-left: 0px none;
    padding-left: initial;
}

</style>