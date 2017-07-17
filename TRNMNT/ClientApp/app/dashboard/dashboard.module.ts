import { NgModule } from "@angular/core"
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms'


import { CoreModule } from "./../core/core.module"
import { RouterModule, Routes } from '@angular/router';

import { DashboardComponent } from './dashboard/dashboard.component'
import { TopbarComponent } from './topbar/topbar.component'
import { EventOverviewComponent } from './event-overview/event-overview.component'
import { EventCreateComponent } from './event-create/event-create.component'
import { CategoryComponent } from './event-create/category.component'
import { BaseComponent } from './base.component'
import { dashboardRoutes } from './dashboard.routing'


@NgModule({

    imports: [
        CoreModule,
        RouterModule.forChild(dashboardRoutes)
    ],
    declarations: [
        DashboardComponent,
        TopbarComponent,
        EventOverviewComponent,
        EventCreateComponent,
        BaseComponent,
        CategoryComponent
    ],

    providers: [],

    exports: [
       
    ]
})

export class DashboardModule {
}
