
import { Location, LocationStrategy, PathLocationStrategy } from '@angular/common';

import { Injectable, Inject } from '@angular/core';
import { Router, NavigationStart } from '@angular/router';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class RouterService {

    constructor(private router: Router, private location: Location, @Inject(Window) private win: Window) {
    }

    navigationStartEvents(): Observable<NavigationStart> {
        return <Observable<NavigationStart>>this.router.events.filter(event => event instanceof NavigationStart);
    }

    navigateByUrl(url: string) {
        this.router.navigateByUrl(url);
    } 

    openNewWindow(url: string) {
        this.win.open(url);
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

    goToEventInfo() {
        this.router.navigateByUrl('event/event-info/');    
    }

    goToRegistration(id: string) {
        this.router.navigateByUrl('event/event-registration/');
    }

    openEventWeightDivisionSpactatorView() {
        this.openNewWindow('/event-admin/run-wd-spectator-view');
    }

    openEventCategorySpactatorView(categoryId: string) {
        this.openNewWindow('/event-admin/run-category-spectator-view/' + categoryId);
    }
}

