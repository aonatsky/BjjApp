import { Injectable } from "@angular/core"
import { LoggerService } from "./logger.service"
import { HttpService } from "./../dal/http/http.service"
import { PaymentDataModel } from "./../model/payment-data.model"
import { ApiMethods } from "./../dal/consts/api-methods.consts"
import { Observable } from 'rxjs/Rx';


@Injectable()
export class PaymentService {
    constructor(private loggerService: LoggerService, private httpService: HttpService) {

    }

    public getPaymentData(eventId: string): Observable<PaymentDataModel> {
        return this.httpService.get(ApiMethods.payment.getPaymentData + "/" + eventId).map(res => this.httpService.getJson(res));
    }


}