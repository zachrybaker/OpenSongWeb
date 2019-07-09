<template>

    <div class="prettySong">
        <song-lyrics-and-chords v-bind="{song:song,transpose:transpose}"></song-lyrics-and-chords>
        
        <ul class="nav song-footer-details">
            <li v-if="song.ccli" class="ccli">CCLI#: {{ song.ccli }} &nbsp;</li>
            <li v-if="song.copyright" class="copyright"> &copy; {{ song.copyright }} &nbsp;</li>
        </ul>
    </div>

</template>

<script lang="ts">
    import Vue from 'vue';
import { Component } from 'vue-property-decorator';

import songModule from "@/common/store/modules/songModule"
import { SongModel } from '@/common/models/songModels';

@Component({
    components: {
        SongLyricsAndChords: require('./SongLyricsAndChords.vue').default
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
</script>
