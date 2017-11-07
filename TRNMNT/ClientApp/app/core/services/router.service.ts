
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

    public goToEventDetails(id: string) {
        this.router.navigateByUrl("/event-admin/details/" + id);
    }

    public goToEventInfo() {
        this.router.navigateByUrl("event/event-info/");    
    }

    public goToRegistration(id: string) {
        this.router.navigateByUrl("event/event-registration/");
    }
}

