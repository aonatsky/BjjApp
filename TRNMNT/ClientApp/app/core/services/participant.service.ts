import { Injectable } from "@angular/core"
import { LoggerService } from "./logger.service"
import { HttpService, SearchParams } from "./../dal/http/http.service"
import { ParticipantModel } from "./../model/participant.model"
import { ApiMethods } from "./../dal/consts/api-methods.consts"
import { Observable } from 'rxjs/Rx';


@Injectable()
export class ParticipantService {
    constructor(private loggerService: LoggerService, private httpService: HttpService) {

    }

    public createParticipant(participant: ParticipantModel): Observable<any> {
        return this.httpService.post(ApiMethods.participant.participant, participant);
    }
   
}