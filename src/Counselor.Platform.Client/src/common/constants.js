export const routePaths = {
    home: { path: '/', name: 'home' },
    editor: { path: '/bot/:id', name: 'editor' },
    login: { path: '/login', name: 'login' },
    register: { path: '/registration', name: 'registration' },
    error: { path: '/error', name: 'error' }
};
export const logLevel = {
    debug: 0,
    information: 1,
    warning: 2,
    error: 3
};
export const storageKeys = {
    user: {
        auth: 'authData',
        data: 'userData'
    }
};
export const httpStatusCodes = {
    Ok: 200,
    Created: 201,
    
    BadRequest: 400,
    Unauthorized: 401,
    Forbidden: 403,
    NotFound: 404,
    RequestTimeout: 408,
    UnprocessableEntity: 422,

    InternalServerError: 500,
    NotImplemented: 501,
    ServiceUnavailable: 503,
    GatewayTimeout: 504
}
export const botState = {
    Created: 'Created',
	Invalid: 'Invalid',
	Pending: 'Pending',
	Started: 'Started',
	Stopped: 'Stopped',
	Deactivated: 'Deactivated'
}