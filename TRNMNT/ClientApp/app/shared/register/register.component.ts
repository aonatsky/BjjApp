import { Component, OnInit } from '@angular/core';
import { AuthService } from './../../core/services/auth.service';
import { LoggerService } from './../../core/services/logger.service';
import { RouterService } from './../../core/services/router.service';
import { StateService } from '../../services/state.service';
import { UserRegistrationModel } from '../../core/model/user.models';

@Component({
  selector: 'register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  model: UserRegistrationModel;
  email: string = '';
  confirmEmail: string = '';
  password: string = '';
  confirmPassword: string = '';

  constructor(
    private authService: AuthService,
    private routerService: RouterService,
    private stateSerivce: StateService
  ) {}

  ngOnInit(): void {
    this.model = this.stateSerivce.getValue('socialUser');
    if (!this.model) {
      this.model = new UserRegistrationModel();
    }
  }
  // private initData() {
  //     this.route.params.subscribe(p => {
  //         const id = p['id'];
  //         if (id && id != '') {
  //             this.eventService.getEvent(id).subscribe(r => this.eventModel = r);
  //         } else {
  //             alert('No data to display');
  //         }
  //     });
  // }

  register(): any {
    if (this.email != this.confirmEmail) {
      //to show error
    } else if (this.password != this.confirmPassword) {
      //to show error
    } else {
      this.authService
        .register(this.email, this.password)
        .subscribe(r => this.authService.login(this.email, this.password));
    }
  }
}
