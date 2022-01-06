<template>
    <div class="login-page">        
        <form class="login-form" v-if="(!isUserAuthernticated)">
            <h2>Login</h2>
            <Input 
                v-model="username"
                type="text"                
                class="login-field username"                
                placeholder="username"
            />            
            <Input
                v-model="password"
                type="password"                
                class="login-field password"                
                placeholder="password"
            />
            <Button class="login-button" :click="sendLoginRequest">
                Login
            </Button>
        </form>        
    </div>
</template>

<script>
export default {
    data() {

    }, 
    computed: {
        isUserAuthernticated() {
            return false;
        }
    },
    methods: {
        sendLoginRequest() {

        }
    }
}
</script>