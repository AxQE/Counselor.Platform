import { LogLevel } from '../types/Logger'
import { LoggingConfig } from '../Config'

class LoggingService {  

    LogDebug(error: any, message: string) {
        if (LoggingConfig.Level >= LogLevel.Debug)
            console.log(message, error);
    }

    LogTrace(error: any, message: string) {
        if (LoggingConfig.Level >= LogLevel.Trace)
            console.log(message, error);
    }

    LogInfo(error: any, message: string) {
        if (LoggingConfig.Level >= LogLevel.Information)
            console.log(message, error);
    }

    LogError(error: any, message: string) {
        if (LoggingConfig.Level >= LogLevel.Error)
            console.log(message, error);
    }

    LogCritical(error: any, message: string) {
        if (LoggingConfig.Level >= LogLevel.Critical)
            console.log(message, error);
    }
}

export default { LoggingService };