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
import { DropdownComponent } from './shared/dropdown/dropdown.component';
//service
import { DataService } from './core/dal/contracts/data.service';
import { ApiProviders } from './core/dal/api.providers';

import {routing, appRoutingProviders} from './app.routing';


@NgModule({
    declarations: [
        AppComponent,
        AppComponent,
        NavMenuComponent,
        HomeComponent,
        ListUploadComponent,
        DropdownComponent,
        ListUploadComponent,
        FighterListComponent,
        DropdownComponent
    ],
    imports: [
       UniversalModule,
        routing,
        DataTableModule,
        FormsModule
        
    ],
    providers: [appRoutingProviders,ApiProviders],
    bootstrap: [AppComponent]
})

export class AppModule {
}
