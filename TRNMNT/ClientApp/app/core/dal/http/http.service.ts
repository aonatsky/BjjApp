import { Injectable } from '@angular/core'
import { Observable } from 'rxjs/Rx';
import { LoggerService } from '../../../core/services/logger.service';
import { RequestOptions, Http, Response, Headers, ResponseContentType } from '@angular/http';

import * as FileSaver from 'file-saver';


import { ApiMethods } from '../consts/api-methods.consts'


import { Fighter } from '../../model/fighter.model'
import { WeightDivision } from '../../model/weight-division.model'
import { FighterFilterModel } from '../../model/fighter-filter.model'
import { Category } from "../../model/category.model";

@Injectable()
export class HttpService {


    constructor(private http: Http, private logger: LoggerService) {
        
    }

    //Common
    private getArray<T>(response: any): T[] {
        let result = response.json();
        if (result.length == 0) {
            return [];
        }
        return result;
    }

    private handleErrorResponse(response: any): Observable<any> {
        let data: any = response;
        return Observable.throw(data);
    }

    private getResult(response: any) {
        return response.json();
    }

    public getExcelFile(response: Response): void {
        var blob = response.blob();
        FileSaver.saveAs(blob, response.headers.get("filename"));
    }

    public get(name: string): Observable<any> {
        return this.http.get(name).map((r: Response) => this.processResponse(r)).catch((error: Response | any) => this.handleError(error));
    }

    public RESTGet<T>(type: { new (): T}): Observable<T> {
        return this.http.get(type.name).map((r: Response) => <T>r.json()).catch((error: Response | any) => this.handleError(error));
    }


    public post(name: string, model: any, responseType?: ResponseContentType): Observable<any> {
        let options = new RequestOptions({
            headers: new Headers({ 'Content-Type': 'application/json' })
        });
        if (responseType) {
            options.responseType = responseType;
        }
        let body = JSON.stringify(model);
        return this.http.post(name, body, options).map((r: Response) => this.processResponse(r)).catch((error: Response | any) => this.handleError(error));
    }

    public put(name: string, model: any): Observable<any> {
        let options = new RequestOptions({
            headers: new Headers({ 'Content-Type': 'application/json' })
        });
        let body = JSON.stringify(model);
        return this.http.put(name, body, options).map((r: Response) => this.processResponse(r)).catch((error: Response | any) => this.handleError(error));
    }

    public delete(name: string, model: any): Observable<any> {
        let options = new RequestOptions({
            headers: new Headers({ 'Content-Type': 'application/json' }),
            body: JSON.stringify(model)
        });
        return this.http.delete(name, options).map((r: Response) => this.processResponse(r)).catch((error: Response | any) => this.handleError(error));
    }

    public deleteById(name: string, id: string): Observable<any> {
        let options = new RequestOptions({
            headers: new Headers({ 'Content-Type': 'application/json' }),
        });
        let url = name + "/" + id;
        return this.http.delete(url, options).map((r: Response) => this.processResponse(r)).catch((error: Response | any) => this.handleError(error));
    }

    public postFile(name: string, file: any): Observable<any> {
        let formData = new FormData();
        formData.append("file", file)
        return this.http.post(name, formData)
            .map((r: Response) => this.processResponse(r))
            .catch((error: Response | any) => this.handleError(error));
    }

    private processResponse(response: any): Observable<any> {
        // add additional processing
        return response;
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
        this.logger.logError(errMsg)
        return Observable.throw(errMsg);
    }

}