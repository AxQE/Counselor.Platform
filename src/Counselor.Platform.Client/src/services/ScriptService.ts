import { ApiConfig } from '../Config';
import { serviceBase } from './ServiceBase';
import { authHeader } from './AuthHelper';

export const scriptService = {
    getAllScripts
}

function getAllScripts(id : number) {
    return serviceBase.GET(`${ApiConfig.BaseUrl}/scripts/${id}`);    
}
