import { Category } from '../../core/model/category.model';

import { WeightDivision } from '../../core/model/weight-division.model';
import { FighterFilterModel } from '../../core/model/fighter-filter.model';
import { Component, Input, Output, OnInit, EventEmitter } from "@angular/core"
import { DropdownComponent, DropDownListOption } from '../dropdown/dropdown.component'
import { DataService } from '../../core/dal/contracts/data.service'
import { DefaultValues } from '../../core/consts/default-values'
import { Observable } from "rxjs/Observable";


@Component({
    selector: 'fighter-filter',
    templateUrl: 'fighter-filter.component.html',
    styleUrls: ['fighter-filter.component.css']
})

export class FighterFilter implements OnInit {

    weightDivisions: WeightDivision[];
    categories: Category[];

    weightDivisionDDLOptions: DropDownListOption[] = [];
    categoryDDLOptions: DropDownListOption[] = [];

    @Output() onFilterChanged: EventEmitter<FighterFilterModel>;
    @Output() onFilterLoaded: EventEmitter<boolean>;
    @Output() currentFilterValue: FighterFilterModel;


    constructor(private dataService: DataService) {
        this.onFilterChanged = new EventEmitter<FighterFilterModel>();
        this.onFilterLoaded = new EventEmitter<boolean>();
    }

    ngOnInit() {
        Observable.forkJoin(this.dataService.getCategories(), this.dataService.getWeightDivisions())
            .subscribe(data => this.initFilter(data))
    }

    //Events
    weightSelect(value) {
        if (value.name == DefaultValues.DROPDOWN_NAME_ANY) {
            this.currentFilterValue.weightDivisions = this.weightDivisions;
        }
        else {
            this.currentFilterValue.weightDivisions = this.weightDivisions.filter(wd => wd.weightDivisionId == value.id);
        }
        this.onFilterChanged.emit(this.currentFilterValue);
    }

    categorySelect(value) {
        if (value.name == DefaultValues.DROPDOWN_NAME_ANY) {
            this.currentFilterValue.categories = this.categories;
        }
        else {
            this.currentFilterValue.categories = this.categories.filter(c => c.categoryId == value.id);
        }
        this.onFilterChanged.emit(this.currentFilterValue);
    }

    //Private methods


    private initFilter(data: [Category[], WeightDivision[]]) {
        let defaultDDLOption = new DropDownListOption(DefaultValues.DROPDOWN_ID_ANY, DefaultValues.DROPDOWN_NAME_ANY);
        this.categories = data[0];
        this.weightDivisions = data[1];
        this.weightDivisionDDLOptions.push(defaultDDLOption);
        this.categoryDDLOptions.push(defaultDDLOption)
        this.categories.map(c => this.categoryDDLOptions.push(new DropDownListOption(c.categoryId, c.name)));
        this.weightDivisions.map(wd => this.weightDivisionDDLOptions.push(new DropDownListOption(wd.weightDivisionId, wd.name)));
        this.currentFilterValue = new FighterFilterModel(this.weightDivisions, this.categories);
        this.onFilterChanged.emit(this.currentFilterValue);
}


}



