
import { Location, LocationStrategy, PathLocationStrategy } from '@angular/common';

import { Injectable } from '@angular/core';
import { Router, NavigationStart } from '@angular/router';

@Injectable()
export class RouterService {

    constructor(private router: Router, private location: Location) {
        router.events
            .filter(event => event instanceof NavigationStart)
            .subscribe((event: NavigationStart) => {
                let test = event;
            });
    }

    public navigateByUrl(url: string) {
        this.router.navigateByUrl(url);
    } 

    public goHome(subdomain: string = "") {
        if (subdomain != "") {
            let path = location.host.replace(subdomain + ".", "");
            location.href = location.protocol + "//" + path;
        }

    }

    public goToLogin(returnUrl? : string) {
        this.router.navigate(['/login'], { queryParams: { returnUrl: returnUrl } });
    }


    public goToOrganizerScreen() {
        this.router.navigateByUrl("/administration/home");
    }


    public goToEventAdmin() {
        this.router.navigateByUrl("/event-admin");
    }


    public goToEditEvent(id: string) {
        this.router.navigateByUrl("/event-admin/edit/" + id);
    }

    public goToEventManagement(id: string) {
        this.router.navigateByUrl("/event-admin/management/" + id);
    }

    public goToEventManagementParticipants(id: string) {
        this.router.navigateByUrl("/event-admin/management/participants/" + id);
    }

    public goToEventInfo() {
        this.router.navigateByUrl("event/event-info/");    
    }

    public goToRegistration(id: string) {
        this.router.navigateByUrl("event/event-registration/");
    }
}

