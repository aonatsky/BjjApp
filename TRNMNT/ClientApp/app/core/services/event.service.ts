import { Injectable } from "@angular/core"

import { LoggerService } from "./logger.service"
import { AuthService } from "./auth.service"
import { HttpService } from "./../dal/http/http.service"
import { EventModel, EventPreviewModel } from "./../model/event.models"
import { ApiMethods } from "./../dal/consts/api-methods.consts"
import { EventStatus } from "./../consts/event-status.consts"
import { Observable } from 'rxjs/Rx';


@Injectable()
export class EventService {
    constructor(private loggerService: LoggerService, private httpService: HttpService) {

    }

    public getEvents(): Observable<EventModel> {
        return this.httpService.get(ApiMethods.event.getEvents);
    }


    public updateEvent(event: EventModel): Observable<any> {
        return this.httpService.post(ApiMethods.event.updateEvent, event);
    }

    public getEventsForOwner(): Observable<EventPreviewModel[]> {
        return this.httpService.get(ApiMethods.event.getEventsForOwner).map(res => this.httpService.getArray<EventModel>(res));
    }

    public getEvent(id): Observable<EventModel> {
        return this.httpService.get(ApiMethods.event.getEvent + "/" + id).map(res => this.httpService.getJson(res)).map(res => this.httpService.convertDate(res));
    }

    public getEventBaseInfo(id): Observable<EventPreviewModel> {
        return this.httpService.get(ApiMethods.event.getEventBaseInfo + "/" + id).map(res => this.httpService.getJson(res)).map(res => this.httpService.convertDate(res));
    }
        
    public uploadEventImage(file, id) {
        return this.httpService.postFile(ApiMethods.event.uploadImage + "/" + id, file);
    }

    public uploadEventTncFile(file, id) {
        return this.httpService.postFile(ApiMethods.event.uploadTnc + "/" + id, file);
    }

    public downloadEventTncFile(tncPath: string): Observable<any> {
        return this.httpService.getPdf(tncPath, "TNC");
    }

    public uploadPromoCodeList(file, id) {
        return this.httpService.postFile(ApiMethods.event.uploadPromoCodeList + "/" + id, file);
    }

    public getEventInfo() {
        return this.httpService.get(ApiMethods.event.getEventInfo + "/").map(res => this.httpService.getJson(res)).map(res => this.httpService.convertDate(res));
    }

    public createEvent(): Observable<string> {
        return this.httpService.get(ApiMethods.event.createEvent).map(res => this.httpService.getJson(res));
    }

    //private methods
   

  

    
}