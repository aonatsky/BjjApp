import { Component, OnInit, Input } from '@angular/core';
import { AuthService } from './../../core/services/auth.service';
import { ActivatedRoute } from '@angular/router';
import { LoggerService } from './../../core/services/logger.service';
import { RouterService } from './../../core/services/router.service';

@Component({
  selector: 'login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  username: string = '';
  password: string = '';
  @Input() returnUrl: string;
  errorMessage: string;

  constructor(
    private route: ActivatedRoute,
    private authService: AuthService,
    private routerService: RouterService,
    private loggerService: LoggerService
  ) {}

  ngOnInit() {
    // reset login status
    this.authService.signout();
    // get return url from route parameters or default to '/'
    
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
      this.routerService.navigateByUrl(this.returnUrl);
    } else {
      this.errorMessage = 'Auuthentication failed, please check your credentials';
    }
  }

  facebookLogin() {
    this.authService.facebookLogin().subscribe(r => this.processLogin);
  }
}
