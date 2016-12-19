import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {FormsModule} from '@angular/forms';
import {HttpModule, JsonpModule} from '@angular/http';
import { UniversalModule } from 'angular2-universal';
import { AppComponent } from './app.component'
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { ListUploadComponent } from './components/listupload/listupload.component';
import { DropdownComponent } from './components/Shared/DropDown/dropdown.component';
import { FileUpload } from './components/Shared/fileUpload/fileUpload.component';

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
