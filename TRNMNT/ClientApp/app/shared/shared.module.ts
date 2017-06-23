import { NgModule } from "@angular/core"
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms'


import { CoreModule } from "./../core/core.module"
import { RouterModule } from '@angular/router';

import { LoginComponent } from './login/login.component'



@NgModule({
    imports: [
        CoreModule,
    ],
    declarations: [
        LoginComponent        
    ],

    providers: [],

    exports: [
       
    ]
})

export class SharedModule {
}
