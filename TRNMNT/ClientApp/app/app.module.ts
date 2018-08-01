import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { CoreModule } from './core/core.module';
import { SharedModule } from './shared/shared.module';
import { EventAdminModule } from './event-admin/event-admin.module';
import { EventModule } from './event/event.module';
import { RoundModule } from './round/round.module';
import { AppRoutingModule } from './app.routing.module';
import { TranslateModule, TranslateLoader, TranslateService } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { HttpClient } from '@angular/common/http';
import { CustomLoader } from './core/helpers/custom-loader';
import { registerLocaleData } from '@angular/common';
import localeEn from '@angular/common/locales/en';
import localeUa from '@angular/common/locales/ru-UA';

registerLocaleData(localeEn, 'en');
registerLocaleData(localeUa, 'ua')

@NgModule({
  bootstrap: [AppComponent],
  declarations: [AppComponent],
  imports: [
    CoreModule,
    SharedModule,
    EventAdminModule,
    EventModule,
    RoundModule,
    AppRoutingModule,
    TranslateModule.forRoot({
      loader: { provide: TranslateLoader, useClass: CustomLoader }
    })
  ],
  providers: [TranslateService]
})
export class AppModule {}
