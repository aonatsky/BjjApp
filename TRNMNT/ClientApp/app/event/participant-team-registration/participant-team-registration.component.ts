import { Component, OnInit } from '@angular/core';
import { EventService } from '../../core/services/event.service';
import { WeightDivisionService } from '../../core/services/weight-division.service';
import { CategoryService } from '../../core/services/category.service';
import { TeamService } from '../../core/services/team.service';
import { ParticipantService } from '../../core/services/participant.service';
import { AuthService } from '../../../../node_modules/angular5-social-login';
import { RouterService } from '../../core/services/router.service';
import { TranslateService } from '../../../../node_modules/@ngx-translate/core';
import { UserModelAthlete } from '../../core/model/user.models';
import { ICrudColumn as CrudColumn, IColumnOptions } from '../../shared/crud/crud.component';
import { ParticipantModelBase, ParticipantRegistrationModel } from '../../core/model/participant.models';
import { forkJoin } from '../../../../node_modules/rxjs';
import { SelectItem } from '../../../../node_modules/primeng/primeng';
import { WeightDivisionModel, WeightDivisionSimpleModel } from '../../core/model/weight-division.models';
import { CategoryModel } from '../../core/model/category.models';
import { PriceModel } from '../../core/model/price.model';

@Component({
  selector: 'participant-team-registration',
  templateUrl: './participant-team-registration.component.html',
  styleUrls: ['./participant-team-registration.component.scss']
})
export class ParticipantTeamRegistrationComponent implements OnInit {
  categories: CategoryModel[];
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
    private authService: AuthService,
    private eventService: EventService,
    private routerService: RouterService,
    private translateService: TranslateService
  ) {}

  allAthletes: UserModelAthlete[];
  selectedAthletes: ParticipantRegistrationModel[] = [];
  currentStep = 0;
  lastStep = 2;

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
    this.teamService.getAthletesForParticipation().subscribe(r => {
      this.allAthletes = r;
    });
    this.loadData();
  }

  private loadData() {
    forkJoin(
      this.categoryService.getCategoriesForCurrentEvent(),
      this.eventService.getCurrentEvent(),
      this.participantService.isFederationMember()
    ).subscribe(data => this.initData(data));
  }

  private initData(data) {
    this.categories = data[1];
    this.eventTitleParameter = { value: data[2].title };
    this.initCategoryDropdown();
    this.initPrice();
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

  addToParicipation(data: UserModelAthlete) {
    const participant = new ParticipantRegistrationModel();
    participant.firstName = data.firstName;
    participant.teamId = data.teamId;
    participant.teamName = data.teamName;
    participant.dateOfBirth = data.dateOfBirth;
    participant.email = data.email;
    this.selectedAthletes.push(participant);
  }

  removeFromParticipation(userId: string) {
    const index = this.selectedAthletes.findIndex(p => p.userId == userId);
    if (index > 0) {
      this.selectedAthletes.splice(index, 1);
    }
  }

  nextStep() {
    this.currentStep++;
    if (this.currentStep == 1) {
      // initPrice()
    }
  }

  initPrice() {
    this.eventService.getTeamPrice(this.selectedAthletes).subscribe(p => (this.price = p));
  }
}
