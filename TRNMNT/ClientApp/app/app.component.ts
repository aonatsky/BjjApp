import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Message } from 'primeng/primeng';
import { NotificationService } from './core/services/notification.service';
import { LoaderService } from './core/services/loader.service';
import { RouterService } from './core/services/router.service';
import { EventService } from './core/services/event.service';
import { Subscription } from 'rxjs';
import { TranslateService } from '@ngx-translate/core';

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
    private eventService: EventService,
    private translate: TranslateService
  ) {
    // this language will be used as a fallback when a translation isn't found in the current language
    translate.setDefaultLang('en');

    // the lang to use, if the lang isn't available, it will use the current loader to get them
    translate.use('ua');
    // const browserLang = translate.getBrowserLang();
    // translate.use(browserLang.match(/en|ua/) ? browserLang : 'ua');
  }

  ngOnInit() {
    this.translate.get('Sign in or').subscribe((res: string) => {
      console.log(res);
  });
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
