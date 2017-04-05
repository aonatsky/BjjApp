import {Injectable} from '@angular/core';
import { AppConfig } from '../../app.config';
import { ApiServer } from "../dal/servers/api.server";
import { ApiMethods } from '../dal/consts/api-methods.consts'
import { Http } from "@angular/http";

@Injectable()
export class LoggerService {

    constructor(private http: Http) {

    }

    logs:string[] = [];

    logInfo(msg:any) {
        this.log(`INFO: ${msg}`);
    }

    logDebug(msg:any) {
        this.log(`DEBUG: ${msg}`);
    }

    logError(msg:any) {
        this.http.post(ApiMethods.log,msg)
        this.log(`ERROR: ${msg}`, true);
    }

    private log(msg:any, isErr = false) {
        if (AppConfig.isDebug) {
            this.logs.push(msg);
            isErr ? console.error(msg) : console.log(msg);
        }
    }
}
