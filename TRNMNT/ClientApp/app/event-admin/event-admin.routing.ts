import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuthGuard } from '../core/routing/auth.guard';
import { RedirectGuard } from '../core/routing/redirect.guard';

import { EventAdminComponent } from './event-admin/event-admin.component'
import { TopbarComponent } from './topbar/topbar.component'
import { EventOverviewComponent } from './event-overview/event-overview.component'
import { EventEditComponent } from './event-edit/event-edit.component'
import { EventDetailsComponent } from './event-details/event-details.component'



export const eventAdminRoutes: Routes = [
    {
        path: 'event-admin', component: EventAdminComponent, children: [
            {
                path: "", component: EventOverviewComponent
            },
            {
                path: "edit/:id", component: EventEditComponent
            },
            {
                path: "details/:id", component: EventDetailsComponent
            },
            {
                path: "edit", component: EventEditComponent
            },
            {
                path: "", outlet: "topmenu", component: TopbarComponent
            },
        ]
        , canActivate: [RedirectGuard]

    },
];