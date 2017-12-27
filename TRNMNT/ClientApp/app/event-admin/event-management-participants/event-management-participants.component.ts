import { ActivatedRoute } from '@angular/router';
import { LoggerService } from './../../core/services/logger.service';
import { RouterService } from './../../core/services/router.service';
import { Component, OnInit } from '@angular/core';
import { ParticipantTableModel } from './../../core/model/participant.models';
import { ParticipantService } from "./../../core/services/participant.service";
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { CrudColumn } from '../../shared/crud/crud.component';
import { DatePipe } from '@angular/common';
import { PagedList } from '../../core/model/paged-list.model';
import { ParticipantFilterModel } from '../../core/model/participant-filter.model';
import { CategoryWithDivisionFilterModel } from '../../core/model/category-with-division-filter.model';

@Component({
    selector: 'event-management-participants',
    templateUrl: './event-management-participants.component.html',
    providers: [DatePipe]
})
export class EventManagementParticipantsComponent implements OnInit {

    public eventId: string = "";
    private participantsListModel: PagedList<ParticipantTableModel>;
    private filter: CategoryWithDivisionFilterModel;
    private participantsLoading: boolean = true;
    private readonly pageLinks: number = 3;

    public get isPaginationEnabled(): boolean {
        return this.participantsModel.length > this.rowsCount;
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

    constructor(private loggerService: LoggerService, private routerService: RouterService, private participantService: ParticipantService, private route: ActivatedRoute, private datePipe: DatePipe) {

    }

    ngOnInit() {
        this.participantsLoading = true;
        this.route.params.subscribe(p => {
            let id = p["id"];
            if (!!id) {
                this.eventId = id;
                let model = new ParticipantFilterModel();
                model.eventId = this.eventId;
                model.pageIndex = 0;
                this.loadParticipants(model);
            } else {
                alert("No data to display");
            }
        });

    }

    loadData($event: LazyLoadEvent) {
        this.loadParticipants(this.getFilterModel($event));
    }

    columns: CrudColumn[] = [
        { propertyName: "firstName", displayName: "First Name", isEditable: true, isSortable : true},
        { propertyName: "lastName", displayName: "Last Name", isEditable: true, isSortable: true },
        { propertyName: "dateOfBirth", displayName: "D.O.B", isEditable: true, isSortable: true, transform: (value) => this.dateTransform(value, this)  },
        { propertyName: "teamName", displayName: "Team", isEditable: true, isSortable: true},
        { propertyName: "categoryName", displayName: "Category", isEditable: true, isSortable: true,  },
        { propertyName: "weightDivisionName", displayName: "Weight division", isEditable: true, isSortable: true,   },
        { propertyName: "isMember", displayName: "Membership", isEditable: true, isSortable: true, useClass : this.getClassCallback  }
    ];

    getClassCallback(value: boolean): string {
        let classes = "fa-square-o";
        if (value) {
            classes = 'fa-check-square-o';
        }
        return `fa ${classes}`;
    }

    dateTransform(value: any, context): string {
        return context.datePipe.transform(value, 'MM.dd.yyyy');
    }

    private loadParticipants(filterModel: ParticipantFilterModel) {
        this.participantsLoading = true;
        this.participantService.getParticipantsTableModel(filterModel).subscribe(r => {
            this.participantsLoading = false;
            this.participantsListModel = r;
        });
    }

    filterParticipants($event: CategoryWithDivisionFilterModel) {
        this.filter = $event;
        this.loadParticipants(this.getFilterModel());
    }

    private getFilterModel($event?: LazyLoadEvent): ParticipantFilterModel {
        let model = new ParticipantFilterModel();
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

    goToOverview() {
        this.routerService.goToEventAdmin();
    }
}
