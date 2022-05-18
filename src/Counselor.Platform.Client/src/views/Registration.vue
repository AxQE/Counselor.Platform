<template>
    <div class="registration-page content-container">        
        <form class="registration-form">
            <h2>Registration</h2>
            <Input
                v-model="username"
                type="text"                
                class="login-field username"
                placeholder="username"
            />
            <Input
                v-model="email"
                type="text"                
                class="login-field email"
                placeholder="email"
            />
            <Input
                v-model="password"
                type="password"                
                class="login-field password"
                placeholder="password"
            />
            <Button class="registration-button" :click="sendRegistrationRequest">
                Registrate
            </Button>
        </form>
    </div>
</template>

<script>
import Input from '../components/Input.vue';
import Button from '../components/Button.vue';
import { registrate } from '../services/auth.service';
import { routePaths } from '../common/constants';

export default {
    name: 'Registration',
    components: {
        Input,
        Button
    },
    data() {
        return {
            username: '',
            email: '',
            password: ''
        }
    },
    methods: {
        async sendRegistrationRequest() {

            if (this.validateInput()) {
                const result = await registrate(this.username, this.email, this.password);
                if (result.success) {
                    this.$router.replace({ path: routePaths.home.path });
                }
                else {
                    console.log(result.error);
                }
            }            
        },
        validateInput() {
            return true;
        }
    }
}
</script>

<style lang="scss">

.registration-page {
    margin-left: auto;
    margin-right: auto;
    max-width: 500px;
}

.registration-page input, .registration-page button {
    margin-top: 10px;
    width: 250px;
    height: 30px;
}

</style>