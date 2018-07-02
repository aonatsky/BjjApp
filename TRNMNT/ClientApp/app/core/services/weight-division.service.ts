import { Injectable } from '@angular/core'
import { LoggerService } from './logger.service'
import { HttpService, SearchParams } from './../dal/http/http.service'
import { WeightDivisionModel, WeightDivisionSimpleModel } from './../model/weight-division.models'
import { ApiMethods } from './../dal/consts/api-methods.consts'
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';


@Injectable()
export class WeightDivisionService {
    constructor(private loggerService: LoggerService, private httpService: HttpService) {
    }

    getWeightDivisionsByEvent(eventId: string): Observable<WeightDivisionModel[]> {
        return this.httpService.get<WeightDivisionModel[]>(ApiMethods.weightDivision.getWeightDivisionsByEvent +
            '/' +
            eventId);
    }

    getWeightDivisionsByCategory(categoryId: string): Observable<WeightDivisionSimpleModel[]> {
        return this.httpService.get<WeightDivisionModel[]>(ApiMethods.weightDivision.getWeightDivisionsByCategory +
            '/' +
            categoryId);
    }
}