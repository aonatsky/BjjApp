import { Injectable } from "@angular/core"
import { LoggerService } from "./logger.service"
import { HttpService } from "./../dal/http/http.service"
import { CategorySimpleModel } from "./../model/category.models"
import { ApiMethods } from "./../dal/consts/api-methods.consts"
import { Observable } from 'rxjs/Rx';


@Injectable()
export class CategoryService {
    constructor(private loggerService: LoggerService, private httpService: HttpService) {

    }

    getCategoriesByEventId(eventId: string): Observable<CategorySimpleModel[]> {
        return this.httpService.get(ApiMethods.category.getCategoriesByEventId + "/" + eventId).map(res => this.httpService.getArray<CategorySimpleModel>(res));
    }

    getCategoriesForCurrentEvent(): Observable<CategorySimpleModel[]> {
        return this.httpService.get(ApiMethods.category.getCategoriesForCurrentEvent).map(res => this.httpService.getArray<CategorySimpleModel>(res));
    }
   
}