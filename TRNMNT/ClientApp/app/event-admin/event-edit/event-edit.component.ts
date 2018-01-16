
import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MenuItem } from 'primeng/primeng';
import { EventModel } from './../../core/model/event.models';
import { AuthService } from './../../core/services/auth.service'
import { EventService } from './../../core/services/event.service'
import './event-edit.component.scss'

@Component({
    selector: 'event-edit',
    templateUrl: './event-edit.component.html',
    encapsulation: ViewEncapsulation.None
})
export class EventEditComponent implements OnInit {
    constructor(private authService: AuthService, private eventService: EventService, private route: ActivatedRoute) {

    }

    private menuItems: MenuItem[];
    private currentStep: number = 0;
    private eventModel: EventModel;
    private categoryCount: number = 0;


    private lastStep: number = 3;


    ngOnInit() {
        this.initMenu();
        this.initData();
    }


    private initData() {
        this.route.params.subscribe(p => {
            let id = p['id'];
            if (id && id != '') {
                this.eventService.getEvent(id).subscribe(r => { this.eventModel = r });
            } else {
                alert('No data to display');
            }
        });
    }

    private initMenu() {
        this.menuItems = [{
            label: 'General Information',
        },
        {
            label: 'Prices',
        },
        {
            label: 'Categories'
        },
        {
            label: 'Additional Information'
        }
        ];
    }

    private modelReload() {
        this.eventService.getEvent(this.eventModel.eventId).subscribe(r => { this.eventModel = r });
    }

    private nextStep() {
        this.currentStep++;
    }

    private previousStep() {
        this.currentStep--;
    }

    private save() {
        this.eventService.updateEvent(this.eventModel).subscribe();
    }



    private onImageUpload(event) {
        this.eventService.uploadEventImage(event.files[0], this.eventModel.eventId).subscribe(r => this.modelReload());
    }

    private onTncUpload(event) {
        this.eventService.uploadEventTncFile(event.files[0], this.eventModel.eventId).subscribe(r => this.modelReload());
    }

    private onPromoCodeUpload(event) {
        this.eventService.uploadPromoCodeList(event.files[0], this.eventModel.eventId).subscribe(r => this.modelReload());
    }

    private downloadTnc() {
        this.eventService.downloadEventTncFile(this.eventModel.tncFilePath).subscribe();
    }
}
