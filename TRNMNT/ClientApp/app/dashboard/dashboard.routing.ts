import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuthGuard } from '../core/routing/auth.guard';

import { DashboardComponent } from './dashboard/dashboard.component'
import { TopbarComponent } from './topbar/topbar.component'
import { EventOverviewComponent } from './event-overview/event-overview.component'



export const dashboardRoutes: Routes = [
    { path: 'dashboard/eventoverview', component: EventOverviewComponent },
    { path: 'dashboard', component: EventOverviewComponent, pathMatch: 'full' },
    { path: 'dashboard/**', component: EventOverviewComponent, pathMatch: 'full' },
    {path: 'dashboard/**', outlet: "topmenu", component: TopbarComponent }
];