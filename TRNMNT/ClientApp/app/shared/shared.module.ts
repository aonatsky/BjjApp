import { NgModule } from "@angular/core"
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms'


import { CoreModule } from "./../core/core.module"
import { RouterModule } from '@angular/router';

import { LoginComponent } from './login/login.component'
import { EventInfoComponent } from './event-info/event-info.component'
import { RegistrationComponent } from './registration/registration.component'



@NgModule({
    imports: [
        CoreModule,
    ],
    declarations: [
        LoginComponent,
        EventInfoComponent,
        RegistrationComponent
    ],

    providers: [],

    exports: [
       
    ]
})

export class SharedModule {
}
