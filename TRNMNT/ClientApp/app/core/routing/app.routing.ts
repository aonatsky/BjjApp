import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuthGuard } from './auth.guard';
import { RedirectGuard } from './redirect.guard';


import { HomeComponent } from '../../administration/home/home.component';
import { EventOverviewComponent } from '../../event-admin/event-overview/event-overview.component';
import { TopbarComponent } from '../../event-admin/topbar/topbar.component';
import { EventAdminComponent } from '../../event-admin/event-admin/event-admin.component';
import { LoginComponent } from '../../shared/login/login.component';
import { EventInfoComponent } from '../../event-participation/event-info/event-info.component';



export const appRoutes: Routes = [
    { path: '', redirectTo: 'event-admin', pathMatch: 'full', canActivate: [RedirectGuard] },
    { path: 'login', component: LoginComponent, pathMatch: 'full' },
    { path: 'home', redirectTo: 'event-admin' },
    { path: '**', redirectTo: 'event-admin', canActivate: [AuthGuard]  }
];




