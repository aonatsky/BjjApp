import { Component, OnInit } from '@angular/core';
import { TeamService } from '../../core/services/team.service';
import { TeamModelFull } from '../../core/model/team.model';
import { ICrudColumn as CrudColumn, IColumnOptions } from '../../shared/crud/crud.component';
import { TranslateService } from '@ngx-translate/core';

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
      displayName: this.translateService.instant('COMMON.APPROVAL.APPROVAL_STATUS'),
      isEditable: false,
      isSortable: true,
      transform: value => this.translateService.instant(value)
    }
  ];
  firstIndex: number = 0;
  sortDirection: number = 1;
  sortField: string = 'name';
  columnOptions: IColumnOptions = {};

  constructor(private teamService: TeamService, private translateService: TranslateService) {}

  ngOnInit() {
    this.teamService.getTeamsForAdmin().subscribe(data => (this.teams = data));
  }

  test(data){
    console.log(data);
  }
}
