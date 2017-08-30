import { DataService } from './../core/dal/contracts/data.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import {LoggerService} from './../core/services/logger.service'
import {AuthService} from './../core/services/auth.service'


@Component({
    selector: 'event',
    templateUrl: './event.component.html',
    styleUrls: ['./event.component.css']
})
export class EventComponent implements OnInit {

    userData: string;

    constructor(private dataService: DataService, private loggerService: LoggerService, private authService: AuthService) {

    }

    ngOnInit() {
        this.userData = JSON.stringify(this.authService.getUser());
    }




}
