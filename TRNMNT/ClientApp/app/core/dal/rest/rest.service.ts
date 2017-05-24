import { Injectable } from '@angular/core'
import { Observable } from 'rxjs/Rx';
import { LoggerService } from '../../../core/services/logger.service';
import { HttpService } from '../../../core/dal/http/http.service';
import { RequestOptions, Http, Response, Headers, ResponseContentType } from '@angular/http';

import * as FileSaver from 'file-saver';


import { ApiMethods } from '../consts/api-methods.consts'


import { Fighter } from '../../model/fighter.model'
import { WeightDivision } from '../../model/weight-division.model'
import { FighterFilterModel } from '../../model/fighter-filter.model'
import { Category } from "../../model/category.model";

@Injectable()
export class RestService {


    private API_URL = "api"

    constructor(private httpService: HttpService, private logger: LoggerService) {
        
    }

    //Common


    public get<T>(type: { new (): T }): Observable<T> {
        return this.httpService.get(this.API_URL + "/" + type.name).map((r: Response) => <T>r.json());
    }

    public update<T>(type: { new (): T }, model: T): Observable<T> {
        return this.httpService.put(this.API_URL + "/" + type.name, model).map((r: Response) => <T>r.json());
    }

    public add<T>(type: { new (): T }, model: T): Observable<T> {
        return this.httpService.post(this.API_URL + "/" + type.name, model).map((r: Response) => <T>r.json());
    }

    public delete<T>(type: { new (): T }, model: T): Observable<T> {
        return this.httpService.delete(this.API_URL + "/" + type.name, model).map((r: Response) => <T>r.json());
    }

    public deleteById<T>(type: { new (): T }, id: string): Observable<T> {
        return this.httpService.deleteById(this.API_URL + "/" + type.name, id).map((r: Response) => <T>r.json());
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