import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuthGuard } from '../core/routing/auth.guard';

import { DashboardComponent } from './dashboard/dashboard.component'
import { TopbarComponent } from './topbar/topbar.component'
import { EventOverviewComponent } from './event-overview/event-overview.component'
import { EventCreateComponent } from './event-create/event-create.component'



export const dashboardRoutes: Routes = [
    {
        path: 'dashboard', component: DashboardComponent, children: [
            {
                path: "", component: EventOverviewComponent
            },
            {
                path: "create", component: EventCreateComponent
            },
            {
                path: "", outlet: "topmenu", component: TopbarComponent
            },
        ]
        , canActivate: []

    },
];