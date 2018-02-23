import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BracketArrayModel} from '../../core/model/bracket.models';
import { BracketService } from '../../core/services/bracket.service';
import { WeightDivisionService } from '../../core/services/weight-division.service';
import { RunEventHubService } from '../../core/hubservices/run-event.hub.serive';


@Component({
    selector: 'event-run-category-view',
    templateUrl: './event-run-category-view.component.html',
})
export class EventRunCategoryViewComponent implements OnInit {

    private categoryId: string;
    private weightDivisions: string[] = [];
    private bracketsList: BracketArrayModel;

    constructor(
        private route: ActivatedRoute,
        private bracketService: BracketService,
        private weightDivisionService: WeightDivisionService,
        private runEventHubService: RunEventHubService) {

    }

    ngOnInit() {
        this.runEventHubService.onRefreshRound().subscribe((model) => {
            console.log("RECIEVED", model);
            let newList = this.bracketsList;
            newList[model.weightDivisionId] = model.bracket;
            this.bracketsList = newList;
        });
        this.route.params.subscribe(p => {
            this.categoryId = p['id'];
            this.bracketService.getBracketsByCategory(this.categoryId).subscribe((data) => {
                this.bracketsList = data;
                this.runEventHubService.joinMultipleWeightDivisionGroups(Object.keys(this.bracketsList));
            });
        });
    }
}
