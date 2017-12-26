import { ActivatedRoute } from '@angular/router';
import { LoggerService } from './../../core/services/logger.service';
import { RouterService } from './../../core/services/router.service';
import { Component, OnInit } from '@angular/core';
import { ParticipantTableModel }  from './../../core/model/participant.models';
import { ParticipantService } from "./../../core/services/participant.service";
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';

@Component({
    selector: 'event-management-participants',
    templateUrl: './event-management-participants.component.html'
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

    constructor(private loggerService: LoggerService, private routerService: RouterService, private participantService: ParticipantService, private route: ActivatedRoute) {

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

    goToOverview() {
        this.routerService.goToEventAdmin();
    }
}
