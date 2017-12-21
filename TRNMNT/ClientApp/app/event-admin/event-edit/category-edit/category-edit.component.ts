import { Input, Component, ViewEncapsulation } from "@angular/core";
import { CategoryModel } from "../../../core/model/category.models";
import './category-edit.component.scss';

@Component({
    selector: 'category-edit',
    templateUrl: "./category-edit.component.html",
    encapsulation: ViewEncapsulation.None
})

export class CategoryEditComponent {

    private display = false;

    @Input() categories: CategoryModel[];

    private addCategory() {
        this.display = true;
    }
}


