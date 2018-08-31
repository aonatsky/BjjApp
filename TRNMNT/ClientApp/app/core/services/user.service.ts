import { Injectable } from '@angular/core';
import { ApiMethods } from '../dal/consts/api-methods.consts';
import { Observable } from 'rxjs';
import { UserModelRegistration, UserModel, UserModelAthlete } from '../model/user.models';
import { AuthService } from './auth.service';
import { ChangePasswordModel } from '../model/change-password.model';
import { map, flatMap } from 'rxjs/operators';
import { HttpService } from '../dal/http/http.service';

@Injectable()
export class UserService {
  IsParticipant: boolean = undefined;
  constructor(private http: HttpService, private authService: AuthService) {}

  register(model: UserModelRegistration): Observable<string> {
    return this.http.post<string>(ApiMethods.user.register, model);
  }

  updateUser(model: UserModel): Observable<any> {
    return this.http.post(ApiMethods.user.updateProfile, model).pipe(flatMap(() => this.authService.getNewToken()));
  }

  changePassword(model: ChangePasswordModel): Observable<any> {
    return this.http.post(ApiMethods.user.changePassword, model);
  }

  setPassword(model: ChangePasswordModel): Observable<any> {
    return this.http.post(ApiMethods.user.setPassword, model);
  }

  getUser() {
    return this.authService.getUser();
  }

  getIsParticipant(): Observable<boolean> {
    return this.http.get<boolean>(ApiMethods.participant.isParticipantExist);
  }

  getUserAthlete(): Observable<UserModelAthlete> {
    return this.http.get<UserModelAthlete>(ApiMethods.team.getCurrentAthlete);
  }
}
