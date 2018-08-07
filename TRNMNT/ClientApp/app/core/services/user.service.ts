import { Injectable } from '@angular/core';
import { ApiMethods } from '../dal/consts/api-methods.consts';
import { Observable } from 'rxjs';
import { UserRegistrationModel, UserModel } from '../model/user.models';
import { AuthService } from './auth.service';
import { ChangePasswordModel } from '../model/change-password.model';
import { map, flatMap } from 'rxjs/operators';
import { HttpService } from '../dal/http/http.service';


@Injectable()
export class UserService {
  constructor(private http: HttpService, private authService : AuthService) {}

  register(model: UserRegistrationModel): Observable<string> {
    return this.http.post<string>(ApiMethods.auth.register, model);
  }

  updateUser(model: UserModel): Observable<any> {
    return this.http.post(ApiMethods.auth.updateProfile, model).pipe(flatMap(() => this.authService.getNewToken()));
  }

  changePassword(model: ChangePasswordModel): Observable<any> {
    return this.http.post(ApiMethods.auth.changePassword, model);
  }

  setPassword(model: ChangePasswordModel): Observable<any> {
    return this.http.post(ApiMethods.auth.setPassword, model);
  }

  getUser(){
      return this.authService.getUser();
  }
}
