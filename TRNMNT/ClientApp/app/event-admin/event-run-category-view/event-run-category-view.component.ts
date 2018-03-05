import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BracketArrayModel, RefreshBracketModel} from '../../core/model/bracket.models';
import { BracketService } from '../../core/services/bracket.service';
import { WeightDivisionService } from '../../core/services/weight-division.service';
import { RunEventHubService } from '../../core/hubservices/run-event.hub.serive';
import { WeightDivisionSimpleModel } from '../../core/model/weight-division.models';


@Component({
    selector: 'event-run-category-view',
    templateUrl: './event-run-category-view.component.html',
})
export class EventRunCategoryViewComponent implements OnInit {

    private categoryId: string;
    private weightDivisions: WeightDivisionSimpleModel[];
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
            this.copyArrayWithSubstitution(model);
        });
        this.route.params.subscribe(p => {
            this.categoryId = p['id'];
            this.weightDivisionService.getWeightDivisionsByCategory(this.categoryId).subscribe((data) => this.weightDivisions = data);
            this.bracketService.getBracketsByCategory(this.categoryId).subscribe((data) => {
                this.bracketsList = data;
                this.runEventHubService.joinMultipleWeightDivisionGroups(Object.keys(this.bracketsList));
            });
        });
    }

    private copyArrayWithSubstitution(model: RefreshBracketModel) {
        let newObj = {};
        let bracketArrayModel = this.bracketsList;
        for (let weightDivisionId in bracketArrayModel) {
            if (bracketArrayModel.hasOwnProperty(weightDivisionId)) {

                if (weightDivisionId.toUpperCase() == model.weightDivisionId.toUpperCase()) {
                    newObj[model.weightDivisionId] = model.bracket;
                } else {
                    newObj[weightDivisionId] = this.bracketsList[weightDivisionId];
                }
            }
        }
        this.bracketsList = newObj;
    }

    private getDivisionNameById(id: string) : string {
        let model = this.weightDivisions.find((d) => d.weightDivisionId == id);
        if (!!model) {
            return model.name;
        }
        return '';
    }
}
