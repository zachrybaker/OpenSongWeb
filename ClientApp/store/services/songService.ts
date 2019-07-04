import axios, { AxiosInstance } from 'axios';
import jQuery from 'jquery';

import { SongModel } from "../../models/songModels"
import serverAPI from '../api';

export default class songService {
    protected readonly appApi: AxiosInstance;

    constructor(appApi: AxiosInstance) {
        this.appApi = appApi;
    }

    public init(): void { }
   
    async getSong(id: string) : Promise<SongModel.Song>
    {
        const resp = await this.appApi.get(`Songs/Song/${id}`);
        return resp.data as SongModel.Song;
    }

    async searchSongs(searchParameters?: SongModel.SongSearchParameters): Promise<SongModel.SongBrief[]>
    {
        const resp = await this.appApi.get('Songs/All' + (searchParameters ? '?' + jQuery.param(searchParameters) : ''));
        return resp.data as SongModel.SongBrief[];
    }

}

