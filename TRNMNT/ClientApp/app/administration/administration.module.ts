import { NgModule } from "@angular/core"
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms'
//Core

import { CoreModule } from "./../core/core.module"
import { RouterModule } from '@angular/router';

//PrimeNG
import { DataTableModule } from "primeng/components/datatable/datatable";
import { DialogModule } from "primeng/components/dialog/dialog";
import { SharedModule } from "primeng/components/common/shared";
import { ButtonModule } from "primeng/components/button/button";
import { InputTextModule } from "primeng/components/inputtext/inputtext";
import { GrowlModule } from 'primeng/components/growl/growl';
import { DropdownModule } from 'primeng/components/dropdown/dropdown';


//Shared
import { FileUpload } from './shared/file-upload/file-upload.component';
import { FighterFilter } from './shared/fighter-filter/fighter-filter.component';
import { StaticHtmlComponent } from './shared/static-html/static-html.component';
import { CrudComponent } from './shared/crud/crud.component';


//Administration components
import { NavMenuComponent } from './navmenu/navmenu.component';
import { HomeComponent } from './home/home.component';
import { FighterListComponent } from './fighter-list/fighter-list.component';
import { TournamentSettingsComponent } from './tournament-settings/tournament-settings.component';



@NgModule({
    imports: [
        CoreModule,
        DataTableModule,
        DialogModule,
        InputTextModule,
        ButtonModule,
        DropdownModule,
        RouterModule,
    ],
    declarations: [
        NavMenuComponent,
        HomeComponent,
        FighterListComponent,
        TournamentSettingsComponent,
        FighterFilter,
        FileUpload,
        CrudComponent,
        StaticHtmlComponent
    ],

    providers: [],

    exports: [
        NavMenuComponent
    ]
})

export class AdministrationModule {
}
