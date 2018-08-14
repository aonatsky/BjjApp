import { Injectable } from '@angular/core';
import { LoggerService } from './logger.service';
import { HttpService } from '../dal/http/http.service';
import { EventModel, EventPreviewModel } from '../model/event.models';
import { ApiMethods } from '../dal/consts/api-methods.consts';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { PriceModel } from '../model/price.model';

@Injectable()
export class EventService {
  constructor(private loggerService: LoggerService, private httpService: HttpService) {}

  private currentEvent: EventModel;

  getCurrentEvent(): Observable<EventModel> {
    if (this.currentEvent) {
      return of(this.currentEvent);
    }
    return this.getEventInfo();
  }

  getEvents(): Observable<EventModel> {
    return this.httpService.get(ApiMethods.event.getEvents);
  }

  updateEvent(event: EventModel): Observable<any> {
    return this.httpService.post(ApiMethods.event.updateEvent, event);
  }

  deleteEvent(eventId: string): Observable<any> {
    return this.httpService.deleteById(ApiMethods.event.deleteEvent, eventId);
  }

  getEventsForOwner(): Observable<EventPreviewModel[]> {
    return this.httpService.get<EventPreviewModel>(ApiMethods.event.getEventsForOwner);
  }

  getEvent(id): Observable<EventModel> {
    return this.httpService
      .get(ApiMethods.event.getEvent + '/' + id)
      .pipe(map(res => this.httpService.convertDate(res)));
  }

  getEventBaseInfo(id): Observable<EventPreviewModel> {
    return this.httpService
      .get(ApiMethods.event.getEventBaseInfo + '/' + id)
      .pipe(map(res => this.httpService.convertDate(res)));
  }

  uploadEventImage(file, id) {
    return this.httpService.postFile(ApiMethods.event.uploadImage + '/' + id, file);
  }

  uploadEventTncFile(file, id) {
    return this.httpService.postFile(ApiMethods.event.uploadTnc + '/' + id, file);
  }

  downloadEventTncFile(tncPath: string): Observable<any> {
    return this.httpService.getPdf(tncPath, 'TNC');
  }

  uploadPromoCodeList(file, id) {
    return this.httpService.postFile(ApiMethods.event.uploadPromoCodeList + '/' + id, file);
  }

  getEventInfo() {
    return this.httpService.get<EventModel>(ApiMethods.event.getEventInfo + '/').pipe(
      map(res => this.httpService.convertDate(res)),
      map(res => {
        this.currentEvent = res;
        return res;
      })
    );
  }

  createEvent(): Observable<string> {
    return this.httpService.get(ApiMethods.event.createEvent);
  }

  getPrice(includeMembership : boolean): Observable<PriceModel> {
    return this.httpService.get<number>(ApiMethods.event.getPrice, {includeMembership});
  }

  //private methods
}
