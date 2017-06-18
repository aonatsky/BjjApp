import { NgModule } from "@angular/core"
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms'
//Core

import { CoreModule } from "./../core/core.module"
import { RouterModule } from '@angular/router';

@NgModule({
    imports: [
        CoreModule,
        //RouterModule.forChild(administrationRoutes),
    ],
    declarations: [
        
    ],

    providers: [],

    exports: [
       
    ]
})

export class SharedModule {
}
