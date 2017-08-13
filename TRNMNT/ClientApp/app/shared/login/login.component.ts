import { Component, OnInit } from '@angular/core';
import { AuthService } from './../../core/services/auth.service';
import { ActivatedRoute } from '@angular/router';
import { LoggerService } from './../../core/services/logger.service';
import { RouterService } from './../../core/services/router.service';


@Component({
    selector: 'login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']
})

export class LoginComponent {

    username: string = "";
    password: string = "";
    returnUrl: string;

    constructor(
        private route: ActivatedRoute,
        private AuthService: AuthService,
        private routerService: RouterService,
        private loggerService: LoggerService) {

    }

    ngOnInit() {
        // reset login status
        this.AuthService.signout();

        // get return url from route parameters or default to '/'
        this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
    }


    login(): any {

        this.AuthService.signin(this.username, this.password).subscribe(data => this.processLogin(data), error => this.loggerService.logError(error));

    }

    processLogin(isAuthenticated: boolean) {
        if (isAuthenticated) {
            this.routerService.navigateByUrl(this.returnUrl);
        }
    }
}
