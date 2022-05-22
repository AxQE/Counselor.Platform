import { httpStatusCodes } from "../../common/constants";
import { GET, POST } from "./base.service.client";

export const getAllBots = async () => {
    return GET('bots');
}

export const createBot = async (bot) => {
    return POST('bots', bot, null, httpStatusCodes.Created);
}