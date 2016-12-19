import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {FormsModule} from '@angular/forms';
import {HttpModule, JsonpModule} from '@angular/http';
import { UniversalModule } from 'angular2-universal';
import { AppComponent } from './app.component'
import { NavMenuComponent } from './administration/navmenu/navmenu.component';
import { HomeComponent } from './administration/home/home.component';
import { ListUploadComponent } from './administration/listupload/listupload.component';
import { DropdownComponent } from './shared/DropDown/dropdown.component';
import { FileUpload } from './shared/fileUpload/fileUpload.component';

import {routing, appRoutingProviders} from './app.routing';


@NgModule({
    declarations: [
        AppComponent,
        AppComponent,
        NavMenuComponent,
        HomeComponent,
        ListUploadComponent,
        DropdownComponent,
        ListUploadComponent

    ],
    imports: [
       UniversalModule,
        routing
    ],
    providers: [appRoutingProviders],
    bootstrap: [AppComponent]
})

export class AppModule {
}
