import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Message } from 'primeng/primeng';
import { NotificationService } from './core/services/notification.service';
import { LoaderService } from './core/services/loader.service';
import { RouterService } from './core/services/router.service';
import { EventService } from './core/services/event.service';
import '../assets/themes/trnmnt/theme.scss';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app',
  templateUrl: './app.component.html',
  styleUrls: ['../assets/themes/trnmnt/theme.scss'],
  encapsulation: ViewEncapsulation.None
})
export class AppComponent implements OnInit {
  notifications: Message[] = [];
  isLoaderShown: boolean = false;
  private loaderSubscription: Subscription;
  private notificationSubscription: Subscription;
  private clearNotificationSubscription: Subscription;

  constructor(
    private notificationService: NotificationService,
    private loaderService: LoaderService,
    private routerService: RouterService,
    private eventService: EventService
  ) {}

  ngOnInit() {
    this.notificationSubscription = this.notificationService.notificationSubject.subscribe(msg =>
      this.notifications.push(msg)
    );
    this.clearNotificationSubscription = this.notificationService.clearNotifications.subscribe(
      () => (this.notifications = [])
    );
    this.loaderSubscription = this.loaderService.loaderCounter.subscribe((counter: number) => {
      setTimeout(() => (this.isLoaderShown = counter !== 0), 0);
    });
  }

  ngOnDestroy() {
    this.loaderSubscription.unsubscribe();
    this.notificationSubscription.unsubscribe();
    this.clearNotificationSubscription.unsubscribe();
  }
}
