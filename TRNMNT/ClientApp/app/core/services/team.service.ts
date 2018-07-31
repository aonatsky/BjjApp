﻿import { Injectable } from '@angular/core';
import { HttpService } from '../dal/http/http.service';
import { TeamModel, TeamRegistrationModel } from '../model/team.model';
import { ApiMethods } from '../dal/consts/api-methods.consts';
import { Observable } from 'rxjs';
import { PriceModel } from '../model/price.model';

@Injectable()
export class TeamService {
  constructor(private httpService: HttpService) {}

  getTeams(): Observable<TeamModel[]> {
    return this.httpService.get<TeamModel[]>(ApiMethods.team.getTeams);
  }

  addTeam(team: TeamModel): Observable<any> {
    return this.httpService.post(ApiMethods.team.getTeams, team);
  }

  processTeamRegistration(model: TeamRegistrationModel): Observable<any> {
    return this.httpService.post(ApiMethods.team.processTeamRegistration, model);
  }
  getTeamRegistrationPrice(): Observable<PriceModel> {
    return this.httpService.get<PriceModel>(ApiMethods.team.getTeamRegistrationPrice);
  }
}
