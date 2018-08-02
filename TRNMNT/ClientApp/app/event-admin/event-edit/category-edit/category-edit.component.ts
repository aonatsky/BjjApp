import { Input, Component, ViewEncapsulation } from '@angular/core';
import { CategoryModel } from '../../../core/model/category.models';
import './category-edit.component.scss';

import { WeightDivisionModel } from '../../../core/model/weight-division.models';
import { MinuteSecondsPipe } from '../../../core/pipes/minutes-seconds.pipe';


@Component({
    selector: 'category-edit',
    templateUrl: './category-edit.component.html',
    encapsulation: ViewEncapsulation.None
})

export class CategoryEditComponent {

    displayPopup = false;
    categoryToEdit = new CategoryModel();
    matchTimeFormatted: string;
    selectedIndex: number;
    isNewCategory: boolean;



    @Input() categories: CategoryModel[];
    @Input() eventId: string;

    addCategory() {
        this.categoryToEdit = new CategoryModel();
        this.categoryToEdit.eventId = this.eventId;
        this.isNewCategory = true;
        this.displayPopup = true;
        this.matchTimeFormatted = '';
    }

    editCategory(index: number) {
        this.selectedIndex = index;
        this.isNewCategory = false;
        this.categoryToEdit = this.cloneCategory(this.categories[index]);
        this.displayPopup = true;
        this.matchTimeFormatted = this.getmatchTimeFormatted();
    }

    saveCategory(): void {
        this.categoryToEdit.matchTime = this.getmatchTime();
        if (!this.isNewCategory) {
            this.categories[this.selectedIndex] = this.categoryToEdit;
        } else {
            this.categories.push(this.categoryToEdit);
        }
        this.displayPopup = false;

    }

    deleteCategory(): void {
        this.categories.splice(this.selectedIndex, 1);
        this.displayPopup = false;
    }



    addWeightDivision(): void {
        this.categoryToEdit.weightDivisionModels.push(new WeightDivisionModel());
    }
    deleteWeightDivision(index: number): void {
        this.categoryToEdit.weightDivisionModels.splice(index, 1);
    }


    private cloneCategory(category: CategoryModel): CategoryModel {
        let cloned = new CategoryModel();
        cloned.categoryId = category.categoryId;
        cloned.matchTime = category.matchTime;
        cloned.eventId = category.eventId;
        cloned.name = category.name;
        cloned.weightDivisionModels = [];
        for (let i = 0; i < category.weightDivisionModels.length; i++) {
            cloned.weightDivisionModels.push({ ...category.weightDivisionModels[i] });
        }
        return cloned;

    }

    getmatchTime(): number {
        let minutes = Number(this.matchTimeFormatted.split(':')[0]);
        let seconds = Number(this.matchTimeFormatted.split(':')[1]);
        if (isNaN(minutes) || isNaN(seconds)) {
            return 0;
        } else {
            return minutes * 60 + seconds;
        };
    }

    private getmatchTimeFormatted() {
        let pipe = new MinuteSecondsPipe();
        return pipe.transform(this.categoryToEdit.matchTime);
    }
}


