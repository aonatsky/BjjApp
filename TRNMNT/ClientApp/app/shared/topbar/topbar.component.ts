import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { UserModel } from '../../core/model/user.models';
import { AuthService } from '../../core/services/auth.service';
import { RouterService } from '../../core/services/router.service';
import { MenuItem } from 'primeng/components/common/menuitem';

@Component({
  selector: 'topbar',
  templateUrl: './topbar.component.html',
  styleUrls: ['./topbar.component.scss']
})
export class TopbarComponent implements OnInit {
  user: UserModel;
  items: MenuItem[];

  constructor(private routerService: RouterService, private authService: AuthService) {}

  ngOnInit() {
    this.user = this.authService.getUser();
    const isLoggedIn = this.authService.isLoggedIn();
    this.items = [
      {
        label: 'Home',
        routerLink: '/'
      },
      //{
      //    label: 'Events',
      //    routerLink: '/'
      //},
      //{
      //    label: 'About',
      //    routerLink: '/'
      //},
      {
        label: isLoggedIn ? 'Logout' : 'Login',
        command: event => {
          if (this.authService.isLoggedIn()) {
            this.authService.signout();
          }
          this.routerService.goToLogin();
        }
      }
    ];
  }

  private goHome() {
    this.routerService.goHome();
  }

  private isTopbarShown(): boolean {
    const urlsToHideMenu = ['run-wd-spectator-view', 'run-category-spectator-view'];
    for (let i = 0; i < urlsToHideMenu.length; i++) {
      if (this.routerService.getCurrentUrl().indexOf(urlsToHideMenu[i]) !== -1) {
        return false;
      }
    }
    return true;
  }
}
