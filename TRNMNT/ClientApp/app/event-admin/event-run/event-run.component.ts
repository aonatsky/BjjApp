import { Component, OnInit, Inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { LoggerService } from './../../core/services/logger.service';
import './event-run.component.scss'
import {BracketModel} from '../../core/model/bracket.models';
import {BracketService} from '../../core/services/bracket.service';
import { CategoryWithDivisionFilterModel } from '../../core/model/category-with-division-filter.model';
import { RunEventHubService } from '../../core/hubservices/run-event.hub.serive';
import { RouterService } from '../../core/services/router.service';


@Component({
    selector: 'event-run',
    templateUrl: './event-run.component.html',
})
export class EventRunComponent implements OnInit {

    private eventId: string;
    private bracket: BracketModel;
    private filter: CategoryWithDivisionFilterModel;
    private previousWeightDivisionId: string;

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
        this.runEventHubService.onRefreshRound().subscribe(m => this.refreshModel(m.bracket));
    }

    private filterSelected($event: CategoryWithDivisionFilterModel) {
        this.filter = $event;
        this.bracket = null;
    }

    private runWeightDivision() {
        this.runEventHubService.joinWeightDivisionGroup(this.filter.weightDivisionId, this.previousWeightDivisionId);
        this.bracketService.getBracket(this.filter.weightDivisionId).subscribe(m => this.refreshModel(m));
        this.previousWeightDivisionId = this.filter.weightDivisionId;
    }

    private runWeightDivisionSpectatorView() {
        this.routerService.openEventWeightDivisionSpactatorView(this.filter.weightDivisionId);
    }

    private runCategorySpectatorView() {
        this.routerService.openEventCategorySpactatorView(this.filter.categoryId);
    }

    private finishRound() {
        this.bracketService.finishRound(this.filter.weightDivisionId).subscribe();
    }

    private refreshModel(model: BracketModel) {
        this.bracket = model;
        console.log("RECIEVED", model);
    }

}
