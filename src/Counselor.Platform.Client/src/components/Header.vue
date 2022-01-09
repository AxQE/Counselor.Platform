<template>
    <header>
        <div class="content-container">
            <router-link to='/'>Counselor.Platform</router-link>
            <div class="auth-container" v-if="(!isLoggedIn)">
                <router-link to='/login'>Login</router-link>
                <router-link to='/registration'>Registration</router-link>
            </div>
            <div class="account-container" v-else>
                <AccountShort />
                <div id="logout" v-on:click="logoutUser">
                    <p>Logout</p>
                </div>
            </div>
        </div>
    </header>
</template>

<script>
import { routePaths } from '../common/constants';
import AccountShort from '../components/AccountShort.vue';
import { logout } from '../services/auth.service';
import { Getters } from '../store';

export default {
    name: 'Header',
    components: {
        AccountShort
    },
    computed: {
        isLoggedIn() {
            return this.$store.getters[Getters.Auth.IsAuthenticated];
        }
    },
    methods: {
        logoutUser() {
            logout();
            this.$router.replace({ path: routePaths.login });
        }
    }
}
</script>


<style lang="scss" scoped>

.content-container div {
    display: inline-block;
}

header {
    background-color: aqua;
    height: 40px;
    width: 100%;
}

#logout {
    width: 100px;
    height: 40px;
}

#logout:hover {
    background-color: brown;
    cursor: pointer;
}

.auth-container {

}

.account-container {

}

</style>