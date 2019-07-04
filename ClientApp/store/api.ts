import axios, { AxiosInstance } from 'axios';

import appModule from "./modules/appModule"

import songService from "./services/songService"
import authService from "./services/authService"

export const _appApi = axios.create({
    baseURL: 'api/',
});

export async function _fetchT<T>(url: string) {
    const resp = await _appApi.get(url);
    return resp.data as T;
}

export default class StoreApi
{
    protected appApi: AxiosInstance;
    public authService: authService;
    public songService: songService;
    public loadingCount: number = 0;

    constructor() {
        this.appApi = _appApi;

        _appApi.interceptors.request.use(config => {
            console.log('making request to', config.url);
            appModule.setIsLoading(++this.loadingCount > 0);
            return config
        })

        _appApi.interceptors.response.use(response => {
            appModule.setIsLoading(--this.loadingCount > 0);
            return response
        });

        this.authService = new authService(this.appApi);
        this.songService = new songService(this.appApi);
    }


    public init() {
        this.authService.init();
        this.songService.init();
    }
}
export const serverAPI = new StoreApi();
serverAPI.init();