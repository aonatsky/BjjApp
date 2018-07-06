import { Component, ViewEncapsulation, ViewChild, ElementRef, OnInit, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TeamService } from './../../core/services/team.service';
import { ParticipantService } from './../../core/services/participant.service';
import { CategoryService } from './../../core/services/category.service';
import { PaymentService } from './../../core/services/payment.service';
import { WeightDivisionService } from './../../core/services/weight-division.service';
import { ParticipantRegistrationModel } from './../../core/model/participant.models';
import { ParticipantRegistrationResultModel } from './../../core/model/result/participant-registration-result.model';
import { TeamModel } from './../../core/model/team.model';
import { PaymentDataModel } from './../../core/model/payment-data.model';
import { CategorySimpleModel } from './../../core/model/category.models';
import { WeightDivisionSimpleModel } from './../../core/model/weight-division.models';
import { LoggerService } from './../../core/services/logger.service';
import { RouterService } from './../../core/services/router.service';
import { forkJoin } from 'rxjs';
import { SelectItem, Message } from 'primeng/primeng';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'event-registration',
  templateUrl: './event-registration.component.html',
  styleUrls: ['./event-registration.component.css'],
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

  private eventId: string;
  private currentStep: number = 0;
  private tncAccepted: boolean = false;
  private messages: Message[] = [];
  private paymentData: string = '';
  private paymentSignature: string = '';

  constructor(
    private routerService: RouterService,
    private loggerService: LoggerService,
    private route: ActivatedRoute,
    private weightDivisionService: WeightDivisionService,
    private categoryService: CategoryService,
    private teamService: TeamService,
    private participantService: ParticipantService,
    private paymentService: PaymentService,
    private cd: ChangeDetectorRef,
    private authService: AuthService
  ) {}

  ngOnInit() {
    console.log(this.authService.getUser());
    this.loadData();
  }

  private loadData() {
    forkJoin(this.teamService.getTeams(), this.categoryService.getCategoriesForCurrentEvent()).subscribe(data =>
      this.initData(data)
    );
  }

  private initData(data) {
    this.teams = data[0];
    this.categories = data[1];
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

  private initWeightDivisionDropdown(event) {
    this.weightDivisionService.getWeightDivisionsByCategory(event.value).subscribe(w => {
      this.weightDivisions = w;
      this.weightDivisionsSelectItems = [];
      this.weightDivisions.forEach(wd => {
        this.weightDivisionsSelectItems.push({ label: wd.name, value: wd.weightDivisionId });
      });
    });
  }

  private getTeam() {
    return this.teams.filter(t => t.teamId === this.participant.teamId)[0].name;
  }

  private getCategory() {
    return this.categories.filter(c => c.categoryId === this.participant.categoryId)[0].name;
  }

  private getWeightDivision() {
    return this.weightDivisions.filter(wd => wd.weightDivisionId === this.participant.weightDivisionId)[0].name;
  }

  private showMessage(message: string) {
    this.messages.push({ severity: 'error', summary: 'Error', detail: message });
  }

  private goToPayment() {
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

  private nextStep() {
    this.currentStep++;
  }

  private previousStep() {
    this.currentStep--;
  }
}
