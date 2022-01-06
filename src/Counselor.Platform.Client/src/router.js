import { createWebHistory, createRouter } from 'vue-router';
import { routePaths } from './common/defaults';

function isAuthenticated() {
    return true;
}

const routes = [
    {
        path: routePaths.home,
        component: () => import('./views/Home')
    },
    {
        path: routePaths.error,        
        component: () => import('./views/Error')
    },
    {
        path: routePaths.login,        
        component: () => import('./views/Login')
    },
    {
        path: routePaths.registration,        
        component: () => import('./views/Registration')
    },
    {
        path: '/:catchAll(.*)',
        component: () => import('./views/Error')
    }
]

const router = createRouter({
    history: createWebHistory(),
    routes
});

router.beforeEach((to, from, next) => {
    if (to.name !== 'login' && !isAuthenticated()) next({ name: 'login' });
    else next();
});

export default router;