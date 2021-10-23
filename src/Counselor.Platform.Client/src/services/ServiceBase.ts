export const serviceBase = {
    authHeader,
    GET,
    POST,
    PATCH
}

function authHeader() : Headers {
    // return authorization header with basic auth credentials
    const user = JSON.parse(localStorage.getItem('user') || '{}');
    const headers = new Headers();

    if (user && user.authdata)
        headers.append('Authorization', 'Basic ' + user.authdata);

    return headers;
}

function GET(url: string) : any {
    const requestOptions = {
        method: 'GET',
        headers: authHeader()
    };

    return fetch(url, requestOptions).then(handleResponse);
}

function POST(url: string, bodyJson: string, header: any = null) : any {
    const requestOptions = {
        method: 'POST',
        headers: header == null? authHeader() : header,
        body: bodyJson
    };

    return fetch(url, requestOptions).then(handleResponse);
}

function PATCH(url: string, bodyJson: string) : any {
    const requestOptions = {
        method: 'PATCH',
        headers: authHeader(),
        body: bodyJson
    };

    return fetch(url, requestOptions).then(handleResponse);
}

function logout() {
    // remove user from local storage to log user out
    localStorage.removeItem('user');
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