import { Component, OnInit } from '@angular/core';
import { LoggerService } from './../../core/services/logger.service';
import { AuthService } from './../../core/services/auth.service';
import './event-admin.component.scss'


@Component({
    selector: 'event-admin',
    templateUrl: './event-admin.component.html',
})
export class EventAdminComponent implements OnInit {

    userData: string;

    constructor(private loggerService: LoggerService, private authService: AuthService) {
        
    }

    ngOnInit() {
        this.userData = JSON.stringify(this.authService.getUser());
    }
}
