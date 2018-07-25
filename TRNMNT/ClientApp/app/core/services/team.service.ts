import { Injectable } from '@angular/core';
import { HttpService } from '../dal/http/http.service';
import { TeamModel, TeamRegistrationModel } from '../model/team.model';
import { ApiMethods } from '../dal/consts/api-methods.consts';
import { Observable } from 'rxjs';

@Injectable()
export class TeamService {
  constructor(private httpService: HttpService) {}

  getTeams(): Observable<TeamModel[]> {
    return this.httpService.get<TeamModel[]>(ApiMethods.team.getTeams);
  }

  addTeam(team: TeamModel): Observable<any> {
    return this.httpService.post(ApiMethods.team.getTeams, team);
  }

  processTeamRegistration(model: TeamRegistrationModel) {
    return this.httpService.post(ApiMethods.team.processTeamRegistration, model);
  }
}
