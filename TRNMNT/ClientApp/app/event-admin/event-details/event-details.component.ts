
import { ActivatedRoute } from '@angular/router';
import { AuthService } from './../../core/services/auth.service';
import { EventService } from './../../core/services/event.service';
import { LoggerService } from './../../core/services/logger.service';
import { RouterService } from './../../core/services/router.service';
import { EventPreviewModel } from './../../core/model/event.models';
import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'event-details',
    templateUrl: './event-details.component.html'
})
export class EventDetailsComponent implements OnInit{
        
    private eventModel: EventPreviewModel;

    constructor(private loggerService: LoggerService, private routerService: RouterService, private eventService: EventService, private route: ActivatedRoute) {

    }

    
    ngOnInit() {
        this.route.params.subscribe(p => {
            let id = p["id"];
            if (id && id != "") {
                this.eventService.getEventBaseInfo(id).subscribe(r => { this.eventModel = r });
            } else {
                alert("No data to display")
            }
        });
    }

    goToOverview() {
        this.routerService.goToEventAdmin();
    }
}
