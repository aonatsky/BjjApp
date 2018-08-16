import { Component, OnInit } from '@angular/core';
import { TeamService } from '../../core/services/team.service';
import { TeamModelFull } from '../../core/model/team.model';
import { ICrudColumn as CrudColumn, IColumnOptions } from '../../shared/crud/crud.component';
import { TranslateService } from '@ngx-translate/core';
import { ApprovalStatus } from '../../core/consts/approval-status.const';

@Component({
  selector: 'app-team-list',
  templateUrl: './team-list.component.html',
  styleUrls: ['./team-list.component.scss']
})
export class TeamListComponent implements OnInit {
  teams: TeamModelFull[];
  columns: CrudColumn[] = [
    {
      propertyName: 'name',
      displayName: this.translateService.instant('TEAM.NAME'),
      isEditable: false,
      isSortable: true
    },
    {
      propertyName: 'contactName',
      displayName: this.translateService.instant('TEAM.CONTACT_NAME'),
      isEditable: false,
      isSortable: true
    },
    {
      propertyName: 'contactPhone',
      displayName: this.translateService.instant('TEAM.CONTACT_PHONE'),
      isEditable: false,
      isSortable: true
    },
    {
      propertyName: 'contactEmail',
      displayName: this.translateService.instant('TEAM.CONTACT_EMAIL'),
      isEditable: false,
      isSortable: true
    },
    {
      propertyName: 'approvalStatus',
      displayName: this.translateService.instant('COMMON.APPROVAL.PAYMENT_APPROVAL_STATUS'),
      isEditable: false,
      isSortable: true,
      transform: value => this.translateService.instant(value)
    }
  ];
  firstIndex: number = 0;
  sortDirection: number = 1;
  sortField: string = 'name';
  columnOptions: IColumnOptions = {};
  approvalStatus = ApprovalStatus;

  constructor(private teamService: TeamService, private translateService: TranslateService) {}

  ngOnInit() {
    this.teamService.getTeamsForAdmin().subscribe(data => (this.teams = data));
  }

  approveTeam(data) {
    this.teamService.approveTeam(data.teamId).subscribe(() => {
      data.federationApprovalStatus = ApprovalStatus.approved;
    });
  }

  declineTeam(data) {
    this.teamService.declineTeam(data.teamId).subscribe(() => {
      data.federationApprovalStatus = ApprovalStatus.declined;
    });
  }

  getApproveButtonStyleClass(buttonType: string, data: TeamModelFull) {
    switch (buttonType) {
      case 'pending': {
        return data.federationApprovalStatus == ApprovalStatus.pending ? 'status-pending' : '';
      }
      case 'decline': {
        return data.federationApprovalStatus == ApprovalStatus.declined ? 'status-declined' : '';
      }
      case 'approve':{
        return data.federationApprovalStatus == ApprovalStatus.approved? 'status-approved' :'' 
      }
    }
    return ''
  }
}
