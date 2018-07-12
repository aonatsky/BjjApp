import { PaymentDataModel } from '../payment-data.model';

export class ParticipantRegistrationResultModel {
    success: boolean;
    reason: string;
    paymentData: PaymentDataModel
    constructor() {
    }

}