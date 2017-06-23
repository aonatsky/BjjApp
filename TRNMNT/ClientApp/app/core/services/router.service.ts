
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable()
export class RouterService {

    constructor(private router: Router) {

    }

    public GoToLogin() {
        this.router.navigateByUrl("/login");
    }

    public GoToOrganizerScreen() {
        this.router.navigateByUrl("/administration/home");
    }

}

