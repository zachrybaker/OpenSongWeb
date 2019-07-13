
export default class ClientAppConfig {
    // Drop hardcoded configuration (that doesn't need to be emitted by the web API)
    firebaseConfig = {
        apiKey: "AIzaSyAAHVkUOgj5IBvMW3ee2kIAxgasEghIx5w",
        authDomain: "opensongweb-auth.firebaseapp.com",
        databaseURL: "https://opensongweb-auth.firebaseio.com",
        projectId: "opensongweb-auth",
        storageBucket: "",
        messagingSenderId: "84624904344",
        appId: "1:84624904344:web:c7dabf3263c5ccc5"
    };

};
export const appConfig = new ClientAppConfig();
