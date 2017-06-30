
import { Component, OnInit, ViewEncapsulation} from '@angular/core';
import { MenuModule, MenuItem } from 'primeng/primeng';
import { EventModel } from './../../core/model/event.model';
import { AuthService } from './../../core/services/auth.service'

@Component({
    selector: 'event-create',
    templateUrl: './event-create.component.html',
    styleUrls: ['./event-create.component.css'],
    encapsulation: ViewEncapsulation.None
})
export class EventCreateComponent implements OnInit {
    constructor(private authService: AuthService) {
    }

    private items: MenuItem[];
    private currentStep: number = 1;
    private eventModel: EventModel;


    ngOnInit() {
        this.eventModel = new EventModel();
        this.items = [{
            label: 'General Information',
            items: [
                { label: 'New', icon: 'fa-plus' },
                { label: 'Open', icon: 'fa-download' }
            ]
        },
        {
            label: 'Category Setup',
            items: [
                { label: 'Undo', icon: 'fa-refresh' },
                { label: 'Redo', icon: 'fa-repeat' }
            ]
            },
        {
            label: 'Confirmation',
            items: [
                { label: 'Undo', icon: 'fa-refresh' },
                { label: 'Redo', icon: 'fa-repeat' }
            ]
        },
        {
            label: 'Payment',
            items: [
                { label: 'Undo', icon: 'fa-refresh' },
                { label: 'Redo', icon: 'fa-repeat' }
            ]
        }
        ];
    }
}
