import { LogLevel, ILogEntry } from '../types/Logger'
import { LoggingConfig } from '../Config'

class Logger { 
    //todo: нужно сделать фоновый обработчик (worker) кторый будет слать логи пачками на бэк
    private static _instance : Logger;

    private constructor() {}

    public static getInstance() : Logger {
        if (!Logger._instance)
            Logger._instance = new Logger();

        return Logger._instance;
    }

    LogDebug(message: string, error?: any, userId?: number) {
        if (LoggingConfig.Level >= LogLevel.Debug){
            if (LoggingConfig.OutputToConsole){
                console.log(message, error);
            }

            if (LoggingConfig.SendToBackend){
                const entry : ILogEntry = {
                    OccurredOn: new Date(),
                    Level: LogLevel.Debug,
                    Message: message,
                    UserId: userId,
                    StackTrack: error
                }
    
                this.SendLogToBackend(entry);
            }
        }            
    }

    LogTrace(message: string, error?: any, userId?: number) {
        if (LoggingConfig.Level >= LogLevel.Trace) {
            if (LoggingConfig.OutputToConsole){
                console.log(message, error);
            }

            if (LoggingConfig.SendToBackend){
                const entry : ILogEntry = {
                    OccurredOn: new Date(),
                    Level: LogLevel.Trace,
                    Message: message,
                    UserId: userId,
                    StackTrack: error
                }
    
                this.SendLogToBackend(entry);
            }
        }
            
    }

    LogInfo(message: string, error?: any, userId?: number) {
        if (LoggingConfig.Level >= LogLevel.Information) {
            if (LoggingConfig.OutputToConsole){
                console.log(message, error);
            }

            if (LoggingConfig.SendToBackend){
                const entry : ILogEntry = {
                    OccurredOn: new Date(),
                    Level: LogLevel.Information,
                    Message: message,
                    UserId: userId,
                    StackTrack: error
                }
    
                this.SendLogToBackend(entry);
            }
        }            
    }

    LogError(message: string, error?: any, userId?: number) {
        if (LoggingConfig.Level >= LogLevel.Error) {
            if (LoggingConfig.OutputToConsole){
                console.error(message, error);
            }

            if (LoggingConfig.SendToBackend){
                const entry : ILogEntry = {
                    OccurredOn: new Date(),
                    Level: LogLevel.Error,
                    Message: message,
                    UserId: userId,
                    StackTrack: error
                }
    
                this.SendLogToBackend(entry);
            }
        }            
    }

    LogCritical(message: string, error?: any, userId?: number) {
        if (LoggingConfig.Level >= LogLevel.Critical) {
            if (LoggingConfig.OutputToConsole){
                console.error(message, error);
            }

            if (LoggingConfig.SendToBackend){
                const entry : ILogEntry = {
                    OccurredOn: new Date(),
                    Level: LogLevel.Critical,
                    Message: message,
                    UserId: userId,
                    StackTrack: error
                }
    
                this.SendLogToBackend(entry);
            }
        }
            
    }

    private SendLogToBackend(entry : ILogEntry){

    }
}

export default { Logger, LogLevel };