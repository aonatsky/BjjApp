import { Component, OnInit } from '@angular/core';
import { EventModel } from '../core/model/event.models';
import { EventService } from '../core/services/event.service';
import { RouterService } from '../core/services/router.service';
import { AuthService } from '../core/services/auth.service';
import  DateHelper from '../core/helpers/date-helper';
import { Title } from '@angular/platform-browser';
import { Roles } from '../core/consts/roles.const';

@Component({
  selector: 'event',
  templateUrl: './event.component.html',
  styleUrls: ['./event.component.scss']
})
export class EventComponent implements OnInit {
  eventModel: EventModel;
  
  eventImageUrl():string{
    if(this.eventModel.imgPath){
      return this.eventModel.imgPath.replace(/\\/g, "/");
    }
    return ''
  };

  displayPopup: boolean = false;
  constructor(
    private routerService: RouterService,
    private eventService: EventService,
    private authService: AuthService,
    private titleService: Title,
    
  ) {}

  ngOnInit() {
    this.eventService.getEventInfo().subscribe(r => {
      this.eventModel = r;
      this.titleService.setTitle(this.eventModel.title);
      if (!this.eventModel) {
        this.routerService.goToMainDomain();
      }
    });

  }

  participate() {
    if (this.authService.isLoggedIn()) {
      if(this.authService.ifRolesMatch([Roles.TeamOwner])){
        this.routerService.goToParticipantTeamRegistration();
      }else{
        this.routerService.goToParticipantRegistration();
      }
      
    } else {
      this.displayPopup = true;
    }
  }

  isRegistrationStarted() : boolean {
    return DateHelper.getCurrentDate() >= DateHelper.getDate(this.eventModel.registrationStartTS);
  }

  isRegistrationEnded() : boolean {
    return DateHelper.getCurrentDate() >= DateHelper.getDate(this.eventModel.registrationEndTS);
  }
}
