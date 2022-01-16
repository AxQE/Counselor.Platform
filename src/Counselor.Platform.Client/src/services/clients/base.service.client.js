import { config } from '../../config';
import { getAuthData, removeAuthData, removeUserData } from '../../common/storageHelpers';
import { httpStatusCodes } from '../../common/constants';
import { Mutations, store } from '../../store';

export const GET = async (url) => {
    const params = {
        method: 'GET',
        headers: createHeaders()
    }

    const response = await fetch(formatRequestUrl(url), params);

    hadleResponse(response);

    return response;
}

export const POST = async (url, body, headers) => {
    const params = {
        method: 'POST',
        body: JSON.stringify(body),
        headers: createHeaders(headers)
    }

    const response = await fetch(formatRequestUrl(url), params);

    hadleResponse(response);

    return response;
}

export const PATCH = (url, body) => {

}

function formatRequestUrl(url) {
    return `${config.api.baseUrl}/${url}`;
}

function createHeaders(headers) {
    const authHeader = getAuthData();
    const contentType = 'application/json';

    if (!headers) {
        headers = {};
    }

    headers['Content-Type'] = contentType;

    if (authHeader) {
        headers['Authorization'] = authHeader;
    }   

    return headers;
}

function hadleResponse(response) {
    if (response.status === httpStatusCodes.Unauthorized) {
        store.commit[Mutations.Auth.Remove];
        removeAuthData();
        removeUserData();
    }
}