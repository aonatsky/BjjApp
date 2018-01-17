import { Injectable } from '@angular/core';
import { LoggerService } from './logger.service';
import { HttpService } from '../dal/http/http.service';
import { Observable } from 'rxjs';

import { ApiMethods } from '../dal/consts/api-methods.consts';
import {BracketModel} from '../model/bracket.models';

@Injectable()
export class BracketService {
    constructor(private loggerService: LoggerService, private httpService: HttpService) {

    }

    createBracket(weightDivisionId): Observable<BracketModel> {
        return this.httpService.get(ApiMethods.bracket.createBracket + '/' + weightDivisionId)
            .map(res => this.httpService.getJson(res));
    }
}