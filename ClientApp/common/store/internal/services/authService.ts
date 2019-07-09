import axios, { AxiosInstance } from 'axios';

export default class authService {
    protected readonly appApi: AxiosInstance;

    constructor(appApi: AxiosInstance) {
        this.appApi = appApi;
    }

    boot(): void {
        console.log('auth service init.');
    }

    setJWT(jwt: string): void {
        this.appApi.defaults.headers.common['Authorization'] = `Token ${jwt}`;
    }

    clearJWT(): void {
        delete this.appApi.defaults.headers.common['Authorization'];
    }

    /* export async function loginUser(user: UserSubmit): Promise<User> {
        const response = await appApi.post('/users/login', {
            user,
        });
        return (response.data as UserResponse).user;
    }
    export async function fetchProfile(username: string): Promise<Profile> {
      const response = await appApi.get(`/profiles/${username}`);
      return (response.data as ProfileResponse).profile;
    }

    export async function fetchUser(): Promise<User> {
      const response = await appApi.get('/user')
      return (response.data as UserResponse).user
    }

    export async function updateUser(user: UserForUpdate): Promise<User> {
      const response = await appApi.put('/user', user)
      return (response.data as UserResponse).user
    }
    */

}

