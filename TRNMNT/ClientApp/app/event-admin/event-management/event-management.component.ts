import { ActivatedRoute } from '@angular/router';
import { EventService } from '../../core/services/event.service';
import { LoggerService } from '../../core/services/logger.service';
import { RouterService } from '../../core/services/router.service';
import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'event-management',
    templateUrl: './event-management.component.html'
})
export class EventManagementComponent implements OnInit {

    private eventId: string;

    constructor(private loggerService: LoggerService, private routerService: RouterService, private eventService: EventService, private route: ActivatedRoute) {

    }


    ngOnInit() {
        this.route.params.subscribe(p => {
            this.eventId = p['id'];
        });
    }
}

