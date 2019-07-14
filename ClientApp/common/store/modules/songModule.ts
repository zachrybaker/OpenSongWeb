import { getModule, Module, MutationAction, VuexModule, Mutation, Action, } from "vuex-module-decorators";
import store from "../store";
import { serverAPI } from "../internal/api";

import { songUtils } from "../internal/helpers/songUtils";
import { SongModel } from "@/common/models/songModels";

@Module({
    namespaced: true,
    dynamic: true, 
    name: "song",
    store: store
})
class SongModule extends VuexModule {

    song: (SongModel.Song | null) = null; // the song we are currently working with. This is causing init problems.
    songResults: SongModel.SongBrief[] | null = null;
    filteredSongResults: SongModel.SongBrief[] | null = null;
    searchParameters?: SongModel.SongSearchParameters; 
    clientFilter: string = "";

    @Mutation
    setSong(song: SongModel.Song) {
        this.song = song;
    }
    @Mutation
    setSongBriefs(songs: SongModel.SongBrief[]) {
        this.songResults = songs;
    }
    @Mutation
    setSearchParams(searchParameters?: SongModel.SongSearchParameters) {
        this.searchParameters = searchParameters;
    }


    @Mutation
    filterSongs(songs: SongModel.SongBrief[]) {
        this.filteredSongResults = songs;
    }

    @Mutation
    updateSong(song: SongModel.Song) {
        // TODO: persist to server.

        this.song = song;
    }

    @Mutation
    setClientFilter(filter: string) {
        this.clientFilter = filter;
    }

    @Action
    async getSong(id: string) {
        let song = await serverAPI.songService.getSong(id);
        console.log("got the song", song);
        this.context.commit("setSong", song)
    }

    @Action
    async searchSongs(searchParameters?: SongModel.SongSearchParameters) {
        console.log("action searchSongs");
        let songs = await serverAPI.songService.searchSongs(searchParameters);
        console.log("got songs",songs);
        this.context.commit("setSearchParams", searchParameters);
        this.context.commit("setSongBriefs", songs);
        this.context.commit("filterSongs", songUtils.filterSongBriefs(this.songResults, this.clientFilter));
    }

    @Action
    async clientFilterChanged(filter: string) {
        this.context.commit("setClientFilter", filter);
        let fs = songUtils.filterSongBriefs(this.songResults, this.clientFilter);
        console.log(" filtered to ", fs.length, " songs");
        this.context.commit("filterSongs", fs);
    }

    @Action
    transposedKey(transpose: number)
    {
        if (!this.song) {
            return "";
        }

        if (transpose == 0 && this.song) {
            return this.song.key || "";
        }

        // TODO: finish
        return "[transposedKey not implemented]";
    }

}

export default getModule(SongModule);
