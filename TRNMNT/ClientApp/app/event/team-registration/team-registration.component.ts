import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { TeamRegistrationModel } from '../../core/model/team.model';
import { PaymentDataModel } from '../../core/model/payment-data.model';

@Component({
  selector: 'team-registration',
  templateUrl: './team-registration.component.html',
  styleUrls: ['./team-registration.component.scss']
})
export class TeamRegistrationComponent implements OnInit {

  constructor() { }

  paymentData: string;
  paymentSignature: string;
  model : TeamRegistrationModel = new TeamRegistrationModel();
  @ViewChild('formPrivatElement') formPrivat: ElementRef;

  ngOnInit() {
    
  }


  goToPayment(){
    this.teamService.processTeamRegistration(this.model).subscribe((r: PaymentDataModel) => {
      this.submitPaymentForm(r);
    });
  }

  private submitPaymentForm(paymentData: PaymentDataModel) {
    this.formPrivat.nativeElement.elements[0].value = paymentData.data;
    this.formPrivat.nativeElement.elements[1].value = paymentData.signature;
    this.formPrivat.nativeElement.submit();
  }
}
