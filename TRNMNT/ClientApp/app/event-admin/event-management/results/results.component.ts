import { Component, OnInit } from '@angular/core';
import { ViewEncapsulation } from '@angular/core';
import { Input } from '@angular/core';
import { CategorySimpleModel } from '../../../core/model/category.models';
import { CategoryService } from '../../../core/services/category.service';
import { TeamResultModel } from '../../../core/model/team-result.model';
import {ResultsService} from '../../../core/services/results.service';

@Component({
    selector: 'results',
    templateUrl: 'results.component.html',
    encapsulation: ViewEncapsulation.None
})

export class ResultsComponent {
    @Input() eventId: string;
    private categories: CategorySimpleModel[] = [];
    private selectedCategories: string[] = [];
    private teamResults: TeamResultModel[];
    
    constructor(private categoryService: CategoryService, private resultsService: ResultsService) {

    }

    ngOnInit() {
        this.categoryService.getCategoriesByEventId(this.eventId).subscribe(r => {
            this.categories = r;
        });
    }
    
    getResults() {
        debugger;
        this.resultsService.getTeamResults(this.selectedCategories).subscribe(r => { this.teamResults = r; });
    }

}

