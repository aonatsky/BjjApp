import { Component, OnInit } from '@angular/core';
import { EventModel } from '../core/model/event.models';
import { EventService } from '../core/services/event.service';
import { RouterService } from '../core/services/router.service';
import { AuthService } from '../core/services/auth.service';

@Component({
  selector: 'event',
  templateUrl: './event.component.html',
  styleUrls: ['./event.component.scss']
})
export class EventComponent implements OnInit {
  eventModel: EventModel;
  
  eventImageUrl():string{
    return this.eventModel.imgPath.replace(/\\/g, "/");
  };

  displayPopup: boolean = false;
  constructor(
    private routerService: RouterService,
    private eventService: EventService,
    private authService: AuthService,
  ) {}

  ngOnInit() {
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
