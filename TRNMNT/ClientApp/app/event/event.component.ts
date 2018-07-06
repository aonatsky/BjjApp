import { Component, OnInit } from '@angular/core';
import { EventModel } from '../core/model/event.models';
import { EventService } from '../core/services/event.service';
import { RouterService } from '../core/services/router.service';

@Component({
  selector: 'event',
  templateUrl: './event.component.html',
  styleUrls: ['./event.component.css']
})
export class EventComponent implements OnInit {
  eventModel: EventModel;

  constructor(private routerService: RouterService, private eventService: EventService) {}

  ngOnInit() {
    this.eventService.getEventInfo().subscribe(r => {
      this.eventModel = r;
      if (!this.eventModel) {
        this.routerService.goHome();
      }
    });
  }

  participate() {
    this.routerService.goToEventRegistration();
  }
}
