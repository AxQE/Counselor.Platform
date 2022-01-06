<template>
    <div class="registration-page">        
        <form class="registration-form" v-if="(!userAuthernticated)">
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
                class="login-field username"
                placeholder="email"
            />
            <Input
                v-model="password"
                type="text"                
                class="login-field password"
                placeholder="password"
            />
            <Button class="registration-button" :click="sendLoginRequest">
                Registrate
            </Button>
        </form>
    </div>
</template>