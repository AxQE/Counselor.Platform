import { httpStatusCodes } from "../../common/constants";
import { GET } from "./base.service.client"

export const getAllBots = async () => {
    let requestResult = {};

    const response = await GET('bots');

    if (response.status === httpStatusCodes.Ok) {
        requestResult.data = JSON.parse(await response.text());
    }
    else {
        requestResult.error = response.statusText;
    }

    return requestResult;
}