import { CategorySimpleModel } from '../../core/model/category.models';
import { WeightDivisionModel } from '../../core/model/weight-division.models';
import { CategoryWithDivisionFilterModel } from '../../core/model/category-with-division-filter.model';

import { Component, Input, Output, OnInit, EventEmitter, OnChanges, SimpleChanges } from '@angular/core'
import { DefaultValues } from '../../core/consts/default-values'
import { Observable } from 'rxjs/Observable';
import { CategoryService } from '../../core/services/category.service';
import { WeightDivisionService } from '../../core/services/weight-division.service';
import { SelectItem } from 'primeng/components/common/selectitem';


@Component({
    selector: 'category-with-division-filter',
    templateUrl: 'category-with-division-filter.component.html',
    styleUrls: ['category-with-division-filter.component.css'],
})

export class CategoryWithDivisionFilter implements OnInit {

    private categorySelectItems: SelectItem[];
    private weightDivisionsSelectItems: SelectItem[];
    private defaultOption: SelectItem;

    @Input() eventId: string;
    @Input() useDataFromInput: boolean = false;
    @Input() autoSelectFirstWeightDivision: boolean = false;
    @Input() categories: CategorySimpleModel[] = [];
    @Input() weightDivisions: WeightDivisionModel[] = [];
    @Input() isMemberFilterEnabled: boolean = false;
    @Input() refreshTrigger: number = 0;

    @Output() onFilterChanged: EventEmitter<CategoryWithDivisionFilterModel>;
    @Output() onFilterLoaded: EventEmitter<boolean>;
    @Output() currentFilterValue: CategoryWithDivisionFilterModel;


    constructor(private categoryService: CategoryService, private weightDivisionService: WeightDivisionService) {
        this.onFilterChanged = new EventEmitter<CategoryWithDivisionFilterModel>();
        this.onFilterLoaded = new EventEmitter<boolean>();
        this.defaultOption = { label: DefaultValues.DROPDOWN_NAME_ANY, value: '' };
        this.currentFilterValue = new CategoryWithDivisionFilterModel('', '', false);
    }

    ngOnInit() {
        if (this.useDataFromInput) {
            this.initFilter([this.categories, this.weightDivisions]);
        } else {
           this.refreshFromServer();
        }
    }

    ngOnChanges(changes: SimpleChanges) {
        this.refreshFromServer();
    }

    //Events

    onSelect(value) {
        this.onFilterChanged.emit(this.currentFilterValue);
    }

    onCategorySelect(event) {
        this.refreshWeightDivisionFilter(event.value);
        this.onSelect(event);
    }

    //#region Private methods

    private initFilter(data: [CategorySimpleModel[], WeightDivisionModel[]]) {
        const categories = data[0];
        const weightDivisions = data[1];
        this.initCategoryFilter(categories);
        this.initWeightDivisionFilter(weightDivisions);
        this.onFilterLoaded.emit(true);
    }

    private initWeightDivisionFilter(weightDivisions: WeightDivisionModel[]) {
        this.weightDivisions = weightDivisions;
        if (this.currentFilterValue.categoryId != '') {
            this.refreshWeightDivisionFilter(this.currentFilterValue.categoryId);
        }
    }

    private refreshWeightDivisionFilter(categoryId: string) {
        var weightDivisions = this.weightDivisions.filter(wd => wd.categoryId.toLowerCase() == categoryId.toLowerCase());
        if (weightDivisions.length > 0) {
            this.weightDivisionsSelectItems = [];
            this.weightDivisionsSelectItems.push(this.defaultOption);
            weightDivisions.map(wd => this.weightDivisionsSelectItems.push(this.getWeightDivisionselectItem(wd)));
            if (this.currentFilterValue.weightDivisionId === '') {
                this.currentFilterValue.weightDivisionId = this.autoSelectFirstWeightDivision ? weightDivisions[0].weightDivisionId : '';    
            }
        } else {
            this.weightDivisionsSelectItems = null;
            this.currentFilterValue.weightDivisionId = '';
        }
    }

    private getWeightDivisionselectItem(weightDivision: WeightDivisionModel) {
        let label = weightDivision.name;
        if (weightDivision.status != 0) {
            if (weightDivision.status == 1) {
                label += ' (IN PROGRESS)';
            } else {
                label += ' (COMPLETED)';
            }
        }

        return { label: label, value: weightDivision.weightDivisionId }
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

    private refreshFromServer() {
        Observable.forkJoin(
                this.categoryService.getCategoriesByEventId(this.eventId),
                this.weightDivisionService.getWeightDivisionsByEvent(this.eventId))
            .subscribe(data => this.initFilter(data));
    }

    //#endregion






}



