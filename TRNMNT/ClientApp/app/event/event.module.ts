import { NgModule } from "@angular/core"
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms'


import { CoreModule } from "./../core/core.module"
import { AuthGuard } from "./../core/routing/auth.guard"
import { RouterModule, Routes } from '@angular/router'

import { EventInfoComponent } from './event-info/event-info.component'
import { ParticipateComponent } from './participate/participate.component'
import { EventComponent } from './event.component'


@NgModule({

    imports: [
        CoreModule,
        RouterModule.forChild(
            [
                {
                    path: 'event', component: EventComponent, children: [
                        {
                            path: 'event-info/:prefix', component: EventInfoComponent
                        },
                        {
                            path: 'participate/:id', component: ParticipateComponent, canActivate : [AuthGuard]
                        }
                    ]
                }
            ]),
    ],
    declarations: [
        EventInfoComponent,
        EventComponent,
        ParticipateComponent
    ],

    providers: [],

    exports: [
    ]
})

export class EventModule {
}
