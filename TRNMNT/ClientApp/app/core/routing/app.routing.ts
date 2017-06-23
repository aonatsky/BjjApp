import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuthGuard } from './auth.guard';


import { HomeComponent } from '../../administration/home/home.component';
import { LoginComponent } from '../../shared/login/login.component';



export const appRoutes: Routes = [
    { path: '', component: LoginComponent, pathMatch: 'full' },
    { path: 'login', component: LoginComponent, pathMatch: 'full' },
    { path: 'home', redirectTo: 'administration/home' },
    { path: '**', redirectTo: 'administration/home' }
];




