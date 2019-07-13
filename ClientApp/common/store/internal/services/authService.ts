import axios, { AxiosInstance } from 'axios';

// TODO: typescript-compliant load of just the portion needed for auth.  This is heavy.
import * as firebase from 'firebase/app';
//import { app as firebase } from 'firebase';
import { auth } from 'firebase';

import { appConfig } from '@/../ClientAppConfig';
import appModule from "../../modules/appModule"
import { ErrorModels } from '@/common/models/commonModels';
import { UserModels } from '../../../models/userModels';

export default class authService {
    protected readonly appApi: AxiosInstance;

    constructor(appApi: AxiosInstance) {
        this.appApi = appApi;
    }

    boot(): void {
        console.log('auth service init.');

        if (!firebase.apps.length) {
            if (appConfig.firebaseConfig) {
                // Initialization of Firebase should happen before vue root.
                firebase.initializeApp(appConfig.firebaseConfig); // do we have to do this?
                firebase.auth().onAuthStateChanged(user => {
                    this._onFirebaseAuthStateChanged(
                        {user: user} as firebase.auth.UserCredential);
                });
            }
            else {
                console.error('firebaseConfig missing');
            }
        }
    }

    _onFirebaseAuthStateChanged(
        //user: firebase.User | null,
        //additionalInfo: firebase.auth.AdditionalUserInfo | null | undefined
        cred: firebase.auth.UserCredential
    ): void {
        if (cred.user) {
            // User is signed in.  Grab the profile info...
            //var displayName = user.displayName;
            //var email = user.email;
            //var emailVerified = user.emailVerified;
            //var photoURL = user.photoURL;
            //var isAnonymous = user.isAnonymous;
            //var uid = user.uid;
            //var providerData = user.providerData;
            console.log('user auth state changed to logged in ', cred.user);
            this._notifyAppOfAuthChangeState({
                isAuthenticated: true,
                isEmailVerified: cred.user.emailVerified,
                isAppUser: cred.additionalUserInfo ? !cred.additionalUserInfo.isNewUser : null
            });

            console.log('credential', cred.credential); // null
            console.log('operationType', cred.operationType); //  "signIn"
            console.log('additionalUserInfo', cred.additionalUserInfo); 

            cred.user.getIdTokenResult(false).then((idTokenResult: auth.IdTokenResult) => {
                this._setJWT(idTokenResult.token);
                console.log('user id token', idTokenResult);
            });

            if (cred.additionalUserInfo && cred.additionalUserInfo.profile) {
                console.log('profile: ', JSON.stringify(cred.additionalUserInfo.profile));
            }
        } else {
            // User is signed out.  Tell server explicitly?
            this._notifyAppOfAuthChangeState({
                isAuthenticated: false,
                isEmailVerified: null,
                isAppUser: null
            });
            this._clearJWT();
        }
    }

    _notifyAppOfAuthChangeState(state: UserModels.AuthState) {
        // call our vuex mutator
        appModule.updateAuthenticationState(state);
    }

    _setJWT(jwt: string): void {
        console.log('changing auth header from ', this.appApi.defaults.headers.common['Authorization'], ' to ', jwt);
        this.appApi.defaults.headers.common['Authorization'] = `Token ${jwt}`;
    }

    _clearJWT(): void {
        delete this.appApi.defaults.headers.common['Authorization'];
    }

    signOut() {
        if (auth().currentUser) {
            console.log('signing out of firebase')
            auth().signOut();
        }
        else {
            console.log('firebase does not show us as logged in');
        }
        this._clearJWT();

        this._notifyAppOfAuthChangeState({
            isAuthenticated: false,
            isEmailVerified: null,
            isAppUser: null
        });
    }

    // TODO: translation of this.
    protected credentials: (auth.UserCredential | null) = null;

    async loginUserByEmailPassword(email: string, password: string): Promise<(ErrorModels.AuthError | null)> {
        var creds = auth().signInWithEmailAndPassword(email, password);

        let err: ErrorModels.AuthError | null = null;
        await creds.then(
            (cred: auth.UserCredential) => {
                console.log('login succeeded!', cred);
                this.credentials = cred;
                this._onFirebaseAuthStateChanged(cred);
            },
            (error: firebase.FirebaseError) => {
                console.log('login failed: ', error);
                err = {
                    code: error.code,
                    message: error.message
                };
            }
        );

        return err;
    }

    // TODO: clean up to match the patterns we land on with email login.
    signUpByEmailPassword(email: string, password: string): Promise<auth.UserCredential> {
        var creds = auth().createUserWithEmailAndPassword(email, password);

        creds.then(
            (creds: auth.UserCredential) => {
                console.log('account creation succeeded!', creds);
                this._onFirebaseAuthStateChanged(creds);
                return creds;
            },
            (error: any) => {
                console.log('account creation failed: ', error);
                var errorCode = error.code;
                var errorMessage = error.message;
                if (errorCode == 'auth/weak-password') {
                    alert('The password is too weak.');
                } else {
                    alert(errorMessage);
                }
                return error;
            }
        );

        return creds;
    }

    sendEmailVerification() {
        let user = auth().currentUser;
        if (user != null) {
            user.sendEmailVerification().then(() => {
                alert('Email Verification Sent!');
            });
        } else {
            this.signOut();
            alert("Please sign in.");
        }
    }

    sendPasswordReset(email : string) {
        auth().sendPasswordResetEmail(email).then(() => {
            alert('Password Reset Email Sent!');
        }).catch(function(error) {
            var errorCode = error.code;
            var errorMessage = error.message;

            if (errorCode == 'auth/invalid-email') {
                alert(errorMessage);
            } else if (errorCode == 'auth/user-not-found') {
                alert(errorMessage);
            } else {
                console.log('unhandled error: ', errorMessage);
            }
        console.log(error);
      });
    }


    /*export async function fetchProfile(username: string): Promise<Profile> {
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

