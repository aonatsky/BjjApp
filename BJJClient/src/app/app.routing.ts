import {ModuleWithProviders}  from '@angular/core';
import {Routes, RouterModule} from '@angular/router';


import { HomeComponent } from './components/home/home.component';
import { ListUploadComponent } from './components/listupload/listupload.component';

const appRoutes: Routes = [
     { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'list-upload', component: ListUploadComponent },
            { path: '**', redirectTo: 'home' }
];


export const appRoutingProviders: any[] = [];

export const routing: ModuleWithProviders = RouterModule.forRoot(appRoutes);
