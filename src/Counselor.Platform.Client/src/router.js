import { createWebHistory, createRouter } from 'vue-router';
import { routePaths } from './common/constants';
import { store } from './store';

const routes = [
    {
        path: routePaths.home,
        component: () => import('./views/Home'),
        meta: {
            requeresAuth: true
        }
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
        path: routePaths.register,        
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
    if (to.matched.some(x => x.meta.requeresAuth) && !store.getters['isAuthenticated'])
    {
        next({ path: routePaths.login });
    }
    else if (to.path === routePaths.login || to.path === routePaths.register)
    {
        next();
    }    
    else
    {
        next();
    }
});

export default router;