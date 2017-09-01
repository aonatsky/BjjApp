import { PaymentDataModel } from './../payment-data.model';

export class ParticipantRegistrationResultModel {
    public success: boolean;
    public reason: string;
    public paymentData: PaymentDataModel
    constructor() {
    }

}