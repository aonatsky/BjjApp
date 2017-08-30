import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app-component/app.component'
import { Routes, RouterModule } from '@angular/router';

//modules
import { CoreModule } from './core/core.module'
import { SharedModule } from './shared/shared.module'
import { EventAdminModule } from './event-admin/event-admin.module'
import { EventModule } from './event/event.module'



import { appRoutes } from './core/routing/app.routing'


@NgModule({
    bootstrap: [AppComponent],
    declarations: [
        AppComponent,
       
    ],
    imports: [
        RouterModule.forRoot(appRoutes),
        CoreModule,
        SharedModule,
        EventAdminModule,
        EventModule
],
    providers: []
})
export class AppModule {
}
