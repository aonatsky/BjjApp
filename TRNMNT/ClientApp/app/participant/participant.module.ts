import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ParticipantComponent } from './participant.component';
import { SharedModule } from '../shared/shared.module';
import { CoreModule } from '../core/core.module';
import { MyEventsComponent } from './my-events/my-events.component';
import { EventInfoPanelComponent } from './event-info-panel/event-info-panel.component';
import { MyTeamComponent } from './my-team/my-team.component';
import { CustomLoader } from '../core/helpers/custom-loader';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';

@NgModule({
  imports: [CommonModule, SharedModule, CoreModule, TranslateModule.forChild({ loader: { provide: TranslateLoader, useClass: CustomLoader } })],
  declarations: [ParticipantComponent, MyEventsComponent, EventInfoPanelComponent, MyTeamComponent]
})
export class ParticipantModule {}
