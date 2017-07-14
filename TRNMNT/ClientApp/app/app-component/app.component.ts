import { Component, OnInit } from '@angular/core';
import { Message } from 'primeng/primeng';
import { NotificationService } from '../core/services/notification.service';
import { LoaderService } from '../core/services/loader.service';
import { RouterService } from '../core/services/router.service';
import { EventService } from '../core/services/event.service';

@Component({
    selector: 'app',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

    private notifications: Message[] = [];
    private isLoaderShown: boolean = false;

    constructor(private notificationservice: NotificationService, private loaderService: LoaderService, private routerService: RouterService, private eventService: EventService) {
    }

    

    ngOnInit() {
        this.notificationservice.notifications.subscribe(data => this.notifications = data);
        this.loaderService.isLoaderShown.subscribe(data => this.isLoaderShown = data);
        this.processSubdomain();
    }


    private processSubdomain() {
        let subdomain = this.getSubdomain();
        this.routerService.GoToEventInfo("test")
        //this.eventService.getEventIdByUrl(subdomain);
    }

    getSubdomain() : string {
        let subdomain = '';
        const domain = window.location.hostname;
        if (domain.indexOf('.') < 0 ||
            domain.split('.')[0] === 'example' || domain.split('.')[0] === 'lvh' || domain.split('.')[0] === 'www') {
            subdomain = '';
        } else {
            subdomain = domain.split('.')[0];
        }
        return subdomain;
    }

}
