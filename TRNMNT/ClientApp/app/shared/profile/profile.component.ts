import { Component, OnInit } from '@angular/core';
import { UserModel, UserModelAthlete } from '../../core/model/user.models';
import { UserService } from '../../core/services/user.service';
import { ChangePasswordModel } from '../../core/model/change-password.model';
import { TeamModel } from '../../core/model/team.model';
import { TeamService } from '../../core/services/team.service';
import { TranslateService } from '@ngx-translate/core';
import { SelectItem } from 'primeng/primeng';
import { ApprovalStatus } from '../../core/consts/approval-status.const';
import { ActivatedRoute } from '@angular/router';
import { RouterService } from '../../core/services/router.service';

@Component({
  selector: 'profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  userModel: UserModelAthlete = new UserModelAthlete();
  changePasswordModel: ChangePasswordModel = new ChangePasswordModel();
  displayPopup: boolean = false;
  teamSelectItems: SelectItem[];
  teams: TeamModel[];
  returnUrl: string;

  constructor(
    private userService: UserService,
    private teamService: TeamService,
    private translateService: TranslateService,
    private route: ActivatedRoute,
    private routerService: RouterService
  ) {}

  ngOnInit() {
    this.returnUrl = this.route.snapshot.queryParams.returnUrl;
    this.userService.getUserAthlete().subscribe(r => (this.userModel = r));
    this.teamService.getTeamsForEvent().subscribe(r => {
      this.teams = r;
      this.initTeamDropdown(r);
    });
  }

  save() {
    debugger;
    this.userService.updateUser(this.userModel).subscribe();
  }

  saveAndContinue(){
    this.userService.updateUser(this.userModel).subscribe(() => this.routerService);

  }

  changePassword() {
    this.changePasswordModel.userId = this.userModel.userId;
    this.userService.changePassword(this.changePasswordModel).subscribe();
  }

  openChangePasswordPopup() {
    this.displayPopup = true;
  }

  getIsApproved(teamId): boolean {
    if (!teamId) {
      return true;
    }
    const team = this.teams.filter(t => (t.teamId == teamId))[0];
    return team.federationApprovalStatus == ApprovalStatus.approved && team.approvalStatus == ApprovalStatus.approved;
  }

  private initTeamDropdown(teams: TeamModel[]) {
    this.teamSelectItems = [];
    this.teamSelectItems.push({ label: this.translateService.instant('COMMON.NO_TEAM'), value: null });
    for (const team of teams) {
      this.teamSelectItems.push({ label: team.name, value: team.teamId });
    }
  }
}
