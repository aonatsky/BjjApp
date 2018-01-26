﻿import { NgModule } from '@angular/core'
import { CoreModule } from './../core/core.module'
import { RouterModule } from '@angular/router';
import { EventAdminComponent } from './event-admin/event-admin.component'
import { TopbarComponent } from './topbar/topbar.component'
import { EventOverviewComponent } from './event-overview/event-overview.component'
import { EventEditComponent } from './event-edit/event-edit.component'
import { eventAdminRoutes } from './event-admin.routing'
import { BracketGenerationComponent } from './event-management/brackets-generation/bracket-generation.component';
import { EventManagementComponent } from './event-management/event-management.component';
import { EventManagementParticipantsComponent } from './event-management-participants/event-management-participants.component';
import { SharedModule } from '../shared/shared.module';
import { PrticipantsListUploadComponent } from './participant-list-upload/participant-list-upload.component';
import { CategoryEditComponent } from './event-edit/category-edit/category-edit.component';
import { WebsocketInteractionComponent } from './event-websocket-interaction/websocket-interactiion.component';


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
        BracketGenerationComponent,
        EventManagementComponent,
        CategoryEditComponent,
        EventManagementParticipantsComponent,
        PrticipantsListUploadComponent,
        WebsocketInteractionComponent
    ],

    providers: [],

    exports: [

    ]
})

export class EventAdminModule {
}
