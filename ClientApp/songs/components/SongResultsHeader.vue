<template>
    <h2>
        <template v-if="songs">
            <template v-if="isAll">
                All {{ songs ? songs.length : "" }}
                {{ (songs ? songs.length : "") | pluralize("Song", "Songs") }}
            
                <template v-if="searchParameters && searchParameters.text && searchParameters.text.length">
                    Matching '<em>{{ searchParameters.text }}</em>'
                </template>
            </template>

            <template v-else-if="isTags">
                {{ songs ? songs.length : "" }}
                {{ (songs ? songs.length : "") | pluralize("Song", "Songs") }} Tagged with '<em>{{ tag }}</em>'
            </template>
            
            <template v-else>
                {{ songs ? songs.length : "" }}
                {{ (songs ? songs.length : "") | pluralize("Song", "Songs") }} [unhandled search scenario in header]
            </template>
        </template>
        <template v-else>
            Please Wait...
        </template>
    </h2>
</template>

<script lang="ts">
    import Vue from "vue";
    import {  Component, Prop, Model } from "vue-property-decorator";

    import { SongModel } from "@/common/models/songModels";
    import Filter  from "@/common/helpers/ui/Filters";

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
</script>
