import { Category } from '../../core/model/category.model';

import { WeightDivision } from '../../core/model/weight-division.model';
import { FighterFilterModel } from '../../core/model/fighter-filter.model';
import { Component, Input, Output, OnInit, EventEmitter, ViewEncapsulation } from "@angular/core"
import { DataService } from '../../core/dal/contracts/data.service'
import { DefaultValues } from '../../core/consts/default-values'
import { Observable } from "rxjs/Observable";

import { SelectItem } from 'primeng/primeng'


@Component({
    selector: 'fighter-filter',
    templateUrl: 'fighter-filter.component.html',
    styleUrls: ['fighter-filter.component.css'],
    encapsulation: ViewEncapsulation.None
})

export class FighterFilter implements OnInit {

    weightDivisions: WeightDivision[];
    categories: Category[];

    weightDivisionSelectOptions: SelectItem[] = [];
    categorySelectOptions: SelectItem[] = [];


    @Output() onFilterChanged: EventEmitter<FighterFilterModel>;
    @Output() currentFilterValue: FighterFilterModel;


    constructor(private dataService: DataService) {
        this.onFilterChanged = new EventEmitter<FighterFilterModel>();
    }

    ngOnInit() {
        Observable.forkJoin(this.dataService.getCategories(), this.dataService.getWeightDivisions())
            .subscribe(data => this.initFilter(data))
    }

    //Events
    weightSelect(event) {
        if (event.value == DefaultValues.DROPDOWN_ID_ANY) {
            this.currentFilterValue.weightDivisionIds = this.weightDivisions.map(wd => wd.weightDivisionId);
        }
        else {
            this.currentFilterValue.weightDivisionIds = this.weightDivisions.filter(wd => wd.weightDivisionId == event.value).map(wd => wd.weightDivisionId);
        }
        this.onFilterChanged.emit(this.currentFilterValue);
    }

    categorySelect(event) {
        if (event.value == DefaultValues.DROPDOWN_ID_ANY) {
            this.currentFilterValue.categoryIds = this.categories.map(c => c.categoryId);
        }
        else {
            this.currentFilterValue.categoryIds = this.categories.filter(c => c.categoryId == event.value).map(c => c.categoryId);
        }
        this.onFilterChanged.emit(this.currentFilterValue);
    }

   

    //Private methods


    private initFilter(data: [Category[], WeightDivision[]]) {
        let defaultDDLOption = { label: DefaultValues.DROPDOWN_NAME_ANY, value: DefaultValues.DROPDOWN_ID_ANY };
        this.categories = data[0];
        this.weightDivisions = data[1];
        this.categorySelectOptions.push(defaultDDLOption)
        this.categories.map(c => this.categorySelectOptions.push({ label: c.name, value: c.categoryId }));
        this.weightDivisionSelectOptions.push(defaultDDLOption);
        this.weightDivisions.map(wd => this.weightDivisionSelectOptions.push({ label: wd.name, value: wd.weightDivisionId }))
        this.currentFilterValue = new FighterFilterModel(this.weightDivisions.map(wd => wd.weightDivisionId), this.categories.map(c => c.categoryId));
        this.onFilterChanged.emit(this.currentFilterValue);
    }

}



