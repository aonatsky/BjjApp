
import { AuthService } from './../../core/services/auth.service';
import { EventService } from './../../core/services/event.service';
import { LoggerService } from './../../core/services/logger.service';
import { RouterService } from './../../core/services/router.service';
import { EventPreviewModel } from './../../core/model/event.models';
import { Component, OnInit } from '@angular/core';
import './event-overview.component.scss'

@Component({
    selector: 'event-overview',
    templateUrl: './event-overview.component.html'
})
export class EventOverviewComponent implements OnInit {

    private events: EventPreviewModel[] = [];

    constructor(private loggerService: LoggerService, private routerService: RouterService, private authService: AuthService, private eventService: EventService) {

    }

    public createEvent() {
        this.eventService.createEvent().subscribe(r => this.routerService.goToEditEvent(r))
    }

    public editEvent(id: string) {
        this.routerService.goToEditEvent(id);
    }

    public openDetails(id: string) {
        this.routerService.goToEventManagement(id);
    }

    public runEvent(id: string) {
        this.routerService.goToEventRun(id);
    }

    ngOnInit() {
        this.eventService.getEventsForOwner().subscribe(res => this.events = res);
    }
}
