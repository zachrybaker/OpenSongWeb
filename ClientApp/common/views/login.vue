<template>
    <div class="login">
        <h3>Sign In</h3>
        <input type="text" v-model="email" placeholder="Email"/><br/>
        <input type="password" v-model="password" placeholder="Password" />
        <button @click="login">submit</button>
    </div>
</template>

<script lang="ts">
    import Vue from "vue";

    import { Component, Prop, Model } from "vue-property-decorator";
    import userModule from "@/common/store/modules/userModule";
    import { ErrorModels } from "@/common/models/commonModels";
    import { UserModels } from "@/common/models/userModels";

    @Component({
        name: "login"
    })
    export default class Login extends Vue {
        // TODO: Make this a dressed-up view, possibly even a modal.
        email: string = "zachgotnosauce@gmail.com";
        password: string = "Gopher@1";
        async login(): Promise<void>
        {
            const emailPassword: UserModels.EmailPassword = {
                email: this.email,
                password: this.password
            };
            let err: (ErrorModels.AuthError | null) =
                await userModule.loginByEmailPassword(emailPassword);

            if (err) {
                if (err.code === "auth/wrong-password") {
                    alert("Wrong password.");
                } else {
                    alert("Login failed:\r\n" + err.message);
                }
            }
            else {
                // TODO: redirecting with router's previous URL.
                this.$router.push("/");

            }
        }
    }
</script>
<style scoped>
</style>
