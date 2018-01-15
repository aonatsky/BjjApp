import { Injectable } from "@angular/core"
import { LoggerService } from "./logger.service"
import { HttpService, SearchParams } from "./../dal/http/http.service"
import { WeightDivisionModel, WeightDivisionSimpleModel } from "./../model/weight-division.models"
import { ApiMethods } from "./../dal/consts/api-methods.consts"
import { Observable } from 'rxjs/Rx';


@Injectable()
export class WeightDivisionService {
    constructor(private loggerService: LoggerService, private httpService: HttpService) {

    }

    public getWeightDivisions(categoryId: string): Observable<WeightDivisionModel[]> {
        let params = { categoryId: categoryId };
        return this.httpService.get(ApiMethods.weightDivision.weightDivision, params).map(res => this.httpService.getArray<WeightDivisionModel>(res));
    }

    public getWeightDivisionsByEvent(eventId: string): Observable<WeightDivisionModel[]> {
        return this.httpService.get(ApiMethods.weightDivision.getWeightDivisionsByEvent + "/" + eventId).map(res => this.httpService.getArray<WeightDivisionModel>(res));
    }

    public getWeightDivisionsByCategory(categoryId: string): Observable<WeightDivisionSimpleModel[]> {
        return this.httpService.get(ApiMethods.weightDivision.getWeightDivisionsByCategory + "/" + categoryId).map(res => this.httpService.getArray<WeightDivisionSimpleModel>(res));
    }

    //public getNewWeightDivision(): Observable<WeightDivisionModel> {
    //    return this.httpService.post(ApiMethods.weightDivision)
    //}
}