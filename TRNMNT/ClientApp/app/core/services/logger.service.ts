import {Injectable} from '@angular/core';
import { AppConfig } from '../../app.config';
import { ApiServer } from "../dal/servers/api.server";
import { ApiMethods } from '../dal/consts/api-methods.consts'
import { Http, Response } from "@angular/http";

@Injectable()
export class LoggerService {
        handleError: any;

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
        this.http.post(ApiMethods.log,msg).subscribe();
        this.log(`ERROR: ${msg}`, true);
    }

    private processResponse(response){
        let body = response.json();
        return body.data || {};
    }


    private log(msg:any, isErr = false) {
        if (AppConfig.isDebug) {
            this.logs.push(msg);
            isErr ? console.error(msg) : console.log(msg);
        }
    }
}
