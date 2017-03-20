import { NgModule } from '@angular/core';
import { HttpModule, Http } from '@angular/http';
import { FormsModule   } from '@angular/Forms';
import {BrowserModule} from '@angular/platform-browser';

import { RouterModule } from '@angular/router';
import { UniversalModule } from 'angular2-universal';
import { AppComponent } from './components/app/app.component'
import { routing, appRoutingProviders } from "./app.routing";

//Administration components
import { NavMenuComponent } from './administration/navmenu/navmenu.component';
import { HomeComponent } from './administration/home/home.component';
import { ListUploadComponent } from './administration/listupload/listupload.component';
import { FighterListComponent } from './administration/fighter-list/fighter-list.component';
import { TournamentSettingsComponent } from './administration/tournament-settings/tournament-settings.component';


//Shared
import { FileUpload } from './shared/file-upload/file-upload.component';
import { DropdownComponent } from './shared/dropdown/dropdown.component';
import { FighterFilter } from './shared/fighter-filter/fighter-filter.component';
import { DataTableModule } from "primeng/components/datatable/datatable";
import { DialogModule } from "primeng/components/dialog/dialog";
import { SharedModule } from "primeng/components/common/shared";
import { ButtonModule } from "primeng/components/button/button";

//Services
import { DataService } from './core/dal/contracts/data.service';
import { ApiProviders } from './core/dal/api.providers';
import { ServerSettingsService } from './core/dal/server.settings.service';
import { LoggerService } from './core/services/logger.service';
import { ApiServer } from './core/dal/servers/api.server';


@NgModule({
    bootstrap: [AppComponent],
    declarations: [
        AppComponent,
        NavMenuComponent,
        HomeComponent,
        ListUploadComponent,
        DropdownComponent,
        FighterListComponent,
        FighterFilter,
        FileUpload,
        TournamentSettingsComponent

    ],
    imports: [
        routing,
        DataTableModule,
        DialogModule,
        FormsModule,
        BrowserModule,
        UniversalModule,
        SharedModule,
        HttpModule
    ],
    providers: [
        appRoutingProviders,
        ApiProviders,
        LoggerService,
        ServerSettingsService
    ]
})
export class AppModule {
}
