import { NgModule } from "@angular/core"
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms'


import { CoreModule } from "./../core/core.module"
import { RouterModule } from '@angular/router';

import { DashboardComponent } from './dashboard/dashboard.component'
import { TopbarComponent } from './topbar/topbar.component'
import { EventOverviewComponent } from './event-overview/event-overview.component'
import { dashboardRoutes } from './dashboard.routing'



@NgModule({
    imports: [
        CoreModule,
        //RouterModule.forChild(dashboardRoutes)
    ],
    declarations: [
        DashboardComponent,
        TopbarComponent,
        EventOverviewComponent
    ],

    providers: [],

    exports: [
       
    ]
})

export class DashboardModule {
}
