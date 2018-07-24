import { Injectable } from '@angular/core';
import { Message } from 'primeng/primeng';
import { MessageLevel } from '../consts/message-level.const';
import { RouterService } from './router.service';
import { Subject } from 'rxjs';
import { DefaultValues } from '../consts/default-values';
import { TranslateService } from '../../../../node_modules/@ngx-translate/core';

@Injectable()
export class NotificationService {
  constructor(private routerService: RouterService, private translateService: TranslateService) {}

  notificationSubject: Subject<Message> = new Subject<Message>();

  get genericErrorMessage() {
    return this.getMessage(
      MessageLevel.Error,
      DefaultValues.SomethingWentWrongMessage,
      'An error occured during request'
    );
  }

  getMessage(level: string, summary: string, detail: string): Message {
    return {
      severity: level,
      summary: !!summary ? this.translateService.instant(summary) : '',
      detail: !!detail ? this.translateService.instant(detail) : ''
    };
  }

  showNotification(message: Message) {
    this.notificationSubject.next(message);
  }

  showInfo(summary: string, detail: string): void {
    this.showNotification(this.getMessage(MessageLevel.Info, summary, detail));
  }

  showWarn(summary: string, detail: string): void {
    this.showNotification(this.getMessage(MessageLevel.Warn, summary, detail));
  }

  showError(summary: string, detail: string): void {
    this.showNotification(this.getMessage(MessageLevel.Error, summary, detail));
  }

  showErrorWithDefaultSummary(detail: string): void {
    this.showError(DefaultValues.SomethingWentWrongMessage, detail);
  }

  showErrorOrGeneric(detail: string): void {
    if (detail == null || detail == '') {
      this.showGenericError();
    } else {
      this.showErrorWithDefaultSummary(detail);
    }
  }

  showSuccess(summary: string, detail: string): void {
    this.showNotification(this.getMessage(MessageLevel.Success, summary, detail));
  }

  showGenericError(): void {
    this.showNotification(this.genericErrorMessage);
  }

  subscribeOnNotifications(notifications: Message[]) {
    this.notificationSubject.subscribe(msg => notifications.push(msg));
  }

  get clearNotifications() {
    return this.routerService.navigationStartEvents();
  }
}
