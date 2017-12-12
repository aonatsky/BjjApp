import { Injectable } from "@angular/core"
import { LoggerService } from "./logger.service"
import { HttpService, SearchParams } from "./../dal/http/http.service"
import { ParticipantRegistrationModel, ParticipantModelBase } from "./../model/participant.models"
import { ParticipantRegistrationResultModel } from "./../model/result/participant-registration-result.model"
import { ApiMethods } from "./../dal/consts/api-methods.consts"
import { Observable } from 'rxjs/Rx';


@Injectable()
export class ParticipantService {
    constructor(private loggerService: LoggerService, private httpService: HttpService) {

    }

    public processParticipantRegistration(participant: ParticipantRegistrationModel): Observable<ParticipantRegistrationResultModel> {
        return this.httpService.post(ApiMethods.participant.processParticipantRegistration, participant).map(r => this.httpService.getJson(r));
    }

    public isParticipantExists(participant: ParticipantModelBase): Observable<boolean> {
        return this.httpService.post(ApiMethods.participant.isParticipantExist, participant).map(r => this.httpService.getJson(r));
    }

    public getParticipantsTableModel(id: string): Observable<any> {
        let params: SearchParams[] = [{ name: "eventId", value: id }];
        return this.httpService.get(ApiMethods.participant.participantsTable, params).map(r => this.httpService.getJson(r));
    }

}