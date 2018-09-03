import { Injectable } from '@angular/core';
import { ResponseContentType } from '@angular/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ApiMethods } from '../dal/consts/api-methods.consts';
import { HttpService } from '../dal/http/http.service';
import { BracketResultModel } from '../model/bracket-result.model';
import { BracketArrayModel, BracketModel } from '../model/bracket.models';
import { MatchResultModel } from '../model/match-result.model';
import { ParticipantInAbsoluteDivisionModel } from '../model/participant.models';
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

  downloadBracket(weightDivisionId: string, fileName: string): Observable<any> {
    return this.httpService.getExcelFile1(ApiMethods.bracket.downloadFile + '/' + weightDivisionId, fileName);
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

  getWinnersByCategory(categoryId): Observable<ParticipantInAbsoluteDivisionModel[]> {
    return this.httpService.get(ApiMethods.bracket.getParticipantsForAbsoluteDivision + '/' + categoryId);
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

  areBracketsCreated(eventId: AAGUID): Observable<boolean> {
    return this.httpService.get<boolean>(ApiMethods.bracket.areBracketsCreated + '/' + eventId);
  }

  createBrackets(eventId: AAGUID): Observable<boolean> {
    return this.httpService.post(ApiMethods.bracket.createBrackets + '/' + eventId);
  }

  deleteBrackets(eventId: AAGUID): Observable<boolean> {
    return this.httpService.post(ApiMethods.bracket.deleteBrackets + '/' + eventId);
  }
}
