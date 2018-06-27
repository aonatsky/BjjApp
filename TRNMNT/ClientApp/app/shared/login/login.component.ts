import { Component, OnInit } from '@angular/core';
import { AuthService } from './../../core/services/auth.service';
import { ActivatedRoute } from '@angular/router';
import { LoggerService } from './../../core/services/logger.service';
import { RouterService } from './../../core/services/router.service';
import { AuthService as SocialAuthService, FacebookLoginProvider } from 'angular5-social-login';

import './login.component.scss';

@Component({
    selector: 'login',
    templateUrl: './login.component.html',

})

export class LoginComponent {

    username: string = '';
    password: string = '';
    returnUrl: string;
    errorMessage: string;

    constructor(
        private route: ActivatedRoute,
        private authService: AuthService,
        private routerService: RouterService,
        private loggerService: LoggerService,
        private socialAuthService: SocialAuthService) {
    }

    ngOnInit() {
        // reset login status
        this.authService.signout();

        // get return url from route parameters or default to '/'
        this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
    }

    login(): any {
        this.authService.signin(this.username, this.password).subscribe(data => this.processLogin(data), error => this.loggerService.logError(error));
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
