import {Injectable} from '@angular/core';
import {Http, Headers, URLSearchParams, Response, RequestOptions} from '@angular/http';
import {Observable} from 'rxjs/Observable';

import {ResponseModel} from '../model/response.model';
import {RequestModel} from '../model/request.model';
import {LoggerService} from '../../core/services/logger.service';

@Injectable()
export class BaseServer {
    constructor(private baseUrl:string, private loggerService: LoggerService, protected http:Http) {
    }

    private getFullUrl(name?:string):string {
        name = name || '';

        return this.baseUrl + name;
    }

    public delete(name: string, request: RequestModel): Observable<ResponseModel> {
        let options = this.createRequestOptions(request);

        return this.http.delete(name, options)
            .map(res => this.handleResponse(res))
            .catch(res => this.handleFailedResponse(res));;
    }

    public getR(name: string, params: any, headers: any): Observable<ResponseModel> {
        let headersArg = new Headers(headers);
        let options = new RequestOptions({ headers: headersArg });
        let queryParams = params;

        if (params && !(params instanceof URLSearchParams)) {
            queryParams = this.createUrlParams(params);
        }

        return this.http.get(this.getFullUrl(name), {search: queryParams, headers: headersArg})
            .map(res => this.handleResponse(res))
            .catch(res => this.handleFailedResponse(res));
    }

    public get(name?:string, params?:URLSearchParams | any): Observable<any> {
        let queryParams = params;

        if (params && !(params instanceof URLSearchParams)) {
            queryParams = this.createUrlParams(params);
        }

        return this.http.get(this.getFullUrl(name), {search: queryParams}).map((res:any) => res.json());
    }

    public post(name: string, request: RequestModel): Observable<ResponseModel> {
        let options = this.createRequestOptions(request);
        let body = request.body || {};

        return this.http.post(this.getFullUrl(name), JSON.stringify(body), options)
            .map(res => this.handleResponse(res))
            .catch(res => this.handleFailedResponse(res));
    }

    public postUrlEncoded(name: string, data: any, headers: any): Observable<ResponseModel> {
        let queryParams = this.createUrlParams(data);
        let headersArg = new Headers(headers);
        let options = new RequestOptions({ headers: headersArg });
        return this.http.post(this.getFullUrl(name), queryParams.toString(), options)
            .map(res => new ResponseModel(res.ok, res.json()))
            .catch(res => this.handleFailedResponse(res));
    }

    private createRequestOptions(request: RequestModel): RequestOptions {
        let options = new RequestOptions();

        if (request.headers) {
            options.headers = new Headers(request.headers);    
        }

        if (request.query) {
            options.search = this.createUrlParams(request.query);    
        }

        return options;
    }

    private createUrlParams(params: any): URLSearchParams {
        let queryParams = new URLSearchParams();

        for (let key in params) {
            queryParams.set(key, params[key].toString());
        }

        return queryParams;
    }

    private handleResponse(response: Response): ResponseModel {
        return new ResponseModel(response.ok, response.text() ? response.json() : {});
    }

    private handleFailedResponse(response: Response): Observable<ResponseModel> {
        this.loggerService.logError(response.toString());
        return Observable.throw(new ResponseModel(response.ok, response.text() ? response.json() : {}));
    }
}
