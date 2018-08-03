import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BracketModel } from '../../core/model/bracket.models';
import { RunEventHubService } from '../../core/hubservices/run-event.hub.service';
import { BracketService } from '../../core/services/bracket.service';
import { DefaultValues } from '../../core/consts/default-values';
import { MatchModel } from '../../core/model/match.models';
import { StorageService } from '../../core/services/storage.service';
import { RunEventCommunicationService } from '../../core/hubservices/run-event.communication.service';

@Component({
  selector: 'event-run-wd-view',
  templateUrl: './event-run-wd-view.component.html'
})
export class EventRunWeightDivisionViewComponent implements OnInit {
  bracket: BracketModel;

  selectedRoundDetails: MatchModel;
  showRoundPanel: boolean;
  private previousWeightDivisionId: string;
  sharedObject: string;

  constructor(
    private route: ActivatedRoute,
    private bracketService: BracketService,
    private runEventHubService: RunEventHubService,
    private runEventCommunicationService: RunEventCommunicationService,
    private cdRef: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.runEventHubService.onRoundComplete().subscribe(model => {
      this.bracket = model.bracket;
      this.showRoundPanel = false;
    });
    // this.runEventHubService.onRoundStart().subscribe(x => {
    //   this.selectedRoundDetails = x;
    //   this.showRoundPanel = true;
    // });
    // this.runEventHubService.onWeightDivisionChange().subscribe(refreshModel => {
    //   this.runEventHubService.joinWeightDivisionGroup(refreshModel.weightDivisionId, this.previousWeightDivisionId);
    //   this.refreshModel(refreshModel.bracket);
    //   this.previousWeightDivisionId = refreshModel.weightDivisionId;
    // });
    const currentRefreshModel = this.runEventCommunicationService.getCurrentBracket();
    this.refreshModel(currentRefreshModel.bracket);
    this.previousWeightDivisionId = currentRefreshModel.weightDivisionId;
    this.runEventCommunicationService.onBracketChange().subscribe(refreshModel => {
      this.refreshModel(refreshModel.bracket);
      this.previousWeightDivisionId = refreshModel.weightDivisionId;
    });

    this.runEventCommunicationService.onMatchStart().subscribe(model => {
      this.selectedRoundDetails = model;
      this.showRoundPanel = true;
    });

    this.runEventCommunicationService.onMatchComplete().subscribe(() => {
      this.showRoundPanel = false;
    });
    this.startSubscription();
  }

  private startSubscription() {
    const synchronizationId = sessionStorage.getItem(DefaultValues.RunEventSessionId);
    if (synchronizationId != null) {
      this.runEventHubService.joinOperatorGroup(synchronizationId);
      const weightDivisionId = localStorage.getItem(`${DefaultValues.RunEventSyncIdPart}${synchronizationId}`);
      if (weightDivisionId) {
        this.bracketService.getBracket(weightDivisionId).subscribe(model => this.refreshModel(model));
      }
    }
  }

  private refreshModel(model: BracketModel): void {
    this.bracket = null;
    this.cdRef.detectChanges();
    this.bracket = model;
  }
}
