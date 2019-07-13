<template>

    <p class="fixedWidthFont text-monospace">
        <template v-for="(line, index) in lines">
            <div v-if="isChordLine(line)" class="chords">{{ makeSubstringLyricsLine(line) | spacify }}</div>
            <template v-else-if="isStanzaLine(line)">
                <br v-if="index > 0" />
                <div class="heading-line">

                    {{ line | longStanza }}
                </div>
            </template>

            <div v-else-if="isCommentLine(line)" class="comments text-secondary">{{ makeSubstringLyricsLine(line) | spacify }}</div>

            <div v-else class="lyrics text-secondary" v-html="$options.filters.spacify(line.substring(1))"></div>

        </template>
    </p>
</template>

<script lang="ts">
    import Vue from 'vue';
import { Component,Prop, Model } from 'vue-property-decorator';

import songModule from "@/common/store/modules/songModule"
import { SongModel } from '@/common/models/songModels';
import Filters from "@/common/helpers/ui/Filters"

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
            type: (SongModel.Song | null),
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
</script>
<style>
    .fixedWidthFont {
        font-family: 'Courier New', Courier, monospace SFMono-Regular, Menlo, Monaco, Consolas, "Liberation Mono", "Courier New", monospace !important;
    }

    .chords {
        color: #336;
        font-weight: bold;
    }

    .lyrics {
        font-weight: normal;
    }

    .heading-line {
        color: #600;
        font-weight: bold;
        text-decoration: underline;
    }

    .comments {
        color: #666;
    }

    @media print {
        .searchTool, .songOptions {
            display: none;
        }

        .author, .comments {
            color: #222;
        }

        #modalTextBar {
            display: none;
        }
    }

    @media not print {
        .heading-line {
            background-color: antiquewhite;
            margin-left: -1rem;
            padding: .5rem;
        }
    }
</style>
