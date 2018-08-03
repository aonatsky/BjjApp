import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { LoggerService } from '../../core/services/logger.service';
import './event-run.component.scss';
import { BracketModel, ChangeWeightDivisionModel, RefreshBracketModel } from '../../core/model/bracket.models';
import { BracketService } from '../../core/services/bracket.service';
import { CategoryWithDivisionFilterModel } from '../../core/model/category-with-division-filter.model';

import { RouterService } from '../../core/services/router.service';
import { v4 as uuid } from 'uuid';
import { DefaultValues } from '../../core/consts/default-values';
import { MatchModel } from '../../core/model/match.models';
import { EventRunCommunicationService } from '../../core/hubservices/event-run.communication.service';

@Component({
  selector: 'event-run',
  templateUrl: './event-run.component.html'
})
export class EventRunComponent implements OnInit, OnDestroy {
  showResultPopup: boolean = false;
  eventId: string;
  bracket: BracketModel;
  private filter: CategoryWithDivisionFilterModel;
  private previousWeightDivisionId: string;
  selectedMatch: MatchModel;
  showRoundPanel: boolean;
  filterRefreshTrigger: number = 0;

  private get isFilterSelected(): boolean {
    return !!this.filter && !!this.filter.weightDivisionId;
  }

  private get isCategorySelected(): boolean {
    return !!this.filter && !!this.filter.categoryId;
  }

  private get synchronizationId(): AAGUID {
    return sessionStorage.getItem(DefaultValues.RunEventSessionId);
  }

  constructor(
    private loggerService: LoggerService,
    private bracketService: BracketService,
    private route: ActivatedRoute,
    private routerService: RouterService,
    
    private runEventCommunicationService: EventRunCommunicationService
  ) {}

  ngOnInit() {
    this.route.params.subscribe(p => {
      this.eventId = p['id'];
    });
    this.runEventCommunicationService.clearBracket();
    if (sessionStorage.getItem(DefaultValues.RunEventSessionId) == null) {
      sessionStorage.setItem(DefaultValues.RunEventSessionId, uuid());
    }
  }

  filterSelected($event: CategoryWithDivisionFilterModel) {
    this.filter = $event;
    this.bracket = null;
    if (this.selectedMatch) {
      this.selectedMatch.weightDivisionId = this.filter.weightDivisionId;
      this.runWeightDivision();
    }
  }

  private runWeightDivision() {
    this.bracketService.runBracket(this.filter.weightDivisionId).subscribe(m => {
      this.refreshModel(m);
      this.runEventCommunicationService.fireBracketChange({
        bracket: m,
        weightDivisionId: this.filter.weightDivisionId
      });
    });
    this.previousWeightDivisionId = this.filter.weightDivisionId;
  }

  runWeightDivisionSpectatorView() {
    this.routerService.openEventWeightDivisionSpectatorView();
  }

  runCategorySpectatorView() {
    this.routerService.openEventCategorySpectatorView(this.filter.categoryId);
  }

  private showResultSetPopup() {
    this.showResultPopup = true;
  }

  private hideResultSetPopup() {
    this.showResultPopup = false;
  }

  private cancelRound() {
    this.selectedMatch = undefined;
  }

  private refreshModel(model: BracketModel) {
    this.bracket = model;
    console.log('RECIEVED', model);
  }

  runMatch(model: MatchModel) {
    this.selectedMatch = model;
    this.selectedMatch.weightDivisionId = this.filter.weightDivisionId;
    this.runEventCommunicationService.fireMatchStart(model);
    this.showRoundPanel = true;
  }

  completeMatch() {
    this.showRoundPanel = false;
    this.selectedMatch = undefined;
    this.filterRefreshTrigger += 1;
    this.runEventCommunicationService.fireMatchCompleted();
    this.runWeightDivision();
  }

  ngOnDestroy(): void {
    localStorage.removeItem(`${DefaultValues.RunEventSyncIdPart}${this.synchronizationId}`);
  }
}
