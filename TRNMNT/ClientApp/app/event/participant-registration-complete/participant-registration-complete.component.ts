﻿import { Component, OnInit } from '@angular/core';
import { RouterService } from '../../core/services/router.service';
import { EventService } from '../../core/services/event.service';
import { EventModel } from '../../core/model/event.models';

@Component({
  selector: 'participant-registration-complete',
  templateUrl: './participant-registration-complete.component.html'
})
export class ParticipantRegistrationCompleteComponent implements OnInit {
  eventModel: EventModel;
  eventTitleParameter: object;
  constructor(private eventService: EventService, private routerService:RouterService) {}

  ngOnInit() {
    this.eventService.getCurrentEvent().subscribe(r => {
      this.eventModel = r;
      this.eventTitleParameter = { value: this.eventModel.title };
    });
  }

  goToEventPage(){
    this.routerService.goToEventInfo();
  }
}
