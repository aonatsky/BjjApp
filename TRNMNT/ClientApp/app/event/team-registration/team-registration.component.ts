import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { TeamRegistrationModel } from '../../core/model/team.model';
import { PaymentDataModel } from '../../core/model/payment-data.model';
import { TeamService } from '../../core/services/team.service';
import { PriceModel } from '../../core/model/price.model';
import { ActivatedRoute } from '@angular/router';
import { UserService } from '../../core/services/user.service';

@Component({
  selector: 'team-registration',
  templateUrl: './team-registration.component.html',
  styleUrls: ['./team-registration.component.scss']
})
export class TeamRegistrationComponent implements OnInit {
  constructor(private teamService: TeamService, private route: ActivatedRoute, private userService: UserService) {}
  price: PriceModel;
  paymentData: string;
  paymentSignature: string;
  model: TeamRegistrationModel = new TeamRegistrationModel();
  @ViewChild('formPrivatElement')
  formPrivat: ElementRef;

  ngOnInit() {
    this.teamService.getTeamRegistrationPrice().subscribe(r => {
      this.price = r;
    });
    this.model.returnUrl = this.route.snapshot.queryParams.returnUrl || '/';
    const user = this.userService.getUser();
    this.model.contactEmail = user.email;
    this.model.contactName = `${user.firstName} ${user.lastName}`;
  }

  goToPayment() {
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
