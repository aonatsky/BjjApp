import { NgModule } from "@angular/core"
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms'


import { CoreModule } from "./../core/core.module"
import { RouterModule } from '@angular/router';

import { LoginComponent } from './login/login.component'
import { RegisterComponent } from './register/register.component'
import { BracketComponent } from './bracket/bracket.component'
import { TournamentUserPlate } from './tournament-user-plate/tournament-user-plate.component'
import { TournamentBracketCreation } from './tournament-bracket-creation/tournament-bracket-creation'


@NgModule({
    imports: [
        CoreModule,
    ],
    declarations: [
        LoginComponent,
        RegisterComponent,
        BracketComponent,
        TournamentUserPlate,
        TournamentBracketCreation
    ],

    providers: [],

    exports: [
       
    ]
})

export class SharedModule {
}
