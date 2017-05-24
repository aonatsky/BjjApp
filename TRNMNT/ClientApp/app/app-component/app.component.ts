import { Component, OnInit } from '@angular/core';
import { Message } from 'primeng/primeng';
import { NotificationService } from '../core/services/notification.service';
import { LoaderService } from '../core/services/loader.service';

@Component({
    selector: 'app',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

    private notifications: Message[] = [];
    private isLoaderShown: boolean = false;

    constructor(private notificationservice: NotificationService, private loaderService: LoaderService) {

    }

    ngOnInit() {
        this.notificationservice.notifications.subscribe(data => this.notifications = data);
        this.loaderService.isLoaderShown.subscribe(data => this.isLoaderShown = data);
    }



}
