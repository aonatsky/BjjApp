import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuthGuard } from './auth.guard';


import { HomeComponent } from '../../administration/home/home.component';
import { FighterListComponent } from '../../administration/fighter-list/fighter-list.component';
import { TournamentSettingsComponent } from '../../administration/tournament-settings/tournament-settings.component';

export const appRoutes: Routes = [
    { path: '', redirectTo: 'administration/home', pathMatch: 'full' },
    { path: 'home', redirectTo: 'administration/home' },
    { path: '**', redirectTo: 'administration/home' }
];




