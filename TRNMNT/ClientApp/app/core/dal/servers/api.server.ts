import {Injectable} from '@angular/core';
import {Http, Response} from '@angular/http';
import {ServerSettingsService} from '../server.settings.service';
import { LoggerService } from '../../../core/services/logger.service';
import { Observable } from "rxjs/Observable";
import { ApiMethods } from "../consts/api-methods.consts";

@Injectable()
export class ApiServer {
    constructor(serverSettings: ServerSettingsService, loggerService: LoggerService, private http: Http ) {
    }


    public get(name:string): Observable<any> {
        return this.http.get(name).map((r:Response) => this.processResponse(r));
    }

    private processResponse(response : Response) : Observable<any>{
        return response.json();
    }

}

