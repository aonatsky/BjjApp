import { Injectable } from "@angular/core"
import { LoggerService } from "./logger.service"
import { HttpService } from "./../dal/http/http.service"
import { ParticipantRegistrationModel, ParticipantModelBase, ParticipantTableModel, ParticipantDdlModel } from "./../model/participant.models"
import { ParticipantRegistrationResultModel } from "./../model/result/participant-registration-result.model"
import { ApiMethods } from "./../dal/consts/api-methods.consts"
import { Observable } from 'rxjs/Rx';
import { PagedList } from "../model/paged-list.model";
import { ParticipantFilterModel } from "../model/participant-filter.model";
import { IUploadResult } from "../model/result/upload-result.model";



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

    public getParticipantsDropdownData(eventId: string): Observable<ParticipantDdlModel> {
        return this.httpService.get(ApiMethods.participant.participantsDropdownData, { eventId: eventId }).map(r => this.httpService.getJson(r));
    }

    public uploadParticipantsFromFile(file: any, eventId: string): Observable<IUploadResult> {
        return this.httpService.postFile(`${ApiMethods.participant.uploadParticipantsFromFile}/${eventId}`, file).map(r => this.httpService.getJson(r));
    }

    public updateParticipant(participant: ParticipantTableModel): Observable<any> {
        return this.httpService.put(ApiMethods.participant.update, participant);
    }

    public deleteParticipant(participantId: string): Observable<any> {
        return this.httpService.deleteById(ApiMethods.participant.delete, participantId);
    }

}