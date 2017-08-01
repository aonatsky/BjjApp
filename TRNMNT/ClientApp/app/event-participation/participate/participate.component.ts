import { Component } from '@angular/core';
import { AuthService } from './../../core/services/auth.service';
import { LoggerService } from './../../core/services/logger.service';
import { RouterService } from './../../core/services/router.service';


@Component({
    selector: 'participate',
    templateUrl: './participate.component.html',
    styleUrls: ['./participate.component.css']
})

export class ParticipateComponent {


    email: string = "";
    password: string = "";
    confirmPassword: string = "";

    constructor(private AuthService: AuthService, private routerService: RouterService, private loggerService: LoggerService) {

    }


    register(): any {

    }

    processLogin(isAuthenticated: boolean) {
        if (isAuthenticated) {
            this.routerService.GoToEventAdmin();
        }
    }
}
