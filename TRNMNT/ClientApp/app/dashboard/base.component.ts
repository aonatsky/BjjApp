import { DataService } from './../core/dal/contracts/data.service';
import { Component, OnInit } from '@angular/core';
import { RouterService } from './../core/services/router.service';
import { AuthService } from './../core/services/auth.service';
import { UserModel } from './../core/model/user.model';


@Component({
    selector: 'base-component',
    template: '',
})
export class BaseComponent implements OnInit {

    protected user: UserModel;

    constructor(private dataService: DataService, private routerService: RouterService, private authService: AuthService) {

    }

    ngOnInit() {
        this.user = this.authService.getUser();
    }


}