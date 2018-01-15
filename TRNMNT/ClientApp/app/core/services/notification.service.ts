import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Message } from 'primeng/primeng'
import { MessageLevel } from '../consts/message-level.const';

@Injectable()
export class NotificationService {

    public static get genericErrorMessage() {
        return NotificationService.getMessage(MessageLevel.Error, "ERROR", "An error occured during request");
    }

    public static getMessage(level: string, summary: string, detail: string) : Message {
        return { severity: level, summary: summary, detail: detail };
    }

    notifications: BehaviorSubject<Message[]> = new BehaviorSubject<Message[]>([]);

    private showNotification(message: Message) {
        this.notifications.next([message]);
    }

    showInfo(summary: string, detail: string): void {
        this.showNotification(NotificationService.getMessage(MessageLevel.Info, summary, detail));
    }

    showWarn(summary: string, detail: string): void {
        this.showNotification(NotificationService.getMessage(MessageLevel.Warn, summary, detail));
    }

    showError(summary: string, detail: string): void {
        this.showNotification(NotificationService.getMessage(MessageLevel.Error, summary, detail));
    }

    showSuccess(summary: string, detail: string): void {
        this.showNotification(NotificationService.getMessage(MessageLevel.Success, summary, detail));
    }

    showGenericError(): void {
        this.showNotification(NotificationService.genericErrorMessage);
    }


    clearNotifications() {
        this.notifications.next([]);
    }
}