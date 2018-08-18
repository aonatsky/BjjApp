import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ParticipantRegistrationComponent } from './participant-registration/participant-registration.component';
// tslint:disable-next-line:max-line-length
import { ParticipantRegistrationCompleteComponent } from './participant-registration-complete/participant-registration-complete.component';
import { EventComponent } from './event.component';
import { FooterComponent } from '../shared/footer/footer.component';
import { TopbarComponent } from '../shared/topbar/topbar.component';
import { AuthGuard } from '../core/guards/auth.guard';
import { TeamRegistrationComponent } from './team-registration/team-registration.component';
import { TeamRegistrationCompleteComponent } from './team-registration-complete/team-registration-complete.component';
import { ParticipantTeamRegistrationComponent } from './participant-team-registration/participant-team-registration.component';
import { Roles } from '../core/consts/roles.const';

const routes: Routes = [
  {
    path: 'event',
    component: EventComponent
  },
  {
    path: 'event/participant-registration',
    component: ParticipantRegistrationComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'event/participant-team-registration',
    component: ParticipantTeamRegistrationComponent,
    canActivate: [AuthGuard],
    data: { expectedRoles: [Roles.TeamOwner] },
  },
  {
    path: 'event/participant-registration-complete',
    component: ParticipantRegistrationCompleteComponent
  },
  {
    path: 'event/team-registration',
    component: TeamRegistrationComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'event/team-registration-complete',
    component: TeamRegistrationCompleteComponent,
    canActivate: [AuthGuard]
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EventRoutingModule {}
