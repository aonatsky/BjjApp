import { Injectable } from "@angular/core"
import { LoggerService } from "./logger.service"
import { HttpService, SearchParams } from "./../dal/http/http.service"
import { ParticipantRegistrationModel } from "./../model/participant-registration.model"
import { ParticipantRegistrationResultModel } from "./../model/result/participant-registration-result.model"
import { ApiMethods } from "./../dal/consts/api-methods.consts"
import { Observable } from 'rxjs/Rx';


@Injectable()
export class ParticipantService {
    constructor(private loggerService: LoggerService, private httpService: HttpService) {

    }

    public createParticipant(participant: ParticipantRegistrationModel): Observable<ParticipantRegistrationResultModel> {
        return this.httpService.post(ApiMethods.participant.registerParticipant, participant).map(r => this.httpService.getJson(r));
    }

}