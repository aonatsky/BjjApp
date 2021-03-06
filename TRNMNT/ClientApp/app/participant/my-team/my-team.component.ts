import { Component, OnInit } from '@angular/core';
import { TeamService } from '../../core/services/team.service';
import { ICrudColumn as CrudColumn, IColumnOptions } from '../../shared/crud/crud.component';
import { TranslateService } from '@ngx-translate/core';
import { UserModelAthlete } from '../../core/model/user.models';
import { ApprovalStatus } from '../../core/consts/approval-status.const';
import { AuthService } from '../../core/services/auth.service';


@Component({
  selector: 'app-my-team',
  templateUrl: './my-team.component.html',
  styleUrls: ['./my-team.component.scss']
})
export class MyTeamComponent implements OnInit {
  constructor(private translateService: TranslateService, private teamService: TeamService, private authService: AuthService) {}

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
    {
      propertyName: 'teamMembershipApprovalStatus',
      displayName: this.translateService.instant('COMMON.APPROVAL.APPROVAL_STATUS'),
      isEditable: false,
      isSortable: true,
      useClass: value => this.getApprovalClassCallback.call(this, value)
    }
  ];
  firstIndex: number = 0;
  sortDirection: number = 1;
  sortField: string = 'name';
  columnOptions: IColumnOptions = {};
  athletes: UserModelAthlete[];
  ownerId: string  = this.authService.getUser().userId;
  
  ngOnInit() {
    this.teamService.getTeamMembers().subscribe(r => (this.athletes = r));
  }

  approveAthlete(data: UserModelAthlete) {
    this.teamService
      .approveTeamMembership(data.userId)
      .subscribe(() => (data.teamMembershipApprovalStatus = ApprovalStatus.approved));
  }
  declineAthlete(data: UserModelAthlete) {
    this.teamService
      .declineTeamMembership(data.userId)
      .subscribe(() => this.athletes.splice(this.athletes.indexOf(data)));
  }

  private getClassCallback(value: boolean): string {
    let classes = 'fa fa-square-o';
    if (value) {
      classes = 'fa fa-check-square-o';
    }
    return `fa ${classes}`;
  }

  private getApprovalClassCallback(value: string): string {
    let classes = 'ui-g-12 ui-g-nopad text-align-center ';
    switch (value) {
      case ApprovalStatus.approved:
        classes += 'fas fa-check-circle';
        break;
      case ApprovalStatus.pending:
        classes += 'fas fa-clock';
        break;
      case ApprovalStatus.declined:
        classes += 'fas fa-times-circle';
        break;
    }
    return classes;
  }

  isAthleteApproved(data: UserModelAthlete) {
    return data.teamMembershipApprovalStatus == ApprovalStatus.approved;
  }
}
