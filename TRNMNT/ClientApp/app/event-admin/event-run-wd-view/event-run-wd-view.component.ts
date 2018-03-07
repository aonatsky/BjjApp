import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import {BracketModel} from '../../core/model/bracket.models';
import { RunEventHubService } from '../../core/hubservices/run-event.hub.serive';
import { BracketService } from '../../core/services/bracket.service';


@Component({
    selector: 'event-run-wd-view',
    templateUrl: './event-run-wd-view.component.html',
})
export class EventRunWeightDivisionViewComponent implements OnInit {

    private weightDivisionId: string;
    private bracket: BracketModel;

    constructor(
        private route: ActivatedRoute,
        private bracketService: BracketService,
        private runEventHubService: RunEventHubService) {

    }

    ngOnInit() {
        this.runEventHubService.onRefreshRound().subscribe((model) => this.bracket = model.bracket);
        this.startSubscriptionFromRouteId(); 
    }

    private startSubscriptionFromRouteId() {
        this.route.params.subscribe(p => {
            let id = p['id'];
            if (id != null) {
                this.weightDivisionId = id;
                this.startSubscription();
            }
        });
    }

    private startSubscription() {
        if (this.weightDivisionId != null) {
            this.runEventHubService.joinWeightDivisionGroup(this.weightDivisionId);
            this.bracketService.getBracket(this.weightDivisionId).subscribe((model) => this.bracket = model);
        }
    }
}
