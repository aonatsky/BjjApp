import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BracketModel } from '../../core/model/bracket.models';
import { RunEventHubService } from '../../core/hubservices/run-event.hub.serive';
import { BracketService } from '../../core/services/bracket.service';
import { RoundModel } from '../../core/model/round.models';
import { DefaultValues } from '../../core/consts/default-values';


@Component({
    selector: 'event-run-wd-view',
    templateUrl: './event-run-wd-view.component.html',
})
export class EventRunWeightDivisionViewComponent implements OnInit {

    private bracket: BracketModel;

    private selectedRoundDetails: RoundModel;
    private showRoundPanel: boolean;
    private previousWeightDivisionId: string;

    constructor(
        private route: ActivatedRoute,
        private bracketService: BracketService,
        private runEventHubService: RunEventHubService) {
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
        this.startSubscriptionFromRouteId();
    }

    private startSubscriptionFromRouteId() {
        this.route.params.subscribe(p => {
            const id = p['id'];
            this.startSubscription(id);
        });
    }

    private startSubscription(syncronizationId) {
        if (syncronizationId != null) {
            this.runEventHubService.joinOperatorGroup(syncronizationId);
            const weightDivisionId = localStorage.getItem(`${DefaultValues.RunEventSyncIdPart}${syncronizationId}`);
            this.bracketService.getBracket(weightDivisionId).subscribe(model => this.refreshModel(model));
        }
    }

    private refreshModel(model: BracketModel) : void {
        this.bracket = model;
    }
}
