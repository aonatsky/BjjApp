import { NgModule } from "@angular/core"
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms'
//Core

import { CoreModule } from "./../core/core.module"
import { RouterModule } from '@angular/router';

import { administrationRoutes } from './administration.routing'



//Shared
import { FileUpload } from './shared/file-upload/file-upload.component';
import { FighterFilter } from './shared/fighter-filter/fighter-filter.component';
import { StaticHtmlComponent } from './shared/static-html/static-html.component';
import { CrudComponent } from './shared/crud/crud.component';


//Administration components
import { AdministrationRootComponent } from './administration.root/administration.root.component';
import { NavMenuComponent } from './navmenu/navmenu.component';
import { HomeComponent } from './home/home.component';
import { FighterListComponent } from './fighter-list/fighter-list.component';
import { TournamentSettingsComponent } from './tournament-settings/tournament-settings.component';



@NgModule({
    imports: [
        CoreModule,
        RouterModule.forChild(administrationRoutes),
    ],
    declarations: [
        AdministrationRootComponent,
        NavMenuComponent,
        HomeComponent,
        FighterListComponent,
        TournamentSettingsComponent,
        FighterFilter,
        FileUpload,
        CrudComponent,
        StaticHtmlComponent,
    ],

    providers: [],

    exports: [
        NavMenuComponent,
        AdministrationRootComponent
    ]
})

export class AdministrationModule {
}
