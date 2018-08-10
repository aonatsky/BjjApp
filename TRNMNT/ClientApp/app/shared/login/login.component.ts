import { Component, OnInit, Input } from '@angular/core';
import { AuthService } from '../../core/services/auth.service';
import { ActivatedRoute } from '@angular/router';
import { LoggerService } from '../../core/services/logger.service';
import { RouterService } from '../../core/services/router.service';
import { TranslateService } from '../../../../node_modules/@ngx-translate/core';

@Component({
  selector: 'login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  username: string = '';
  password: string = '';
  @Input()
  returnUrl: string;
  @Input()
  socialLoginEnabled: boolean = true;
  @Input()
  registrationEnabled: boolean = true;
  errorMessage: string = '';

  constructor(
    private authService: AuthService,
    private routerService: RouterService,
    private loggerService: LoggerService,
    private translateService: TranslateService
  ) {}

  ngOnInit() {}

  login(): any {
    this.errorMessage = '';
    this.authService.signin(this.username, this.password).subscribe(
      data => this.processLogin(data),
      error => {
        if ((error.status = 401)) {
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
      if (this.returnUrl) {
        this.routerService.navigateByUrl(this.returnUrl);
      }
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
