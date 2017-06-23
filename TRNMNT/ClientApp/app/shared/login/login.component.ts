import { Component } from '@angular/core';
import { AuthenticationService } from './../../core/services/authentication.service';


@Component({
    selector: 'login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']
})

export class LoginComponent {

    username: string = "";
    password: string = "";

    constructor(private authenticationService: AuthenticationService) {

    }


    login(): any {

        this.authenticationService.signin(this.username, this.password).subscribe();

    }
}
