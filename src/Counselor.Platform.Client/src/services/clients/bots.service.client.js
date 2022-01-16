import { httpStatusCodes } from "../../common/constants";
import { GET } from "./base.service.client"

export const getAllBots = async () => {
    let requestResult = {};

    const response = await GET('bots');

    if (response.status === httpStatusCodes.Ok) {

        const result = await response.text();
        requestResult.data = JSON.parse(result);
    }
    else {
        requestResult.error = response.statusText;
    }

    return requestResult;
}