import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../../core/services/auth.service';
import { LoggerService } from '../../core/services/logger.service';
import { RouterService } from '../../core/services/router.service';
import { TranslateService } from '@ngx-translate/core';
import { UserModel } from '../../core/model/user.models';

@Component({
  selector: 'login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  username: string = '';
  password: string = '';
  @Input()
  returnUrl: string = '/';
  @Input()
  returnUrlsByRoles: ReturnURLRoleModel[];
  @Input()
  socialLoginEnabled: boolean = true;
  @Input()
  registrationEnabled: boolean = true;
  errorMessage: string = '';

  @Output()
  onLogin: EventEmitter<UserModel>;

  constructor(
    private authService: AuthService,
    private routerService: RouterService,
    private loggerService: LoggerService,
    private translateService: TranslateService
  ) {}

  login(): any {
    this.errorMessage = '';
    this.authService.signin(this.username, this.password).subscribe(
      data => this.processLogin(data),
      error => {
        if (error.status === 401) {
          this.processLogin(false);
        } else {
          this.loggerService.logError(error);
        }
      }
    );
  }

  signUp(): any {
    this.routerService.goToRegistration(this.returnUrl);
  }

  processLogin(isAuthenticated: boolean) {
    if (isAuthenticated) {
      this.onLogin.emit(this.authService.getUser());
      let url = this.returnUrl;
      if (this.returnUrlsByRoles) {
        this.returnUrlsByRoles.forEach(m => {
          if (this.authService.ifRolesMatch(m.roles)) {
            url = m.returnUrl;
          }
        });
      }
      this.routerService.navigateByUrl(url);
    } else {
      this.errorMessage = this.translateService.instant('ERROR.EMAIL_OR_PASSWORD_IS_INVALID');
    }
  }

  facebookLogin() {
    this.authService.facebookLogin().subscribe((r: boolean) => {
      return this.processLogin(r);
    });
  }
}

export class ReturnURLRoleModel {
  roles: string[];
  returnUrl: string;
}
