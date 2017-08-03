import { Injectable } from "@angular/core"
import { LoggerService } from "./logger.service"
import { HttpService } from "./../dal/http/http.service"
import { WeightDivisionModel } from "./../model/weight-division.model"
import { ApiMethods } from "./../dal/consts/api-methods.consts"
import { Observable } from 'rxjs/Rx';


@Injectable()
export class WeightDivisionService {
    constructor(private loggerService: LoggerService, private httpService: HttpService) {

    }

    public getWeightDivisions(categoryId: string): Observable<WeightDivisionModel[]> {
        return this.httpService.get(ApiMethods.weightDivision.weightDivision).map(res => this.httpService.getArray<WeightDivisionModel>(res));
    }
}