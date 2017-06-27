import { DataService } from '../../core/dal/contracts/data.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import {LoggerService} from './../../core/services/logger.service'
import {UserService} from './../../core/services/user.service'


@Component({
    selector: 'dashboard',
    templateUrl: './dashboard.component.html'
})
export class DashboardComponent implements OnInit {

    userData: string;

    constructor(private dataService: DataService, private loggerService: LoggerService, private userService: UserService) {

    }

    ngOnInit() {
        this.userData = this.userService.getUser();
    }




}
