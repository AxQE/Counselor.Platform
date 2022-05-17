import { httpStatusCodes } from "../../common/constants";
import { GET } from "./base.service.client";

export const getAllBots = async () => {
    const response = await GET('bots');

    return response;
}