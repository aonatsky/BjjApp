import { Injectable } from '@angular/core'
import { LoggerService } from './logger.service'
import { HttpService } from './../dal/http/http.service'
import { CategorySimpleModel } from './../model/category.models'
import { ApiMethods } from './../dal/consts/api-methods.consts'
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';


@Injectable()
export class CategoryService {
    constructor(private loggerService: LoggerService, private httpService: HttpService) {

    }

    getCategoriesByEventId(eventId: string): Observable<CategorySimpleModel[]> {
        return this.httpService.get(ApiMethods.category.getCategoriesByEventId + '/' + eventId).pipe(map(res => this.httpService.getArray<CategorySimpleModel>(res)));
    }

    getCategoriesForCurrentEvent(): Observable<CategorySimpleModel[]> {
        return this.httpService.get(ApiMethods.category.getCategoriesForCurrentEvent).pipe(map(res => this.httpService.getArray<CategorySimpleModel>(res)));
    }
   
}