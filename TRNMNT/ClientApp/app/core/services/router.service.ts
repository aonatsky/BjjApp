
import { Location, LocationStrategy, PathLocationStrategy } from '@angular/common';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable()
export class RouterService {

    constructor(private router: Router, private location: Location) {

    }

    public navigateByUrl(url: string) {
        this.router.navigateByUrl(url);
    } 

    public GoHome(subdomain: string = "") {
        if (subdomain != "") {
            let path = location.host.replace(subdomain + ".", "");
            location.href = location.protocol + "//" + path;
        }

    }

    public GoToLogin(returnUrl? : string) {
        this.router.navigate(['/login'], { queryParams: { returnUrl: returnUrl } });
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

    public GoToEventInfo(subdomain: string) {
        this.router.navigateByUrl("event/event-info/" + subdomain, { skipLocationChange: false });    
    }

    public GoToParticipate(id: string) {
        this.router.navigateByUrl("event/participate/" + id, { skipLocationChange: false });
    }
}

