import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app-component/app.component'
import { Routes, RouterModule } from '@angular/router';

//modules
import { CoreModule } from './core/core.module'
import { AdministrationModule } from './administration/administration.module'


import { appRoutes } from './core/routing/app.routing'


@NgModule({
    bootstrap: [AppComponent],
    declarations: [
        AppComponent,
       
    ],
    imports: [
        RouterModule.forRoot(appRoutes),

        CoreModule,
        AdministrationModule
      
],
    providers: [
    ]
})
export class AppModule {
}
