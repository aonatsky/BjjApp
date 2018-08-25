import { EventService } from '../../core/services/event.service';
import { RouterService } from '../../core/services/router.service';
import { EventPreviewModel } from '../../core/model/event.models';
import { Component, OnInit } from '@angular/core';
import './event-list.component.scss';

@Component({
  selector: 'event-list',
  templateUrl: './event-list.component.html'
})
export class EventListComponent implements OnInit {
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
  dashboard(id: string) {
    this.routerService.goToEventDashboard(id);
  }

  ngOnInit() {
    this.eventService.getEventsForOwner().subscribe(res => (this.events = res));
  }
}
