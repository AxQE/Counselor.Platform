import { IAuthData } from '../types/AuthData'

export function authHeader() : Headers {
    // return authorization header with basic auth credentials
    const user = JSON.parse(localStorage.getItem('user') || '{}');
    const headers = new Headers();

    if (user && user.authdata)
        headers.append('Authorization', 'Basic ' + user.authdata);

    return headers;
}

export function userData() : IAuthData {
    const user = JSON.parse(localStorage.getItem('user') || '{}');
    return user;
}