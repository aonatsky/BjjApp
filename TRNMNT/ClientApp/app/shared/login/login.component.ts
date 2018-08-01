import { Component, OnInit, Input } from '@angular/core';
import { AuthService } from '../../core/services/auth.service';
import { ActivatedRoute } from '@angular/router';
import { LoggerService } from '../../core/services/logger.service';
import { RouterService } from '../../core/services/router.service';

@Component({
  selector: 'login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  username: string = '';
  password: string = '';
  @Input() returnUrl: string;
  @Input() socialLoginEnabled: boolean = true;
  errorMessage: string;

  constructor(
    private route: ActivatedRoute,
    private authService: AuthService,
    private routerService: RouterService,
    private loggerService: LoggerService
  ) {}

  ngOnInit() {
    // this.authService.signout();
  }

  login(): any {
    this.authService
      .signin(this.username, this.password)
      .subscribe(data => this.processLogin(data), error => this.loggerService.logError(error));
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
      this.errorMessage = 'Authentication failed, please check your credentials';
    }
  }

  facebookLogin() {
    this.authService.facebookLogin().subscribe((r: boolean) => {
      return this.processLogin(r);
    });
  }
}
