﻿import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EventRegistrationComponent } from './event-registration/event-registration.component';
// tslint:disable-next-line:max-line-length
import { EventRegistrationCompleteComponent } from './event-registration-complete/event-registration-complete.component';
import { EventComponent } from './event.component';
import { FooterComponent } from '../shared/footer/footer.component';
import { TopbarComponent } from '../shared/topbar/topbar.component';
import { AuthGuard } from '../core/guards/auth.guard';
import { TeamRegistrationComponent } from './team-registration/team-registration.component';
import { TeamRegistrationCompleteComponent } from './team-registration-complete/team-registration-complete.component';

const routes: Routes = [
  {
    path: 'event',
    component: EventComponent
  },
  {
    path: 'event/event-registration',
    component: EventRegistrationComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'event/event-registration-complete',
    component: EventRegistrationCompleteComponent
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
