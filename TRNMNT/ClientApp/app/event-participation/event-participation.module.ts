import { NgModule } from "@angular/core"
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms'


import { CoreModule } from "./../core/core.module"
import { RouterModule, Routes } from '@angular/router';

import { EventInfoComponent } from './event-info/event-info.component'


@NgModule({

    imports: [
        CoreModule,
    ],
    declarations: [
        EventInfoComponent
    ],

    providers: [],

    exports: [
    ]
})

export class EventPaticipationModule {
}
