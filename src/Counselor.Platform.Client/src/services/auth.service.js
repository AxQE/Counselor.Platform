import { getAuthData, getUserData, removeAuthData, removeUserData, saveAuthData, saveUserData } from "../common/storageHelpers"
import { Getters, Mutations, store } from "../store"
import { authenticate, createUser } from "./clients/users.service.client"

export const login = async (username, password) => {
    const user = await authenticate(username, password);
    let result = {};

    if (user.data !== undefined)
    {        
        store.commit(Mutations.Auth.Success, { id: user.id, username: user.username });
        saveAuthData(username, password);
        saveUserData(user.data);

        result.success = true;
    }
    else
    {
        result.success = false;
        result.error = '';
    }

    return result;
}

export const registrate = async (username, email, password) => {
    const user = await createUser(username, email, password);
    let result = {};

    if (user.data !== undefined) 
    {
        store.commit(Mutations.Auth.Success, { id: user.id, username: user.username });
        saveAuthData(username, password);
        saveUserData(user.data);

        result.success = true;
    }
    else
    {
        result.success = false;
        result.error = '';
    }

    return result;
}

export const logout = () => {
    store.commit(Mutations.Auth.Remove);
    removeAuthData();
    removeUserData();
}

export const isAuthenticated = () => {
    return this.$store.getters[Getters.Auth.IsAuthenticated];
}

export const setAuthStatus = () => {
    const authData = getAuthData();
    const userData = getUserData();

    if (authData && userData)
    {
        store.commit(Mutations.Auth.Success, userData);
    }
}