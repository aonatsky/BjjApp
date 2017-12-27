import { Injectable } from "@angular/core"
import { LoggerService } from "./logger.service"
import { HttpService } from "./../dal/http/http.service"
import { ParticipantRegistrationModel, ParticipantModelBase, ParticipantTableModel } from "./../model/participant.models"
import { ParticipantRegistrationResultModel } from "./../model/result/participant-registration-result.model"
import { ApiMethods } from "./../dal/consts/api-methods.consts"
import { Observable } from 'rxjs/Rx';
import { PagedList } from "../model/paged-list.model";
import { ParticipantFilterModel } from "../model/participant-filter.model";



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

    public getParticipantsTableModel(filterModel: ParticipantFilterModel): Observable<PagedList<ParticipantTableModel>> {
        return this.httpService.get(ApiMethods.participant.participantsTable, filterModel).map(r => this.httpService.getJson(r));
    }

}