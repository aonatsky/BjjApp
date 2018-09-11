import { Component, OnInit } from '@angular/core';
import { EventModel } from '../core/model/event.models';
import { EventService } from '../core/services/event.service';
import { RouterService } from '../core/services/router.service';
import { AuthService } from '../core/services/auth.service';
import DateHelper from '../core/helpers/date-helper';
import { Title } from '@angular/platform-browser';
import { Roles } from '../core/consts/roles.const';
import { UserService } from '../core/services/user.service';

@Component({
  selector: 'event',
  templateUrl: './event.component.html',
  styleUrls: ['./event.component.scss']
})
export class EventComponent implements OnInit {
  eventModel: EventModel;
  isParticipant: boolean;
  displayPopup: boolean = false;

  constructor(
    private routerService: RouterService,
    private eventService: EventService,
    private authService: AuthService,
    private titleService: Title,
    private userService: UserService
  ) {}

  eventImageUrl(): string {
    if (this.eventModel.imgPath) {
      return this.eventModel.imgPath.replace(/\\/g, '/');
    }
    return '';
  }

  ngOnInit() {
    this.eventService.getEventInfo().subscribe(r => {
      this.eventModel = r;
      this.titleService.setTitle(this.eventModel.title);
      if (!this.eventModel) {
        this.routerService.goToMainDomain();
      }
    });
    this.checkIsParticipant();
  }

  participate() {
    if (this.authService.isLoggedIn()) {
      this.processToParticipation();
    } else {
      this.displayPopup = true;
    }
  }

  isRegistrationStarted(): boolean {
    return DateHelper.getCurrentDate() >= DateHelper.getDate(this.eventModel.registrationStartTS);
  }

  isRegistrationEnded(): boolean {
    return DateHelper.getCurrentDate() >= DateHelper.getDate(this.eventModel.registrationEndTS);
  }

  processToParticipation() {
    this.userService.getIsParticipant().subscribe(r => {
      this.isParticipant = r;
      if (this.authService.ifRolesMatch([Roles.TeamOwner])) {
        this.routerService.goToParticipantTeamRegistration();
      } else {
        if (this.isParticipant) {
          this.routerService.goToMyEvents();
        } else {
          this.routerService.goToParticipantRegistration();
        }
      }
    });
  }

  private checkIsParticipant() {
    if (this.authService.isLoggedIn()) {
      this.userService.getIsParticipant().subscribe(r => (this.isParticipant = r));
    }
  }
}
