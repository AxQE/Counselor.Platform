import { httpStatusCodes } from '../../common/constants';
import { POST } from './base.service.client';

export const isAuthenticated = () => {
    return false;
}

export const authenticate = async (username, password) => {
    const response = await POST(
        'users/authenticate', 
        {
            Username: username, 
            Password: password 
        });

    return response;
}

export const createUser = async (username, email, password) => {
    const response = await POST(
        'users',
        {
            Username: username,
            Email: email,
            Password: password
        },
        null,
        httpStatusCodes.Created);

    return response;
}