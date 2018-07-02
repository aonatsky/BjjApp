import { Injectable } from '@angular/core'
import { LoggerService } from './logger.service'
import { HttpService } from './../dal/http/http.service'
import { TeamModel } from './../model/team.model'
import { ApiMethods } from './../dal/consts/api-methods.consts'
import { Observable } from 'rxjs';


@Injectable()
export class TeamService {
    constructor(private loggerService: LoggerService, private httpService: HttpService) {

    }

    getTeams(): Observable<TeamModel[]> {
        return this.httpService.get<TeamModel[]>(ApiMethods.team.getTeams);
    }

    addTeam(team: TeamModel): Observable<any> {
        return this.httpService.post(ApiMethods.team.getTeams, team);
    }

}