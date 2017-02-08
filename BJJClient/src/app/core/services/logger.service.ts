import {Injectable} from '@angular/core';
import {AppConfig} from '../../app.config';

@Injectable()
export class LoggerService {
    logs:string[] = [];

    logInfo(msg:any) {
        this.log(`INFO: ${msg}`);
    }

    logDebug(msg:any) {
        this.log(`DEBUG: ${msg}`);
    }

    logError(msg:any) {
        this.log(`ERROR: ${msg}`, true);
    }

    private log(msg:any, isErr = false) {
        if (AppConfig.isDebug) {
            this.logs.push(msg);
            isErr ? console.error(msg) : console.log(msg);
        }
    }
}
