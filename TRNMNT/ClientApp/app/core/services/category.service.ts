import { Injectable } from "@angular/core"
import { LoggerService } from "./logger.service"
import { HttpService, SearchParams } from "./../dal/http/http.service"
import { CategoryModel, CategorySimpleModel } from "./../model/category.models"
import { ApiMethods } from "./../dal/consts/api-methods.consts"
import { Observable } from 'rxjs/Rx';


@Injectable()
export class CategoryService {
    constructor(private loggerService: LoggerService, private httpService: HttpService) {

    }

    public getCategories(eventId: string): Observable<CategoryModel[]> {
        let params: SearchParams[] = [{ name: "eventId", value: eventId }];
        return this.httpService.get(ApiMethods.category.category, params).map(res => this.httpService.getArray<CategoryModel>(res));
    }

    public getCategoriesForEvent(): Observable<CategorySimpleModel[]> {
        return this.httpService.get(ApiMethods.category.getCategoriesForEvent).map(res => this.httpService.getArray<CategorySimpleModel>(res));
    }
   
}