import { DataService } from '../../core/dal/contracts/data.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import {LoggerService} from './../../core/services/logger.service'
import {AuthService} from './../../core/services/auth.service'


@Component({
    selector: 'dashboard',
    templateUrl: './dashboard.component.html'
})
export class DashboardComponent implements OnInit {

    userData: string;

    constructor(private dataService: DataService, private loggerService: LoggerService, private authService: AuthService) {

    }

    ngOnInit() {
        this.userData = JSON.stringify(this.authService.getUser());
    }




}
