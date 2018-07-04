import { Routes } from '@angular/router';
import { RedirectGuard } from '../core/guards/redirect.guard';
import { EventAdminPageComponent } from './event-admin.page.component';
import { TopbarComponent } from './../shared/topbar/topbar.component';
import { EventOverviewComponent } from './event-overview/event-overview.component';
import { EventEditComponent } from './event-edit/event-edit.component';
import { EventManagementComponent } from './event-management/event-management.component';
import { EventRunComponent } from './event-run/event-run.component';
import { EventRunWeightDivisionViewComponent } from './event-run-wd-view/event-run-wd-view.component';
import { EventRunCategoryViewComponent } from './event-run-category-view/event-run-category-view.component';
import { FooterComponent } from '../shared/footer/footer.component';
import { AuthGuard } from '../core/guards/auth.guard';

export const eventAdminRoutes: Routes = [
  {
    path: 'event-admin',
    component: EventAdminPageComponent,
    children: [
      {
        path: '',
        component: EventOverviewComponent
      },
      {
        path: 'edit/:id',
        component: EventEditComponent
      },
      {
        path: 'management/:id',
        component: EventManagementComponent
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
        path: '',
        outlet: 'topmenu',
        component: TopbarComponent
      },
      {
        path: '',
        outlet: 'footer',
        component: FooterComponent
      }
    ],
    canActivate: [RedirectGuard, AuthGuard]
  }
];
