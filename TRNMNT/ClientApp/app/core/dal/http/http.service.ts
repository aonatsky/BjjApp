import { Injectable } from '@angular/core';
import { RequestOptions, Http, Response, Headers, ResponseContentType, URLSearchParams } from '@angular/http';
import { LoggerService } from '../../../core/services/logger.service';
import { LoaderService } from '../../../core/services/loader.service';
import { RouterService } from '../../../core/services/router.service';
import { Observable } from "rxjs/Observable";
import * as FileSaver from 'file-saver';
import { AuthHttp } from 'angular2-jwt';
import { AuthService } from '../../services/auth.service';
import 'rxjs/Rx';
import { NotificationService } from '../../services/notification.service';

@Injectable()
export class HttpService {
    constructor(
        private loggerService: LoggerService,
        private http: AuthHttp,
        private loaderService: LoaderService,
        private routerService: RouterService,
        private notificationService: NotificationService,
        private authService: AuthService
    ) {
    }


    public get(name: string, paramsHolder?: object, responseType?: ResponseContentType): Observable<any> {
        let options = new RequestOptions({
            headers: new Headers({ 'Content-Type': 'application/json' })
        });

        if (responseType) {
            options.responseType = responseType;
        }
        if (paramsHolder != null) {
            var keys = Reflect.ownKeys(paramsHolder);
            let urlSearchParams = new URLSearchParams();
            for (var i = 0; i < keys.length; i++) {
                urlSearchParams.set(keys[i].toString(), paramsHolder[keys[i]]);
            }
            options.search = urlSearchParams;
        }

        this.loaderService.showLoader();

        return this.handleRequest(() => this.http.get(name, options));
    }

    public getById(name: string, id: string): Observable<any> {
        this.loaderService.showLoader();
        
        return this.handleRequest(() => this.http.get(name)); 
    }

    public post(name: string, model?: any, responseType?: ResponseContentType): Observable<any> {
        this.loaderService.showLoader();
        let options = new RequestOptions({
            headers: new Headers({ 'Content-Type': 'application/json' })
        });
        if (responseType) {
            options.responseType = responseType;
        }
        debugger;
        let body = JSON.stringify(model);

        return this.handleRequest(() => this.http.post(name, body, options));
    }

    public put(name: string, model: any): Observable<any> {
        this.loaderService.showLoader();
        let options = new RequestOptions({
            headers: new Headers({ 'Content-Type': 'application/json' })
        });
        let body = JSON.stringify(model);

        return this.handleRequest(() => this.http.put(name, body, options));
    }

    public delete(name: string, model: any): Observable<any> {
        this.loaderService.showLoader();
        let options = new RequestOptions({
            headers: new Headers({ 'Content-Type': 'application/json' }),
            body: JSON.stringify(model)
        });
        return this.handleRequest(() => this.http.delete(name, options));
    }

    public deleteById(name: string, id: any): Observable<any> {
        this.loaderService.showLoader();
        let options = new RequestOptions({
            headers: new Headers({ 'Content-Type': 'application/json' }),
        });
        let url = name + "/" + id;
        return this.handleRequest(() => this.http.delete(url, options));
    }

    public postFile(name: string, file: any): Observable<any> {
        this.loaderService.showLoader();
        let formData = new FormData();
        formData.append("file", file);
        return this.handleRequest(() => this.http.post(name, formData));
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

    private handleErrorRepeater(error: Response | any, repeatRequest: Function) {
        // In a real world app, you might use a remote logging infrastructure
        let errMsg: string;
        if (error instanceof Response) {
            if (error.status == 401) {
                this.loaderService.showLoader();
                return this.authService.getNewToken().map(() => {

                    this.loaderService.showLoader();
                    return repeatRequest()
                        .map((r: Response) => this.processResponse(r))
                        .catch((error: Response | any) => this.handleError(error))
                        .finally(() => this.loaderService.hideLoader());

                }).finally(() => this.loaderService.hideLoader());
            }
            errMsg = this.getErrorMessage(error);
        } else {
            errMsg = error.message ? error.message : error.toString();
        }
        console.error(errMsg);
        this.loggerService.logError(errMsg);
        return Observable.throw(errMsg);
    }

    private handleError(error: Response | any) {
        let errMsg = this.getErrorMessage(error);
        console.error(errMsg);
        this.notificationService.showGenericError();
        this.loggerService.logError(errMsg);
        return Observable.throw(errMsg);
    }

    private handleRequest(httpHandler: () => Observable<Response>): Observable<any> {
        return httpHandler()
            .map((r: Response) => this.processResponse(r))
            .catch((error: Response | any) => this.handleErrorRepeater(error, () => httpHandler()))
            .finally(() => this.loaderService.hideLoader());;
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

    private getErrorMessage(error: Response) {
        return `Error during request. Status: '${error.statusText}', code: '${error.status || ''}' url: '${error.url}'`;
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

