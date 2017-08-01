import { NgModule } from "@angular/core"
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms'


import { CoreModule } from "./../core/core.module"
import { RouterModule, Routes } from '@angular/router';

import { EventAdminComponent } from './event-admin/event-admin.component'
import { TopbarComponent } from './topbar/topbar.component'
import { EventOverviewComponent } from './event-overview/event-overview.component'
import { EventCreateComponent } from './event-create/event-create.component'
import { CategoryComponent } from './event-create/category.component'
import { eventAdminRoutes } from './event-admin.routing'


@NgModule({

    imports: [
        CoreModule,
        RouterModule.forChild(eventAdminRoutes)
    ],
    declarations: [
        EventAdminComponent,
        TopbarComponent,
        EventOverviewComponent,
        EventCreateComponent,
        CategoryComponent
    ],

    providers: [],

    exports: [
       
    ]
})

export class EventAdminModule {
}
