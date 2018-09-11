import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Roles } from '../../core/consts/roles.const';
import { AuthService } from '../../core/services/auth.service';
import { RouterService } from '../../core/services/router.service';
@Component({
  selector: 'login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.scss']
})
export class LoginPageComponent implements OnInit {
  returnUrl: string;
  loginReturnUrls = [
    { roles: [Roles.TeamOwner, Roles.Participant], returnUrl: '/participant/my-events' },
    { roles: [Roles.Admin, Roles.FederationOwner, Roles.Owner], returnUrl: '/event-admin/event-list' }
  ];

  constructor(private route: ActivatedRoute, private authService: AuthService, private routerService: RouterService) {}

  ngOnInit() {
    this.returnUrl = this.route.snapshot.queryParams.returnUrl;
    if (this.authService.isLoggedIn()) {
      let url = '/';
      this.loginReturnUrls.forEach(r => {
        if (this.authService.ifRolesMatch(r.roles)) {
          url = r.returnUrl;
        }
      });
      this.routerService.navigateByUrl(url);
    }
  }
}
