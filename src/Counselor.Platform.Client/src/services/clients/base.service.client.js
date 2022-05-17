import { config } from '../../config';
import { getAuthData, removeAuthData, removeUserData } from '../../common/storageHelpers';
import { httpStatusCodes } from '../../common/constants';
import { Mutations, store } from '../../store';

export const GET = async (url, headers, successStatusCode = httpStatusCodes.Ok) => {
    const params = {
        method: 'GET',
        headers: createHeaders(headers)
    }
    
    const response = await fetch(formatRequestUrl(url), params);

    cancelAuthIfUnauthorizedResponse(response);

    return handleResponse(response, successStatusCode);
}

export const POST = async (url, body, headers, successStatusCode = httpStatusCodes.Ok) => {
    const params = {
        method: 'POST',
        body: JSON.stringify(body),
        headers: createHeaders(headers)
    }

    const response = await fetch(formatRequestUrl(url), params);

    cancelAuthIfUnauthorizedResponse(response);

    return handleResponse(response, successStatusCode);
}

export const PATCH = (url, body, headers, successStatusCode = httpStatusCodes.Ok) => {

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

function cancelAuthIfUnauthorizedResponse(response) {
    if (response.status === httpStatusCodes.Unauthorized) {
        store.commit[Mutations.Auth.Remove];
        removeAuthData();
        removeUserData();
    }
}

async function handleResponse (response, successStatusCode = httpStatusCodes.Ok) {
    let requestResult = {};
    requestResult.status = response.status;

    const result = JSON.parse(await response.text());

    if (response.status === successStatusCode) {
        requestResult.data = result.payload;
    }
    else {
        requestResult.error = result.error;        
        requestResult.statusText = response.statusText;
    }

    return requestResult;
}