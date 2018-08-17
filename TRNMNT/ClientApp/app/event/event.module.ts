import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { NgModule } from '@angular/core';
import { CoreModule } from '../core/core.module';
import { ParticipantRegistrationComponent } from './participant-registration/participant-registration.component';
import { ParticipantTeamRegistrationComponent } from './participant-team-registration/participant-team-registration.component';
// tslint:disable-next-line:max-line-length
import { ParticipantRegistrationCompleteComponent } from './participant-registration-complete/participant-registration-complete.component';
import { EventComponent } from './event.component';
import { EventRoutingModule } from './event.routing.module';
import { SharedModule } from '../shared/shared.module';
import { CustomLoader } from '../core/helpers/custom-loader';
import { TeamRegistrationComponent } from './team-registration/team-registration.component';
import { TeamRegistrationCompleteComponent } from './team-registration-complete/team-registration-complete.component';

@NgModule({
  imports: [
    CoreModule,
    SharedModule,
    EventRoutingModule,
    TranslateModule.forChild({ loader: { provide: TranslateLoader, useClass: CustomLoader } })
  ],
  declarations: [
    EventComponent,
    ParticipantRegistrationComponent,
    ParticipantRegistrationCompleteComponent,
    TeamRegistrationComponent,
    TeamRegistrationCompleteComponent,
    ParticipantTeamRegistrationComponent
  ],
  providers: [],
  exports: []
})
export class EventModule {}
