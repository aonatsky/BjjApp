import {ModuleWithProviders}  from '@angular/core';
import {Routes, RouterModule} from '@angular/router';


import { HomeComponent } from './administration/home/home.component';
import { ListUploadComponent } from './administration/listupload/listupload.component';
import { FighterListComponent } from './administration/fighter-list/fighter-list.component';
import { TournamentSettingsComponent } from './administration/tournament-settings/tournament-settings.component';

const appRoutes: Routes = [
     { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'settings', component: TournamentSettingsComponent },
            { path: 'fighter-list', component: FighterListComponent },
            { path: '**', redirectTo: 'home' }
];


export const appRoutingProviders: any[] = [];

export const routing: ModuleWithProviders = RouterModule.forRoot(appRoutes);
