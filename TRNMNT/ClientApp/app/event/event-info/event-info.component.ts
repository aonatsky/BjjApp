import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';
import { LoggerService } from '../../core/services/logger.service';
import { RouterService } from '../../core/services/router.service';
import { EventService } from '../../core/services/event.service';
import { EventModel } from '../../core/model/event.models';

@Component({
  selector: 'event-info',
  templateUrl: './event-info.component.html',
  styleUrls: ['./event-info.component.css']
})
export class EventInfoComponent implements OnInit {
  private eventModel: EventModel;

  constructor(
    private routerService: RouterService,
    private loggerService: LoggerService,
    private eventService: EventService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.eventService.getEventInfo().subscribe(r => {
      this.eventModel = r;
      if (!this.eventModel) {
        this.routerService.goHome();
      }
    });
  }

  private participate() {
    this.routerService.goToEventRegistration();
  }
}
