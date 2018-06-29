import { NgModule } from '@angular/core'
import { FormsModule } from '@angular/forms';

import { JwtModule } from '@auth0/angular-jwt';
import { HttpClientModule } from '@angular/common/http';

import { RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { BrowserModule } from '@angular/platform-browser'

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

//PrimeNG
import { DataTableModule } from 'primeng/components/datatable/datatable';
import { DialogModule } from 'primeng/components/dialog/dialog';
import { ButtonModule } from 'primeng/components/button/button';
import { InputTextModule } from 'primeng/components/inputtext/inputtext';
import { GrowlModule } from 'primeng/components/growl/growl';
import { DropdownModule } from 'primeng/components/dropdown/dropdown';
import { TableModule } from 'primeng/table';
import { RadioButtonModule } from 'primeng/radiobutton';
import { InputMaskModule } from 'primeng/primeng';
import { DragDropModule, MenubarModule } from 'primeng/primeng';
import { StepsModule, CalendarModule, InputTextareaModule, FileUploadModule, AutoCompleteModule, CheckboxModule, TabViewModule, ToggleButtonModule, ConfirmDialogModule, } from 'primeng/primeng';
import { AuthGuard } from './routing/auth.guard';
import { RedirectGuard } from './routing/redirect.guard';
import { BracketService } from './services/bracket.service';
import { MinuteSecondsPipe } from './pipes/minutes-seconds.pipe';
import { SignalRHubService } from './dal/signalr/signalr-hub.service';
import { TestSocketService } from './services/test-socket.service';
import { FormatTimerPipe } from './pipes/format-timer.pipe';
import { RunEventHubService } from './hubservices/run-event.hub.service';
import { ToDictionaryPipe } from './pipes/to-dictionary.pipe';
import { ResultsService } from './services/results.service';



@
NgModule({
    imports: [
        FormsModule,
        HttpClientModule,
        JwtModule.forRoot({
            config: {
                tokenGetter: () => localStorage.getItem('id_token'),
                whitelistedDomains: ['localhost:53432/api'],
                blacklistedRoutes: ['localhost:53432/api/auth/', 'localhost:53432/api/log/'],
                throwNoTokenError: false
                }
            }),
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
            ConfirmDialogModule,
            TableModule,
            RadioButtonModule,
            MenubarModule
        ],
        declarations: [
            MinuteSecondsPipe,
            ToDictionaryPipe,
            FormatTimerPipe
        ],
        providers: [
            HttpService,
            LoggerService,
            LoaderService,
            NotificationService,
            AuthService,
            EventService,
            TeamService,
            SignalRHubService,
            TestSocketService,
            RunEventHubService,
            { provide: RunEventHubService, useFactory: (l: LoggerService) => new RunEventHubService(new SignalRHubService(l)), deps: [LoggerService] },

            CategoryService,
            WeightDivisionService,
            ParticipantService,
            BracketService,
            ResultsService,
//            provideAuth({
//                headerName: 'Authorization',
//                headerPrefix: 'bearer',
//                tokenName: 'token',
//                tokenGetter: (() => localStorage.getItem('id_token')),
//                noJwtError: true
//            }),
            { provide: Window, useValue: window },
            AuthGuard, RedirectGuard, RouterService, PaymentService],

        exports: [
            FormsModule,
            HttpClientModule,
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
            ToDictionaryPipe,
            ToggleButtonModule,
            ConfirmDialogModule,
            TableModule,
            RadioButtonModule,
            MenubarModule,
            FormatTimerPipe
        ]
    })
export class CoreModule { }