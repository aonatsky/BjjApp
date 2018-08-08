import { TranslateModule } from '@ngx-translate/core';
import { NgModule } from '@angular/core';
import { CoreModule } from '../core/core.module';
import { LoginComponent } from './login/login.component';
import { LoginPageComponent } from './login/login-page.component';
import { RegisterComponent } from './register/register.component';
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
import { PopupComponent } from './popup/popup.component';
import { PageComponent } from './page/page.component';
import { ProfileComponent } from './profile/profile.component';
import { ParticipantListComponent } from './participant-list/participant-list.component';

@NgModule({
  imports: [CoreModule, BrowserAnimationsModule, RoundModule, SocialLoginModule, TranslateModule],
  declarations: [
    LoginComponent,
    RegisterComponent,
    CrudComponent,
    CategoryWithDivisionFilter,
    LoaderComponent,
    BracketComponent,
    TablePickerListComponent,
    FooterComponent,
    TopbarComponent,
    LoginPageComponent,
    PopupComponent,
    PageComponent,
    ProfileComponent,
    ParticipantListComponent
  ],

  providers: [
    {
      provide: AuthServiceConfig,
      useFactory: getAuthServiceConfigs
    }
  ],

  exports: [
    CrudComponent,
    CategoryWithDivisionFilter,
    LoaderComponent,
    BracketComponent,
    TablePickerListComponent,
    PopupComponent,
    LoginComponent,
    PageComponent,
    ParticipantListComponent
  ]
})
export class SharedModule {}
