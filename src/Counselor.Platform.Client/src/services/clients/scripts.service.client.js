import { httpStatusCodes } from "../../common/constants";
import { GET, POST } from "./base.service.client";

export const getAllScripts = () => {
    return GET('scripts');
}

export const getScript = (id) => {
    return GET(`scripts/${id}`);
}

export const createScript = (script) => {
    return POST('scripts', script, null, httpStatusCodes.Created);
}