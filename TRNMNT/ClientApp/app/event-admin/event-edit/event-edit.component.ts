
import { Component, OnInit, ViewEncapsulation, ViewChildren } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MenuModule, MenuItem } from 'primeng/primeng';
import { EventModel } from './../../core/model/event.models';
import { CategoryModel } from './../../core/model/category.models';
import { WeightDivisionModel } from './../../core/model/weight-division.models';
import { AuthService } from './../../core/services/auth.service'
import { EventService } from './../../core/services/event.service'
import { CategoryComponent } from './category.component'


@Component({
    selector: 'event-edit',
    templateUrl: './event-edit.component.html',
    styleUrls: ['./event-edit.component.css'],
    encapsulation: ViewEncapsulation.None
})
export class EventEditComponent implements OnInit {
    constructor(private authService: AuthService, private eventService: EventService, private route: ActivatedRoute) {

    }

    private menuItems: MenuItem[];
    private currentStep: number = 0;
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
                this.eventService.getEvent(id).subscribe(r => { this.eventModel = r });
                this.isNew = false;
            } else {
                alert("No data to display")
            }
        })
    }


    private initMenu() {
        this.menuItems = [{
            label: 'General Information',
        },
        {
            label: 'Category Setup',
        },
        {
            label: 'Additional information',
        }
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
        let index = this.eventModel.categoryModels.indexOf(model);
        if (index !== -1) {
            this.eventModel.categoryModels.splice(index, 1);
            this.categoryCount = this.eventModel.categoryModels.length;
        }

    }

    private categoryCreate() {
        let category = new CategoryModel();
        category.name = "Category";
        category.eventId = this.eventModel.eventId;
        category.weightDivisionModels.push(new WeightDivisionModel("Weight Division"));

        this.eventModel.categoryModels.push(category);
    }



    private onNewCategoryCreate() {
        if (this.categoryComponents) {
            let controls = (this.categoryComponents.toArray());
            if (controls.length > this.categoryCount) {
                controls[controls.length - 1].categoryEdit();
                this.categoryCount = this.eventModel.categoryModels.length;
            };
        }
    }

    private onImageUpload(event) {
        this.eventService.uploadEventImage(event.files[0], this.eventModel.eventId).subscribe();
    }

    private onTncUpload(event) {
        this.eventService.uploadEventTncFile(event.files[0], this.eventModel.eventId).subscribe();
    }

    private onPromoCodeUpload(event) {
        this.eventService.uploadPromoCodeList(event.files[0], this.eventModel.eventId).subscribe();
    }

    private downloadTnc() {
        this.eventService.downloadEventTncFile(this.eventModel.tncFilePath).subscribe();
    }
}
