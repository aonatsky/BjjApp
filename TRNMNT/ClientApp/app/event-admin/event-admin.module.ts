import { NgModule } from "@angular/core"
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms'


import { CoreModule } from "./../core/core.module"
import { RouterModule, Routes } from '@angular/router';

import { EventAdminComponent } from './event-admin/event-admin.component'
import { TopbarComponent } from './topbar/topbar.component'
import { EventOverviewComponent } from './event-overview/event-overview.component'
import { EventEditComponent } from './event-edit/event-edit.component'
import { CategoryComponent } from './event-edit/category.component'
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
        EventEditComponent,
        CategoryComponent
    ],

    providers: [],

    exports: [
       
    ]
})

export class EventAdminModule {
}
