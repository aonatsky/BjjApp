import { NgModule } from '@angular/core';
import { AppComponent } from './app-component/app.component';
import { RouterModule } from '@angular/router';
import { CoreModule } from './core/core.module';
import { SharedModule } from './shared/shared.module';
import { EventAdminModule } from './event-admin/event-admin.module';
import { EventModule } from './event/event.module';
import { RoundModule } from './round/round.module';
import { appRoutes } from './app.routing';

@NgModule({
  bootstrap: [AppComponent],
  declarations: [AppComponent],
  imports: [CoreModule, SharedModule, EventAdminModule, EventModule, RoundModule, RouterModule.forRoot(appRoutes)],
  providers: []
})
export class AppModule {}
