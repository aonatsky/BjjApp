import { Component, OnInit } from '@angular/core';
import { LoggerService } from './../core/services/logger.service'
import { AuthService } from './../core/services/auth.service'


@Component({
    selector: 'event',
    templateUrl: './event.component.html',
    styleUrls: ['./event.component.css']
})
export class EventComponent implements OnInit {

    userData: string;

    constructor(private loggerService: LoggerService, private authService: AuthService) {

    }

    ngOnInit() {
        this.userData = JSON.stringify(this.authService.getUser());
    }
}
