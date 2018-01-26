import { NgModule } from '@angular/core'
import { HttpModule, Http } from '@angular/http';
import { FormsModule } from '@angular/forms';

import { AuthHttp, AuthConfig, AUTH_PROVIDERS, provideAuth } from 'angular2-jwt';

import { RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { BrowserModule } from '@angular/platform-browser'


import { DataService } from './dal/contracts/data.service';
import { HttpService } from './dal/http/http.service';
import { AuthService } from './services/auth.service';
import { LoggerService } from './services/logger.service';
import { RouterService } from './services/router.service';
import { NotificationService } from './services/notification.service';
import { LoaderService } from './services/loader.service';
import { EventService } from './services/event.service';
import { TeamService } from './services/team.service';
import { ParticipantService } from './services/participant.service';
import { WeightDivisionService } from './services/weight-division.service';
import { CategoryService } from './services/category.service';
import { PaymentService } from './services/payment.service';

import { UserModel } from './model/user.model'

//PrimeNG
import { DataTableModule } from 'primeng/components/datatable/datatable';
import { DialogModule } from 'primeng/components/dialog/dialog';
import { SharedModule } from 'primeng/components/common/shared';
import { ButtonModule } from 'primeng/components/button/button';
import { InputTextModule } from 'primeng/components/inputtext/inputtext';
import { GrowlModule } from 'primeng/components/growl/growl';
import { DropdownModule } from 'primeng/components/dropdown/dropdown';
import { InputMaskModule } from 'primeng/primeng';
import { StepsModule, CalendarModule, InputTextareaModule, FileUploadModule, AutoCompleteModule, CheckboxModule, TabViewModule, ToggleButtonModule, ConfirmDialogModule, DragDropModule } from 'primeng/primeng';
import { AuthGuard } from './routing/auth.guard';
import { RedirectGuard } from './routing/redirect.guard';
import {BracketService} from './services/bracket.service';
import { MinuteSecondsPipe } from "./pipes/minutes-seconds.pipe";
import { WebsocketService } from './dal/websocket/websocket.service';
import { TestWebsocketService } from './services/test-websocket.service';



@
NgModule({
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
        InputMaskModule,
        StepsModule,
        CalendarModule,
        InputTextareaModule,
        AutoCompleteModule,
        CheckboxModule,
        DragDropModule,
        ToggleButtonModule,
        ConfirmDialogModule
    ],
    declarations: [
        MinuteSecondsPipe
    ],
    providers: [
        HttpService,
        WebsocketService,
        LoggerService,
        LoaderService,
        NotificationService,
        AuthService,
        AuthHttp,
        EventService,
        TeamService,
        TestWebsocketService,
        CategoryService,
        WeightDivisionService,
        ParticipantService,
        BracketService,
        provideAuth({
            headerName: 'Authorization',
            headerPrefix: 'bearer',
            tokenName: 'token',
            tokenGetter: (() => localStorage.getItem('id_token')),
            noJwtError: true
        }),
        AuthGuard, RedirectGuard, RouterService, PaymentService],

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
        InputMaskModule,
        StepsModule,
        CalendarModule,
        InputTextareaModule,
        FileUploadModule,
        AutoCompleteModule,
        CheckboxModule,
        TabViewModule,
        DragDropModule,
        MinuteSecondsPipe,
        ToggleButtonModule,
        ConfirmDialogModule
    ]
})
export class CoreModule { }