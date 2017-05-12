import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Message } from 'primeng/primeng'

@Injectable()
export class NotificationService {

    notifications: BehaviorSubject<Message[]> = new BehaviorSubject<Message[]>([]);

    private showNotification(message: Message) {
        this.notifications.next([message])
    }


    showInfo(summary: string, detail: string): void {
        this.showNotification({ severity: "info", summary: summary, detail: detail });
    }

    showWarn(summary: string, detail: string): void {
        this.showNotification({ severity: "warn", summary: summary, detail: detail });
    }

    showError(summary: string, detail: string): void {
        this.showNotification({ severity: "error", summary: summary, detail: detail });
    }

    showSuccess(summary: string, detail: string): void {
        this.showNotification({ severity: "success", summary: summary, detail: detail });
    }

    showGenericError(): void {
        this.showNotification({ severity: "error", summary: "ERROR", detail: "An error occured during request" });
    }


    clearNotifications() {
        this.notifications.next([])
    }
}