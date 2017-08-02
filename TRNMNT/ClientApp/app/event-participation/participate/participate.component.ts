import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TeamService } from './../../core/services/team.service';
import { ParticipantModel } from './../../core/model/participant.model';
import { TeamModel } from './../../core/model/team.model';
import { LoggerService } from './../../core/services/logger.service';
import { RouterService } from './../../core/services/router.service';


@Component({
    selector: 'participate',
    templateUrl: './participate.component.html',
    styleUrls: ['./participate.component.css']
})

export class ParticipateComponent {

    private eventId: string;
    private participation: ParticipantModel;
    private existingTeams: TeamModel[] = [];


    constructor(private routerService: RouterService, private loggerService: LoggerService, private route: ActivatedRoute, private teamService: TeamService) {

    }

    ngOnInit() {
        this.route.params.subscribe(p => {
            this.eventId = p['id'];
        });
        this.participation = new ParticipantModel();
        this.teamService.getTeams().subscribe(r => this.existingTeams = r);
    }

 

}
