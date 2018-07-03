import { Injectable } from '@angular/core';
import { ResponseContentType } from '@angular/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ApiMethods } from '../dal/consts/api-methods.consts';
import { HttpService } from '../dal/http/http.service';
import { BracketResultModel } from '../model/bracket-result.model';
import { BracketArrayModel, BracketModel } from '../model/bracket.models';
import { MatchResultModel } from '../model/match-result.model';
import { ParticipantInAbsoluteDivisionMobel } from '../model/participant.models';
import { LoggerService } from './logger.service';

@Injectable()
export class BracketService {
  constructor(private loggerService: LoggerService, private httpService: HttpService) {}

  getBracket(weightDivisionId): Observable<BracketModel> {
    return this.httpService.get(ApiMethods.bracket.createBracket + '/' + weightDivisionId);
  }

  runBracket(weightDivisionId): Observable<BracketModel> {
    return this.httpService.get(ApiMethods.bracket.runBracket + '/' + weightDivisionId);
  }

  downloadBracket(weightDivisionId: string, fileName: string) {
    return this.httpService
      .get(ApiMethods.bracket.downloadFile + '/' + weightDivisionId, null, ResponseContentType.Blob)
      .pipe(map((r) => this.httpService.getExcelFile(r, fileName)));
  }

  updateBracket(model: BracketModel): Observable<void> {
    return this.httpService.post(ApiMethods.bracket.updateBracket, model);
  }

  getBracketsByCategory(categoryId): Observable<BracketArrayModel> {
    return this.httpService.get(ApiMethods.bracket.getBracketsByCategory + '/' + categoryId);
  }

  isCategoryCompleted(categoryId): Observable<boolean> {
    return this.httpService.get(ApiMethods.bracket.isCategoryCompleted + '/' + categoryId);
  }

  getWinnersByCategory(categoryId): Observable<ParticipantInAbsoluteDivisionMobel[]> {
    return this.httpService.get(ApiMethods.bracket.getParticipnatsForAbsoluteDivision + '/' + categoryId);
  }

  manageAbsoluteWeightDivision(participantsIds, categoryId): Observable<void> {
    return this.httpService.post(ApiMethods.bracket.manageAbsoluteWeightDivision, {
      participantsIds,
      categoryId
    });
  }

  setRoundResult(roundResult: MatchResultModel): Observable<void> {
    return this.httpService.post(ApiMethods.bracket.setRoundResult, roundResult);
  }

  setBracketResult(bracketResult: BracketResultModel) {
    return this.httpService.post(ApiMethods.bracket.setBracketResult, bracketResult);
  }
}
