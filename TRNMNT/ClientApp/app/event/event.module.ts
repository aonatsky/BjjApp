import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { NgModule } from '@angular/core';
import { CoreModule } from '../core/core.module';
import { EventRegistrationComponent } from './event-registration/event-registration.component';
// tslint:disable-next-line:max-line-length
import { EventRegistrationCompleteComponent } from './event-registration-complete/event-registration-complete.component';
import { EventComponent } from './event.component';
import { EventRoutingModule } from './event.routing.module';
import { SharedModule } from '../shared/shared.module';
import { CustomLoader } from '../core/helpers/custom-loader';
import { TeamRegistrationComponent } from './team-registration/team-registration.component';

@NgModule({
  imports: [
    CoreModule,
    SharedModule,
    EventRoutingModule,
    TranslateModule.forChild({ loader: { provide: TranslateLoader, useClass: CustomLoader } })
  ],
  declarations: [
    EventComponent,
    EventRegistrationComponent,
    EventRegistrationCompleteComponent,
    TeamRegistrationComponent
  ],
  providers: [],
  exports: []
})
export class EventModule {}
