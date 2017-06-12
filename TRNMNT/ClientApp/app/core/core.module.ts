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
import { NotificationService } from './services/notification.service';
import { LoaderService } from './services/loader.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { BrowserModule } from '@angular/platform-browser'


import { AuthGuard } from './routing/auth.guard';

@NgModule({
    imports: [
        FormsModule,
        HttpModule,
        RouterModule,
        BrowserAnimationsModule,
        BrowserModule
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
        AuthGuard],

    exports: [
        FormsModule,
        HttpModule,
        RouterModule,
        BrowserAnimationsModule,
        BrowserModule
    ]
})
export class CoreModule { }