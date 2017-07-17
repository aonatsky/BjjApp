import { Component } from '@angular/core';
import { AuthService } from './../../core/services/auth.service';
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

    constructor(private AuthService: AuthService, private routerService: RouterService, private loggerService: LoggerService) {

    }


    login(): any {

        this.AuthService.signin(this.username, this.password).subscribe(data => this.processLogin(data), error => this.loggerService.logError(error));

    }

    processLogin(isAuthenticated: boolean) {
        if (isAuthenticated) {
            this.routerService.GoToDashboard();
        }
    }
}
