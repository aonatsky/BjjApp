import { EventService } from '../../core/services/event.service';
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
import { TranslateService } from '@ngx-translate/core';
import { PriceModel } from '../../core/model/price.model';
import DateHelper from '../../core/helpers/date-helper';
import { Roles } from '../../core/consts/roles.const';
import { ApprovalStatus } from '../../core/consts/approval-status.const';
import { UserService } from '../../core/services/user.service';

@Component({
  selector: 'participant-registration',
  templateUrl: './participant-registration.component.html',
  styleUrls: ['./participant-registration.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ParticipantRegistrationComponent implements OnInit {
  @ViewChild('formPrivatElement')
  formPrivat: ElementRef;
  participant: ParticipantRegistrationModel = new ParticipantRegistrationModel();
  teamMembershipApproved: boolean;
  isFederationMember: boolean = false;
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
  dateHelper = DateHelper;
  isTeamOwner: boolean;

  constructor(
    private weightDivisionService: WeightDivisionService,
    private categoryService: CategoryService,
    private teamService: TeamService,
    private participantService: ParticipantService,
    private eventService: EventService,
    private routerService: RouterService,
    private translateService: TranslateService,
    private userService: UserService
  ) {}

  ngOnInit() {
    this.userService.getUserAthlete().subscribe(r => {
      this.participant.userId = r.userId;
      this.participant.dateOfBirth = r.dateOfBirth ? r.dateOfBirth : this.getDefaultDateOfBirth();
      this.participant.firstName = r.firstName;
      this.participant.lastName = r.lastName;
      this.participant.email = r.email;
      this.participant.includeMembership = false;
      this.participant.teamId = r.teamId;
      this.participant.teamName = r.teamName;
      this.teamMembershipApproved = r.teamMembershipApprovalStatus == ApprovalStatus.approved;
      this.isTeamOwner = r.isTeamOwner;
    });
    this.loadData();
  }

  private loadData() {
    forkJoin(
      this.teamService.getTeamsForEvent(),
      this.categoryService.getCategoriesForCurrentEvent(),
      this.eventService.getCurrentEvent(),
      this.participantService.isFederationMember()
    ).subscribe(data => this.initData(data));
  }

  private initData(data) {
    this.teams = data[0];
    this.categories = data[1];
    this.eventTitleParameter = { value: data[2].title };
    this.isFederationMember = data[3];
    this.initCategoryDropdown();
    this.initTeamDropdown();
    this.initPrice();
  }

  private getDefaultDateOfBirth() {
    return DateHelper.getDefaultDateOfBirth();
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
    return this.participant.teamName ? this.participant.teamName : this.translateService.instant('COMMON.NO_TEAM');
  }

  getCategory(): string {
    return this.categories.find(c => c.categoryId == this.participant.categoryId).name;
  }

  getWeightDivision() {
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

  goToProfile() {
    this.routerService.goToMyProfile("/event/participant-registration");
  }

  goToPayment() {
    this.participantService.processParticipantRegistration(this.participant).subscribe((r: PaymentDataModel) => {
      this.submitPaymentForm(r);
    });
  }

  initPrice() {
    this.eventService.getPrice(this.participant.includeMembership).subscribe(p => (this.price = p));
  }

  getIsApproved(teamId): boolean{
    if(!teamId){
      return true;
    }
    const team = this.teams.filter(t => t.teamId == teamId)[0];
    if(!team){
      return true;
    }
    return team.federationApprovalStatus == ApprovalStatus.approved && team.approvalStatus == ApprovalStatus.approved && this.teamMembershipApproved;
  }

  private submitPaymentForm(paymentData: PaymentDataModel) {
    this.formPrivat.nativeElement.elements[0].value = paymentData.data;
    this.formPrivat.nativeElement.elements[1].value = paymentData.signature;
    this.formPrivat.nativeElement.submit();
  }
}
