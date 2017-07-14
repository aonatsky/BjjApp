import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuthGuard } from './auth.guard';
import { RedirectGuard } from './redirect.guard';


import { HomeComponent } from '../../administration/home/home.component';
import { EventOverviewComponent } from '../../dashboard/event-overview/event-overview.component';
import { TopbarComponent } from '../../dashboard/topbar/topbar.component';
import { DashboardComponent } from '../../dashboard/dashboard/dashboard.component';
import { LoginComponent } from '../../shared/login/login.component';
import { EventInfoComponent } from '../../shared/event-info/event-info.component';



export const appRoutes: Routes = [
    { path: '', redirectTo: 'dashboard', pathMatch: 'full', canActivate: [RedirectGuard] },
    { path: 'login', component: LoginComponent, pathMatch: 'full' },
    { path: 'home', redirectTo: 'dashboard' },
    { path: 'event-info/:id', component: EventInfoComponent },
    { path: '**', redirectTo: 'dashboard', canActivate: [RedirectGuard]  }
];




