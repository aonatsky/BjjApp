import { Injectable } from '@angular/core';
import { ApiMethods } from '../dal/consts/api-methods.consts'
import { RequestOptions, Http, Headers } from '@angular/http';
import { isDevMode } from '@angular/core';

@Injectable()
export class LoggerService {
    handleError: any;

    constructor(private http: Http) {

    }

    logs: string[] = [];

    logInfo(msg: any) {
        this.log(msg, LogLevel.Info, this.getArgs(arguments));
    }

    logDebug(msg: any) {
        this.log(msg, LogLevel.Debug, this.getArgs(arguments));
    }

    logError(msg: any) {
        this.log(msg, LogLevel.Error, this.getArgs(arguments));
    }

    logWarn(msg: any) {
        this.log(msg, LogLevel.Warn, this.getArgs(arguments));
    }

    private log(msg: any, level: LogLevel, params: any[]) {

        this.postLog(new LogModel(level, msg));

        if (isDevMode()) {
            let message = `${this.getLogPrefix(level)} ${msg}`;

            params.unshift(message);
            this.logs.push(message);
            this.getLogFunction(level).apply(console, params);

            let isNeedTrace = level !== LogLevel.Info;
            if (isNeedTrace) {
                console.groupCollapsed('Full stack trace');
                console.trace();
                console.groupEnd();
            }
        }
    }

    private getArgs(params: any): any[] {
        var args = Array.prototype.slice.call(params, 1);
        
        return args;
    }

    private getLogPrefix(level: LogLevel): string {
        switch (level) {
            case LogLevel.Error:
                return "ERROR:";
            case LogLevel.Warn:
                return "WARN:";
            case LogLevel.Info:
                return "INFO:";
            case LogLevel.Debug:
                return "DEBUG:";
            default:
                return "";
        }
    }

    private getLogFunction(level: LogLevel): Function {
        switch (level) {
            case LogLevel.Error:
                return console.error;
            case LogLevel.Warn:
                return console.warn;
            case LogLevel.Info:
                return console.info;
            case LogLevel.Debug:
                return console.log;
            default:
                return console.error;
        }
    }

    private postLog(log: LogModel) {
        let options = new RequestOptions({
            headers: new Headers({ 'Content-Type': 'application/json' })
        });
        let body = JSON.stringify(log);
        this.http.post(ApiMethods.log, body, options).subscribe();
    }

}
// mapped with backend LogLevel enum
export enum LogLevel {
    Debug = 1,
    Info = 2,
    Warn = 3,
    Error = 4
}

export class LogModel {
    constructor(private level: LogLevel, private message: string) {
    }
}
