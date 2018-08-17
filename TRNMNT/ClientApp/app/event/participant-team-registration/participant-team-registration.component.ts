import { Component, OnInit } from '@angular/core';
import { EventService } from '../../core/services/event.service';
import { WeightDivisionService } from '../../core/services/weight-division.service';
import { CategoryService } from '../../core/services/category.service';
import { TeamService } from '../../core/services/team.service';
import { ParticipantService } from '../../core/services/participant.service';
import { AuthService } from '../../../../node_modules/angular5-social-login';
import { RouterService } from '../../core/services/router.service';
import { TranslateService } from '../../../../node_modules/@ngx-translate/core';
import { UserModelAthlete } from '../../core/model/user.models';
import { ICrudColumn as CrudColumn, IColumnOptions } from '../../shared/crud/crud.component';
import { ParticipantModelBase, ParticipantRegistrationModel } from '../../core/model/participant.models';

@Component({
  selector: 'participant-team-registration',
  templateUrl: './participant-team-registration.component.html',
  styleUrls: ['./participant-team-registration.component.scss']
})
export class ParticipantTeamRegistrationComponent implements OnInit {
  constructor(
    private weightDivisionService: WeightDivisionService,
    private categoryService: CategoryService,
    private teamService: TeamService,
    private participantService: ParticipantService,
    private authService: AuthService,
    private eventService: EventService,
    private routerService: RouterService,
    private translateService: TranslateService
  ) {}

  allAthletes: UserModelAthlete[];
  selectedAthletes: ParticipantRegistrationModel[];
  columns: CrudColumn[] = [
    {
      propertyName: 'firstName',
      displayName: this.translateService.instant('COMMON.FIRST_NAME'),
      isEditable: false,
      isSortable: true
    },
    {
      propertyName: 'lastName',
      displayName: this.translateService.instant('COMMON.LAST_NAME'),
      isEditable: false,
      isSortable: true
    },
    {
      propertyName: 'email',
      displayName: this.translateService.instant('COMMON.EMAIL'),
      isEditable: false,
      isSortable: true
    },
  ];
  firstIndex: number = 0;
  sortDirection: number = 1;
  sortField: string = 'name';
  columnOptions: IColumnOptions = {};
  ngOnInit() {
    this.teamService.getTeamMembers().subscribe(r => (this.allAthletes = r));

  }

  add(data: UserModelAthlete){
    const t = new ParticipantRegistrationModel { 
      email = data.email,
    firstName :data.firstName,
  teamId : data.teamId };
    this.selectedAthletes.push(  )
  }
}
