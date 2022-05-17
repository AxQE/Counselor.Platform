import { getAuthData, getUserData, removeAuthData, removeUserData, saveAuthData, saveUserData } from "../common/storageHelpers"
import { Getters, Mutations, store } from "../store"
import { authenticate, createUser } from "./clients/users.service.client"

export const login = async (username, password) => {
    const response = await authenticate(username, password);
    let result = {};

    if (response.data !== undefined)
    {
        const user = response.data;
        store.commit(Mutations.Auth.Success, { id: user.id, username: user.username });
        saveAuthData(username, password);
        saveUserData(user);

        result.success = true;
    }
    else
    {        
        result.success = false;
        result.error = response.error.message;
    }

    return result;
}

export const registrate = async (username, email, password) => {
    const response = await createUser(username, email, password);
    let result = {};

    if (response.data !== undefined) 
    {       
        const user = response.data; 
        store.commit(Mutations.Auth.Success, { id: user.id, username: user.username });
        saveAuthData(username, password);
        saveUserData(user);

        result.success = true;
    }
    else
    {
        result.success = false;
        result.error = response.error.message;
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