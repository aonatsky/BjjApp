import { Injectable } from '@angular/core';
import { RequestOptions, Http, Response, Headers } from '@angular/http';
import { ServerSettingsService } from '../server.settings.service';
import { LoggerService } from '../../../core/services/logger.service';
import { Observable } from "rxjs/Observable";
import { ApiMethods } from "../consts/api-methods.consts";

@Injectable()
export class ApiServer {
    constructor(private serverSettings: ServerSettingsService, private loggerService: LoggerService, private http: Http) {
    }


    public get(name: string): Observable<any> {
        return this.http.get(name).map((r: Response) => this.processResponse(r)).catch((error: Response | any) => this.handleError(error));
    }

    public post(name: string, model: any): Observable<any> {
        let options = new RequestOptions({
            headers: new Headers({ 'Content-Type': 'application/json' })
        });
        let body = JSON.stringify(model);
        return this.http.post(name, body, options).map((r: Response) => this.processResponse(r)).catch((error: Response | any) => this.handleError(error));
    }

    public put(name: string, model: any): Observable<any> {
        let body = JSON.stringify(model);
        return this.http.post(name, body).map((r: Response) => this.processResponse(r)).catch((error: Response | any) => this.handleError(error));
    }

    public delete(name: string, model: any): Observable<any> {
        let options = new RequestOptions({
            headers: new Headers({ 'Content-Type': 'application/json' }),
            body: JSON.stringify(model)
        });
        return this.http.delete(name, options).map((r: Response) => this.processResponse(r)).catch((error: Response | any) => this.handleError(error));
    }

    private processResponse(response: Response): Observable<any> {
        let body = response.json();
        return body.data || {};

    }
    private handleError(error: Response | any) {
        // In a real world app, you might use a remote logging infrastructure
        let errMsg: string;
        if (error instanceof Response) {
            errMsg = `${error.status} - ${error.statusText || ''} url: ${error.url}`;
        } else {
            errMsg = error.message ? error.message : error.toString();
        }
        console.error(errMsg);
        this.loggerService.logError(errMsg)
        return Observable.throw(errMsg);
    }
}

