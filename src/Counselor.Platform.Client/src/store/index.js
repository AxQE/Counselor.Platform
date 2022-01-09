import { createStore } from 'vuex';


export const Getters = {
    Auth: {
        IsAuthenticated: 'isAuthenticated'
    }
}

export const Mutations = {
    Auth: {
        Success: 'Success',        
        Remove: 'Remove'
    }
};

export const store =  createStore({
    state () {
        return {
            status: '',
            user: {
                isAuthenticated: false
            }
        }
    },    
    getters: {
        isAuthenticated(state) {
            return state.user.isAuthenticated;
        }
    },
    actions: {
    },
    mutations: {
        [Mutations.Auth.Success](state, user) {
            state.user.isAuthenticated = true;
            state.user.id = user.id;
            state.user.username = user.username;
        },        
        [Mutations.Auth.Remove](state) {
            state.user.isAuthenticated = false;
            state.user.id = null;
            state.user.username = null;
        }
    }    
});