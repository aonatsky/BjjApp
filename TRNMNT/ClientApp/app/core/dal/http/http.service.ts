import { Message } from 'primeng/primeng';
import { Injectable } from '@angular/core';
import { Response, ResponseContentType } from '@angular/http';
import { LoggerService } from '../../services/logger.service';
import { LoaderService } from '../../services/loader.service';
import { RouterService } from '../../services/router.service';
import { Observable, throwError, of } from 'rxjs';
import { map, flatMap, catchError, finalize } from 'rxjs/operators';
import * as FileSaver from 'file-saver';
import { AuthService } from '../../services/auth.service';
import { NotificationService } from '../../services/notification.service';
import { HttpClient, HttpParams } from '@angular/common/http';

@Injectable()
export class HttpService {
  constructor(
    private loggerService: LoggerService,
    private loaderService: LoaderService,
    private routerService: RouterService,
    private notificationService: NotificationService,
    private authService: AuthService,
    private http: HttpClient
  ) {}

  //#region Public Methods

  get<T>(name: string, paramsHolder?: object, notifyMessage?: string, showLoader: boolean = true): Observable<any> {
    return this.handleRequest(
      () => this.http.get<T>(name, { params: this.convertParams(paramsHolder) }),
      notifyMessage, showLoader
    );
  }

  post<T>(name: string, model?: any, responseType?: ResponseContentType, notifyMessage?: string): Observable<any> {
    return this.handleRequest(() => this.http.post<T>(name, model), notifyMessage);
  }

  put<T>(name: string, model: any, notifyMessage?: string): Observable<any> {
    return this.handleRequest(() => this.http.put<T>(name, model), notifyMessage);
  }

  delete(name: string, model: any, notifyMessage?: string): Observable<any> {
    return this.handleRequest(() => this.http.delete(name), notifyMessage);
  }

  deleteById(name: string, id: any, notifyMessage?: string): Observable<any> {
    const url = name + '/' + id;
    return this.handleRequest(() => this.http.delete(url), notifyMessage);
  }

  postFile<T>(name: string, file: any, notifyMessage?: string): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    return this.handleRequest(() => this.http.post<T>(name, formData), notifyMessage);
  }

  getPdf(url, fileName): Observable<any> {
    return this.http.get(url, { responseType: 'blob' }).pipe(
      map(res => {
        FileSaver.saveAs(res, fileName);
      })
    );
  }

  getExcelFile(response: any, fileName: string): void {
    FileSaver.saveAs(response.blob(), fileName);
  }

  convertDate(input) {
    if (typeof input !== 'object') {
      return input;
    }

    for (let key in input) {
      if (!input.hasOwnProperty(key)) continue;

      let value = input[key];
      const type = typeof value;
      let match;
      if (type == 'string' && (match = value.match(this.iso8601RegEx))) {
        input[key] = new Date(value);
      } else if (type === 'object') {
        value = this.convertDate(value);
      }
    }
    return input;
  }

  //#endregion

  //#region Private Methods

  private handleRequest(
    httpHandler: () => Observable<any>,
    notifyMessage?: string,
    showLoader: boolean = true
  ): Observable<any> {
    if (showLoader) {
      this.loaderService.showLoader();
    }
    return httpHandler().pipe(
      map(r => this.convertDate(r)),
      catchError((error: Response | any) => this.handleErrorRepeater(error, () => httpHandler(), notifyMessage)),
      finalize(() => this.loaderService.hideLoader())
    );
  }

  private handleErrorRepeater(error: Response | any, repeatRequest: Function, notifyMessage?: string) {
    // In a real world app, you might use a remote logging infrastructure
    if (error instanceof Response && error.status === 401) {
      this.loaderService.showLoader();
      return this.authService
        .getNewToken()
        .pipe(
          flatMap(isSucceed => {
            if (!isSucceed) {
              throwError(error);
            }
            this.loaderService.showLoader();
            return repeatRequest().pipe(
              catchError((error: Response | any) => this.handleError(error, notifyMessage)),
              finalize(() => this.loaderService.hideLoader())
            );
          })
        )
        .pipe(
          catchError((error: Response | any) => this.goToLogin(error)),
          finalize(() => this.loaderService.hideLoader())
        );
    }
    return this.handleError(error, notifyMessage);
  }

  private handleError(errorResponse: Response | any, notifyMessage?: string) {
    if (errorResponse.status == 400) {
      this.notificationService.showWarn('', errorResponse.error);
      return throwError(errorResponse.error);
    } else {
      const errMsg = this.getErrorMessage(errorResponse);
      this.notificationService.showErrorOrGeneric(notifyMessage);
      this.loggerService.logError(errMsg);
      return throwError(errMsg);
    }
  }

  private convertParams(paramsHolder): HttpParams {
    return new HttpParams({
      fromObject: paramsHolder
    });
  }

  private goToLogin(error?: Response | any) {
    this.routerService.goToLogin();
    return this.handleError(error, 'Please sign in');
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

  //#endregion
}

export interface SearchParams {
  name: string;
  value: string;
}
