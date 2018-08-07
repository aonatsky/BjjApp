import { Injectable } from '@angular/core';
import { LoggerService } from './logger.service';
import { HttpService } from '../dal/http/http.service';
import {
  ParticipantRegistrationModel,
  ParticipantModelBase,
  ParticipantTableModel
} from '../model/participant.models';
import { ParticipantDdlModel } from '../model/participant-ddl.model';
import { ApiMethods } from '../dal/consts/api-methods.consts';
import { Observable } from 'rxjs';
import { PagedList } from '../model/paged-list.model';
import { ParticipantFilterModel } from '../model/participant-filter.model';
import { IUploadResult } from '../model/result/upload-result.model';
import { PaymentDataModel } from '../model/payment-data.model';

@Injectable()
export class ParticipantService {
  constructor(private loggerService: LoggerService, private httpService: HttpService) {}

  processParticipantRegistration(
    participant: ParticipantRegistrationModel
  ): Observable<PaymentDataModel> {
    return this.httpService.post<PaymentDataModel>(ApiMethods.participant.processParticipantRegistration, participant);
  }

  isParticipantExists(participant: ParticipantModelBase): Observable<boolean> {
    return this.httpService.post<boolean>(ApiMethods.participant.isParticipantExist, participant);
  }

  getParticipantsTableModel(filterModel: ParticipantFilterModel): Observable<PagedList<ParticipantTableModel>> {
    return this.httpService.post<PagedList<ParticipantTableModel>>(
      ApiMethods.participant.participantsTable,
      filterModel,
      null,
      'Could not load participants data'
    );
  }

  getParticipantsDropdownData(eventId: string): Observable<ParticipantDdlModel> {
    return this.httpService.get<ParticipantDdlModel>(
      ApiMethods.participant.participantsDropdownData,
      { eventId },
      'Could not load categories and weight divisions data'
    );
  }

  uploadParticipantsFromFile(file: any, eventId: string): Observable<IUploadResult> {
    return this.httpService.postFile<IUploadResult>(`${ApiMethods.participant.uploadParticipantsFromFile}/${eventId}`, file);
  }

  updateParticipant(participant: ParticipantTableModel): Observable<any> {
    return this.httpService.put(ApiMethods.participant.update, participant, 'Could not update participant');
  }

  deleteParticipant(participantId: string): Observable<any> {
    return this.httpService.deleteById(ApiMethods.participant.delete, participantId, 'Could not delete participant');
  }

  setParticipantWeightIn(participantId: string, status: string): Observable<any> {
    return this.httpService.post(`${ApiMethods.participant.setWeightInStatus}/${participantId}`, {data: status});
  }
}
