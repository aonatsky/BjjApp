import { NgModule } from '@angular/core';
import { routing, appRoutingProviders } from "./app.routing";
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app-component/app.component'

//modules
import { CoreModule } from './core/core.module'
import { AdministrationModule } from './administration/administration.module'


//PrimeNG
import { DataTableModule } from "primeng/components/datatable/datatable";
import { DialogModule } from "primeng/components/dialog/dialog";
import { SharedModule } from "primeng/components/common/shared";
import { ButtonModule } from "primeng/components/button/button";
import { InputTextModule } from "primeng/components/inputtext/inputtext";
import { GrowlModule } from 'primeng/components/growl/growl';
import { DropdownModule } from 'primeng/components/dropdown/dropdown';


//Administration components
import { NavMenuComponent } from './administration/navmenu/navmenu.component';
import { HomeComponent } from './administration/home/home.component';
import { FighterListComponent } from './administration/fighter-list/fighter-list.component';
import { TournamentSettingsComponent } from './administration/tournament-settings/tournament-settings.component';




//Services



@NgModule({
    bootstrap: [AppComponent],
    declarations: [
        AppComponent,
       
    ],
    imports: [
        routing,
        SharedModule,
        CoreModule,
        AdministrationModule,
        GrowlModule
],
    providers: [
        appRoutingProviders,
    ]
})
export class AppModule {
}
