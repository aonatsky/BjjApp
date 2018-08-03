import { Injectable } from '@angular/core';
import { StorageService } from '../services/storage.service';
import { Observable } from 'rxjs';
import { RefreshBracketModel } from '../model/bracket.models';
import { filter, map } from 'rxjs/operators';
import { MatchModel } from '../model/match.models';

@Injectable()
export class RunEventCommunicationService {
  constructor(private storageService: StorageService) {}

  private bracketChangeKeyName: string = 'BracketChanged';

  fireBracketChange(bracketRefreshModel: RefreshBracketModel) {
    this.storageService.store(this.bracketChangeKeyName, bracketRefreshModel);
  }

  onBracketChange(): Observable<RefreshBracketModel> {
    return this.storageService.changes.pipe(
      filter(data => data.key === this.bracketChangeKeyName),
      map(data => data.value)
    );
  }

  getCurrentBracket(): RefreshBracketModel {
    return this.storageService.getObject(this.bracketChangeKeyName);
  }

  matchStartEventName = 'MatchStarted';
  matchCompleteEventName = 'MatchCompleted';

  fireMatchStart(matchDetails: MatchModel): void {
    this.storageService.store(this.matchStartEventName, matchDetails);
  }

  fireMatchCompleted(): void {
    this.storageService.clear(this.matchCompleteEventName);
    this.storageService.store(this.matchCompleteEventName, {date: Date.now()});
  }

  onMatchStart(): Observable<MatchModel> {
    return this.storageService.changes.pipe(
      filter(data => data.key === this.matchStartEventName),
      map(data => data.value)
    );
  }

  onMatchComplete(): Observable<any> {
    return this.storageService.changes.pipe(
      filter(data => data.key === this.matchCompleteEventName),
      map(m => m)
    );
  }
}
