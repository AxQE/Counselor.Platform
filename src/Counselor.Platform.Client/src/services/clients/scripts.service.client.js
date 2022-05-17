import { httpStatusCodes } from "../../common/constants";
import { GET } from "./base.service.client";

export const getScript = (id) => {
    let requestResult = {};

    const response = GET(`scripts/${id}`);


    const result = await response.text();

    if (response.status === httpStatusCodes.Ok) {
        requestResult.data = result.payload;
    }
    else{
        requestResult.error = result.error;        
        requestResult.statusText = response.statusText;
    }

    return requestResult;
}