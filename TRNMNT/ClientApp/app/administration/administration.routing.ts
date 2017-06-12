import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuthGuard } from '../core/routing/auth.guard';

import { HomeComponent } from './home/home.component';
import { FighterListComponent } from './fighter-list/fighter-list.component';
import { TournamentSettingsComponent } from './tournament-settings/tournament-settings.component';



export const administrationRoutes: Routes = [
    { path: 'administration/home', component: HomeComponent },
    { path: 'administration/fighter-list', component: FighterListComponent, canActivate: [AuthGuard] },
    { path: 'administration/settings', component: TournamentSettingsComponent },
];