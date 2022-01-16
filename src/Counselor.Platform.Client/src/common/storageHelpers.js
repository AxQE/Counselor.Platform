import { storageKeys } from "./constants"


export const saveAuthData = (username, password) => {
    localStorage.setItem(storageKeys.user.auth, `Basic ${window.btoa(`${username}:${password}`)}`);
}

export const getAuthData = () => {
    return localStorage.getItem(storageKeys.user.auth);
}

export const removeAuthData = () => {
    localStorage.removeItem(storageKeys.user.auth);
}



export const saveUserData = (user) => {
    localStorage.setItem(storageKeys.user.data, JSON.stringify(user))
}

export const getUserData = () => {
    return JSON.parse(localStorage.getItem(storageKeys.user.data));
}

export const removeUserData = () => {
    localStorage.removeItem(storageKeys.user.data)
}