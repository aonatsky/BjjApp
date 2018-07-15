import { Component, OnInit } from '@angular/core';
import { EventModel } from '../core/model/event.models';
import { EventService } from '../core/services/event.service';
import { RouterService } from '../core/services/router.service';
import { AuthService } from '../core/services/auth.service';
import { TranslateService } from '../../../node_modules/@ngx-translate/core';

@Component({
  selector: 'event',
  templateUrl: './event.component.html',
  styleUrls: ['./event.component.scss']
})
export class EventComponent implements OnInit {
  eventModel: EventModel;
  displayPopup: boolean = false;
  constructor(
    private routerService: RouterService,
    private eventService: EventService,
    private authService: AuthService,
    private translateService: TranslateService
  ) {}

  ngOnInit() {
    
    this.translateService.onLangChange.subscribe(r => {
      this.translateService.get('EVENT_INFO.EVENT_STARTS').subscribe(r => console.log(r));
      console.log(r);
    });
    this.translateService.use('ua')
    this.eventService.getEventInfo().subscribe(r => {
      this.eventModel = r;
      if (!this.eventModel) {
        this.routerService.goHome();
      }
    });
  }

  participate() {
    if (this.authService.isLoggedIn()) {
      this.routerService.goToEventRegistration();
    } else {
      this.displayPopup = true;
    }
  }
}
