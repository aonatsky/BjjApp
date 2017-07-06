
import { Component, Input, Output, OnInit, ViewChild, ElementRef, Renderer2 } from '@angular/core';
import { EventModel } from './../../core/model/event.model';
import { Category } from './../../core/model/category.model';
import { WeightDivision } from './../../core/model/weight-division.model';

@Component({
    selector: 'category',
    templateUrl: "./event-create.category.component.html",
    styleUrls: ["./event-create.component.css"]
})



export class EventCreateCategoryComponent {

    //@Input() category: Category;

    @ViewChild('inputCategoryName') inputCategoryName: ElementRef;

    category: Category
    isEdit: boolean = false;

    

    constructor() {
            
    }

    ngOnInit() {
        this.category = new Category();
        this.category.name = "TestCategory";
        
    }

    private edit() {
        this.isEdit = true;
        this.inputCategoryName.nativeElement.focus();
        
    }

    private save() {
        this.isEdit = false;
    }

    private delete() {
        //rodo
    }


}

