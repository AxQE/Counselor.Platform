export const LogLevel = {
    Debug: 0,
    Trace: 1,
    Information: 2,
    Error: 3,
    Critical: 4
}

export interface ILogEntry{
    OccurredOn: Date,
    UserId?: number,    
    Level: number,
    Message: string,
    StackTrack?: string
}