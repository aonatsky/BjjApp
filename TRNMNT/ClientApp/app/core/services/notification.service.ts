import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Message } from 'primeng/primeng'

@Injectable()
export class NotificationService {

    notifications: BehaviorSubject<Message[]> = new BehaviorSubject<Message[]>([]);

    showNotification(message: Message) {
        let value = this.notifications.getValue();
        value.push(message)
        this.notifications.next(value)
    }

    clearNotifications() {
        this.notifications.next([])
    }
}