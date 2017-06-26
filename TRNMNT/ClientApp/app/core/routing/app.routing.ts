import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuthGuard } from './auth.guard';


import { HomeComponent } from '../../administration/home/home.component';
import { EventOverviewComponent } from '../../dashboard/event-overview/event-overview.component';
import { TopbarComponent } from '../../dashboard/topbar/topbar.component';
import { DashboardComponent } from '../../dashboard/dashboard/dashboard.component';
import { LoginComponent } from '../../shared/login/login.component';



export const appRoutes: Routes = [
    { path: '', component: LoginComponent, pathMatch: 'full' },
    { path: 'login', component: LoginComponent, pathMatch: 'full' },
    { path: 'home', redirectTo: 'administration/home' },
    {
        path: 'dashboard', component: DashboardComponent, children: [
            {
                path: "", component: EventOverviewComponent
            },
            {
                path: "", outlet: "topmenu", component: TopbarComponent
            }]

    },
    { path: '**', redirectTo: 'administration/home' }
];




