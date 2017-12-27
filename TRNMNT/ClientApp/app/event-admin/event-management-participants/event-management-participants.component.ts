import { ActivatedRoute } from '@angular/router';
import { LoggerService } from './../../core/services/logger.service';
import { RouterService } from './../../core/services/router.service';
import { Component, OnInit } from '@angular/core';
import { ParticipantTableModel } from './../../core/model/participant.models';
import { ParticipantService } from "./../../core/services/participant.service";
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { CrudColumn } from '../../shared/crud/crud.component';
import { DatePipe } from '@angular/common';

@Component({
    selector: 'event-management-participants',
    templateUrl: './event-management-participants.component.html',
    providers: [DatePipe]
})
export class EventManagementParticipantsComponent implements OnInit {

    public eventId: string = "";
    private participantsModel: ParticipantTableModel[] = [];
    private participantsLoading: boolean = true;
    private readonly rowsCount: number = 10;
    private readonly pageLinks: number = 3;

    get isPaginationEnabled(): boolean {
        return this.participantsModel.length > this.rowsCount;
    }

    constructor(private loggerService: LoggerService, private routerService: RouterService, private participantService: ParticipantService, private route: ActivatedRoute, private datePipe: DatePipe) {

    }

    ngOnInit() {
        this.participantsLoading = true;
        this.route.params.subscribe(p => {
            let id = p["id"];
            if (!!id) {
                this.eventId = id;
                this.participantService.getParticipantsTableModel(id).subscribe(r => {
                    this.participantsLoading = false;
                    this.participantsModel = r;
                });
            } else {
                alert("No data to display");
            }
        });

    }

    loadData($event: LazyLoadEvent) {

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

    goToOverview() {
        this.routerService.goToEventAdmin();
    }
}
