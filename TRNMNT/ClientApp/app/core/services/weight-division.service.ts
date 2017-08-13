import { Injectable } from "@angular/core"
import { LoggerService } from "./logger.service"
import { HttpService, SearchParams } from "./../dal/http/http.service"
import { WeightDivisionModel } from "./../model/weight-division.model"
import { ApiMethods } from "./../dal/consts/api-methods.consts"
import { Observable } from 'rxjs/Rx';


@Injectable()
export class WeightDivisionService {
    constructor(private loggerService: LoggerService, private httpService: HttpService) {

    }

    public getWeightDivisions(categoryId: string): Observable<WeightDivisionModel[]> {
        let params: SearchParams[] = [{ name: "categoryId", value: categoryId }];
        return this.httpService.get(ApiMethods.weightDivision.weightDivision, params).map(res => this.httpService.getArray<WeightDivisionModel>(res));
    }

    //public getNewWeightDivision(): Observable<WeightDivisionModel> {
    //    return this.httpService.post(ApiMethods.weightDivision)
    //}
}