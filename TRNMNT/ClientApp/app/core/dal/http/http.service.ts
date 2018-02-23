import { Injectable } from '@angular/core';
import { RequestOptions, Response, Headers, ResponseContentType, URLSearchParams } from '@angular/http';
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

    //#region Public Methods

    public get(name: string, paramsHolder?: object, responseType?: ResponseContentType, notifyMessage?: string): Observable<any> {
        let options = this.jsonRequestOptions(responseType);
        if (paramsHolder != null) {
            var keys = Reflect.ownKeys(paramsHolder);
            let urlSearchParams = new URLSearchParams();
            for (var i = 0; i < keys.length; i++) {
                urlSearchParams.set(keys[i].toString(), paramsHolder[keys[i]]);
            }
            options.search = urlSearchParams;
        }
        return this.handleRequest(() => this.http.get(name, options), notifyMessage);
    }

    public getById(name: string, id: string, notifyMessage?: string): Observable<any> {
        return this.handleRequest(() => this.http.get(name), notifyMessage); 
    }

    public post(name: string, model?: any, responseType?: ResponseContentType, notifyMessage?: string): Observable<any> {
        let options = this.jsonRequestOptions(responseType);
        let body = JSON.stringify(model);
        return this.handleRequest(() => this.http.post(name, body, options), notifyMessage);
    }

    public put(name: string, model: any, notifyMessage?: string): Observable<any> {
        let body = JSON.stringify(model);
        return this.handleRequest(() => this.http.put(name, body, this.jsonRequestOptions()), notifyMessage);
    }

    public delete(name: string, model: any, notifyMessage?: string): Observable<any> {
        let options = this.jsonRequestOptions();
        options.body = JSON.stringify(model);
        return this.handleRequest(() => this.http.delete(name, options), notifyMessage);
    }

    public deleteById(name: string, id: any, notifyMessage?: string): Observable<any> {
        let url = name + "/" + id;
        return this.handleRequest(() => this.http.delete(url, this.jsonRequestOptions()), notifyMessage);
    }

    public postFile(name: string, file: any, notifyMessage?: string): Observable<any> {
        let formData = new FormData();
        formData.append("file", file);
        return this.handleRequest(() => this.http.post(name, formData), notifyMessage);
    }

    public getPdf(url, fileName): Observable<any> {
        return this.http.get(url, { responseType: ResponseContentType.Blob }).map((res) => {
            FileSaver.saveAs(new Blob([res.blob()], { type: 'application/pdf' }), fileName);
        });
    }

    public getExcelFile(response: Response, fileName: string): void {
        FileSaver.saveAs(response.blob(), fileName);
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

    public convertDate(input) {
        if (typeof input !== "object") {
            return input;
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

    //#endregion

    //#region Private Methods

    private processResponse(response: any): Observable<any> {
        return response;
    }

    private handleRequest(httpHandler: () => Observable<Response>, notifyMessage?: string): Observable<any> {
        this.loaderService.showLoader();
        return httpHandler()
            .map((r: Response) => this.processResponse(r))
            .catch((error: Response | any) => this.handleErrorRepeater(error, () => httpHandler(), notifyMessage))
            .finally(() => this.loaderService.hideLoader());
    }

    private handleErrorRepeater(error: Response | any, repeatRequest: Function, notifyMessage?: string) {
        // In a real world app, you might use a remote logging infrastructure
        if (error instanceof Response && error.status == 401) {
            this.loaderService.showLoader();
            return this.authService.revokeRefreshToken().flatMap((isSucceed) => {
                    if (!isSucceed) {
                        Observable.throw(error);
                    }
                    this.loaderService.showLoader();
                    return repeatRequest()
                        .map((r: Response) => this.processResponse(r))
                        .catch((error: Response | any) => this.handleError(error, notifyMessage))
                        .finally(() => this.loaderService.hideLoader());
                })
                .catch((error: Response | any) => this.goToLogin(error))
                .finally(() => this.loaderService.hideLoader());
        }
        return this.handleError(error, notifyMessage);
    }

    private handleError(error: Response | any, notifyMessage?: string) {
        let errMsg = this.getErrorMessage(error);
        this.notificationService.showErrorOrGeneric(notifyMessage);
        this.loggerService.logError(errMsg);
        return Observable.throw(errMsg);
    }

    private goToLogin(error?: Response | any) {
        this.routerService.goToLogin();
        return this.handleError(error, "Please autentificate");
    }

    private getErrorMessage(error: Response | any) : string {
        let message = "Error during request";
        if (error instanceof Response) {
            return `${message}. Status: '${error.statusText}', code: '${error.status || ''}' url: '${error.url}'`;
        }
        if (!!error) {
            return `${message}. Message: '${error.message ? error.message : error.toString()}'`;
        }
        return message;
    }

    private iso8601RegEx = /(19|20|21)\d\d([-/.])(0[1-9]|1[012])\2(0[1-9]|[12][0-9]|3[01])T(\d\d)([:/.])(\d\d)([:/.])(\d\d)/;

    private jsonRequestOptions(responseType?: ResponseContentType): RequestOptions {
        let options =  new RequestOptions({
            headers: new Headers({ 'Content-Type': 'application/json' }),
        });
        if (responseType) {
            options.responseType = responseType;
        }
        return options;
    }

    //#endregion
}

    

export interface SearchParams {
    name: string;
    value: string;
}

