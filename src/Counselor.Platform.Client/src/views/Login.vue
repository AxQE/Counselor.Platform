<template>
    <div class="login-page content-container">        
        <form class="login-form">
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
import Input from '../components/Input.vue';
import Button from '../components/Button.vue';
import { login } from '../services/auth.service';
import { routePaths } from '../common/constants';

export default {
    name: 'Login',
    components: {
        Input,
        Button
    },    
    data() {
        return {
            username: '',
            password: ''
        }
    }, 
    computed: {
    },
    methods: {
        async sendLoginRequest() {

            if (this.validateInput())
            {
                const result = await login(this.username, this.password);

                if (result.success) {
                    this.$router.replace({ path: routePaths.home.path });
                }
                else {
                    console.log(result.error);
                }
            }
        },
        validateInput() {
            return !!this.username && !!this.password;
        }
    }
}
</script>

<style lang="scss">
//форма логина и регистрации  https://codepen.io/Rh2o/pen/yLgxJoG
@import '@/styles/colours';
@import '@/styles/fonts';

.login-page {    
    margin-left: auto;
    margin-right: auto;
    max-width: 500px;
}

.login-page input, .login-page button {
    margin-top: 10px;
    width: 250px;
    height: 30px;
}

</style>