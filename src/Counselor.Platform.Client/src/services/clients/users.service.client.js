import { httpStatusCodes } from '../../common/constants';
import { POST } from './base.service.client';

export const isAuthenticated = () => {
    return false;
}

export const authenticate = async (username, password) => {
    let requestResult = {};

    const response = await POST(
        'users/authenticate', 
        {
            Username: username, 
            Password: password 
        });

    if (response.status === httpStatusCodes.Ok) {
        requestResult.data = JSON.parse(await response.text());         
    }
    else{
        requestResult.error = response.statusText;
    }

    return requestResult;
}

export const createUser = async (username, email, password) => {
    let requestResult = {};

    const response = await POST(
        'users',
        {
            Username: username,
            Email: email,
            Password: password
        });

    if (response.status === httpStatusCodes.Created) {
        requestResult.data = JSON.parse(await response.text());
    }
    else {
        requestResult.error = response.statusText;
    }

    return requestResult;
}