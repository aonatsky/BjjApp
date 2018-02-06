import { Injectable } from '@angular/core';
import { Message } from 'primeng/primeng'
import { MessageLevel } from '../consts/message-level.const';
import { RouterService } from './router.service';
import { Subject } from 'rxjs';
import { DefaultValues } from '../consts/default-values';

@Injectable()
export class NotificationService {

    constructor(private routerService: RouterService) {
    }

    public notificationSubject: Subject<Message> = new Subject<Message>();

    public get genericErrorMessage() {
        return this.getMessage(MessageLevel.Error, DefaultValues.SomethingWentWrongMessage, "An error occured during request");
    }

    public getMessage(level: string, summary: string, detail: string) : Message {
        return { severity: level, summary: summary, detail: detail };
    }

    public showNotification(message: Message) {
        this.notificationSubject.next(message);
    }

    public showInfo(summary: string, detail: string): void {
        this.showNotification(this.getMessage(MessageLevel.Info, summary, detail));
    }

    public showWarn(summary: string, detail: string): void {
        this.showNotification(this.getMessage(MessageLevel.Warn, summary, detail));
    }

    public showError(summary: string, detail: string): void {
        this.showNotification(this.getMessage(MessageLevel.Error, summary, detail));
    }

    public showErrorWithDefaultSummary(detail: string): void {
        this.showError(DefaultValues.SomethingWentWrongMessage, detail);
    }

    public showErrorOrGeneric(detail: string): void {
        if (detail == null || detail == "") {
            this.showGenericError();
        } else {
            this.showErrorWithDefaultSummary(detail);
        }
    }

    public showSuccess(summary: string, detail: string): void {
        this.showNotification(this.getMessage(MessageLevel.Success, summary, detail));
    }

    public showGenericError(): void {
        this.showNotification(this.genericErrorMessage);
    }

    public subscribeOnNotifications(notifications: Message[]) {
        this.notificationSubject.subscribe((msg) => notifications.push(msg));
        
    }

    public get clearNotifications() {
        return this.routerService.navigationStartEvents();
    }
}