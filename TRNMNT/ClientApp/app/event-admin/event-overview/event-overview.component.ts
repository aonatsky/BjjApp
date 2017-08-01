
import { AuthService } from './../../core/services/auth.service';
import { EventService } from './../../core/services/event.service';
import { LoggerService } from './../../core/services/logger.service';
import { RouterService } from './../../core/services/router.service';
import { EventModel } from './../../core/model/event.model';
import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'event-overview',
    templateUrl: './event-overview.component.html'
})
export class EventOverviewComponent {

    private events: EventModel[] = [];

    constructor(private loggerService: LoggerService, private routerService: RouterService, private authService: AuthService, private eventService: EventService) {

    }

    public createEvent() {
        this.eventService.createEvent().subscribe(r => this.routerService.GoToEditEvent(r))
        //this.routerService.GoToEditEvent();
    }

    public editEvent(id: string) {
        this.routerService.GoToEditEvent(id);
    }

    ngOnInit() {
        this.eventService.getEventsForUser().subscribe(res => this.events = res);
    }
}
