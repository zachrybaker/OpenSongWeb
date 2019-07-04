import { SongModel } from "../models/songModels"

export default class SongUtils {

    filterSongBriefs(songs: (SongModel.SongBrief[] | null), clientFilter: string): SongModel.SongBrief[] {
        if (!songs) {
            return [];
        } else {
            if (clientFilter == '') {
                return songs;
            }

            let reg = new RegExp(clientFilter.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&'), "gi")
            let res =
                songs.filter(song => {
                    return song &&
                        (song.title && song.title.search(reg) >= 0) ||
                        (song.author && song.author.search(reg) >= 0) ||
                        (song.themes && song.themes.some(theme => theme.search(reg) >= 0));
                });
            return res;
        }
    }
}

export const songUtils = new SongUtils();