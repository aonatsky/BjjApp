import { Injectable } from '@angular/core';
import { StorageService } from '../services/storage.service';
import { Observable } from 'rxjs';
import { RefreshBracketModel } from '../model/bracket.models';
import { filter, map } from 'rxjs/operators';
import { MatchModel } from '../model/match.models';
import { MatchDetailsModel } from '../model/match-details.model';

@Injectable()
export class EventRunCommunicationService {
  constructor(private storageService: StorageService) {}

  private bracketChangeKeyName: string = 'BracketChanged';

  fireBracketChange(bracketRefreshModel: RefreshBracketModel) {
    this.storageService.store(this.bracketChangeKeyName, bracketRefreshModel);
  }

  clearBracket() {
    this.storageService.clear(this.bracketChangeKeyName);
  }

  onBracketChange(): Observable<RefreshBracketModel> {
    return this.storageService.observeByKey<RefreshBracketModel>(this.bracketChangeKeyName);
  }

  getCurrentBracket(): RefreshBracketModel {
    return this.storageService.getObject(this.bracketChangeKeyName);
  }

  private matchStartEventName = 'MatchStarted';
  private matchCompleteEventName = 'MatchCompleted';
  private matchDetailsUpdated = 'MatchDetailsUpdated';

  fireMatchStart(matchDetails: MatchModel): void {
    this.storageService.store(this.matchStartEventName, matchDetails);
  }

  fireMatchCompleted(): void {
    this.storageService.clear(this.matchStartEventName);
    this.storageService.store(this.matchCompleteEventName, { date: Date.now() });
  }

  onMatchStart(): Observable<MatchModel> {
    return this.storageService.observeByKey<MatchModel>(this.matchStartEventName);
  }

  onMatchComplete(): Observable<any> {
    return this.storageService.observeByKey(this.matchCompleteEventName);
  }

  fireMatchDetailsUpdate(matchDetails: MatchDetailsModel): any {
    this.storageService.store(this.matchDetailsUpdated, matchDetails);
  }

  onMatchDetailsUpdate(): Observable<any> {
    return this.storageService.observeByKey(this.matchDetailsUpdated);
  }
}
