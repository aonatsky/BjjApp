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

  constructor(
    private routerService: RouterService,
    private authService: AuthService,
    private translateService: TranslateService
  ) {}

  ngOnInit() {
    this.isLoggedIn = this.authService.isLoggedIn();
    this.authService.getLoggedInStatus.subscribe(status => {
      this.isLoggedIn = status;
      this.initMenu();
    });
    this.initMenu();
  }

  initMenu() {
    this.items = [
      {
        label: this.translateService.instant('MENU.HOME'),
        routerLink: '/',
        visible: this.isLoggedIn && this.authService.getRole() in [Roles.Admin, Roles.FederationOwner, Roles.Owner]
      },
      {
        label: this.translateService.instant('MENU.LOGIN'),
        command: event => {
          this.routerService.goToLogin();
        },
        visible: !this.isLoggedIn && !this.routerService.isEventPortal()
      },
      {
        label: this.translateService.instant('MENU.LOGOUT'),
        routerLink: '/',
        visible: this.isLoggedIn,
        command: c => {
          this.authService.signout();
        }
      }
    ];
  }

  goHome(){
    // this.routerService.goHome();
    this.routerService.navigateByUrl('/');
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
