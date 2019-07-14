/* 
From https://firebase.google.com/docs/web/setup?authuser=0#config-object
Note: The Firebase config object contains unique, but non-secret identifiers for your Firebase project. 
Visit Understand Firebase Projects to learn more about this config object.
*/
export default class ClientAppConfig {
    // Drop hardcoded configuration (that doesn't need to be emitted by the web API)
    firebaseConfig = {
        apiKey: "[my-non-secret-api-key]",
        authDomain: "opensongweb-auth.firebaseapp.com",
        databaseURL: "https://opensongweb-auth.firebaseio.com",
        projectId: "opensongweb-auth",
        storageBucket: "",
        messagingSenderId: "[my non-secret messaging sender id]S",
        appId: "[my non-secret app id]"
    };

};
export const appConfig = new ClientAppConfig();
