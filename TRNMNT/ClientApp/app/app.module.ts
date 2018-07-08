import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { CoreModule } from './core/core.module';
import { SharedModule } from './shared/shared.module';
import { EventAdminModule } from './event-admin/event-admin.module';
import { EventModule } from './event/event.module';
import { RoundModule } from './round/round.module';
import { AppRoutingModule } from './app.routing.module';

@NgModule({
  bootstrap: [AppComponent],
  declarations: [AppComponent],
  imports: [CoreModule, SharedModule, EventAdminModule, EventModule, RoundModule, AppRoutingModule],
  providers: []
})
export class AppModule {}
