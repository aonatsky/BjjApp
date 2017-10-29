import { Injectable } from '@angular/core';
import { RequestOptions, Http, Response, Headers, ResponseContentType, URLSearchParams } from '@angular/http';
import { LoggerService } from '../../../core/services/logger.service';
import { LoaderService } from '../../../core/services/loader.service';
import { RouterService } from '../../../core/services/router.service';
import { Observable } from "rxjs/Observable";
import { ApiMethods } from "../consts/api-methods.consts";
import * as FileSaver from 'file-saver';
import { AuthHttp } from 'angular2-jwt';
import 'rxjs/Rx';

@Injectable()
export class HttpService {
    constructor(private loggerService: LoggerService, private http: AuthHttp, private loaderService: LoaderService, private routerService: RouterService) {
    }


    public get(name: string, params: SearchParams[] = []): Observable<any> {
        let httpRequest = this.http.get(name);

        if (params && params.length > 0) {
            let urlSearchParams = new URLSearchParams();
            for (var i = 0; i < params.length; i++) {
                urlSearchParams.set(params[i].name, params[i].value);
            }
            httpRequest = this.http.get(name, { search: urlSearchParams });
        }

        this.loaderService.showLoader();
        return httpRequest
            .map((r: Response) => this.processResponse(r))
            .catch((error: Response | any) => this.handleError(error))
            .finally(() => this.loaderService.hideLoader());
    }

    public getById(name: string, id: string): Observable<any> {
        this.loaderService.showLoader();
        return this.http.get(name).map((r: Response) => this.processResponse(r)).catch((error: Response | any) => this.handleError(error)).finally(() => this.loaderService.hideLoader());
    }

    public post(name: string, model?: any, responseType?: ResponseContentType): Observable<any> {
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

    public postFile(name: string, file: any): Observable<any> {
        this.loaderService.showLoader();
        let formData = new FormData();
        formData.append("file", file)
        return this.http.post(name, formData)
            .map((r: Response) => this.processResponse(r))
            .catch((error: Response | any) => this.handleError(error)).finally(() => this.loaderService.hideLoader());;
    }

    public getPdf(url, fileName): Observable<any> {
        return this.http.get(url, { responseType: ResponseContentType.Blob }).map((res) => {
            FileSaver.saveAs(new Blob([res.blob()], { type: 'application/pdf' }), fileName);
        });
    }


    //PRIVATE
    private processResponse(response: any): Observable<any> {
        return response;
    }

    private handleError(error: Response | any) {
        // In a real world app, you might use a remote logging infrastructure
        let errMsg: string;
        if (error instanceof Response) {
            if (error.status == 401) {
                this.routerService.goToLogin();
            }
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

    public getJson(response: Response) {
        return response.json();
    }

    public getString(response: Response) {
        return response.text();
    }

    public getExcelFile(response: Response, fileName: string): void {
        FileSaver.saveAs(response.blob(), fileName);
    }




    private iso8601RegEx = /(19|20|21)\d\d([-/.])(0[1-9]|1[012])\2(0[1-9]|[12][0-9]|3[01])T(\d\d)([:/.])(\d\d)([:/.])(\d\d)/;

    convertDate(input) {
        if (typeof input !== "object") {
            return input
        };

        for (var key in input) {
            if (!input.hasOwnProperty(key)) continue;

            var value = input[key];
            var type = typeof value;
            var match;
            if (type == 'string' && (match = value.match(this.iso8601RegEx))) {
                input[key] = new Date(value)
            }
            else if (type === "object") {
                value = this.convertDate(value);
            }
        }
        return input;
    }
}

export interface SearchParams {
    name: string;
    value: string;
}

