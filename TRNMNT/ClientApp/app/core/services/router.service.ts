
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


    public GoToEventAdmin() {
        this.router.navigateByUrl("/event-admin");
    }


    public GoToEditEvent(id: string) {
        this.router.navigateByUrl("/event-admin/edit/" + id);
    }

    public GoToEventInfo(id: string) {
        this.router.navigateByUrl("event-participation/event-info/" + id, { skipLocationChange: false });    
    }

    public GoToParticipate(id: string) {
        this.router.navigateByUrl("event-participation/participate/" + id, { skipLocationChange: false });
    }
}

