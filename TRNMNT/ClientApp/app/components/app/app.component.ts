import { Component, OnInit } from '@angular/core';
import { Message } from 'primeng/primeng';
import { NotificationService } from '../../core/services/notification.service';

@Component({
    selector: 'app',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

    private notifications: Message[] = [];

    constructor(private notificationservice: NotificationService) {

    }

    ngOnInit() {
        this.notificationservice.notifications.subscribe(data => this.notifications = data); 
    }

}
