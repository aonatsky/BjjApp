import { NgModule } from '@angular/core';
import { CoreModule } from './../core/core.module';
import { RouterModule } from '@angular/router';
import { EventInfoComponent } from './event-info/event-info.component';
import { EventRegistrationComponent } from './event-registration/event-registration.component';
// tslint:disable-next-line:max-line-length
import { EventRegistrationCompleteComponent } from './event-registration-complete/event-registration-complete.component';
import { EventComponent } from './event.component';
import { FooterComponent } from '../shared/footer/footer.component';
import { TopbarComponent } from '../shared/topbar/topbar.component';
import { AuthGuard } from '../core/guards/auth.guard';

@NgModule({
  imports: [
    CoreModule,
    RouterModule.forChild([
      {
        path: 'event',
        component: EventComponent,
        children: [
          {
            path: 'event-info/',
            component: EventInfoComponent
          },
          {
            path: 'event-registration/',
            component: EventRegistrationComponent,
            canActivate: [AuthGuard]
          },
          {
            path: 'event-registration-complete/',
            component: EventRegistrationCompleteComponent
          },
          {
            path: '',
            outlet: 'footer',
            component: FooterComponent
          },
          {
            path: '',
            outlet: 'topbar',
            component: TopbarComponent
          }
        ]
      }
    ])
  ],
  declarations: [EventInfoComponent, EventComponent, EventRegistrationComponent, EventRegistrationCompleteComponent],
  providers: [],
  exports: []
})
export class EventModule {}
