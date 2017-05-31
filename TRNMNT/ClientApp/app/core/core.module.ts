import { NgModule } from '@angular/core'
import { HttpModule, Http } from '@angular/http';
import { FormsModule } from '@angular/forms';

import { RouterModule } from '@angular/router';
import { DataService } from './dal/contracts/data.service';
import { HttpService } from './dal/http/http.service';
import { ApiProviders } from './dal/api.providers';
import { LoggerService } from './services/logger.service';
import { NotificationService } from './services/notification.service';
import { LoaderService } from './services/loader.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { BrowserModule } from '@angular/platform-browser'

@NgModule({
    imports: [
        FormsModule,
        HttpModule,
        RouterModule,
        BrowserAnimationsModule,
        BrowserModule
        
    ],
    declarations: [],

    providers: [HttpService, ApiProviders, LoggerService, LoaderService, NotificationService],

    exports: [
        FormsModule,
        HttpModule,
        RouterModule,
        BrowserAnimationsModule,
        BrowserModule
    ]
})
export class CoreModule { }