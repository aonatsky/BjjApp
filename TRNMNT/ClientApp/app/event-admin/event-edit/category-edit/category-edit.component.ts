import { Input, Component, ViewEncapsulation } from "@angular/core";
import { CategoryModel } from "../../../core/model/category.models";
import './category-edit.component.scss';
import Weightdivisionmodels = require('../../../core/model/weight-division.models');
import WeightDivisionModel = Weightdivisionmodels.WeightDivisionModel;

@Component({
    selector: 'category-edit',
    templateUrl: "./category-edit.component.html",
    encapsulation: ViewEncapsulation.None
})

export class CategoryEditComponent {

    private displayPopup = false;
    private categoryToEdit = new CategoryModel();
    private selectedIndex: number;
    private isNewCategory: boolean;


    @Input() categories: CategoryModel[];

    private addCategory() {

        this.categoryToEdit = new CategoryModel();
        this.isNewCategory = true;
        this.displayPopup = true;
    }

    private editCategory(index: number) {
        this.selectedIndex = index;
        this.isNewCategory = false;
        this.categoryToEdit = this.cloneCategory(this.categories[index]);
        this.displayPopup = true;
    }

    private saveCategory(): void {
        if (!this.isNewCategory) {
            this.categories[this.selectedIndex] = this.categoryToEdit;
        } else {
            this.categories.push(this.categoryToEdit);
        }
        this.displayPopup = false;

    }

    private deleteCategory(): void {
        this.categories.splice(this.selectedIndex, 1);
        this.displayPopup = false;
    }



    private addWeightDivision(): void {
        this.categoryToEdit.weightDivisionModels.push(new WeightDivisionModel());
    }
    private deleteWeightDivision(index: number): void {
        this.categoryToEdit.weightDivisionModels.splice(index, 1);
    }


    private cloneCategory(category: CategoryModel) {
        let cloned = { ...category };
        cloned.weightDivisionModels = [];
        for (let i = 0; i < category.weightDivisionModels.length; i++) {
            cloned.weightDivisionModels.push({ ...category.weightDivisionModels[i] });
        }
        return cloned;

    }
}


