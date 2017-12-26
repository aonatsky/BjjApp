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
import { EventManagementComponent } from "./event-management/event-management.component";
import { EventManagementParticipantsComponent } from "./event-management-participants/event-management-participants.component";
import { SharedModule } from "../shared/shared.module";


@NgModule({

    imports: [
        CoreModule,
        SharedModule,
        RouterModule.forChild(eventAdminRoutes)
    ],
    declarations: [
        EventAdminComponent,
        TopbarComponent,
        EventOverviewComponent,
        EventEditComponent,
        CategoryComponent,
        EventManagementComponent,
        EventManagementParticipantsComponent
    ],

    providers: [],

    exports: [
       
    ]
})

export class EventAdminModule {
}
