import { Component, OnInit } from '@angular/core';
import { AuthService } from './../../core/services/auth.service';
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
  returnUrl: string;

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

  register(): any {
    this.authService.register(this.model).subscribe(r => {
      this.authService.signin(this.model.email, this.model.password).subscribe((r: boolean) => {
        if (r === true) {
          this.routerService.navigateByUrl(this.returnUrl);
        }
      });
    });
  }
}
