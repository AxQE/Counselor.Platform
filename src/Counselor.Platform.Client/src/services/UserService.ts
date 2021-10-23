import { ApiConfig } from '../Config';
import { authHeader } from './AuthHelper';
import { serviceBase } from './ServiceBase';
import Logger from  './Logger';

export const userService = {
    login,
    logout,
    getCurrent,
    createUser
};

function login(username : string, password : string) {

    const user = serviceBase.POST(`${ApiConfig.BaseUrl}/users/authenticate`, JSON.stringify({ username, password }), { 'Content-Type': 'application/json' });

    if (user) {
        // store user details and basic auth credentials in local storage 
        // to keep user logged in between page refreshes
        user.authdata = window.btoa(username + ':' + password);
        localStorage.setItem('user', JSON.stringify(user));
    }

    return user;
}

function logout() {
    // remove user from local storage to log user out
    localStorage.removeItem('user');
}

function getCurrent() {
    return serviceBase.GET(`${ApiConfig.BaseUrl}/users/current`);    
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