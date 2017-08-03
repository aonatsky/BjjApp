import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TeamService } from './../../core/services/team.service';
import { CategoryService } from './../../core/services/category.service';
import { WeightDivisionService } from './../../core/services/weight-division.service';
import { ParticipantModel } from './../../core/model/participant.model';
import { TeamModel } from './../../core/model/team.model';
import { LoggerService } from './../../core/services/logger.service';
import { RouterService } from './../../core/services/router.service';
import { Observable } from "rxjs/Observable";

@Component({
    selector: 'participate',
    templateUrl: './participate.component.html',
    styleUrls: ['./participate.component.css']
})

export class ParticipateComponent {

    private eventId: string;
    private participation: ParticipantModel;
    private existingTeams: TeamModel[] = [];


    constructor(
        private routerService: RouterService,
        private loggerService: LoggerService,
        private route: ActivatedRoute,
        private weightDivisionservice: WeightDivisionService,
        private categoryService: CategoryService,
        private teamService: TeamService,

    ) {

    }

    ngOnInit() {
        this.route.params.subscribe(p => {
            this.eventId = p['id'];
        });
        this.participation = new ParticipantModel();
        this.loadData();
    }


    private loadData() {
        Observable.forkJoin(this.teamService.getTeams(), this.categoryService.getCategories(this.eventId))
            .subscribe(data => this.initData(data));
    }

    private initData(data: Object[]) {
        let teams = data[0];
        let categories = data[1];
        console.log(teams);
        console.log(categories);
    }


}
