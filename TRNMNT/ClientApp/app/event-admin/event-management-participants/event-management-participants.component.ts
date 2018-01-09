import { ActivatedRoute } from '@angular/router';
import { LoggerService } from './../../core/services/logger.service';
import { RouterService } from './../../core/services/router.service';
import { Component, OnInit } from '@angular/core';
import { ParticipantTableModel, ParticipantDdlModel } from './../../core/model/participant.models';
import { ParticipantService } from "./../../core/services/participant.service";
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { CrudColumn, ColumnType, IColumnOptions, IDdlColumnChangeEvent } from '../../shared/crud/crud.component';
import { DatePipe } from '@angular/common';
import { PagedList } from '../../core/model/paged-list.model';
import { ParticipantFilterModel } from '../../core/model/participant-filter.model';
import { CategoryWithDivisionFilterModel } from '../../core/model/category-with-division-filter.model';
import { ParticpantSortField } from '../../core/consts/participant-sort-field.const';
import { SelectItem } from 'primeng/components/common/selectitem';

@Component({
    selector: 'event-management-participants',
    templateUrl: './event-management-participants.component.html',
    providers: [DatePipe]
})
export class EventManagementParticipantsComponent implements OnInit {

    private participantsListModel: PagedList<ParticipantTableModel>;
    private participantDdlModel: ParticipantDdlModel;
    private filter: CategoryWithDivisionFilterModel;

    private teamSelectItems: SelectItem[];
    private categorySelectItems: SelectItem[];
    private weightDivisionSelectItems: SelectItem[];

    private readonly pageLinks: number = 3;
    private eventId: string = "";
    private participantsLoading: boolean = true;
    private ddlDataLoading: boolean = true;
    private sortDirection: number = 1;
    private sortField: string = "firstName";

    public get isPaginationEnabled(): boolean {
        return this.participantsModel.length > this.rowsCount;
    }

    public get isDataLoading(): boolean {
        return this.participantsLoading || this.ddlDataLoading;
    }

    public get participantsModel(): ParticipantTableModel[] {
        if (this.participantsListModel != null) {
            return this.participantsListModel.innerList;
        }
        return [];
    }

    public get rowsCount(): number {
        if (this.participantsListModel != null) {
            return this.participantsListModel.pageSize;
        }
        return 10;
    }

    public get totalCount(): number {
        if (this.participantsListModel != null) {
            return this.participantsListModel.totalCount;
        }
        return 0;
    }


    constructor(private loggerService: LoggerService,
        private routerService: RouterService,
        private participantService: ParticipantService,
        private route: ActivatedRoute,
        private datePipe: DatePipe) {

    }

    ngOnInit() {
        this.ddlDataLoading = true;
        this.route.params.subscribe(p => {
            let id = p["id"];
            if (!!id) {
                this.eventId = id;
                this.participantService.getParticipantsDropdownData(this.eventId).subscribe(result => {
                    this.ddlDataLoading = false;
                    this.participantDdlModel = result;
                    this.mapDdlModel(result);
                });
                this.loadParticipants(this.getFilterModel());
            } else {
                alert("No data to display");
            }
        });

    }

    columns: CrudColumn[] = [
        { propertyName: "firstName", displayName: "First Name", isEditable: true, isSortable: true },
        { propertyName: "lastName", displayName: "Last Name", isEditable: true, isSortable: true },
        {
            propertyName: "dateOfBirth",
            displayName: "D.O.B",
            isEditable: true,
            isSortable: true,
            transform: (value) => this.dateTransform.call(this, value),
            columnType: ColumnType.Date
        },
        <CrudColumn>{
            propertyName: "teamName",
            displayName: "Team",
            isEditable: true,
            isSortable: true,
            columnType: ColumnType.Dropdown,
            keyPropertyName: "teamId"
        },
        <CrudColumn>{
            propertyName: "categoryName",
            displayName: "Category",
            isEditable: true,
            isSortable: true,
            columnType: ColumnType.Dropdown,
            keyPropertyName: "categoryId",
            onChange: (event) => this.onCategoryChange.call(this, event)
        },
        <CrudColumn>{
            propertyName: "weightDivisionName",
            displayName: "Weight division",
            isEditable: true,
            isSortable: true,
            columnType: ColumnType.Dropdown,
            keyPropertyName: "weightDivisionId"
        },
        {
            propertyName: "isMember",
            displayName: "Membership",
            isEditable: true,
            isSortable: true,
            useClass: (value) => this.getClassCallback.call(this, value),
            columnType: ColumnType.Boolean
        }
    ];

    columnOptions: IColumnOptions = {};

    public getClassCallback(value: boolean): string {
        let classes = "fa-square-o";
        if (value) {
            classes = 'fa-check-square-o';
        }
        return `fa ${classes}`;
    }

    public dateTransform(value: Date): string {
        return this.datePipe.transform(value, 'MM.dd.yyyy');
    }

    public onCategoryChange($event: IDdlColumnChangeEvent) {
        let categoryId = $event.value;
        this.weightDivisionSelectItems = this.getWeightDivisionsForCategory(categoryId);
        this.refreshDdlModel();
    }

    public filterParticipants($event: CategoryWithDivisionFilterModel) {
        this.filter = $event;
        this.loadParticipants(this.getFilterModel());
    }

    public onEntitySelected(participant: ParticipantTableModel) {
        this.weightDivisionSelectItems = this.getWeightDivisionsForCategory(participant.categoryId);
        this.refreshDdlModel();
    }

    public onEntityUpdate($event) {
        
    }

    public onEntityDelete($event) {
        
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

    private getFilterModel($event?: LazyLoadEvent): ParticipantFilterModel {
        let model = new ParticipantFilterModel();

        model.sortField = ParticpantSortField[this.sortField];
        model.sortDirection = this.sortDirection;
        model.eventId = this.eventId;

        if ($event != null) {
            model.pageIndex = $event.first / $event.rows;
        } else {
            model.pageIndex = 0;
        }

        if (this.filter != null) {
            model.categoryId = this.filter.categoryId;
            model.weightDivisionId = this.filter.weightDivisionId;
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

    private getWeightDivisionsForCategory(categoryId: string) {
        let weightDivisions = this.participantDdlModel.weightDivisions.filter(wd => wd.categoryId.toUpperCase() == categoryId.toUpperCase());
        if (weightDivisions.length > 0) {
            return this.mapDdls(weightDivisions, "weightDivisionId");
        }
        return [];
    }

    private mapDdlModel(model: ParticipantDdlModel) {
        this.teamSelectItems = this.mapDdls(model.teams, "teamId");
        this.categorySelectItems = this.mapDdls(model.categories, "categoryId");
        this.weightDivisionSelectItems = this.mapDdls(model.weightDivisions, "weightDivisionId");
        this.refreshDdlModel();
    }
    private refreshDdlModel() {
        this.columnOptions = {
            "weightDivisionName": this.weightDivisionSelectItems,
            "categoryName": this.categorySelectItems,
            "teamName": this.teamSelectItems
        };
    }
    private mapDdls(array: any[], idPropName: string): SelectItem[] {
        return array.map(t => {
            return { label: t.name, value: t[idPropName] };
        });
    }

    goToOverview() {
        this.routerService.goToEventAdmin();
    }
}
