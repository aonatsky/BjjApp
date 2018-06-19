import { Component } from '@angular/core';
import { AuthService } from './../../core/services/auth.service';
import { LoggerService } from './../../core/services/logger.service';
import { RouterService } from './../../core/services/router.service';


@Component({
    selector: 'register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.css']
})

export class RegisterComponent {


    email: string = '';
    confirmEmail: string = '';
    password: string = '';
    confirmPassword: string = '';

    constructor(private authService: AuthService, private routerService: RouterService, private loggerService: LoggerService) {

    }


    register(): any {
        if (this.email != this.confirmEmail) {
            //to show error
        } else if (this.password != this.confirmPassword) {
            //to show error
        } else {
            this.authService.register(this.email, this.password).subscribe(r => this.authService.login(this.email, this.password));
        }
        
    }

    
}
