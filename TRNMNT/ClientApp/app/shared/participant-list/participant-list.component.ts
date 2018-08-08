import { Component, OnInit, Input } from '@angular/core';
import { ParticipantTableModel } from '../../core/model/participant.models';
import { PagedList } from '../../core/model/paged-list.model';
import { ParticpantSortField } from '../../core/consts/participant-sort-field.const';
import { ParticipantFilterModel } from '../../core/model/participant-filter.model';
import { LazyLoadEvent, SelectItem } from 'primeng/primeng';
import { CategoryWithDivisionFilterModel } from '../../core/model/category-with-division-filter.model';
import { ApprovalStatus } from '../../core/consts/approval-status.const';
import { ParticipantDdlModel } from '../../core/model/participant-ddl.model';
import { ParticipantService } from '../../core/services/participant.service';
import { BracketService } from '../../core/services/bracket.service';
import { TranslateService } from '@ngx-translate/core';
import { DatePipe } from '@angular/common';
import {
  ICrudColumn as CrudColumn,
  IColumnOptions,
} from '../crud/crud.component';
@Component({
  selector: 'participant-list',
  templateUrl: './participant-list.component.html',
  styleUrls: ['./participant-list.component.scss'],
  providers: [DatePipe]
})
export class ParticipantListComponent implements OnInit {
  
  constructor(
    private participantService: ParticipantService,
    private bracketService: BracketService,
    private datePipe: DatePipe,
    private translateService: TranslateService
  ) {}

  

  @Input() eventId: string;

  approvalStatus = ApprovalStatus;
  participantsListModel: PagedList<ParticipantTableModel>;
  participantDdlModel: ParticipantDdlModel;
  filter: CategoryWithDivisionFilterModel;

  teamSelectItems: SelectItem[];
  categorySelectItems: SelectItem[];
  weightDivisionSelectItems: SelectItem[];

  private readonly pageLinks: number = 3;
  firstIndex: number = 0;
  participantsLoading: boolean = true;
  ddlDataLoading: boolean = true;
  sortDirection: number = 1;
  sortField: string = 'firstName';

  get isPaginationEnabled(): boolean {
    return this.participantsModel.length > this.rowsCount;
  }

  get isDataLoading(): boolean {
    return this.participantsLoading || this.ddlDataLoading;
  }

  get selectedCategoryId() {
    if (this.filter != null) {
      return this.filter.categoryId;
    }
    return null;
  }

  get participantsModel(): ParticipantTableModel[] {
    if (this.participantsListModel != null) {
      return this.participantsListModel.innerList;
    }
    return [];
  }

  get rowsCount(): number {
    if (this.participantsListModel != null) {
      return this.participantsListModel.pageSize;
    }
    return 10;
  }

  get totalCount(): number {
    if (this.participantsListModel != null) {
      return this.participantsListModel.totalCount;
    }
    return 0;
  }
  isAllWinnersSelected: boolean = false;
  private targetModel: any[] = [];

  

  ngOnInit() {
    this.refreshFilter();
  }

  columns: CrudColumn[] = [
    {
      propertyName: 'firstName',
      displayName: this.translateService.instant('COMMON.FIRST_NAME'),
      isEditable: true,
      isSortable: true
    },
    {
      propertyName: 'lastName',
      displayName: this.translateService.instant('COMMON.LAST_NAME'),
      isEditable: true,
      isSortable: true
    },
    {
      propertyName: 'dateOfBirth',
      displayName: this.translateService.instant('COMMON.DATE_OF_BIRTH_SHORT'),
      isEditable: true,
      isSortable: true,
      transform: value => this.dateTransform.call(this, value),
    },
    {
      propertyName: 'teamName',
      displayName: this.translateService.instant('COMMON.TEAM'),
      isEditable: true,
      isSortable: true,
    },
    {
      propertyName: 'categoryName',
      displayName: this.translateService.instant('COMMON.CATEGORY'),
      isEditable: true,
      isSortable: true,
    },
    {
      propertyName: 'weightDivisionName',
      displayName: this.translateService.instant('COMMON.WEIGHT_DIVISION'),
      isEditable: true,
      isSortable: true,
    },
    {
      propertyName: 'approvalStatus',
      displayName: this.translateService.instant('COMMON.APPROVAL.APPROVAL_STATUS'),
      isEditable: false,
      isSortable: true,
      useClass: value => this.getApprovalClassCallback.call(this, value)
    }
  ];

  columnOptions: IColumnOptions = {};

  refreshFilter() {
    this.ddlDataLoading = true;
    this.participantService.getParticipantsDropdownData(this.eventId).subscribe(result => {
      this.ddlDataLoading = false;
      this.participantDdlModel = result;
      this.mapDdlModel(result);
    });
  }

  private mapDdlModel(model: ParticipantDdlModel) {
    this.teamSelectItems = this.mapDdls(model.teams, 'teamId');
    this.categorySelectItems = this.mapDdls(model.categories, 'categoryId');
    this.weightDivisionSelectItems = this.mapDdls(model.weightDivisions, 'weightDivisionId');
    this.refreshDdlModel();
  }
  private refreshDdlModel() {
    this.columnOptions = {
      weightDivisionName: this.weightDivisionSelectItems,
      categoryName: this.categorySelectItems,
      teamName: this.teamSelectItems
    };
  }
  private mapDdls(array: any[], idPropName: string): SelectItem[] {
    return array.map(t => {
      return { label: t.name, value: t[idPropName] };
    });
  }

  private getApprovalClassCallback(value: string): string {
    let classes = 'ui-g-12 ui-g-nopad text-allign-center ';
    switch (value) {
      case ApprovalStatus.approved:
        classes += 'fas fa-check-circle';
        break;
      case ApprovalStatus.pending:
        classes += 'fas fa-clock';
        break;
      case ApprovalStatus.declined:
        classes += 'fas fa-times-circle';
        break;
    }
    return classes;
  }

  dateTransform(value: Date): string {
    return this.datePipe.transform(value, 'MM.dd.yyyy');
  }

  filterParticipants($event: CategoryWithDivisionFilterModel) {
    this.filter = $event;
    this.loadParticipants(this.getFilterModel());
  }

  private onLoadData($event: LazyLoadEvent) {
    this.sortField = $event.sortField;
    this.sortDirection = $event.sortOrder;
    this.loadParticipants(this.getFilterModel($event));
  }

  private loadParticipants(filterModel: ParticipantFilterModel) {
    this.participantsLoading = true;
    this.participantService.getParticipantsTableModel(filterModel).subscribe(r => {
      this.participantsLoading = false;
      this.participantsListModel = this.mapParticipants(r);
    });
  }

  private getFilterModel($event?: LazyLoadEvent, refreshPages: boolean = true): ParticipantFilterModel {
    let model = new ParticipantFilterModel();

    if ($event != null) {
      model.pageIndex = $event.first / $event.rows;
    } else if (!refreshPages) {
      model.pageIndex = this.firstIndex / this.rowsCount;
    } else {
      this.firstIndex = 0;
      model.pageIndex = 0;
    }
    model.sortField = ParticpantSortField[this.sortField];
    model.sortDirection = this.sortDirection;
    model.eventId = this.eventId;

    if (this.filter != null) {
      model.categoryId = this.filter.categoryId;
      model.weightDivisionId = this.filter.weightDivisionId;
      model.isMembersOnly = this.filter.isMembersOnly;
    }
    return model;
  }

  private mapParticipants(jsonData: PagedList<ParticipantTableModel>) {
    jsonData.innerList = jsonData.innerList.map(p => {
      p.dateOfBirth = new Date(p.dateOfBirth);
      p.categoryId = p.categoryId.toUpperCase();
      p.teamId = p.teamId.toUpperCase();
      p.weightDivisionId = p.weightDivisionId.toUpperCase();
      return p;
    });
    return jsonData;
  }
}
