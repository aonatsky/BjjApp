import { Injectable } from '@angular/core';
import { AppConfig } from '../../app.config';
import { ApiServer } from "../dal/servers/api.server";
import { ApiMethods } from '../dal/consts/api-methods.consts'
import { RequestOptions, Http, Headers, Response } from '@angular/http';

@Injectable()
export class LoggerService {
    handleError: any;

    constructor(private http: Http) {

    }

    logs: string[] = [];

    logLevel = Object.freeze({
        info: "INFO",
        warn: "WARN",
        error: "ERROR",
        debug: "DEBUG"

    })

    logInfo(msg: any) {
        this.postLog(new LogModel(this.logLevel.info,msg))
        this.log(`INFO: ${msg}`);
    }

    logDebug(msg: any) {
        this.postLog(new LogModel(this.logLevel.debug,msg))
        this.log(`DEBUG: ${msg}`);
    }

    logError(msg: any) {
        this.postLog(new LogModel(this.logLevel.error,msg))
        this.log(`ERROR: ${msg}`, true);
    }


    private log(msg: any, isErr = false) {
        if (AppConfig.isDebug) {
            this.logs.push(msg);
            isErr ? console.error(msg) : console.log(msg);
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


export class LogModel {
    constructor(private Level: string, private message: string) {
    }
}
