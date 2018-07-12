import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { CoreModule } from './core/core.module';
import { SharedModule } from './shared/shared.module';
import { EventAdminModule } from './event-admin/event-admin.module';
import { EventModule } from './event/event.module';
import { RoundModule } from './round/round.module';
import { AppRoutingModule } from './app.routing.module';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { HttpClient } from '@angular/common/http';
import { CustomLoader } from './core/helpers/custom-loader';

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, './locale/', '.json');
}

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
  providers: []
})
export class AppModule {}
