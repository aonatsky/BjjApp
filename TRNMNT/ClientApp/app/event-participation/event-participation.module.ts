import { NgModule } from "@angular/core"
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms'


import { CoreModule } from "./../core/core.module"
import { RouterModule, Routes } from '@angular/router';

import { EventInfoComponent } from './event-info/event-info.component'
import { ParticipateComponent } from './participate/participate.component'
import { EventParticipationComponent } from './event-participation.component'


@NgModule({

    imports: [
        CoreModule,
        RouterModule.forChild(
            [
                {
                    path: 'event-participation', component: EventParticipationComponent, children: [
                        {
                             path: 'event-info/:prefix', component: EventInfoComponent 
                        },
                        {
                            path: 'participate/:id', component: ParticipateComponent
                        }
                    ]
                }
            ]),
    ],
    declarations: [
        EventInfoComponent,
        EventParticipationComponent,
        ParticipateComponent
    ],

    providers: [],

    exports: [
    ]
})

export class EventPaticipationModule {
}
