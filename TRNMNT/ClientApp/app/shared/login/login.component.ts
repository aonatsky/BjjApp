import { Component } from '@angular/core';
import { AuthenticationService } from './../../core/services/authentication.service';
import { RouterService } from './../../core/services/router.service';


@Component({
    selector: 'login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']
})

export class LoginComponent {

    username: string = "";
    password: string = "";

    constructor(private authenticationService: AuthenticationService, private routerService: RouterService) {

    }


    login(): any {

        this.authenticationService.signin(this.username, this.password).subscribe(data => this.processLogin(data));

    }

    processLogin(isAuthenticated: boolean) {
        if (isAuthenticated) {
            this.routerService.GoToDashboard();
        }
    }
}
