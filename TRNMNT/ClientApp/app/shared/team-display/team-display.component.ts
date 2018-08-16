import { Component, OnInit, Input } from '@angular/core';
import { TeamModelBase } from '../../core/model/team.model';
import { ApprovalStatus } from '../../core/consts/approval-status.const';
@Component({
  selector: 'team-display',
  templateUrl: './team-display.component.html'
})
export class TeamDisplayComponent implements OnInit {
  constructor() {}

  @Input()
  team: TeamModelBase;

  ngOnInit() {}

  isApproved() {
    return (
      this.team.federationApprovalStatus == ApprovalStatus.approved &&
      this.team.approvalStatus == ApprovalStatus.approved
    );
  }
}
