import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ParticipantComponent } from './participant.component';
import { AuthGuard } from '../core/guards/auth.guard';
import { MyEventsComponent } from './my-events/my-events.component';
import { MyTeamComponent } from './my-team/my-team.component';

const routes: Routes = [
  {
    path: 'participant',
    component: ParticipantComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: '',
        redirectTo: 'my-events',
        pathMatch: 'full'
      },
      {
        path: 'my-events',
        component: MyEventsComponent,
        pathMatch: 'full'
      },
      {
        path: 'my-team',
        component: MyTeamComponent,
        pathMatch: 'full'
      }
      ,
    ]
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ParticipantRoutingModule {}
