import Vue from 'vue';
import { Component,Prop, Model } from 'vue-property-decorator';

import songModule from '../../store/modules/songModule'
import { SongModel } from '../../models/songModels';
import Filters from "../../scripts/Filters"

@Component({
    filters: {
        longStanza(value: any): string {
            return SongModel.Song.stanza_makeLongVersion(value ? value.toString().trim() : '');
        },
        spacify(value: any): string {
            return Filters.spacify(value);
        }
    },
    components: {
    },
    props: {
        song: {
            type: SongModel.Song,
            default: null
        },
        transpose: Number
    }
})
export default class SongLyricsAndChords extends Vue {
    
    get lines() {
        return this.$props.song.content.replace('\r\n', '\n').split('\n');
    }

    lineStartsWith(line: string, char: string, minLength: number): boolean {
        return line ? line.startsWith(char) && line.length >= minLength : false;
    }

    isChordLine(line: string) : boolean {
        return this.lineStartsWith(line, '.', 2);
    }

    isStanzaLine(line: string): boolean {
        return this.lineStartsWith(line, '[', 3);
    }

    isCommentLine(line: string): boolean {
        return this.lineStartsWith(line, ';', 2);
    }

    makeSubstringLyricsLine(line: string) {
        return line ? Filters.spacify(line.substring(1)) : "";
    }
}