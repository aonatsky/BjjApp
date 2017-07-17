
import { Location, LocationStrategy, PathLocationStrategy } from '@angular/common';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable()
export class RouterService {

    constructor(private router: Router, private location: Location) {

    }

    public GoHome(subdomain: string = "") {
        if (subdomain != "") {
            let path = location.host.replace(subdomain + ".", "");
            location.href = location.protocol + "//" + path;
        }

    }

    public GoToLogin() {
        this.router.navigateByUrl("/login");
    }

    public GoToOrganizerScreen() {
        this.router.navigateByUrl("/administration/home");
    }


    public GoToDashboard() {
        this.router.navigateByUrl("/dashboard");
    }


    public GoToEditEvent(id: string) {
        this.router.navigateByUrl("/dashboard/edit/" + id);
    }

    public GoToEventInfo(id: string) {
        this.router.navigateByUrl("/event-info/" + id, { skipLocationChange: true });
    }
}

