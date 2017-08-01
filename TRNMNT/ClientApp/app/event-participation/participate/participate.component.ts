import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from './../../core/services/auth.service';
import { ParticipationModel } from './../../core/model/participation.model';
import { LoggerService } from './../../core/services/logger.service';
import { RouterService } from './../../core/services/router.service';


@Component({
    selector: 'participate',
    templateUrl: './participate.component.html',
    styleUrls: ['./participate.component.css']
})

export class ParticipateComponent {

    private eventId: string;
    private participation: ParticipationModel;


    constructor(private AuthService: AuthService, private routerService: RouterService, private loggerService: LoggerService, private route: ActivatedRoute) {

    }

    ngOnInit() {
        this.route.params.subscribe(p => {
            this.eventId = p['id'];
        });
        this.participation = new ParticipationModel();
    }

    processLogin(isAuthenticated: boolean) {
        if (isAuthenticated) {
            this.routerService.GoToEventAdmin();
        }
    }

}
