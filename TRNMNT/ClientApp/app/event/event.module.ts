import { NgModule } from "@angular/core"
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms'


import { CoreModule } from "./../core/core.module"
import { AuthGuard } from "./../core/routing/auth.guard"
import { RouterModule, Routes } from '@angular/router'

import { EventInfoComponent } from './event-info/event-info.component'
import { EventRegistrationComponent } from './event-registration/event-registration.component'
import { EventRegistrationCompleteComponent } from './event-registration-complete/event-registration-complete.component'
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
                            path: 'event-registration/:id', component: EventRegistrationComponent, canActivate : [AuthGuard]
                        },
                        {
                            path: 'event-registration-complete/:id', component: EventRegistrationCompleteComponent, canActivate: [AuthGuard]
                        }
                    ]
                }
            ]),
    ],
    declarations: [
        EventInfoComponent,
        EventComponent,
        EventRegistrationComponent,
        EventRegistrationCompleteComponent
    ],

    providers: [],

    exports: [
    ]
})

export class EventModule {
}
