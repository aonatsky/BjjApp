import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import {LoggerService} from './../../core/services/logger.service'
import {AuthService} from './../../core/services/auth.service'


@Component({
    selector: 'event-admin',
    templateUrl: './event-admin.component.html',
    styleUrls:['./event-admin.component.css']
})
export class EventAdminComponent implements OnInit {

    userData: string;

    constructor(private loggerService: LoggerService, private authService: AuthService) {

    }

    ngOnInit() {
        this.userData = JSON.stringify(this.authService.getUser());
    }




}
