import { Component, OnInit } from '@angular/core';
import { ViewEncapsulation } from '@angular/core';
import { Input } from '@angular/core';
import { CategorySimpleModel } from '../../../core/model/category.models';
import { CategoryService } from '../../../core/services/category.service';
import { TeamResultModel } from '../../../core/model/team-result.model';
import { ResultsService } from '../../../core/services/results.service';
import { ICrudColumn as CrudColumn, IColumnOptions } from '../../../shared/crud/crud.component';

@Component({
    selector: 'results',
    templateUrl: 'results.component.html',
    encapsulation: ViewEncapsulation.None
})
export class ResultsComponent {
    @Input()
    eventId: string;
    private categories: CategorySimpleModel[] = [];
    private selectedCategories: string[] = [];
    private teamResults: TeamResultModel[];
    columns: CrudColumn[] = [
        { propertyName: "teamName", displayName: "Name", isEditable: false, isSortable: false },
        { propertyName: "points", displayName: "Points", isEditable: false, isSortable: true }
    ];
    private readonly pageLinks: number = 3;
    private firstIndex: number = 0;
    private participantsLoading: boolean = true;
    private ddlDataLoading: boolean = true;
    private sortDirection: number = 1;
    private sortField: string = 'teamName';
    private columnOptions: IColumnOptions = {};

    constructor(private categoryService: CategoryService, private resultsService: ResultsService) {

    }

    ngOnInit() {
        this.categoryService.getCategoriesByEventId(this.eventId).subscribe(r => {
            this.categories = r;
        });
    }

    getResults() {
        this.resultsService.getTeamResults(this.selectedCategories).subscribe(r => { this.teamResults = r; });
    }

    get totalCount(): number {
        if (this.teamResults) {
            return this.teamResults.length;
        }
        return 0;
    }

}

