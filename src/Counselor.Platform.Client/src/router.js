import { createWebHistory, createRouter } from 'vue-router';
import { routePaths } from './common/constants';
import { store } from './store';

const routes = [
    {
        path: routePaths.home.path,
        name: routePaths.home.name,
        component: () => import('./views/Home'),
        meta: {
            requeresAuth: true
        }
    },
    {
        path: routePaths.editor.path,
        name: routePaths.editor.name,
        component: () => import('./views/Editor'),
        meta: {
            requeresAuth: true
        }
    },
    {
        path: routePaths.error.path,        
        name: routePaths.error.name,
        component: () => import('./views/Error')
    },
    {
        path: routePaths.login.path,        
        name: routePaths.login.name,
        component: () => import('./views/Login')
    },
    {
        path: routePaths.register.path,        
        name: routePaths.register.name,
        component: () => import('./views/Registration')
    },
    {
        path: '/:catchAll(.*)',
        component: () => import('./views/Error')
    }
]

const router = createRouter({
    history: createWebHistory(),
    routes: routes
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