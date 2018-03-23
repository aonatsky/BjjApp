import { Injectable } from '@angular/core';
import { LoggerService } from './logger.service';
import { HttpService } from '../dal/http/http.service';
import { Observable } from 'rxjs';

import { ApiMethods } from '../dal/consts/api-methods.consts';
import { BracketModel, BracketArrayModel } from '../model/bracket.models';
import { ResponseContentType } from '@angular/http';


@Injectable()
export class ResultService {
    constructor(private loggerService: LoggerService, private httpService: HttpService) {

    }

    getTeamResults(): Observable<> {

    }
}