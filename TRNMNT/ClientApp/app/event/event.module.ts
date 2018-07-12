import { TranslateModule } from '@ngx-translate/core';
import { NgModule } from '@angular/core';
import { CoreModule } from '../core/core.module';
import { EventInfoComponent } from './event-info/event-info.component';
import { EventRegistrationComponent } from './event-registration/event-registration.component';
// tslint:disable-next-line:max-line-length
import { EventRegistrationCompleteComponent } from './event-registration-complete/event-registration-complete.component';
import { EventComponent } from './event.component';
import { EventRoutingModule } from './event.routing.module';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  imports: [CoreModule, SharedModule, EventRoutingModule, TranslateModule],
  declarations: [EventInfoComponent, EventComponent, EventRegistrationComponent, EventRegistrationCompleteComponent],
  providers: [],
  exports: []
})
export class EventModule {}
