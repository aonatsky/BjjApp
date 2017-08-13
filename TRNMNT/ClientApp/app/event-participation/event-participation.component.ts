import { DataService } from './../core/dal/contracts/data.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import {LoggerService} from './../core/services/logger.service'
import {AuthService} from './../core/services/auth.service'


@Component({
    selector: 'event-participation',
    templateUrl: './event-participation.component.html',
    styleUrls: ['./event-participation.component.css']
})
export class EventParticipationComponent implements OnInit {

    userData: string;

    constructor(private dataService: DataService, private loggerService: LoggerService, private authService: AuthService) {

    }

    ngOnInit() {
        this.userData = JSON.stringify(this.authService.getUser());
    }




}
