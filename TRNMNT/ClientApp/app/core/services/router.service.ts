
import { Location, LocationStrategy, PathLocationStrategy } from '@angular/common';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable()
export class RouterService {

    constructor(private router: Router, private location: Location) {

    }

    navigateByUrl(url: string) {
        this.router.navigateByUrl(url);
    } 

    goHome(subdomain: string = '') {
        if (subdomain != '') {
            let path = location.host.replace(subdomain + '.', '');
            location.href = location.protocol + '//' + path;
        }

    }

    goToLogin(returnUrl? : string) {
        this.router.navigate(['/login'], { queryParams: { returnUrl: returnUrl } });
    }


    goToOrganizerScreen() {
        this.router.navigateByUrl('/administration/home');
    }


    goToEventAdmin() {
        this.router.navigateByUrl('/event-admin');
    }


    goToEditEvent(id: string) {
        this.router.navigateByUrl('/event-admin/edit/' + id);
    }

    goToEventManagement(id: string) {
        this.router.navigateByUrl('/event-admin/management/' + id);
    }

    goToEventRun(id: string) {
        this.router.navigateByUrl('/event-admin/run/' + id);
    }

    goToEventManagementParticipants(id: string) {
        this.router.navigateByUrl('/event-admin/management/participants/' + id);
    }

    goToEventInfo() {
        this.router.navigateByUrl('event/event-info/');    
    }

    goToRegistration(id: string) {
        this.router.navigateByUrl('event/event-registration/');
    }
}

