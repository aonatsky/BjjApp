import { Component } from '@angular/core';
import { AuthService } from './../../core/services/auth.service';
import { LoggerService } from './../../core/services/logger.service';
import { RouterService } from './../../core/services/router.service';


@Component({
    selector: 'registration',
    templateUrl: './registration.component.html',
    styleUrls: ['./registration.component.css']
})

export class RegistrationComponent {


    email: string = "";
    password: string = "";
    confirmPassword: string = "";

    constructor(private AuthService: AuthService, private routerService: RouterService, private loggerService: LoggerService) {

    }


    register(): any {

    }

    processLogin(isAuthenticated: boolean) {
        if (isAuthenticated) {
            this.routerService.GoToDashboard();
        }
    }
}
