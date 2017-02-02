import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {FormsModule} from '@angular/forms';
import {HttpModule, JsonpModule} from '@angular/http';
import { UniversalModule } from 'angular2-universal';
//external tools
import { DataTableModule } from 'angular-2-data-table';

//Components
import { AppComponent } from './app.component'
import { NavMenuComponent } from './administration/navmenu/navmenu.component';
import { HomeComponent } from './administration/home/home.component';
import { ListUploadComponent } from './administration/listupload/listupload.component';
import { FighterListComponent } from './administration/fighterlist/fighterlist.component';
import { BracketsComponent } from './administration/brackets/brackets.component';
import { DropdownComponent } from './shared/dropdown/dropdown.component';
import { FighterFilter } from './shared/fighter-filter/fighter-filter.component';

//service
import { DataService } from './core/dal/contracts/data.service';
import { ApiProviders } from './core/dal/api.providers';
import { ServerSettingsService } from './core/dal/server.settings.service';


import {routing, appRoutingProviders} from './app.routing';


@NgModule({
    declarations: [
        AppComponent,
        AppComponent,
        NavMenuComponent,
        //administration        
        HomeComponent,
        FighterListComponent,
        ListUploadComponent,
        BracketsComponent,
        //shared
        DropdownComponent,
        FighterFilter
    ],
    imports: [
       UniversalModule,
        routing,
        DataTableModule,
        FormsModule
        
    ],
    providers: [
        appRoutingProviders,
        ApiProviders,
        ServerSettingsService
        ],
    bootstrap: [AppComponent]
})

export class AppModule {
}
