import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Message } from 'primeng/primeng';
import { NotificationService } from '../core/services/notification.service';
import { LoaderService } from '../core/services/loader.service';
import { RouterService } from '../core/services/router.service';
import { EventService } from '../core/services/event.service';
import '../shared/styles/shared.scss';

@Component({
    selector: 'app',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css'],
    encapsulation: ViewEncapsulation.None
})
export class AppComponent implements OnInit {

    private notifications: Message[] = [];
    private isLoaderShown: boolean = false;

    constructor(private notificationservice: NotificationService,
        private loaderService: LoaderService,
        private routerService: RouterService,
        private eventService: EventService) {
    }
    

    ngOnInit() {
        this.notificationservice.notifications.subscribe((msgs) => msgs.map(m => this.notifications.push(m)));
    }
}
