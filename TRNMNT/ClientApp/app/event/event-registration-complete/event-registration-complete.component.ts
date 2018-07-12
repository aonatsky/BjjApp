import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';
import { LoggerService } from '../../core/services/logger.service';
import { RouterService } from '../../core/services/router.service';
import { EventService } from '../../core/services/event.service';
import { EventModel } from '../../core/model/event.models';

@Component({
  selector: 'event-registration-complete',
  templateUrl: './event-registration-complete.component.html'
})
export class EventRegistrationCompleteComponent implements OnInit {
  eventModel: EventModel;

  constructor(
    private routerService: RouterService,
    private loggerService: LoggerService,
    private eventService: EventService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.route.params.subscribe(p => {
      const eventId = p.id;
      this.loadEvent(eventId);
    });
  }

  private loadEvent(eventId: string) {
    this.eventService.getEvent(eventId).subscribe(e => (this.eventModel = e));
  }
}
