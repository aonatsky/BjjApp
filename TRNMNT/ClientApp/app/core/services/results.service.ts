import { Injectable } from '@angular/core';
import { LoggerService } from './logger.service';
import { HttpService } from '../dal/http/http.service';
import { TeamResultModel } from '../model/team-result.model';
import { Observable } from 'rxjs';
import { ApiMethods } from '../dal/consts/api-methods.consts';
import { ResponseContentType } from '@angular/http';
    
@Injectable()
export class ResultsService {
    constructor(private loggerService: LoggerService, private httpService: HttpService) {

    }

    getTeamResults(categoryIds: string[]): Observable<TeamResultModel[]> {
        return this.httpService.post(ApiMethods.results.getTeamResutls, categoryIds).map(r => this.httpService.getArray(r));
    }

    getPersonalResultsFile(categoryIds: string[], fileName: string) {
        return this.httpService.post(ApiMethods.results.getPersonalResultsFile, categoryIds, ResponseContentType.Blob)
            .map(r => this.httpService.getExcelFile(r, fileName));
    }
}