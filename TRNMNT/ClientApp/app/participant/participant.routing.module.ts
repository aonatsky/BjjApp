import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ParticipantComponent } from './participant.component';
import { AuthGuard } from '../core/guards/auth.guard';
import { MyEventsComponent } from './my-events/my-events.component';

const routes: Routes = [
  {
    path: 'participant',
    component: ParticipantComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: '',
        redirectTo: 'my-events'
      },
      {
        path: 'my-events',
        component: MyEventsComponent
      }
    ]
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EventRoutingModule {}
