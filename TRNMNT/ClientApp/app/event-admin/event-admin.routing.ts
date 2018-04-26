import { Routes } from '@angular/router';
import { RedirectGuard } from '../core/routing/redirect.guard';
import { EventAdminComponent } from './event-admin/event-admin.component'
import { TopbarComponent } from './topbar/topbar.component'
import { EventOverviewComponent } from './event-overview/event-overview.component'
import { EventEditComponent } from './event-edit/event-edit.component'
import { EventManagementComponent } from './event-management/event-management.component';
import { EventRunComponent } from './event-run/event-run.component';
import { EventRunWeightDivisionViewComponent } from './event-run-wd-view/event-run-wd-view.component';
import { EventRunCategoryViewComponent } from './event-run-category-view/event-run-category-view.component';




export const eventAdminRoutes: Routes = [
    {
        path: 'event-admin', component: EventAdminComponent, children: [
            {
                path: '', component: EventOverviewComponent
            },
            {
                path: 'edit/:id', component: EventEditComponent
            },
            {
                path: 'management/:id', component: EventManagementComponent,
            },
            {
                path: 'run/:id', component: EventRunComponent,
            },
            {
                path: 'run-wd-spectator-view/:id', component: EventRunWeightDivisionViewComponent,
            },
            {
                path: 'run-category-spectator-view/:id', component: EventRunCategoryViewComponent,
            },
            {
                path: 'edit', component: EventEditComponent
            },
            {
                path: '', outlet: 'topmenu', component: TopbarComponent
            },
        ]
        , canActivate: [RedirectGuard]

    },
];