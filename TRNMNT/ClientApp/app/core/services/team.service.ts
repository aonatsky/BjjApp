import { Injectable } from '@angular/core';
import { HttpService } from '../dal/http/http.service';
import { TeamModel, TeamRegistrationModel, TeamModelFull } from '../model/team.model';
import { ApiMethods } from '../dal/consts/api-methods.consts';
import { Observable } from 'rxjs';
import { PriceModel } from '../model/price.model';
import { UserModelAthlete } from '../model/user.models';

@Injectable()
export class TeamService {
  constructor(private httpService: HttpService) {}

  getTeamsForEvent(): Observable<TeamModel[]> {
    return this.httpService.get<TeamModel[]>(ApiMethods.team.getTeamsForEvent);
  }

  getTeamsForAdmin(): Observable<TeamModelFull[]> {
    return this.httpService.get<TeamModelFull[]>(ApiMethods.team.getTeamsForAdmin);
  }

  processTeamRegistration(model: TeamRegistrationModel): Observable<any> {
    return this.httpService.post(ApiMethods.team.processTeamRegistration, model);
  }

  getTeamRegistrationPrice(): Observable<PriceModel> {
    return this.httpService.get<PriceModel>(ApiMethods.team.getTeamRegistrationPrice);
  }

  getTeamMembers(): Observable<UserModelAthlete[]> {
    return this.httpService.get<UserModelAthlete[]>(ApiMethods.team.getTeamMembers);
  }

  approveTeamMembership(userId: string): Observable<any> {
    return this.httpService.post(`${ApiMethods.team.approveTeamMembership}/${userId}`);
  }
  declineTeamMembership(userId: string): Observable<any> {
    return this.httpService.post(`${ApiMethods.team.declineTeamMembership}/${userId}`);
  }
}
