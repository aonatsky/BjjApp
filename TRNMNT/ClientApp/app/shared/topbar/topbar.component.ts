import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { AuthService } from '../../core/services/auth.service';
import { RouterService } from '../../core/services/router.service';
import { MenuItem } from 'primeng/components/common/menuitem';
import { TranslateService } from '@ngx-translate/core';
import { Roles } from '../../core/consts/roles.const';

@Component({
  selector: 'topbar',
  templateUrl: './topbar.component.html',
  styleUrls: ['./topbar.component.scss']
})
export class TopbarComponent implements OnInit {
  items: MenuItem[];
  isLoggedIn: boolean;
  role: string;

  constructor(
    private routerService: RouterService,
    private authService: AuthService,
    private translateService: TranslateService
  ) {}

  ngOnInit() {
    this.authService.getLoggedInStatus.subscribe(status => {
      this.initMenu(status);
    });
    this.initMenu(this.authService.isLoggedIn());
  }

  initMenu(loggedIn: boolean) {
    this.isLoggedIn = loggedIn;
    this.items = [
      {
        label: this.translateService.instant('MENU.HOME'),
        routerLink: ['/'],
        visible: this.isLoggedIn && this.authService.ifRolesMatch([Roles.Admin, Roles.FederationOwner, Roles.Owner])
      },
      {
        label: this.translateService.instant('MENU.TEAMS'),
        routerLink: ['event-admin/teams'],
        visible: this.isLoggedIn && this.authService.ifRolesMatch([Roles.Admin, Roles.FederationOwner, Roles.Owner])
      },
      {
        label: this.translateService.instant('MENU.PROFILE'),
        routerLink: ['profile'],
        visible: this.isLoggedIn
      },
      {
        label: this.translateService.instant('MENU.MY_EVENTS'),
        routerLink: ['participant/my-events'],
        visible: this.isLoggedIn && this.authService.ifRolesMatch([Roles.TeamOwner, Roles.Participant])
      },
      {
        label: this.translateService.instant('MENU.MY_TEAM'),
        routerLink: ['participant/my-team'],
        visible: this.isLoggedIn && this.authService.ifRolesMatch([Roles.TeamOwner])
      },
      {
        label: this.translateService.instant('MENU.FEDERATION'),
        routerLink: ['event-admin/federation-edit'],
        visible: this.isLoggedIn && this.authService.ifRolesMatch([Roles.FederationOwner, Roles.Admin])
      },
      {
        label: this.translateService.instant('MENU.LOGIN'),
        command: event => {
          this.routerService.goToLogin();
        },
        visible: !this.isLoggedIn
      },
      {
        label: this.translateService.instant('MENU.LOGOUT'),
        visible: this.isLoggedIn,
        command: c => {
          this.authService.signout();
        }
      }
    ];
  }

  getHomeLink(): string {
    if (this.routerService.isEventPortal()) {
      return '/event';
    }
    return '/';
  }

  isTopbarShown(): boolean {
    const urlsToHideMenu = ['run-wd-spectator-view', 'run-category-spectator-view'];
    for (let i = 0; i < urlsToHideMenu.length; i++) {
      if (this.routerService.getCurrentUrl().indexOf(urlsToHideMenu[i]) !== -1) {
        return false;
      }
    }
    return true;
  }
}
