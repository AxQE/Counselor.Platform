import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import { setAuthStatus } from './services/auth.service';
import { store } from './store'

createApp(App)
    .use(router)
    .use(store)
    .mount('#app');

setAuthStatus();