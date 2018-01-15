import { ActivatedRoute } from '@angular/router';
import { AuthService } from './../../core/services/auth.service';
import { EventService } from './../../core/services/event.service';
import { LoggerService } from './../../core/services/logger.service';
import { RouterService } from './../../core/services/router.service';
import { EventPreviewModel } from './../../core/model/event.models';
import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'event-management',
    templateUrl: './event-management.component.html'
})
export class EventManagementComponent implements OnInit{
        
    private eventModel: EventPreviewModel;

    constructor(private loggerService: LoggerService, private routerService: RouterService, private eventService: EventService, private route: ActivatedRoute) {

    }

    
    ngOnInit() {
    }

    goToOverview() {
        this.routerService.goToEventAdmin();
    }
}
