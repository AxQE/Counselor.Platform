import { ApiConfig } from '../Config'
import Logger from  './Logger'

function authHeader() : Headers {
    // return authorization header with basic auth credentials
    const user = JSON.parse(localStorage.getItem('user') || '{}');
    const headers = new Headers();

    if (user && user.authdata)
        headers.append('Authorization', 'Basic ' + user.authdata);

    return headers;
}

export const userService = {
    login,
    logout,
    getCurrent,
    createUser
};

function login(username : string, password : string) {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ username, password })
    };

    return fetch(`${ApiConfig.BaseUrl}/users/authenticate`, requestOptions)
        .then(handleResponse)
        .then(user => {
            // login successful if there's a user in the response
            if (user) {
                // store user details and basic auth credentials in local storage 
                // to keep user logged in between page refreshes
                user.authdata = window.btoa(username + ':' + password);
                localStorage.setItem('user', JSON.stringify(user));
            }

            return user;
        });
}

function logout() {
    // remove user from local storage to log user out
    localStorage.removeItem('user');
}

function getCurrent() {
    const params: RequestInit = {
        method: 'GET',        
        headers: authHeader()
    };

    return fetch(`${ApiConfig.BaseUrl}/users/current`, params).then(handleResponse);
}

function createUser(username: string, password: string) {
    const params: RequestInit = {
        method: 'POST',
        headers: authHeader(),
        body: JSON.stringify({Username: username, Password: password})
    };

    return fetch(`${ApiConfig.BaseUrl}/users`, params).then(handleResponse);
}

function handleResponse(response: any) {
    return response.text().then((text: string) => {
        const data = text && JSON.parse(text);
        if (!response.ok) {
            if (response.status === 401) {
                // auto logout if 401 response returned from api
                logout();
                window.location.reload(true);
            }

            const error = (data && data.message) || response.statusText;
            return Promise.reject(error);
        }

        return data;
    });
}