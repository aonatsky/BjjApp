import { Injectable } from '@angular/core';
import { RequestOptions, Http, Response, Headers, ResponseContentType } from '@angular/http';
import { LoggerService } from '../../../core/services/logger.service';
import { LoaderService } from '../../../core/services/loader.service';
import { Observable } from "rxjs/Observable";
import { ApiMethods } from "../consts/api-methods.consts";
import * as FileSaver from 'file-saver';
import 'rxjs/Rx';

@Injectable()
export class HttpService {
    constructor(private loggerService: LoggerService, private http: Http, private loaderService: LoaderService) {
    }


    public get(name: string): Observable<any> {
        this.loaderService.showLoader();
        return this.http.get(name).map((r: Response) => this.processResponse(r)).catch((error: Response | any) => this.handleError(error)).finally(() => this.loaderService.hideLoader());
    }

    public post(name: string, model: any, responseType?: ResponseContentType): Observable<any> {
        this.loaderService.showLoader();
        let options = new RequestOptions({
            headers: new Headers({ 'Content-Type': 'application/json' })
        });
        if (responseType) {
            options.responseType = responseType;
        }
        let body = JSON.stringify(model);
        return this.http.post(name, body, options).map((r: Response) => this.processResponse(r)).catch((error: Response | any) => this.handleError(error)).finally(() => this.loaderService.hideLoader());;
    }

    public put(name: string, model: any): Observable<any> {
        this.loaderService.showLoader();
        let options = new RequestOptions({
            headers: new Headers({ 'Content-Type': 'application/json' })
        });
        let body = JSON.stringify(model);
        return this.http.put(name, body, options).map((r: Response) => this.processResponse(r)).catch((error: Response | any) => this.handleError(error)).finally(() => this.loaderService.hideLoader());;
    }

    public delete(name: string, model: any): Observable<any> {
        this.loaderService.showLoader();
        let options = new RequestOptions({
            headers: new Headers({ 'Content-Type': 'application/json' }),
            body: JSON.stringify(model)
        });
        return this.http.delete(name, options).map((r: Response) => this.processResponse(r)).catch((error: Response | any) => this.handleError(error)).finally(() => this.loaderService.hideLoader());;
    }

    public deleteById(name: string, id: any): Observable<any> {
        this.loaderService.showLoader();
        let options = new RequestOptions({
            headers: new Headers({ 'Content-Type': 'application/json' }),
        });
        let url = name + "/" + id;
        return this.http.delete(url, options).map((r: Response) => this.processResponse(r)).catch((error: Response | any) => this.handleError(error)).finally(() => this.loaderService.hideLoader());;
    }

    public postFile(name: string, file: any): Observable<any>{
        this.loaderService.showLoader();
        let formData = new FormData();
        formData.append("file",file)
        return this.http.post(name, formData)
            .map((r: Response) => this.processResponse(r))
            .catch((error: Response | any) => this.handleError(error)).finally(() => this.loaderService.hideLoader());;
    }

    private processResponse(response: any): Observable<any> {
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
        this.loggerService.logError(errMsg)
        return Observable.throw(errMsg);
    }

    public getArray<T>(response: any): T[] {
        let result = response.json();
        if (result.length == 0) {
            return [];
        }
        return result;
    }

    public getJson(response: any) {
        return response.json();
    }

    public getExcelFile(response: Response): void {
        var blob = response.blob();
        FileSaver.saveAs(blob, response.headers.get("filename"));
    }
}

