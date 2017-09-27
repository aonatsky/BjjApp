
import { Component, Input, Output, OnInit, ViewChild, ElementRef, Renderer2, EventEmitter } from '@angular/core';
import { EventModel } from './../../core/model/event.models';
import { CategoryModel } from './../../core/model/category.models';
import { WeightDivisionModel } from './../../core/model/weight-division.models';

@Component({
    selector: 'category',
    templateUrl: "./category.component.html",
    styleUrls: ["./category.component.css"]
})



export class CategoryComponent {

    @Input() category: CategoryModel;

    @ViewChild('inputCategoryName') inputCategoryName: ElementRef;
    isEdit: boolean = false;
    @Output() onCategoryDelete: EventEmitter<CategoryModel> = new EventEmitter<CategoryModel>();;

    constructor() {
            
    }

    ngOnInit() {
        
    }


    public categoryEdit() {
        this.isEdit = true;
        this.inputCategoryName.nativeElement.disabled = false;
        this.inputCategoryName.nativeElement.focus();
        this.inputCategoryName.nativeElement.select()
        
    }

    private categorySave() {
        this.isEdit = false;
        this.inputCategoryName.nativeElement.disabled = true;
    }

    private categoryDelete() {
        this.onCategoryDelete.emit(this.category);
    }


    private weightDivisionDelete(wd: WeightDivisionModel) {
        let index = this.category.weightDivisionModels.indexOf(wd)
        this.category.weightDivisionModels.splice(index,1);
    }

    private weightDivisionAdd() {
        let wd = new WeightDivisionModel("Weight Divison");
        wd.categoryId = this.category.categoryId;
        this.category.weightDivisionModels.push(wd);
    }

}

