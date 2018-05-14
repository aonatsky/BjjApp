import { Injectable } from '@angular/core'
import { LoggerService } from './logger.service'
import { HttpService } from './../dal/http/http.service'
import { EventModel, EventPreviewModel } from './../model/event.models'
import { ApiMethods } from './../dal/consts/api-methods.consts'
import { Observable } from 'rxjs/Rx';


@Injectable()
export class EventService {
    constructor(private loggerService: LoggerService, private httpService: HttpService) {

    }

    getEvents(): Observable<EventModel> {
        return this.httpService.get(ApiMethods.event.getEvents);
    }


    updateEvent(event: EventModel): Observable<any> {
        return this.httpService.post(ApiMethods.event.updateEvent, event);
    }

    deleteEvent(eventId: string): Observable<any> {
        return this.httpService.post(ApiMethods.event.deleteEvent, event);
    }

    getEventsForOwner(): Observable<EventPreviewModel[]> {
        return this.httpService.get(ApiMethods.event.getEventsForOwner).map(res => this.httpService.getArray<EventPreviewModel>(res));
    }

    getEvent(id): Observable<EventModel> {
        return this.httpService.get(ApiMethods.event.getEvent + "/" + id).map(res => this.httpService.getJson(res)).map(res => this.httpService.convertDate(res));
    }

    getEventBaseInfo(id): Observable<EventPreviewModel> {
        return this.httpService.get(ApiMethods.event.getEventBaseInfo + "/" + id).map(res => this.httpService.getJson(res)).map(res => this.httpService.convertDate(res));
    }

    uploadEventImage(file, id) {
        return this.httpService.postFile(ApiMethods.event.uploadImage + "/" + id, file);
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
        return this.httpService.get(ApiMethods.event.getEventInfo + '/').map(res => this.httpService.getJson(res)).map(res => this.httpService.convertDate(res));
    }

    createEvent(): Observable<string> {
        return this.httpService.get(ApiMethods.event.createEvent).map(res => this.httpService.getJson(res));
    }

    //private methods





}