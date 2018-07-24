import { EventService } from './../../core/services/event.service';
import { Component, ViewEncapsulation, ViewChild, ElementRef, OnInit, ChangeDetectorRef } from '@angular/core';
import { TeamService } from '../../core/services/team.service';
import { ParticipantService } from '../../core/services/participant.service';
import { CategoryService } from '../../core/services/category.service';
import { PaymentService } from '../../core/services/payment.service';
import { WeightDivisionService } from '../../core/services/weight-division.service';
import { ParticipantRegistrationModel } from '../../core/model/participant.models';
import { ParticipantRegistrationResultModel } from '../../core/model/result/participant-registration-result.model';
import { TeamModel } from '../../core/model/team.model';
import { PaymentDataModel } from '../../core/model/payment-data.model';
import { CategorySimpleModel } from '../../core/model/category.models';
import { WeightDivisionSimpleModel } from '../../core/model/weight-division.models';
import { forkJoin } from 'rxjs';
import { SelectItem, Message } from 'primeng/primeng';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'event-registration',
  templateUrl: './event-registration.component.html',
  styleUrls: ['./event-registration.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class EventRegistrationComponent implements OnInit {
  @ViewChild('formPrivatElement') formPrivat: ElementRef;
  participant: ParticipantRegistrationModel = new ParticipantRegistrationModel();
  categories: CategorySimpleModel[] = [];
  weightDivisions: WeightDivisionSimpleModel[] = [];
  categorySelectItems: SelectItem[];
  weightDivisionsSelectItems: SelectItem[];
  teamSelectItems: SelectItem[] = [];
  teams: TeamModel[] = [];

  eventId: string;
  price: number;
  eventTitleParameter: object;
  currentStep: number = 0;
  tncAccepted: boolean = false;
  private messages: Message[] = [];
  paymentData: string = '';
  paymentSignature: string = '';

  constructor(
    private weightDivisionService: WeightDivisionService,
    private categoryService: CategoryService,
    private teamService: TeamService,
    private participantService: ParticipantService,
    private authService: AuthService,
    private eventService: EventService
  ) {}

  ngOnInit() {
    const user = this.authService.getUser();
    this.participant.userId = user.userId;
    this.participant.dateOfBirth = user.dateOfBirth ? user.dateOfBirth : this.getDefaultDateOfBirth();
    this.participant.firstName = user.firstName;
    this.participant.lastName = user.lastName;
    this.participant.email = user.email;
    this.loadData();
  }

  private loadData() {
    forkJoin(
      this.teamService.getTeams(),
      this.categoryService.getCategoriesForCurrentEvent(),
      this.eventService.getCurrentEvent(),
      this.eventService.getPrice()
    ).subscribe(data => this.initData(data));
  }

  private initData(data) {
    this.teams = data[0];
    this.categories = data[1];
    this.eventTitleParameter = { value: data[2].title };
    this.price = data[3];
    this.initCategoryDropdown();
    this.initTeamDropdown();
  }

  private getDefaultDateOfBirth() {
    const date = new Date();
    date.setFullYear(date.getFullYear() - 20);
    return date;
  }

  private initTeamDropdown() {
    this.teamSelectItems = [];
    for (const team of this.teams) {
      this.teamSelectItems.push({ label: team.name, value: team.teamId });
    }
  }

  private initCategoryDropdown() {
    this.categorySelectItems = [];
    for (const category of this.categories) {
      this.categorySelectItems.push({ label: category.name, value: category.categoryId });
    }
  }





  private showMessage(message: string) {
    this.messages.push({ severity: 'error', summary: 'Error', detail: message });
  }

  goToPayment() {
    // todo click payment form

    this.participantService
      .processParticipantRegistration(this.participant)
      .subscribe((r: ParticipantRegistrationResultModel) => {
        if (!r.success) {
          this.showMessage(r.reason);
        } else {
          this.submitPaymentForm(r.paymentData);
        }
      });
  }

  private submitPaymentForm(paymentData: PaymentDataModel) {
    this.formPrivat.nativeElement.elements[0].value = paymentData.data;
    this.formPrivat.nativeElement.elements[1].value = paymentData.signature;
    this.formPrivat.nativeElement.submit();
  }


}
