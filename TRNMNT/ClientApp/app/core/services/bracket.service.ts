import { Injectable } from '@angular/core';
import { LoggerService } from './logger.service';
import { HttpService } from '../dal/http/http.service';
import { Observable } from 'rxjs';

import { ApiMethods } from '../dal/consts/api-methods.consts';
import { BracketModel, BracketArrayModel } from '../model/bracket.models';
import { ResponseContentType } from '@angular/http';
import { ParticipantSmallTableModel } from '../model/participant.models';


@Injectable()
export class BracketService {
    constructor(private loggerService: LoggerService, private httpService: HttpService) {

    }

    getBracket(weightDivisionId): Observable<BracketModel> {
        return this.httpService.get(ApiMethods.bracket.createBracket + '/' + weightDivisionId)
            .map(res => this.httpService.getJson(res));
    }

    downloadBracket(weightDivisionId: string, fileName: string) {
        return this.httpService.get(ApiMethods.bracket.downloadFile + '/' + weightDivisionId, null, ResponseContentType.Blob)
            .map(r => this.httpService.getExcelFile(r, fileName));
    }

    updateBracket(model: BracketModel): Observable<void> {
        return this.httpService.post(ApiMethods.bracket.updateBracket, model);
    }

    finishRound(weightDivisionId: string): Observable<void> {
        return this.httpService.post(ApiMethods.bracket.finishRound, weightDivisionId);
    }

    getBracketsByCategory(categoryId): Observable<BracketArrayModel> {
        return this.httpService.get(ApiMethods.bracket.getBracketsByCategory + '/' + categoryId)
            .map(res => this.httpService.getJson(res));
    }

    getWinnersByCategory(categoryId): Observable<ParticipantSmallTableModel[]> {
        return this.httpService.get(ApiMethods.bracket.getWinners + '/' + categoryId)
            .map(res => this.httpService.getJson(res));
    }
}