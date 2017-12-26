import { CategorySimpleModel } from '../../core/model/category.models';
import { WeightDivisionModel } from '../../core/model/weight-division.models';
import { CategoryWithDivisionFilterModel } from '../../core/model/category-with-division-filter.model';

import { Component, Input, Output, OnInit, EventEmitter } from "@angular/core"
import { DefaultValues } from '../../core/consts/default-values'
import { Observable } from "rxjs/Observable";
import { CategoryService } from '../../core/services/category.service';
import { WeightDivisionService } from '../../core/services/weight-division.service';
import { SelectItem } from 'primeng/components/common/selectitem';


@Component({
    selector: 'category-with-division-filter',
    templateUrl: 'category-with-division-filter.component.html',
    styleUrls: ['category-with-division-filter.component.css']
})

export class CategoryWithDivisionFilter implements OnInit {

    private categories: CategorySimpleModel[] = [];
    private weightDivisions: WeightDivisionModel[] = [];
    private categorySelectItems: SelectItem[];
    private weightDivisionsSelectItems: SelectItem[];
    private defaultOption: SelectItem;

    @Input() eventId: string;

    @Output() onFilterChanged: EventEmitter<CategoryWithDivisionFilterModel>;
    @Output() onFilterLoaded: EventEmitter<boolean>;
    @Output() currentFilterValue: CategoryWithDivisionFilterModel;


    constructor(private categoryService: CategoryService, private weightDivisionService: WeightDivisionService) {
        this.onFilterChanged = new EventEmitter<CategoryWithDivisionFilterModel>();
        this.onFilterLoaded = new EventEmitter<boolean>();
        this.defaultOption = { label: DefaultValues.DROPDOWN_NAME_ANY, value: "-1" };
        this.currentFilterValue = new CategoryWithDivisionFilterModel("", "");
    }

    ngOnInit() {
        Observable.forkJoin(
                this.categoryService.getCategoriesByEventId(this.eventId),
                this.weightDivisionService.getWeightDivisions(this.eventId))
            .subscribe(data => this.initFilter(data));
    }

    //Events

    onSelect(value) {
        this.onFilterChanged.emit(this.currentFilterValue);
    }

    //#region Private methods

    private initFilter(data: [CategorySimpleModel[], WeightDivisionModel[]]) {
        const categories = data[0];
        const weightDivisions = data[1];
        this.initCategoryFilter(categories);
        this.initWeightDivisionFilter(weightDivisions);
    }

    private initWeightDivisionFilter(weightDivisions: WeightDivisionModel[]) {
        this.weightDivisions = weightDivisions;
    }

    private refreshWeightDivisionFilter(categoryId: string) {
        var weightDiwisions = this.weightDivisions.filter(wd => wd.categoryId == categoryId);
        this.weightDivisionsSelectItems = [];
        this.weightDivisionsSelectItems.push(this.defaultOption);
        weightDiwisions.map(wd => this.weightDivisionsSelectItems.push({ label: wd.name, value: wd.weightDivisionId }));
    }

    private initCategoryFilter(categories: CategorySimpleModel[]) {
        this.categories = categories;
        this.categorySelectItems = [];
        this.categorySelectItems.push(this.defaultOption);
        this.categories.map(c => this.categorySelectItems.push({ label: c.name, value: c.categoryId }));
    }

    private get weightDivisionIds(): string[] {
        if (this.weightDivisions == null) {
            return [];
        }
        return this.weightDivisions.map(c => c.weightDivisionId);
    }

    private get categoryIds(): string[] {
        return this.categories.map(c => c.categoryId);
    }

    //#endregion

     




}


