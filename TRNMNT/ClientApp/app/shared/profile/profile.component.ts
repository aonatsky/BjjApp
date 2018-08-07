import { Component, OnInit } from '@angular/core';
import { UserModel } from '../../core/model/user.models';
import { UserService } from '../../core/services/user.service';
import { ChangePasswordModel } from '../../core/model/change-password.model';

@Component({
  selector: 'profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  userModel: UserModel;
  changePasswordModel: ChangePasswordModel = new ChangePasswordModel();
  displayPopup: boolean = false;
  constructor(private userService: UserService) {
    this.userModel = userService.getUser();
  }
  save() {
    this.userService.updateUser(this.userModel).subscribe();
  }
  changePassword() {
    this.changePasswordModel.userId == this.userModel.userId;
    this.userService.changePassword(this.changePasswordModel).subscribe();
  }

  openChangePasswordPopup() {
    this.displayPopup = true;
  }

  ngOnInit() {}
}
