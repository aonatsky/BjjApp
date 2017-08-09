
import { Component, OnInit, ViewEncapsulation, ViewChildren } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MenuModule, MenuItem } from 'primeng/primeng';
import { EventModel } from './../../core/model/event.model';
import { CategoryModel } from './../../core/model/category.model';
import { WeightDivisionModel } from './../../core/model/weight-division.model';
import { AuthService } from './../../core/services/auth.service'
import { EventService } from './../../core/services/event.service'
import { CategoryComponent } from './../event-create/category.component'


@Component({
    selector: 'event-create',
    templateUrl: './event-create.component.html',
    styleUrls: ['./event-create.component.css'],
    encapsulation: ViewEncapsulation.None
})
export class EventCreateComponent implements OnInit {
    constructor(private authService: AuthService, private eventService: EventService, private route: ActivatedRoute) {
       
    }

    private menuItems: MenuItem[];
    private currentStep: number = 0;
    private eventId: string = "";
    private eventModel: EventModel;
    private categoryCount: number = 0;

    private isNew: boolean = true;

    private lastStep: number = 3;

    @ViewChildren(CategoryComponent) categoryComponents;
    
    ngOnInit() {
        this.initMenu();
        this.initData();
        

    }


    ngDoCheck() {
        this.onNewCategoryCreate();
    }


    private initData() {
        this.route.params.subscribe(p => {
            let id = p["id"];
            if (id && id != "") {
                this.eventService.getEvent(id).subscribe(r => this.eventModel = r);
                this.isNew = false;
            } else {
                this.isNew = true;
                this.eventService.addEvent().subscribe(r => this.eventModel = r)
            }
        })
    }

    private fillSampleData() {
        this.eventModel = new EventModel();
        this.eventModel.title = "Kiev open 2020"
        this.eventModel.eventDate = new Date(2017, 5, 5)
        this.eventModel.registrationStartTS = new Date(2017, 4, 5)
        this.eventModel.registrationEndTS = new Date(2017, 5, 2)
        this.eventModel.description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque quis enim neque. Sed congue, enim vitae varius vulputate, lorem mauris feugiat enim, sit amet congue elit purus eget felis. Donec maximus consectetur nibh. Vestibulum tellus turpis, venenatis eu blandit id, tincidunt vitae nisi. Proin in erat vitae metus accumsan consectetur. Cras nec ipsum eros. Sed eu auctor urna. Nullam efficitur dolor ut scelerisque blandit. Fusce ut rhoncus felis, et auctor est. Vivamus vel ornare nisi, ac auctor mauris. Pellentesque a diam urna. Aenean vitae mi egestas turpis dignissim suscipit non et leo. Integer lacus diam, placerat non scelerisque vel, luctus vitae turpis. Duis nec libero vel ligula tempus lacinia vel sed eros."
        this.eventModel.address = "In in congue elit. Donec feugiat neque nec lacus vehicula tristique. Ut in libero nec odio malesuada interdum sed non nulla. Vestibulum orci erat, cursus at purus vitae"

        let wd1 = new WeightDivisionModel("weight division 1");
        let wd2 = new WeightDivisionModel("weight division 2");
        let wd3 = new WeightDivisionModel("weight division 3");
        var cat1 = new CategoryModel();
        cat1.categoryId = "02b44457-be9e-4196-a666-e5602e04ee61";
        cat1.name = "category 1"
        cat1.weightDivisions = [wd1, wd2, wd3];


        var cat2 = new CategoryModel();
        cat2.categoryId = "02b44457-be9e-4196-a666-e5602e04ee63";
        cat2.name = "category 2"
        cat2.weightDivisions = [wd1, wd2];


        this.eventModel.categories = [cat1, cat2];
    }


    private initMenu() {
        this.menuItems = [{
            label: 'General Information',
        },
        {
            label: 'Additional information',
        },
        {
            label: 'Category Setup',
        },
        {
            label: 'Confirmation',
        },
        ];
    }


    private nextStep() {
        this.currentStep++;
    }

    private previousStep() {
        this.currentStep--;
    }

    private saveAsDraft() {
        this.eventService.updateEvent(this.eventModel).subscribe();
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



    private onNewCategoryCreate() {
        if (this.categoryComponents) {
            let controls = (this.categoryComponents.toArray());
            if (controls.length > this.categoryCount) {
                controls[controls.length - 1].categoryEdit();
                this.categoryCount = this.eventModel.categories.length;
            };
        }
    }

    private onImageUpload(event) {
        this.eventService.uploadEventImage(event.files[0], this.eventModel.eventId).subscribe();
    }

    private onTncUpload(event) {
        this.eventService.uploadEventTncFile(event.files[0], this.eventModel.eventId).subscribe();
    }
}
