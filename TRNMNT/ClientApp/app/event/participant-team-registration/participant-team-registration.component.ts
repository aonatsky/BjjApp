import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { EventService } from '../../core/services/event.service';
import { WeightDivisionService } from '../../core/services/weight-division.service';
import { CategoryService } from '../../core/services/category.service';
import { TeamService } from '../../core/services/team.service';
import { ParticipantService } from '../../core/services/participant.service';
import { TranslateService } from '../../../../node_modules/@ngx-translate/core';
import { UserModelAthlete } from '../../core/model/user.models';
import { ICrudColumn as CrudColumn, IColumnOptions } from '../../shared/crud/crud.component';
import { ParticipantRegistrationModel } from '../../core/model/participant.models';
import { SelectItem } from '../../../../node_modules/primeng/primeng';
import { WeightDivisionSimpleModel } from '../../core/model/weight-division.models';
import { CategorySimpleModel } from '../../core/model/category.models';
import { PriceModel } from '../../core/model/price.model';
import DateHelper from '../../core/helpers/date-helper';
import { PaymentDataModel } from '../../core/model/payment-data.model';

@Component({
  selector: 'participant-team-registration',
  templateUrl: './participant-team-registration.component.html',
  styleUrls: ['./participant-team-registration.component.scss']
})
export class ParticipantTeamRegistrationComponent implements OnInit {
  categories: CategorySimpleModel[];
  eventTitleParameter: { value: any };
  categorySelectItems: SelectItem[];
  weightDivisionsSelectItems: SelectItem[];
  weightDivisions: WeightDivisionSimpleModel[];
  price: PriceModel;
  constructor(
    private weightDivisionService: WeightDivisionService,
    private categoryService: CategoryService,
    private teamService: TeamService,
    private participantService: ParticipantService,
    private eventService: EventService,
    private translateService: TranslateService
  ) {}

  dateHelper = DateHelper;
  @ViewChild('formPrivatElement')
  formPrivat: ElementRef;
  allAthletes: UserModelAthlete[];
  participationData: ParticipationData[] = [];
  currentStep = 0;
  lastStep = 1;
  paymentData: string = '';
  paymentSignature: string = '';

  columns: CrudColumn[] = [
    {
      propertyName: 'firstName',
      displayName: this.translateService.instant('COMMON.FIRST_NAME'),
      isEditable: false,
      isSortable: true
    },
    {
      propertyName: 'lastName',
      displayName: this.translateService.instant('COMMON.LAST_NAME'),
      isEditable: false,
      isSortable: true
    },
    {
      propertyName: 'email',
      displayName: this.translateService.instant('COMMON.EMAIL'),
      isEditable: false,
      isSortable: true
    }
  ];
  firstIndex: number = 0;
  sortDirection: number = 1;
  sortField: string = 'name';
  columnOptions: IColumnOptions = {};
  ngOnInit() {
    this.eventService.getCurrentEvent().subscribe(r => (this.eventTitleParameter = { value: r.title }));
    this.teamService.getAthletesForParticipation().subscribe(r => {
      this.allAthletes = r;
    });
  }

  private initDataForParticipation() {
    this.categoryService.getCategoriesForCurrentEvent().subscribe(r => {
      this.categories = r;
      this.initCategorySelectItems();
    });
  }

  private initCategorySelectItems() {
    this.categorySelectItems = [];
    for (const category of this.categories) {
      this.categorySelectItems.push({ label: category.name, value: category.categoryId });
    }
  }

  initWeightDivisionDropdown(participationData: ParticipationData) {
    this.weightDivisionService.getWeightDivisionsByCategory(participationData.participant.categoryId).subscribe(w => {
      participationData.weightDivisionsSelectItems = [];
      w.forEach(wd => {
        participationData.weightDivisionsSelectItems.push({ label: wd.name, value: wd.weightDivisionId });
      });
    });
  }

  addToParticipation(data: UserModelAthlete) {
    const participant = new ParticipantRegistrationModel();
    participant.firstName = data.firstName;
    participant.lastName = data.lastName;
    participant.teamId = data.teamId;
    participant.teamName = data.teamName;
    participant.dateOfBirth = data.dateOfBirth;
    participant.email = data.email;
    participant.userId = data.userId;
    let pData = new ParticipationData();
    pData.participant = participant;
    pData.weightDivisionsSelectItems = [];
    this.participationData.push(pData);
  }

  isAlreadyAdded(data: UserModelAthlete) {
    return this.participationData.filter(d => d.participant.userId == data.userId).length > 0;
  }

  removeFromParticipation(data: UserModelAthlete) {
    const index = this.participationData.findIndex(p => p.participant.userId == data.userId);
    if (index > -1) {
      this.participationData.splice(index, 1);
    }
  }

  nextStep() {
    this.currentStep++;
    if (this.currentStep == 1) {
      this.initDataForParticipation();
      this.initPrice();
    }
  }

  previousStep() {
    this.currentStep--;
  }

  initPrice() {
    this.eventService.getTeamPrice(this.participationData.map(p => p.participant)).subscribe(p => (this.price = p));
  }

  goToPayment() {
    this.participantService
      .processParticipantTeamRegistration(this.participationData.map(p => p.participant))
      .subscribe((r: PaymentDataModel) => {
        this.submitPaymentForm(r);
      });
  }

  isDataValid(): boolean {
    return this.participationData.filter(d => !d.isValid()).length == 0;
  }

  private submitPaymentForm(paymentData: PaymentDataModel) {
    this.formPrivat.nativeElement.elements[0].value = paymentData.data;
    this.formPrivat.nativeElement.elements[1].value = paymentData.signature;
    this.formPrivat.nativeElement.submit();
  }
}

export class ParticipationData {
  participant: ParticipantRegistrationModel;
  weightDivisionsSelectItems: SelectItem[];
  isValid(): boolean {
    return (
      this.participant.firstName &&
      this.participant.lastName &&
      this.participant.categoryId &&
      this.participant.weightDivisionId &&
      !!this.participant.dateOfBirth
    );
  }
}
