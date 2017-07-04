
import { AuthService } from './../../core/services/auth.service';
import { LoggerService } from './../../core/services/logger.service';
import { RouterService } from './../../core/services/router.service';
import { Component } from '@angular/core';

@Component({
    selector: 'event-overview',
    templateUrl: './event-overview.component.html'
})
export class EventOverviewComponent {
    constructor(private loggerService: LoggerService, private routerService: RouterService, private authService: AuthService) {
        
    }

    public createEvent() {
        this.routerService.GoToCreateEvent();
    }
}
