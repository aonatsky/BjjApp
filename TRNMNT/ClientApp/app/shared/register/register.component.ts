import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../core/services/auth.service';
import { RouterService } from '../../core/services/router.service';
import { UserModelRegistration } from '../../core/model/user.models';
import { UserService } from '../../core/services/user.service';
import { ActivatedRoute } from '@angular/router';
import DateHelper from '../../core/helpers/date-helper';
import { TeamModel } from '../../core/model/team.model';
import { TeamService } from '../../core/services/team.service';
import { TranslateService } from '@ngx-translate/core';
import { SelectItem } from 'primeng/primeng';
import { ApprovalStatus } from '../../core/consts/approval-status.const';

@Component({
  selector: 'register',
  templateUrl: './register.component.html'
})
export class RegisterComponent implements OnInit {
  model: UserModelRegistration;
  teams: TeamModel[];
  returnUrl: string;
  dateHelper = DateHelper;
  teamSelectItems: SelectItem[] = [];
  constructor(
    private authService: AuthService,
    private routerService: RouterService,
    private userService: UserService,
    private route: ActivatedRoute,
    private teamService: TeamService,
    private translateService: TranslateService
  ) {}

  ngOnInit(): void {
    this.model = new UserModelRegistration();
    this.teamService.getTeamsForEvent().subscribe(r => {
      this.teams = r;
      this.initTeamDropdown(r)});
    this.returnUrl = this.route.snapshot.queryParams.returnUrl || '/';
  }

  register(): any {
    this.userService.register(this.model).subscribe(r => {
      this.authService.signin(this.model.email, this.model.password).subscribe((r: boolean) => {
        if (r === true) {
          if (this.model.isTeamOwner) {
            this.routerService.goToTeamRegistration(this.returnUrl);
          } else {
            this.routerService.navigateByUrl(this.returnUrl);
          }
        }
      });
    });
  }

  private initTeamDropdown(teams: TeamModel[]) {
    this.teamSelectItems = [];
    this.teamSelectItems.push({ label: this.translateService.instant('COMMON.NO_TEAM'), value: null });
    for (const team of teams) {
      this.teamSelectItems.push({ label: team.name, value: team.teamId });
    }
  }

  setNoTeam() {
    if (this.model.isTeamOwner) {
      this.model.teamId = null;
    }
  }

  getIsApproved(teamId): boolean{
    if(!teamId){
      return true;
    }
    const team = this.teams.filter(t => t.teamId = teamId)[0];
    return team.federationApprovalStatus == ApprovalStatus.approved && team.approvalStatus == ApprovalStatus.approved;
  }
}
