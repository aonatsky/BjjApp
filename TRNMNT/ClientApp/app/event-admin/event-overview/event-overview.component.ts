import { EventService } from '../../core/services/event.service';
import { RouterService } from '../../core/services/router.service';
import { EventPreviewModel } from '../../core/model/event.models';
import { Component, OnInit } from '@angular/core';
import './event-overview.component.scss';

@Component({
  selector: 'event-overview',
  templateUrl: './event-overview.component.html'
})
export class EventOverviewComponent implements OnInit {
  events: EventPreviewModel[] = [];

  constructor(private routerService: RouterService, private eventService: EventService) {}

  createEvent() {
    this.eventService.createEvent().subscribe(r => this.routerService.goToEditEvent(r));
  }

  editEvent(id: string) {
    this.routerService.goToEditEvent(id);
  }

  openDetails(id: string) {
    this.routerService.goToEventManagement(id);
  }

  runEvent(id: string) {
    this.routerService.goToEventRun(id);
  }

  ngOnInit() {
    this.eventService.getEventsForOwner().subscribe(res => (this.events = res));
  }
}
