import { NgModule } from '@angular/core'
import { CoreModule } from './../core/core.module'
import { LoginComponent } from './login/login.component'
import { RegisterComponent } from './register/register.component'
import { CategoryWithDivisionFilter } from './category-with-division-filter/category-with-division-filter.component';
import { CrudComponent } from './crud/crud.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LoaderComponent } from './loader/loader';
import { BracketComponent } from './bracket/bracket.component';
import { TablePickerListComponent } from './table-picker-list/table-picker-list.component';
import { FooterComponent } from './footer/footer.component';
import { TopbarComponent } from './topbar/topbar.component';
import { RoundModule } from '../round/round.module';
import { SocialLoginModule, AuthServiceConfig } from 'angular5-social-login';
import { getAuthServiceConfigs } from '../socialLoginConfig';


@NgModule({
    imports: [
        CoreModule,
        BrowserAnimationsModule,
        RoundModule,
        SocialLoginModule
    ],
    declarations: [
        LoginComponent,
        RegisterComponent,
        CrudComponent,
        CategoryWithDivisionFilter,
        LoaderComponent,
        BracketComponent,
        TablePickerListComponent,
        FooterComponent,
        TopbarComponent
    ],

    providers: [
        {
            provide: AuthServiceConfig,
            useFactory: getAuthServiceConfigs
        }],

    exports: [
        CrudComponent,
        CategoryWithDivisionFilter,
        LoaderComponent,
        BracketComponent,
        TablePickerListComponent,
    ]
})

export class SharedModule {
}
