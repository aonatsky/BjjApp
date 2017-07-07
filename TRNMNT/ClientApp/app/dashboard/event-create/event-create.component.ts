
import { Component, OnInit, ViewEncapsulation, ViewChildren } from '@angular/core';
import { MenuModule, MenuItem } from 'primeng/primeng';
import { EventModel } from './../../core/model/event.model';
import { CategoryModel } from './../../core/model/category.model';
import { WeightDivisionModel } from './../../core/model/weight-division.model';
import { AuthService } from './../../core/services/auth.service'
import { CategoryComponent } from './../event-create/category.component'


@Component({
    selector: 'event-create',
    templateUrl: './event-create.component.html',
    styleUrls: ['./event-create.component.css'],
    encapsulation: ViewEncapsulation.None
})
export class EventCreateComponent implements OnInit {
    constructor(private authService: AuthService) {
    }

    private menuItems: MenuItem[];
    private currentStep: number = 0;
    private eventModel: EventModel;
    private categoryCount: number;


    @ViewChildren(CategoryComponent) categoryComponents;


    ngOnInit() {
        this.initMenu();
        this.initData();
        
        
    }


    ngDoCheck() {
        this.onNewCategoryCreate();
    }


    private initData() {
        this.eventModel = new EventModel();
        this.eventModel.title = "Kiev open 2020"

        let wd = new WeightDivisionModel("02b44457-be9e-4196-a666-e5602e04ee61","weight division 1",90);
        var cat1 = new CategoryModel();
        cat1.categoryId = "02b44457-be9e-4196-a666-e5602e04ee61";
        cat1.name = "category 1"
        cat1.weightDivisions = [wd];


        var cat2 = new CategoryModel();
        cat2.categoryId = "02b44457-be9e-4196-a666-e5602e04ee63";
        cat2.name = "category 2"

        this.eventModel.categories = [cat1, cat2];
        this.categoryCount = this.eventModel.categories.length;
    }


    private initMenu() {
        this.menuItems = [{
            label: 'General Information',
        },
        {
            label: 'Category Setup',
        },
        {
            label: 'Confirmation',
        },
        {
            label: 'Payment',
        }
        ];}


    private nextStep() {
        this.currentStep++;
    }

    private saveAsDraft() {
        //todo 
    }

    private categoryDelete(model: CategoryModel) {
        let index = this.eventModel.categories.indexOf(model);
        if (index !== -1) {
            this.eventModel.categories.splice(index, 1);
            this.categoryCount = this.eventModel.categories.length;
        }
        
    }

    private categoryCreate() {
        let category = new CategoryModel();
        category.name = "Category";
        this.eventModel.categories.push(category);
    }

    trackCategory(index, category) {
        return index;
    }

    private onNewCategoryCreate() {
        if (this.categoryComponents) {
            let controls = (this.categoryComponents.toArray());
            if (controls.length > this.categoryCount) {
                controls[controls.length - 1].categoryEdit();
                this.categoryCount = this.eventModel.categories.length;
            };
        }
    }
}
