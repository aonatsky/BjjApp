import { Injectable } from '@angular/core';
import { RequestOptions, Response, Headers, ResponseContentType, URLSearchParams } from '@angular/http';
import { LoggerService } from '../../../core/services/logger.service';
import { LoaderService } from '../../../core/services/loader.service';
import { RouterService } from '../../../core/services/router.service';
import { Observable, throwError, pipe } from 'rxjs';
import { map, flatMap, catchError, finalize } from 'rxjs/operators';
import * as FileSaver from 'file-saver';
import { AuthService } from '../../services/auth.service';
import { NotificationService } from '../../services/notification.service';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class HttpService {
    constructor(
        private loggerService: LoggerService,
        private loaderService: LoaderService,
        private routerService: RouterService,
        private notificationService: NotificationService,
        private authService: AuthService,
        private http: HttpClient
    ) {
    }

    //#region Public Methods

    get<T>(name: string, paramsHolder?: object, responseType?: ResponseContentType, notifyMessage?: string): Observable<any> {
        const options = this.jsonRequestOptions(responseType);
        if (paramsHolder != null) {
            const keys = Reflect.ownKeys(paramsHolder);
            const urlSearchParams = new URLSearchParams();
            for (let i = 0; i < keys.length; i++) {
                urlSearchParams.set(keys[i].toString(), paramsHolder[keys[i]]);
            }
            options.search = urlSearchParams;
        }
        return this.handleRequest(() => this.http.get<T>(name), notifyMessage);
    }


    post<T>(name: string, model?: any, responseType?: ResponseContentType, notifyMessage?: string): Observable<any> {
        const options = this.jsonRequestOptions(responseType);
        const body = JSON.stringify(model);
        return this.handleRequest(() => this.http.post<T>(name, model), notifyMessage);
    }

    put<T>(name: string, model: any, notifyMessage?: string): Observable<any> {
        const body = JSON.stringify(model);
        return this.handleRequest(() => this.http.put<T>(name, body), notifyMessage);
    }

    delete(name: string, model: any, notifyMessage?: string): Observable<any> {
        const options = this.jsonRequestOptions();
        options.body = JSON.stringify(model);
        return this.handleRequest(() => this.http.delete(name), notifyMessage);
    }

    deleteById(name: string, id: any, notifyMessage?: string): Observable<any> {
        const url = name + '/' + id;
        return this.handleRequest(() => this.http.delete(url), notifyMessage);
    }

    postFile(name: string, file: any, notifyMessage?: string): Observable<any> {
        const formData = new FormData();
        formData.append('file', file);
        return this.handleRequest(() => this.http.post(name, formData), notifyMessage);
    }

    getPdf(url, fileName): Observable<any> {
        return this.http.get<Blob>(url).pipe(map((res) => {
            FileSaver.saveAs(res, 'application/pdf', fileName);
        }));
    }

    getExcelFile(response: any, fileName: string): void {
        FileSaver.saveAs(response.blob(), fileName);
    }

    getArray<T>(response: any): T[] {
        const result = response.json();
        if (result.length == 0) {
            return [];
        }
        return result;
    }

    getJson(response: Response) {
        return response.json();
    }

    getString(response: Response) {
        return response.text();
    }

    convertDate(input) {
        if (typeof input !== 'object') {
            return input;
        };

        for (let key in input) {
            if (!input.hasOwnProperty(key)) continue;

            let value = input[key];
            const type = typeof value;
            let match;
            if (type == 'string' && (match = value.match(this.iso8601RegEx))) {
                input[key] = new Date(value)
            }
            else if (type === 'object') {
                value = this.convertDate(value);
            }
        }
        return input;
    }

    //#endregion

    //#region Private Methods


    private handleRequest(httpHandler: () => Observable<any>, notifyMessage?: string): Observable<any> {
        this.loaderService.showLoader();
        return httpHandler().pipe(catchError((error: Response | any) => this.handleErrorRepeater(error, () => httpHandler(), notifyMessage))
            , finalize(() => this.loaderService.hideLoader()));
    }

    private handleErrorRepeater(error: Response | any, repeatRequest: Function, notifyMessage?: string) {
        // In a real world app, you might use a remote logging infrastructure
        if (error instanceof Response && error.status == 401) {
            this.loaderService.showLoader();
            return this.authService.revokeRefreshToken().pipe(flatMap((isSucceed) => {
                if (!isSucceed) {
                    throwError(error);
                }
                this.loaderService.showLoader();
                return repeatRequest().pipe(
                    catchError((error: Response | any) => this.handleError(error, notifyMessage)),
                    finalize(() => this.loaderService.hideLoader()));

            })).pipe(catchError((error: Response | any) => this.goToLogin(error)),
                finalize(() => this.loaderService.hideLoader()));

        }
        return this.handleError(error, notifyMessage);
    }

    private handleError(error: Response | any, notifyMessage?: string) {
        const errMsg = this.getErrorMessage(error);
        this.notificationService.showErrorOrGeneric(notifyMessage);
        this.loggerService.logError(errMsg);
        return throwError(errMsg);
    }

    private goToLogin(error?: Response | any) {
        this.routerService.goToLogin();
        return this.handleError(error, 'Please sigh in');
    }

    private getErrorMessage(error: Response | any): string {
        const message = 'Error during request';
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
        const options = new RequestOptions({
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

