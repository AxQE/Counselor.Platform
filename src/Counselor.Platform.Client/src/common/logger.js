import { config } from "../config";
import { logLevel } from './defaults';

export const Logger = {
    logError: (exception, message) => {
        if (config.logLevel <= logLevel.error)
        {
            console.error(message, exception);
        }
    },

    logWarning: (exception, message) => {
        if (config.logLevel <= logLevel.warning)
        {
            console.log(message);
        }
    },

    logInfo: (message) => {
        if (config.logLevel <= logLevel.information)
        {
            console.log(message);
        }
    },

    logDebug: (message) => {
        if (config.logLevel <= logLevel.debug)
        {
            console.debug(message);
        }
    }
};