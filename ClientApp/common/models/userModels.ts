
export module UserModels {

    /// TODO: replace!
    export interface User {
        id: string;
        name: string;

    }

    export interface IUserProfile {
        displayName: string | null;
        email: string | null;
        username: string | null;
        photoURL: string | null;
        uid: string | null;

        /*
      type AdditionalUserInfo = {
        isNewUser: boolean;
        profile: Object | null;
        providerId: string;
        username?: string | null;
      };*/
    }

    export interface AuthState {
        isAuthenticated: boolean,
        isEmailVerified: boolean | null,
        isAppUser: boolean | null
    }

    export interface EmailPassword {
        email: string;
        password: string;
    }
}