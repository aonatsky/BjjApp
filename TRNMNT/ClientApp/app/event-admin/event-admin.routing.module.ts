import { Routes, RouterModule } from '@angular/router';
import { RedirectGuard } from '../core/guards/redirect.guard';
import { EventAdminPageComponent } from './event-admin.page.component';
import { EventListComponent } from './event-list/event-list.component';
import { EventEditComponent } from './event-edit/event-edit.component';
import { EventManagementComponent } from './event-management/event-management.component';
import { EventRunComponent } from './event-run/event-run.component';
import { EventRunWeightDivisionViewComponent } from './event-run-wd-view/event-run-wd-view.component';
import { EventRunCategoryViewComponent } from './event-run-category-view/event-run-category-view.component';
import { AuthGuard } from '../core/guards/auth.guard';
import { NgModule } from '@angular/core';
import { Roles } from '../core/consts/roles.const';
import { TeamListComponent } from './team-list/team-list.component';
import { FederationEditComponent } from './federation-edit/federation-edit.component';
import { EventDashboardComponent } from './event-dashboard/event-dashboard.component';

export const eventAdminRoutes: Routes = [
  {
    path: 'event-admin',
    component: EventAdminPageComponent,
    data: { expectedRoles: [Roles.Admin, Roles.Owner, Roles.FederationOwner] },
    canActivate: [RedirectGuard, AuthGuard],
    children: [
      {
        path: '',
        component: EventListComponent
      },
      {
        path: 'event-list',
        component: EventListComponent
      },
      {
        path: 'edit/:id',
        component: EventEditComponent
      },
      {
        path: 'dashboard/:id',
        component: EventDashboardComponent
      },
      {
        path: 'management/:id',
        component: EventManagementComponent
      },
      {
        path: 'dashboard/:id',
        component: EventDashboardComponent
      },
      
      {
        path: 'run/:id',
        component: EventRunComponent
      },
      {
        path: 'run-wd-spectator-view',
        component: EventRunWeightDivisionViewComponent
      },
      {
        path: 'run-category-spectator-view/:id',
        component: EventRunCategoryViewComponent
      },
      {
        path: 'edit',
        component: EventEditComponent
      },
      {
        path: 'federation-edit',
        component: FederationEditComponent,
        data: { expectedRoles: [Roles.Admin, Roles.FederationOwner] },
        canActivate: [AuthGuard]
      },
      {
        path: 'teams',
        component : TeamListComponent
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(eventAdminRoutes)],
  providers: [RouterModule]
})
export class EventAdminRoutingModule {}
