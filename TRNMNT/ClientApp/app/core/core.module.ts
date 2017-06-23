import { NgModule } from '@angular/core'
import { HttpModule, Http } from '@angular/http';
import { FormsModule } from '@angular/forms';

import { AuthHttp, AuthConfig, AUTH_PROVIDERS, provideAuth } from 'angular2-jwt';

import { RouterModule } from '@angular/router';
import { DataService } from './dal/contracts/data.service';
import { HttpService } from './dal/http/http.service';
import { AuthenticationService } from './services/authentication.service';
import { ApiProviders } from './dal/api.providers';
import { LoggerService } from './services/logger.service';
import { RouterService } from './services/router.service';
import { NotificationService } from './services/notification.service';
import { LoaderService } from './services/loader.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { BrowserModule } from '@angular/platform-browser'

//PrimeNG
import { DataTableModule } from "primeng/components/datatable/datatable";
import { DialogModule } from "primeng/components/dialog/dialog";
import { SharedModule } from "primeng/components/common/shared";
import { ButtonModule } from "primeng/components/button/button";
import { InputTextModule } from "primeng/components/inputtext/inputtext";
import { GrowlModule } from 'primeng/components/growl/growl';
import { DropdownModule } from 'primeng/components/dropdown/dropdown';
import { InputMaskModule } from 'primeng/primeng';


import { AuthGuard } from './routing/auth.guard';

@NgModule({
    imports: [
        FormsModule,
        HttpModule,
        RouterModule,
        BrowserAnimationsModule,
        BrowserModule,
        DataTableModule,
        DialogModule,
        InputTextModule,
        ButtonModule,
        DropdownModule,
        GrowlModule,
        InputMaskModule

    ],
    declarations: [],

    providers: [HttpService, ApiProviders, LoggerService, LoaderService, NotificationService, AuthenticationService, AuthHttp, provideAuth({
        headerName: 'Authorization',
        headerPrefix: 'bearer',
        tokenName: 'token',
        tokenGetter: (() => localStorage.getItem('id_token')),
        globalHeaders: [{ 'Content-Type': 'application/json' }],
        noJwtError: true
    }),
        AuthGuard, RouterService],

    exports: [
        FormsModule,
        HttpModule,
        RouterModule,
        BrowserAnimationsModule,
        BrowserModule,
        DataTableModule,
        DialogModule,
        InputTextModule,
        ButtonModule,
        DropdownModule,
        GrowlModule,
        InputMaskModule
    ]
})
export class CoreModule { }