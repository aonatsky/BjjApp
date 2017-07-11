﻿import {Injectable} from "@angular/core"

import { LoggerService } from "./logger.service"
import { AuthService } from "./auth.service"
import { HttpService } from "./../dal/http/http.service"
import { EventModel } from "./../model/event.model"
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


    public saveAsDraft(event: EventModel): Observable<any> {
        event.statusId = EventStatus.Draft;
        return this.addEvent(event);
    }


    private addEvent(event: EventModel): Observable<any> {
        return this.httpService.post(ApiMethods.event.addEvent, event);
    }
}