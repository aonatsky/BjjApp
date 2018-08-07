import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../core/services/auth.service';
import { RouterService } from '../../core/services/router.service';
import { UserRegistrationModel } from '../../core/model/user.models';
import { UserService } from '../../core/services/user.service';

@Component({
  selector: 'register',
  templateUrl: './register.component.html',
})
export class RegisterComponent implements OnInit {
  model: UserRegistrationModel;
  returnUrl: string;

  constructor(
    private authService: AuthService,
    private routerService: RouterService,
    private userService: UserService,
    
  ) {}

  ngOnInit(): void {
    this.model = new UserRegistrationModel();
  }

  register(): any {
    this.userService.register(this.model).subscribe(r => {
      this.authService.signin(this.model.email, this.model.password).subscribe((r: boolean) => {
        if (r === true) {
          this.routerService.navigateByUrl(this.returnUrl);
        }
      });
    });
  }
}
