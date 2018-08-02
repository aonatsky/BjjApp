import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { LoggerService } from '../../core/services/logger.service';
import './event-run.component.scss';
import { BracketModel, ChageWeightDivisionModel } from '../../core/model/bracket.models';
import { BracketService } from '../../core/services/bracket.service';
import { CategoryWithDivisionFilterModel } from '../../core/model/category-with-division-filter.model';
import { RunEventHubService } from '../../core/hubservices/run-event.hub.service';
import { RouterService } from '../../core/services/router.service';
import { v4 as uuid } from 'uuid';
import { DefaultValues } from '../../core/consts/default-values';
import { MatchModel } from '../../core/model/match.models';
import { StorageService } from '../../core/services/storage.service';

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
  selectedRoundDetails: MatchModel;
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
    private runEventHubService: RunEventHubService,
    private storageService : StorageService
  ) {}

  ngOnInit() {
    this.route.params.subscribe(p => {
      this.eventId = p['id'];
    });
    this.runEventHubService.onRoundComplete().subscribe(m => {
      this.refreshModel(m.bracket);
      this.showRoundPanel = false;
    });
    this.runEventHubService.onRoundStart().subscribe(x => {
      this.selectedRoundDetails = x;
      this.showRoundPanel = true;
    });
    if (sessionStorage.getItem(DefaultValues.RunEventSessionId) == null) {
      sessionStorage.setItem(DefaultValues.RunEventSessionId, uuid());
    }
    this.runEventHubService.joinOperatorGroup(this.synchronizationId);
  }

  filterSelected($event: CategoryWithDivisionFilterModel) {
    this.filter = $event;
    this.bracket = null;
    if (this.selectedRoundDetails) {
      this.selectedRoundDetails.weightDivisionId = this.filter.weightDivisionId;
      this.runWeightDivision();
    }
  }

  private runWeightDivision() {
    localStorage.setItem(`${DefaultValues.RunEventSyncIdPart}${this.synchronizationId}`, this.filter.weightDivisionId);
    this.storageService.store("run","test")

    this.runEventHubService
      .joinWeightDivisionGroup(this.filter.weightDivisionId, this.previousWeightDivisionId)
      .then(() => {
        const model = new ChageWeightDivisionModel();
        model.weightDivisionId = this.filter.weightDivisionId;
        model.synchronizationId = this.synchronizationId;
        this.runEventHubService.fireWeightDivisionChange(model);
      });

    this.bracketService.runBracket(this.filter.weightDivisionId).subscribe(m => this.refreshModel(m));
    this.previousWeightDivisionId = this.filter.weightDivisionId;
  }

  runWeightDivisionSpectatorView() {
    this.routerService.openEventWeightDivisionSpectatorView();
  }

  runCategorySpectatorView() {
    this.routerService.openEventCategorySpectatorView(this.filter.categoryId);
  }

  completeRound() {
    this.showRoundPanel = false;
    this.selectedRoundDetails = undefined;
    this.filterRefreshTrigger += 1;
    this.runEventHubService.fireRoundComplete(this.filter.weightDivisionId);
  }

  private showResultSetPopup() {
    this.showResultPopup = true;
  }

  private hideResultSetPopup() {
    this.showResultPopup = false;
  }

  private cancelRound() {
    this.selectedRoundDetails = undefined;
  }

  private refreshModel(model: BracketModel) {
    this.bracket = model;
    console.log('RECIEVED', model);
  }

  runMatch(model: MatchModel) {
    console.log(model);
    this.selectedRoundDetails = model;
    this.selectedRoundDetails.weightDivisionId = this.filter.weightDivisionId;
    this.runEventHubService.fireRoundStart(model);
    this.showRoundPanel = true;
  }

  ngOnDestroy(): void {
    localStorage.removeItem(`${DefaultValues.RunEventSyncIdPart}${this.synchronizationId}`);
  }
}
