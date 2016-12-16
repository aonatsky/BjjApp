import {ModuleWithProviders}  from '@angular/core';
import {Routes, RouterModule} from '@angular/router';

//import {HomeComponent} from './home/home.component';
import {AboutComponent} from './about/about.component';
import { HomeComponent } from './components/home/home.component';
// import { FetchDataComponent } from './components/fetchdata/fetchdata.component';
// import { CounterComponent } from './components/counter/counter.component';
import { ListUploadComponent } from './components/listupload/listupload.component';

const appRoutes: Routes = [
     { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            // { path: 'counter', component: CounterComponent },
            // { path: 'fetch-data', component: FetchDataComponent },
            { path: 'list-upload', component: ListUploadComponent },
            { path: '**', redirectTo: 'home' }
];
// const appRoutes: Routes = [
//     { path: '', component: HomeComponent },
//     { path: 'about', component: AboutComponent }
// ];

export const appRoutingProviders: any[] = [];

export const routing: ModuleWithProviders = RouterModule.forRoot(appRoutes);
