import { EventService } from './../../core/services/event.service';
import { Component, ViewEncapsulation, ViewChild, ElementRef, OnInit } from '@angular/core';
import { TeamService } from '../../core/services/team.service';
import { ParticipantService } from '../../core/services/participant.service';
import { CategoryService } from '../../core/services/category.service';
import { WeightDivisionService } from '../../core/services/weight-division.service';
import { ParticipantRegistrationModel } from '../../core/model/participant.models';
import { TeamModel } from '../../core/model/team.model';
import { PaymentDataModel } from '../../core/model/payment-data.model';
import { CategorySimpleModel } from '../../core/model/category.models';
import { WeightDivisionSimpleModel } from '../../core/model/weight-division.models';
import { forkJoin } from 'rxjs';
import { SelectItem } from 'primeng/primeng';
import { AuthService } from '../../core/services/auth.service';
import { RouterService } from '../../core/services/router.service';
import { TranslateService } from '../../../../node_modules/@ngx-translate/core';
import { PriceModel } from '../../core/model/price.model';

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
  price: PriceModel;
  eventTitleParameter: object;
  currentStep: number = 0;
  lastStep: number = 1;
  tncAccepted: boolean = false;
  paymentData: string = '';
  paymentSignature: string = '';

  constructor(
    private weightDivisionService: WeightDivisionService,
    private categoryService: CategoryService,
    private teamService: TeamService,
    private participantService: ParticipantService,
    private authService: AuthService,
    private eventService: EventService,
    private routerService: RouterService,
    private translateService : TranslateService
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
    console.log(this.authService.isLoggedIn());
  }

  private getDefaultDateOfBirth() {
    const date = new Date();
    date.setFullYear(date.getFullYear() - 20);
    return date;
  }

  private initTeamDropdown() {
    this.teamSelectItems = [];
    this.teamSelectItems.push({ label: this.translateService.instant('COMMON.NO_TEAM'), value: null });
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

  initWeightDivisionDropdown(event) {
    this.weightDivisionService.getWeightDivisionsByCategory(event.value).subscribe(w => {
      this.weightDivisions = w;
      this.weightDivisionsSelectItems = [];
      this.weightDivisions.forEach(wd => {
        this.weightDivisionsSelectItems.push({ label: wd.name, value: wd.weightDivisionId });
      });
    });
  }

  getTeam(): string {
    return this.teams.find(t => t.teamId == this.participant.teamId).name;
  }

  getCategory() : string{
    return this.categories.find(c => c.categoryId == this.participant.categoryId).name;
  }

  getWeightDivision(){
    return this.weightDivisions.find(w => w.weightDivisionId == this.participant.weightDivisionId).name;
  }

  nextStep() {
    this.currentStep++;
  }

  previousStep() {
    this.currentStep--;
  }

  goToTeamRegistration() {
    this.routerService.goToTeamRegistration();
  }

  goToPayment() {
      this.participantService.processParticipantRegistration(this.participant).subscribe((r: PaymentDataModel) => {
      this.submitPaymentForm(r);
    });
  }

  private submitPaymentForm(paymentData: PaymentDataModel) {
    this.formPrivat.nativeElement.elements[0].value = paymentData.data;
    this.formPrivat.nativeElement.elements[1].value = paymentData.signature;
    this.formPrivat.nativeElement.submit();
  }
}
