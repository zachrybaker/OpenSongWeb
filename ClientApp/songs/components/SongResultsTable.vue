<template>
    <div v-if="songs" class="table-responsive">

        <table id="songs-results-list" class="table table-sm table-striped table-condensed table-hover">
            <!-- uncomment after hiding via CSS when screen readers are not in play? <caption>List of songs</caption>-->
            <thead>
                <tr>
                    <th scope="col">
                        Title &amp;<br />Author
                    </th>
                    <th scope="col">
                        Key
                    </th>
                    <th scope="col">
                        Capo
                    </th>
                    <th scope="col" class="hidden">
                        Hymn #
                    </th>
                    <th scope="col">
                        Themes
                    </th>
                    <th scope="col">
                        Created
                    </th>
                    <th scope="col">
                        Updated
                    </th>
                    <th scope="col"></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="song in songs" :key="song.id" :song="song" class="">
                    <td>
                        <router-link :to="'/songs/view/' +  song.id + '/' + song.title" :exact="true" class="text-primary">{{ song.title }}</router-link><br />
                        <em class="text-muted">{{ song.author }}</em>
                    </td>
                    <td>
                        {{ song.key }}
                    </td>
                    <td>
                        {{ song.capo }}
                    </td>
                    <td class="hidden">
                        {{ song.hymnNumber }}
                    </td>
                    <td>
                        <template v-for="theme in song.themes">
                            <router-link :to="'/songs/tagged-with/' + encodeURIComponent(theme) + '/'" class="badge badge-secondary">{{ theme }}</router-link>
                        </template>
                    </td>
                    <td>
                        {{ song.createdDateUTC | shortDate }}
                    </td>
                    <td>
                        {{ song.lastUpdatedDateUTC | shortDate }}
                    </td>
                    <td>
                        <router-link :to="'/song/edit/' +  song.id + '/' + song.title" :exact="true" class="text-primary">Edit</router-link> |
                        <router-link :to="'/song/delete/' +  song.id + '/' + song.title" :exact="true" class="text-primary">Delete</router-link>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script lang="ts">
    import Vue from "vue";
    import {  Component, Prop, Model } from "vue-property-decorator";
    
    import { SongModel } from "@/common/models/songModels";
    import Filter  from "@/common/helpers/ui/Filters";

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

