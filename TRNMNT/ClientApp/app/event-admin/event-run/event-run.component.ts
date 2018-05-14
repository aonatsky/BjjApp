import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { LoggerService } from './../../core/services/logger.service';
import './event-run.component.scss'
import { BracketModel, ChageWeightDivisionModel } from '../../core/model/bracket.models';
import { BracketService } from '../../core/services/bracket.service';
import { CategoryWithDivisionFilterModel } from '../../core/model/category-with-division-filter.model';
import { RunEventHubService } from '../../core/hubservices/run-event.hub.serive';
import { RouterService } from '../../core/services/router.service';
import { RoundModel } from '../../core/model/round.models';
import { v4 as uuid } from 'uuid';
import { DefaultValues } from '../../core/consts/default-values';

@Component({
    selector: 'event-run',
    templateUrl: './event-run.component.html',
})
export class EventRunComponent implements OnInit, OnDestroy {

    private eventId: string;
    private bracket: BracketModel;
    private filter: CategoryWithDivisionFilterModel;
    private previousWeightDivisionId: string;
    private selectedRoundDetails: RoundModel;
    private showRoundPanel: boolean;
    private syncronizationId: AAGUID;

    private get isFilterSelected(): boolean {
        return !!this.filter && !!this.filter.weightDivisionId;
    }

    private get isCategorySelected(): boolean {
        return !!this.filter && !!this.filter.categoryId;
    }

    constructor(
        private loggerService: LoggerService,
        private bracketService: BracketService,
        private route: ActivatedRoute,
        private routerService: RouterService,
        private runEventHubService: RunEventHubService) {

    }

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
        this.syncronizationId = uuid();
        this.runEventHubService.joinOperatorGroup(this.syncronizationId);
    }

    private filterSelected($event: CategoryWithDivisionFilterModel) {
        this.filter = $event;
        this.bracket = null;
        if (this.selectedRoundDetails) {
            this.selectedRoundDetails.weightDivisionId = this.filter.weightDivisionId;
            this.runWeightDivision();       
        }
    }

    private runWeightDivision() {
        localStorage.setItem(`${DefaultValues.RunEventSyncIdPart}${this.syncronizationId}`, this.filter.weightDivisionId);

        this.runEventHubService.joinWeightDivisionGroup(this.filter.weightDivisionId, this.previousWeightDivisionId)
            .then(() => {
                const model = new ChageWeightDivisionModel();
                model.weightDivisionId = this.filter.weightDivisionId;
                model.syncronizationId = this.syncronizationId;
                this.runEventHubService.fireWeightDivisionChange(model);
            });

        this.bracketService.runBracket(this.filter.weightDivisionId).subscribe(m => this.refreshModel(m));
        this.previousWeightDivisionId = this.filter.weightDivisionId;
    }

    private runWeightDivisionSpectatorView() {
        this.routerService.openEventWeightDivisionSpactatorView(this.syncronizationId);
    }

    private runCategorySpectatorView() {
        this.routerService.openEventCategorySpactatorView(this.filter.categoryId);
    }

    private completeRound() {
        this.showRoundPanel = false;
        this.selectedRoundDetails = undefined;
        this.runEventHubService.fireRoundComplete(this.filter.weightDivisionId);
    }

    private cancelRound() {
        this.selectedRoundDetails = undefined;
    }

    private refreshModel(model: BracketModel) {
        this.bracket = model;
        console.log("RECIEVED", model);
    }

    private runRound(model: RoundModel) {
        console.log(model);
        this.selectedRoundDetails = model;
        this.selectedRoundDetails.weightDivisionId = this.filter.weightDivisionId;
        this.runEventHubService.fireRoundStart(model);
        this.showRoundPanel = true;
    }

    ngOnDestroy(): void {
        localStorage.removeItem(`${DefaultValues.RunEventSyncIdPart}${this.syncronizationId}`);
    }
}
