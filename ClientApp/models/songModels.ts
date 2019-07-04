import { UserModel } from './userModels';

export module SongModel {
    export enum EmbedLinkType {
        YouTube,
        Vimeo
    }

    export enum key {
        G = 0,
        Ab,
        A, // 2
        Bb,
        B, // 4
        C,
        Db, // 6
        D,
        Eb, // 8
        E,
        F, // 10
        Gb,
        Unknown = 12
    } 

    export enum SongFormat {
        OpenSong
    }

    /// the minimum items needed to expose this on a set list or song results view.
    export interface SongBrief {
        format: SongFormat;
        id: number;
        title: string;
        author: string;
        key?: string;
        presentation?: string;
        capo?: number;
        hymnNumber?: string;
        themes?: string[];

        // TBD: string vs number of UTC epoch seconds vs date type.
        createdDateUTC?: Date;
        lastUpdatedDateUTC?: Date;

        CreatedBy?: UserModel.User;
    }

    /* Represents a fully-fleshed out song as it comes from the server. */
    export interface SongDetail extends SongBrief {
        // adds the following:
        copyright?: string;
        content?: string;
        ccliNumber?: string;
        tempo?: string;
        videoLinkType?: EmbedLinkType;
        videoEmbedId?: string;
    }

    /* Represents a song that has been adjusted in some way 
     * and how to get back to unmodified
     */
    export interface ModifiedSong extends SongDetail {
        originalContent?: string;
        originalCapo?: number;
        originalKey?: string;
        transposed: number;
    }

    export class Song implements ModifiedSong {
        
        // from Brief
        ////////////////////////
        format: SongFormat = SongFormat.OpenSong;
        id: number = 0;
        title: string = '';
        author: string = '';
        key?: string;
        presentation?: string;
        capo?: number;
        hymnNumber?: string; 
        themes?: string[];

        createdDateUTC?: Date;
        lastUpdatedDateUTC?: Date;

        CreatedBy?: UserModel.User;

        // From Detail
        ////////////////////////
        copyright?: string;
        content?: string;
        ccliNumber?: string;
        tempo?: string;
        videoLinkType?: EmbedLinkType;
        videoEmbedId?: string;

        // From Modified
        ////////////////////////
        originalContent?: string;
        originalCapo?: number;
        originalKey?: string;
        transposed: number = 0;


        reset() {
            this.content = this.originalContent;
            this.transposed = 0;
            this.key = this.originalKey;
            this.capo = this.originalCapo;
        }

        transpose(amount: number /* can be -12 to 12*/) {

            // if this is our first time transposing we need to now remember the translation.
            if (this.transposed === 0) {
                if (this.content && !this.originalContent) {
                    this.originalContent = this.content;
                }
                if (this.capo !== null && !this.originalCapo) {
                    this.originalCapo = this.capo;
                }
                if (this.originalKey && !this.key) {
                    this.originalKey = this.key;
                }
            }
        }

        // TODO: move to utilities
        static stanza_makeLongVersion(line: string) {
            if (line.length < 6 && line.startsWith("[")) {
                let heading = '';
            
                switch (line.substr(1, 1).toUpperCase()) {
                    case "V":
                        heading = line.replace("V", "Verse ");
                        break;
                    case "C":
                        heading = line.replace("C", "Chorus ");
                        break;
                    case "P":
                        heading = line.replace("P", "PreChorus ");
                        break;
                    case "B":
                        heading = line.replace("B", "Bridge ");
                        break;
                    case "T":
                        heading = line.replace("T", "Tag ");
                        break;
                    default:
                        heading = line;
                        break;
                }

                return heading.replace("[", "").replace("]", "");
            }

            return line;
        }
    }


    export enum SongSearchType {
        All,
        Title,
        Author,
        Lyrics, // not searchable at the client.
        Tags
    }

    
    export class SongSearchParameters {
        text: string = '';
        type: SongSearchType = SongSearchType.All;

        // not yet handled...
        paged: boolean = false;
        page?: number;
        pageSize?: number;
        pageCount?: number;

        get typeName(): string 
        {
            return SongSearchType[this.type];

            //switch (this.type) {
            //    case SongSearchType.Title:
            //        return "title";
            //        break;
            //    case SongSearchType.Author:
            //        return "author";
            //        break;
            //    case SongSearchType.Lyrics:
            //        return "lyrics";
            //        break;
            //    case SongSearchType.All:
            //        default:
            //        return "all";
            //        break;
            //}
        }
    }
}