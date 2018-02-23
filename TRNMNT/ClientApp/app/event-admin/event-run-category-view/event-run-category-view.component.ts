import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BracketArrayModel} from '../../core/model/bracket.models';
import { BracketService } from '../../core/services/bracket.service';
import { WeightDivisionService } from '../../core/services/weight-division.service';


@Component({
    selector: 'event-run-category-view',
    templateUrl: './event-run-category-view.component.html',
})
export class EventRunCategoryViewComponent implements OnInit {

    private categoryId: string;
    private weightDivisions: string[] = [];
    private bracket: BracketArrayModel;

    constructor(
        private route: ActivatedRoute,
        private bracketService: BracketService,
        private weightDivisionService: WeightDivisionService) {

    }

    ngOnInit() {
        this.route.params.subscribe(p => {
            this.categoryId = p['id'];
            this.weightDivisionService.getWeightDivisionsByCategory(this.categoryId).subscribe((data) => {
                this.weightDivisions = data.map(w => w.weightDivisionId);
            });
        });
    }
}
