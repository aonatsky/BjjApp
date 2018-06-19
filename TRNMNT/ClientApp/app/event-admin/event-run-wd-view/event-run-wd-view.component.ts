import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BracketModel } from '../../core/model/bracket.models';
import { RunEventHubService } from '../../core/hubservices/run-event.hub.serive';
import { BracketService } from '../../core/services/bracket.service';
import { DefaultValues } from '../../core/consts/default-values';
import { MatchModel } from '../../core/model/match.models';


@Component({
    selector: 'event-run-wd-view',
    templateUrl: './event-run-wd-view.component.html',
})
export class EventRunWeightDivisionViewComponent implements OnInit {

    private bracket: BracketModel;

    private selectedRoundDetails: MatchModel;
    private showRoundPanel: boolean;
    private previousWeightDivisionId: string;

    constructor(
        private route: ActivatedRoute,
        private bracketService: BracketService,
        private runEventHubService: RunEventHubService,
        private cdRef: ChangeDetectorRef) {
    }

    ngOnInit() {
        this.runEventHubService.onRoundComplete().subscribe((model) => {
            this.bracket = model.bracket;
            this.showRoundPanel = false;
        });
        this.runEventHubService.onRoundStart().subscribe(x => {
            this.selectedRoundDetails = x;
            this.showRoundPanel = true;
        });
        this.runEventHubService.onWeightDivisionChange().subscribe(refreshModel => {
            this.runEventHubService.joinWeightDivisionGroup(refreshModel.weightDivisionId, this.previousWeightDivisionId);
            this.refreshModel(refreshModel.bracket);
            this.previousWeightDivisionId = refreshModel.weightDivisionId;
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
